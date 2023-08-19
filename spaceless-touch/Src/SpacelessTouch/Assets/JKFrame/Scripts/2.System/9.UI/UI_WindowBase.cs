using System;
using UnityEngine;

namespace JKFrame
{
    /// <summary>
    /// 窗口基类
    /// </summary>
    public class UI_WindowBase : MonoBehaviour
    {
        public bool UIEnable { get; private set; }

        // 窗口类型
        private Type Type => GetType();

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init() { }

        public virtual void ShowGeneralLogic()
        {
            UIEnable = true;
            OnUpdateLanguage();
            RegisterEventListener();
            OnShow();
        }

        /// <summary>
        /// 显示
        /// </summary>
        protected virtual void OnShow() { }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            UISystem.Close(Type);
        }
		
		public void CloseGeneralLogic()
        {
            UIEnable = false;
            CancelEventListener();
            OnClose();
        }
		
        /// <summary>
        /// 关闭时额外执行的内容
        /// </summary>
        protected virtual void OnClose() { }

        /// <summary>
        /// 注册事件
        /// </summary>
        protected virtual void RegisterEventListener()
        {
            JKEventSystem.AddEventListener("UpdateLanguage", OnUpdateLanguage);
        }
        /// <summary>
        /// 取消事件
        /// </summary>
        protected virtual void CancelEventListener()
        {
            JKEventSystem.RemoveEventListener("UpdateLanguage", OnUpdateLanguage);
        }
        protected virtual void OnUpdateLanguage() { }
    }
}