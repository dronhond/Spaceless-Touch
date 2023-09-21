using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
    public class TreeViewItemData<T> where T : ItemDataBase, new()
    {
        public string mName;
        //public string mIcon;
        public List<T> mChildItemDataList = new List<T>();

        public int ChildCount
        {
            get { return mChildItemDataList.Count; }
        }        

        public T AddNewItemChild(int index,int childIndex)
        {
            List<T> childItemDataList = mChildItemDataList;
            T childItemData = new T();
            childItemData.Init(childIndex, index);
            
            if (childIndex < 0)
            {
                childItemDataList.Insert(0, childItemData);
            }
            else if(childIndex >= childItemDataList.Count)
            {
                childItemDataList.Add(childItemData);                
            }
            else
            {
                childItemDataList.Insert(childIndex, childItemData);
                for(int i = childIndex; i<childItemDataList.Count; ++i)
                {
                    T tmpChildItemData = childItemDataList[i];
                    tmpChildItemData.OnIndexChanged(i, index);
                }
            }
            return childItemData;
        }    

        public T GetItemChildDataByIndex(int childIndex)
        {            
            return GetChild(childIndex);
        }

        public void RefreshItemDataList(int index, int childItemCount)
        {
            mName = "Item" + index;
            //mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 20));
            int childCount = childItemCount;
            for (int childIndex = 0; childIndex < childCount; ++childIndex)
            {
                T childItemData = new T();
                childItemData.Init(childIndex,index);                                
                AddChild(childItemData);
            }
        }    

        void AddChild(T data)
        {
            mChildItemDataList.Add(data);
        }

        T GetChild(int index)
        {
            if(index < 0 || index >= mChildItemDataList.Count)
            {
                return null;
            }
            return mChildItemDataList[index];
        }
    }
}