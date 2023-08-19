using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{

    public class ResponsiveViewDemoScript : MonoBehaviour
    {
        public LoopListView2 mLoopListView;
        public int mTotalDataCount = 10000;
        DataSourceMgr<ItemData> mDataSourceMgr;
        int mItemCountPerRow = 3;
        int mMinWidth = 200;
        public DragChangSizeScript mDragChangSizeScript;
        ButtonPanelSpecial mButtonPanel;

        // Use this for initialization
        void Start()
        {
            mDataSourceMgr = new DataSourceMgr<ItemData>(mTotalDataCount);
            int count = mDataSourceMgr.TotalItemCount / mItemCountPerRow;
            if (mDataSourceMgr.TotalItemCount % mItemCountPerRow > 0)
            {
                count++;
            }
            mLoopListView.InitListView(count, OnGetItemByIndex);   
            mDragChangSizeScript.mOnDragEndAction = OnViewPortSizeChanged;
            OnViewPortSizeChanged();
            InitButtonPanel();
        }

        void InitButtonPanel()
        {
            mButtonPanel = new ButtonPanelSpecial();
            mButtonPanel.mLoopListView = mLoopListView;
            mButtonPanel.mDataSourceMgr = mDataSourceMgr;
            mButtonPanel.mItemCountPerRow = mItemCountPerRow;
            mButtonPanel.Start();
        }       


        void UpdateItemPrefab()
        {
            ItemPrefabConfData tData = mLoopListView.GetItemPrefabConfData("ItemPrefab");
            GameObject prefabObj = tData.mItemPrefab;
            RectTransform rf = prefabObj.GetComponent<RectTransform>();
            BaseHorizontalItemList itemScript = prefabObj.GetComponent<BaseHorizontalItemList>();
            float w = mLoopListView.ViewPortWidth;
            int count = itemScript.mItemList.Count;
            GameObject p0 = itemScript.mItemList[0].gameObject;
            RectTransform rf0 = p0.GetComponent<RectTransform>();
            float w0 = rf0.rect.width;
            int c = Mathf.FloorToInt(w / w0);
            if(c == 0)
            {
                c = 1;
            }
            mItemCountPerRow = c;
            float padding = (w - w0 * c) / (c + 1);
            if(padding < 0)
            {
                padding = 0;
            }
            if(w < mMinWidth)
            {
                w = mMinWidth;
            }
            rf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            if (c > count)
            {
                int dif = c - count;
                for (int i = 0; i < dif; ++i)
                {
                    GameObject go = Object.Instantiate(p0,Vector3.zero,Quaternion.identity,rf);
                    RectTransform trf = go.GetComponent<RectTransform>();
                    trf.localScale = Vector3.one;
                    trf.anchoredPosition3D = Vector3.zero;
                    trf.rotation = Quaternion.identity;
                    BaseHorizontalItem t = go.GetComponent<BaseHorizontalItem>();
                    itemScript.mItemList.Add(t);
                }
            }
            else if (c < count)
            {
                int dif = count - c;
                for (int i = 0; i < dif; ++i)
                {
                    BaseHorizontalItem go = itemScript.mItemList[itemScript.mItemList.Count - 1];
                    itemScript.mItemList.RemoveAt(itemScript.mItemList.Count - 1);
                    Object.DestroyImmediate(go.gameObject);
                }
            }
            float curX = padding;
            for (int k = 0; k < itemScript.mItemList.Count; ++k)
            {
                GameObject obj = itemScript.mItemList[k].gameObject;
                obj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(curX, 0, 0);
                curX = curX + w0 + padding;
            }
            mLoopListView.OnItemPrefabChanged("ItemPrefab");
        }

        void OnViewPortSizeChanged()
        {
            UpdateItemPrefab();
            int count1 = mDataSourceMgr.TotalItemCount / mItemCountPerRow;
            if (mDataSourceMgr.TotalItemCount % mItemCountPerRow > 0)
            {
                count1++;
            }
            mLoopListView.SetListItemCount(count1, false);
            mLoopListView.RefreshAllShownItem();
        }

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }
            LoopListViewItem2 item = listView.NewListViewItem("ItemPrefab");
            BaseHorizontalItemList itemScript = item.GetComponent<BaseHorizontalItemList>();
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
                itemScript.Init();
            }
            for (int i = 0; i < mItemCountPerRow; ++i)
            {
                int itemIndex = index * mItemCountPerRow + i;
                if (itemIndex >= mDataSourceMgr.TotalItemCount)
                {
                    itemScript.mItemList[i].gameObject.SetActive(false);
                    continue;
                }
                ItemData itemData = mDataSourceMgr.GetItemDataByIndex(itemIndex);
                if (itemData != null)
                {
                    itemScript.mItemList[i].gameObject.SetActive(true);
                    itemScript.mItemList[i].SetItemData(itemData, itemIndex);
                }
                else
                {
                    itemScript.mItemList[i].gameObject.SetActive(false);
                }
            }
            return item;
        }    
    }
}
