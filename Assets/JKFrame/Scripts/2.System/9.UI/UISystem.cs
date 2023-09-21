using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JKFrame
{
    /// <summary>
    /// UI根节点
    /// </summary>
    public class UISystem : MonoBehaviour
    {
        private static UISystem instance;
        public static void Init()
        {
            instance = JKFrameRoot.RootTransform.GetComponentInChildren<UISystem>();
        }
        #region 内部类
        [Serializable]
        private class UILayer
        {
            public Transform root;
            public bool enableMask = true;
            public Image maskImage;
            private int _count;
            public void OnWindowShow()
            {
                _count += 1;
                Update();
            }
            public void OnWindowClose(bool update = true)
            {
                _count -= 1;
                if (update) Update();
            }
            private void Update()
            {
                maskImage.name = $"Mask{_count}";
                if (enableMask == false) return;
                maskImage.raycastTarget = _count != 0;
                int posIndex = root.childCount - 2;
                maskImage.transform.SetSiblingIndex(posIndex < 0 ? 0 : posIndex);
            }
            public void Reset()
            {
                _count = 0;
                Update();
            }
        }
        #endregion

        private static Dictionary<string, UIWindowData> UIWindowDataDic => JKFrameRoot.Setting.UIWindowDataDic;
        [SerializeField] private UILayer[] uiLayers;
        [SerializeField] private RectTransform dragLayer;
        /// <summary>
        /// 拖拽层，位于所有UI的最上层
        /// </summary>
        public static RectTransform DragLayer { get => instance.dragLayer; }
        private static UILayer[] UILayers { get => instance.uiLayers; }

        [SerializeField] GameObject UITipsItemPrefab;
        [SerializeField] private RectTransform UITipsItemParent;

        #region 动态加载/移除窗口数据
        // UI系统的窗口数据中主要包含：预制体路径、是否缓存、当前窗口对象实例等重要信息
        // 为了方便使用，所以窗口数据必须先存放于UIWindowDataDic中，才能通过UI系统显示、关闭等

        /// <summary>
        /// 初始化UI元素数据
        /// 只执行OnInit，不执行OnShow
        /// 会自动SetActive(false)
        /// </summary>
        /// <param name="windowKey">自定义的名称，可以是资源路径或类型名称或其他自定义</param>
        /// <param name="windowData">窗口的重要数据</param>
        /// <param name="instantiateAtOnce">是否立刻实例化，前提是有缓存必要</param>
        public static void AddUIWindowData(string windowKey, UIWindowData windowData, bool instantiateAtOnce = false)
        {
            if (UIWindowDataDic.TryAdd(windowKey, windowData))
            {
                if (instantiateAtOnce)
                {
                    if (windowData.isCache)
                    {
                        UI_WindowBase window = ResSystem.InstantiateGameObject<UI_WindowBase>(windowData.assetPath, UILayers[windowData.layerNum].root, windowKey);
                        windowData.instance = window;
                        window.Init();
                        window.gameObject.SetActive(false);
                    }
                    else
                    {
                        JKLog.Warning("JKFrame:UIWindowData中的isCache=false，但instantiateAtOnce=true!提前实例化对于不需要缓存的窗口来说没有意义");
                    }
                }
            }
        }

        /// <summary>
        /// 初始化UI元素数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="windowData"></param>
        /// <param name="instantiateAtOnce">
        /// 立刻实例化，但是：
        /// 只执行OnInit，不执行OnShow
        /// 会自动SetActive(false)
        /// </param>
        public static void AddUIWindowData(Type type, UIWindowData windowData, bool instantiateAtOnce = false)
        {
            AddUIWindowData(type.FullName, windowData, instantiateAtOnce);
        }

        /// <summary>
        /// 初始化UI元素数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="windowData"></param>
        /// <param name="instantiateAtOnce">
        /// 立刻实例化，但是：
        /// 只执行OnInit，不执行OnShow
        /// 会自动SetActive(false)
        /// </param>
        public static void AddUIWindowData<T>(UIWindowData windowData, bool instantiateAtOnce = false)
        {
            AddUIWindowData(typeof(T), windowData, instantiateAtOnce);
        }

        /// <summary>
        /// 获取UI窗口数据
        /// </summary>
        /// <param name="windowKey"></param>
        /// <returns>可能为Null</returns>
        public static UIWindowData GetUIWindowData(string windowKey)
        {
            if (UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData))
            {
                return windowData;
            }
            return null;
        }

        public static UIWindowData GetUIWindowData(Type windowType)
        {
            return GetUIWindowData(windowType.FullName);
        }

        public static UIWindowData GetUIWindowData<T>()
        {
            return GetUIWindowData(typeof(T));
        }

        /// <summary>
        /// 尝试获取UI窗口数据
        /// </summary>
        /// <param name="windowKey"></param>
        public static bool TryGetUIWindowData(string windowKey, out UIWindowData windowData)
        {
            return UIWindowDataDic.TryGetValue(windowKey, out windowData);
        }

        /// <summary>
        /// 移除UI窗口数据,已存在的窗口会被强行删除
        /// </summary>
        /// <param name="windowKey"></param>
        /// <returns></returns>
        public static bool RemoveUIWindowData(string windowKey)
        {
            if (TryGetUIWindowData(windowKey, out UIWindowData windowData))
            {
                if (windowData.instance != null)
                {
                    Destroy(windowData.instance.gameObject);
                }
            }
            return UIWindowDataDic.Remove(windowKey);
        }

        /// <summary>
        /// 清除所有UI窗口数据
        /// </summary>
        public static void ClearUIWindowData()
        {
            var enumerator = UIWindowDataDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Destroy(enumerator.Current.Value.instance.gameObject);
            }
            UIWindowDataDic.Clear();
        }
        #endregion

        #region UI窗口生命周期管理
        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <param name="layer">层级 -1等于不设置</param>
        public static T Show<T>(int layer = -1) where T : UI_WindowBase
        {
            return Show(typeof(T), layer) as T;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <typeparam name="T">要返回的窗口类型</typeparam>
        /// <param name="windowKey">窗口的Key</param>
        /// <param name="layer">层级 -1等于不设置</param>
        public static T Show<T>(string windowKey, int layer = -1) where T : UI_WindowBase
        {
            return Show(windowKey, layer) as T;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="type">窗口类型</param>
        /// <param name="layer">层级 -1等于不设置</param>
        public static UI_WindowBase Show(Type type, int layer = -1)
        {
            return Show(type.FullName, layer);
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="windowKey">窗口的key</param>
        /// <param name="layer">层级 -1等于不设置</param>
        public static UI_WindowBase Show(string windowKey, int layer = -1)
        {
            if (UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData))
            {
                return Show(windowData, windowKey, layer);
            }
            // 资源库中没有意味着不允许显示
            JKLog.Log($"JKFrame:不存在{windowKey}的UIWindowData");
            return null;
        }

        private static UI_WindowBase Show(UIWindowData windowData, string windowKey, int layer = -1)
        {
            int layerNum = layer == -1 ? windowData.layerNum : layer;
            // 实例化实例或者获取到实例，保证窗口实例存在
            if (windowData.instance != null)
            {
                // 原本就激活使用状态，避免内部计数问题，进行一次层关闭
                if (windowData.instance.UIEnable)
                {
                    UILayers[layerNum].OnWindowClose(false);
                }
                windowData.instance.gameObject.SetActive(true);
                windowData.instance.transform.SetParent(UILayers[layerNum].root);
                windowData.instance.transform.SetAsLastSibling();
                windowData.instance.ShowGeneralLogic();
            }
            else
            {
                UI_WindowBase window = ResSystem.InstantiateGameObject<UI_WindowBase>(windowData.assetPath, UILayers[layerNum].root, windowKey);
                windowData.instance = window;
                window.Init();
                window.ShowGeneralLogic();
            }
            windowData.layerNum = layerNum;
            UILayers[layerNum].OnWindowShow();
            return windowData.instance;
        }
        #endregion

        #region 获取与销毁窗口
        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="windowKey">窗口Key</param>
        /// <returns>没找到会为Null</returns>
        public static UI_WindowBase GetWindow(string windowKey)
        {
            if (UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData))
            {
                return windowData.instance;
            }
            return null;
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="windowKey">窗口Key</param>
        /// <returns>没找到会为Null</returns>
        public static T GetWindow<T>(string windowKey) where T : UI_WindowBase
        {
            return GetWindow(windowKey) as T;
        }


        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns>没找到会为Null</returns>
        public static T GetWindow<T>() where T : UI_WindowBase
        {
            return GetWindow(typeof(T).FullName) as T;
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns>没找到会为Null</returns>
        public static UI_WindowBase GetWindow(Type windowType)
        {
            return GetWindow(windowType.FullName);
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="windowKey">窗口Key</param>
        /// <returns>没找到会为Null</returns>
        public static T GetWindow<T>(Type windowType) where T : UI_WindowBase
        {
            return GetWindow(windowType.FullName) as T;
        }

        /// <summary>
        /// 尝试获取窗口
        /// </summary>
        /// <param name="windowKey"></param>
        public static bool TryGetWindow(string windowKey, out UI_WindowBase window)
        {
            UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData);
            window = windowData.instance;
            return window != null;
        }

        /// <summary>
        /// 尝试获取窗口
        /// </summary>
        /// <param name="windowKey"></param>
        public static bool TryGetWindow<T>(string windowKey, out T window) where T : UI_WindowBase
        {
            UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData);
            window = windowData.instance as T;
            return window != null;
        }

        /// <summary>
        /// 销毁窗口
        /// </summary>
        public static void DestroyWindow(string windowKey)
        {
            UI_WindowBase window = GetWindow(windowKey);
            if (window != null)
            {
                DestroyImmediate(window.gameObject);
            }
        }
        #endregion

        #region 关闭窗口
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        public static void Close<T>()
        {
            Close(typeof(T));
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <typeparam name="Type">窗口类型</typeparam>
        public static void Close(Type type)
        {
            Close(type.FullName);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="windowKey"></param>
        public static void Close(string windowKey)
        {
            if (TryGetUIWindowData(windowKey, out UIWindowData windowData))
            {
                if (windowData.instance != null && CloseWindow(windowData))
                {
                    UILayers[windowData.layerNum].OnWindowClose();
                }
                else JKLog.Warning("JKFrame:您需要关闭的窗口不存在或已经关闭");
            }
            else JKLog.Warning("JKFrame:未查询到UIWindowData");
        }


        private static bool CloseWindow(UIWindowData windowData)
        {
            if (windowData.instance.UIEnable)
            {
                windowData.instance.CloseGeneralLogic();
                // 缓存则隐藏
                if (windowData.isCache)
                {
                    windowData.instance.transform.SetAsFirstSibling();
                    windowData.instance.gameObject.SetActive(false);
                }
                // 不缓存则销毁
                else
                {
#if ENABLE_ADDRESSABLES
                    ResSystem.UnloadInstance(windowData.instance.gameObject);
#endif
                    DestroyImmediate(windowData.instance.gameObject);
                    windowData.instance = null;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭全部窗口
        /// </summary>
        public static void CloseAllWindow()
        {
            // 处理缓存中所有状态的逻辑
            var enumerator = UIWindowDataDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.instance != null
                    && enumerator.Current.Value.instance.gameObject.activeInHierarchy == true)
                {
                    CloseWindow(enumerator.Current.Value);
                }
            }
            for (int i = 0; i < UILayers.Length; i++)
            {
                UILayers[i].Reset();
            }
        }
        #endregion

        #region UITips
        public static void AddTips(string tips)
        {
            UITipsItem item = PoolSystem.GetGameObject<UITipsItem>(instance.UITipsItemPrefab.name, instance.UITipsItemParent);
            if (item == null) item = GameObject.Instantiate(instance.UITipsItemPrefab, instance.UITipsItemParent).GetComponent<UITipsItem>();
            item.Init(tips);
        }
        #endregion

        #region 工具

        private static List<RaycastResult> raycastResultList = new List<RaycastResult>();
        /// <summary>
        /// 检查鼠标是否在UI上,会屏蔽名称为Mask的物体
        /// </summary>
        public static bool CheckMouseOnUI()
        {
            return CheckPositoinOnUI(Input.mousePosition);
        }

        /// <summary>
        /// 检查一个坐标是否在UI上,会屏蔽名称为Mask的物体
        /// </summary>
        public static bool CheckPositoinOnUI(Vector2 pos)
        {
            UnityEngine.EventSystems.EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current;
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = pos;
            // 射线去检测有没有除了Mask以外的任何UI物体
            eventSystem.RaycastAll(pointerEventData, raycastResultList);
            for (int i = 0; i < raycastResultList.Count; i++)
            {
                // 是UI，同时还不是Mask作用的物体
                if (raycastResultList[i].gameObject.name != "Mask")
                {
                    raycastResultList.Clear();
                    return true;
                }
            }
            raycastResultList.Clear();
            return false;
        }
        #endregion

    }
}
