using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
    public class DescArray
    {
        public static string[] mStrList = 
        {
            "Item count infinite.",           
            "Item in different height.",
            "Item in different width.",
            "Item with different prefab.",
            "Item with unknown size at init time.",
            "Item various padding.",
            "Item scroll with an offset.",
            "Item count changed at runtime.",
            "Item height width changed at runtime.",
            "Item width changed at runtime.",
            "Item snapped to any position.",
            "Item scroll looping.",
            "Item refreshed and reloaded.",
            "Item cached using pool.",           
            "Item count with high performance.",
            "Item recycled efficiently.",
        };

    }

    public class ItemDataBase
    {
        public virtual void Init(int index)
        {
        }

        public virtual void Init(int index, int parentIndex)
        {
        }
       
        public virtual void OnIndexChanged(int index) 
        {
        }   

        public virtual void OnIndexChanged(int index, int parentIndex) 
        {
        }    

        public virtual bool IsFilterMatched(string filterStr)
        {
            return true;
        }

    }

    public class ItemData : ItemDataBase
    {
        public int mIndex;
        public int mParentIndex = -1; //Only used by treeview.
        public string mName;
        public string mDesc;
        public string mDescExtend;
        public string mIcon;
        public int mStarCount;
        public bool mChecked;
        public bool mIsExpand;
        public float mSliderValue;
        public string mInputFieldText;
        public string mContentImage;        

        public override void Init(int index)
        {
            mIndex = index;
            mName = "Item" + index;
            int count = DescArray.mStrList.Length;
            mDescExtend = DescArray.mStrList[Random.Range(0, 99) % count];                
            mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 20));
            mStarCount = Random.Range(0, 6);
            int fileSize = Random.Range(20, 999);
            mDesc = fileSize.ToString()+"KB";
            mChecked = false;
            mIsExpand = false;      
            mSliderValue = Random.Range(0.1f, 0.9f);          
            mInputFieldText = "";       
            mContentImage = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 20));
        }

        public override void Init(int index, int parentIndex = -1)
        {
            mIndex = index;
            mParentIndex = parentIndex;            
            if(parentIndex != -1)
            {
                mName = "Item" + parentIndex + ": Child"+ index;
            }
            else
            {
                mName = "Item" + index;
            }
            
            int count = DescArray.mStrList.Length;
            mDescExtend = DescArray.mStrList[Random.Range(0, 99) % count];                
            mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 20));
            mStarCount = Random.Range(0, 6);
            int fileSize = Random.Range(20, 999);
            mDesc = fileSize.ToString()+"KB";
        }       

        public override void OnIndexChanged(int index)
        {
            mIndex = index;
            mName = "Item" + index;
        }

        public override void OnIndexChanged(int index, int parentIndex = -1)
        {
            mIndex = index;
            if(mParentIndex != -1)
            {
                mName = "Item" + mParentIndex + ": Child"+ index;
            }
            else
            {
                mName = "Item" + index;
            }
        }        

        public override bool IsFilterMatched(string filterStr)
        {
            return mName.Contains(filterStr);
        }
    }

    public class SimpleItemData : ItemDataBase
    {
        public int mIndex;
        public int mParentIndex = -1; //Only used by treeview.
        public string mName;
        
        public override void Init(int index)
        {
            mIndex = index;
            mName = "Item" + index;
        }

        public override void Init(int index, int parentIndex = -1)
        {
            mIndex = index;
            mParentIndex = parentIndex;            
            if(parentIndex != -1)
            {
                mName = "Item" + parentIndex + ": Child"+ index;
            }
            else
            {
                mName = "Item" + index;
            }           
        }        

        public override void OnIndexChanged(int index)
        {
            mIndex = index;
            mName = "Item" + index;
        }

        public override void OnIndexChanged(int index, int parentIndex = -1)
        {
            mIndex = index;
            if(mParentIndex != -1)
            {
                mName = "Item" + mParentIndex + ": Child"+ index;
            }
            else
            {
                mName = "Item" + index;
            }
        }

        public override bool IsFilterMatched(string filterStr)
        {
            return mName.Contains(filterStr);
        }
    }

    public class NestedItemData : ItemDataBase
    {
        public string mName;
        public int mIndex;       
        public DataSourceMgr<ItemData> mDataSourceMgr;
        int mNestedCount = 100;
        public override void Init(int index)
        {
            mIndex = index;
            mName = "Item" + index;
            mDataSourceMgr = new DataSourceMgr<ItemData>(mNestedCount);
        }

        public override void OnIndexChanged(int index)
        {
            mIndex = index;
            mName = "Item" + index;
        }
    }   
}