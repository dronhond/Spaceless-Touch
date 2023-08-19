using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView
{    
    public class ButtonPanelMenuList : MonoBehaviour 
    {
        static string[] sceneArray0 =
        {
            "ListViewTopToBottomDemo",
            "ListViewLeftToRightDemo",
            "ListViewPullDownRefreshDemo",
            "ListViewPullUpLoadMoreDemo",            
            "ListViewSelectDeleteDemo",
            "ListViewChangeItemHeightDemo",            
            "ListViewFilterDemo",
            "ListViewMultiplePrefabTopToBottomDemo",
            "ListViewMultiplePrefabLeftToRightDemo",
            "ListViewBottomToTopDemo",
            "ListViewRightToLeftDemo",
            "ListViewClickLoadMoreDemo",
            "ListViewSimpleTopToBottomDemo",
            "ListViewSimpleLeftToRightDemo",
        };

        static string[] sceneArray1 =
        {
            "GridViewTopToBottomDemo",
            "GridViewLeftToRightDemo",
            "GridViewBottomToTopDemo",
            "GridViewRightToLeftDemo",
            "GridViewClickLoadMoreDemo",
            "GridViewSelectDeleteDemo",
            "GridViewMultiplePrefabLeftToRightDemo",
            "GridViewMultiplePrefabTopToBottomDemo",
            "GridViewDiagonalDemo",
            "GridViewDiagonalReverseDemo",
            "GridViewDiagonalSelectDeleteDemo",
            "GridViewSimpleTopToBottomDemo",
            "GridViewSimpleLeftToRightDemo",
            "PageViewDemo",
            "PageViewSimpleDemo",
        };

        static string[] sceneArray2 =
        {
            "SpecialGridViewTopToBottomDemo",
            "SpecialGridViewLeftToRightDemo",
            "SpecialGridViewPullDownRefreshDemo",
            "SpecialGridViewPullUpLoadMoreDemo",
            "SpecialGridViewSelectDeleteDemo",
            "SpecialGridViewFeatureLeftToRightDemo",
            "SpecialGridViewFeatureTopToBottomDemo",
            "ResponsiveViewDemo",
            "ResponsiveViewRefreshLoadDemo",
            "NestedTopToBottomDemo",
            "NestedGridViewTopToBottomDemo",
            "NestedLeftToRightDemo",
            "NestedGridViewLeftToRightDemo",
            "SpecialGridViewSimpleTopToBottomDemo",
            "SpecialGridViewSimpleLeftToRightDemo",
        };

        static string[] sceneArray3 =
        {
            "StaggeredViewTopToBottomDemo",
            "StaggeredViewLeftToRightDemo",
            "StaggeredViewBottomToTopDemo",
            "StaggeredViewRightToLeftDemo",
            "StaggeredViewSimpleTopToBottomDemo",
            "StaggeredViewLeftToRightDemo",
            "ChatViewDemo",
            "ChatViewChangeViewportHeightDemo",
            "GalleryVerticalDemo",
            "GalleryHorizontalDemo",
            "SpinDatePickerDemo",
            "TreeViewDemo",
            "TreeViewWithChildIndentDemo",
            "TreeViewWithStickyHeadDemo",
            "TreeViewSimpleDemo",
        };

        static string[][] allSceneArray = new string[][] { sceneArray0, sceneArray1, sceneArray2, sceneArray3 };

        static string[] mainMenuSceneArray = new string[] { "MenuListView", "MenuGridViewPageView", "MenuSpecialViewNestedView", "MenuStaggeredChatGalleryTreeSpin" };


        Button button;
        int sceneArrayIndex;
        void Start() 
        {
            Scene curScene = SceneManager.GetActiveScene();
            string curSceneName = curScene.name;
            for (int i = 0;i< mainMenuSceneArray.Length;++i)
            {
                if(mainMenuSceneArray[i] == curSceneName)
                {
                    sceneArrayIndex = i;
                    break;
                }
            }
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            int index = GetSelfIndexInParent();
            string sceneName = GetSceneName(sceneArrayIndex, index);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public static void BackToMainMenu()
        {
            Scene curScene = SceneManager.GetActiveScene();
            string curSceneName = curScene.name;
            int sceneArrayIndex = GetSceneArrayIndexByName(curSceneName);
            if(sceneArrayIndex < 0)
            {
                return;
            }
            string mainMenuSceneName = mainMenuSceneArray[sceneArrayIndex];
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
        }


        static int GetSceneArrayIndexByName(string sceneName)
        {
            for (int i = 0; i < allSceneArray.Length; ++i)
            {
                string[] sceneArray = allSceneArray[i];
                foreach (string name in sceneArray)
                {
                    if (name == sceneName)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        string GetSceneName(int sceneArrayIndex,int index)
        {
            string[] sceneArray = allSceneArray[sceneArrayIndex];
            return sceneArray[index];
        }

        int GetSelfIndexInParent()
        {
            Transform parentTrans = gameObject.transform.parent;
            for(int i =0;i< parentTrans.childCount;++i)
            {
                if(parentTrans.GetChild(i) == gameObject.transform)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}