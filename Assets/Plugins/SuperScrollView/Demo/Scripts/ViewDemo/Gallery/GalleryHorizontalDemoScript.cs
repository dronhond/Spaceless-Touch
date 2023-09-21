using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{
    public class GalleryHorizontalDemoScript : MonoBehaviour
    {
        public LoopListView2 mLoopListView;
        public int mTotalDataCount = 10000;
        DataSourceMgr<ItemData> mDataSourceMgr;
        ButtonPanelGallery mButtonPanel;

        // Use this for initialization
        void Start()
        {
            mDataSourceMgr = new DataSourceMgr<ItemData>(mTotalDataCount);
            mLoopListView.InitListView(mDataSourceMgr.TotalItemCount, OnGetItemByIndex);
            InitButtonPanel();
        }

        void InitButtonPanel()
        {
            mButtonPanel = new ButtonPanelGallery();
            mButtonPanel.mLoopListView = mLoopListView;
            mButtonPanel.mDataSourceMgr = mDataSourceMgr;
            mButtonPanel.Start();
        }  

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0 || index >= mDataSourceMgr.TotalItemCount)
            {
                return null;
            }

            ItemData itemData = mDataSourceMgr.GetItemDataByIndex(index);
            if (itemData == null)
            {
                return null;
            }
            //get a new item. Every item can use a different prefab, the parameter of the NewListViewItem is the prefab’name. 
            //And all the prefabs should be listed in ItemPrefabList in LoopListView2 Inspector Setting
            LoopListViewItem2 item = listView.NewListViewItem("ItemPrefab");
            GalleryHorizontalItem itemScript = item.GetComponent<GalleryHorizontalItem>();
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
                itemScript.Init();
            }
            itemScript.SetItemData(itemData, index);
            return item;
        }

        void LateUpdate()
        {
            mLoopListView.UpdateAllShownItemSnapData();
            int count = mLoopListView.ShownItemCount;
            for (int i = 0; i < count; ++i)
            {
                LoopListViewItem2 item = mLoopListView.GetShownItemByIndex(i);
                GalleryHorizontalItem itemScript = item.GetComponent<GalleryHorizontalItem>();
                float scale = 1 - Mathf.Abs(item.DistanceWithViewPortSnapCenter) / 800f;
                scale = Mathf.Clamp(scale,0.4f,1);
                itemScript.mContentRootObj.GetComponent<CanvasGroup>().alpha = scale;
                itemScript.mContentRootObj.transform.localScale = new Vector3(scale, scale, 1);
            }
        }
    }
}
