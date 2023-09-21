using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{   
    public class SimpleItem : MonoBehaviour
    {    
        public Text mNameText;          
        SimpleItemData mItemData;
        int mItemDataIndex = -1;
        
        public Image mImageSelect;
        Button mButton;  

        public int ItemIndex
        {
            get
            {
                return mItemDataIndex;
            }
            set
            {
                mItemDataIndex = value;
            }
        }      
         
        System.Action<int> mOnClickItemCallBack;
        public void Init(System.Action<int> OnClickItemCallBack = null)
        {
            mOnClickItemCallBack = OnClickItemCallBack;
            mButton = GetComponent<Button>();
            if(mButton != null)
            {
                mButton.onClick.AddListener(OnButtonClicked);
            }                
        }

        void OnButtonClicked()
        {
            if(mOnClickItemCallBack != null)
            {
                mOnClickItemCallBack(mItemDataIndex);
            }
        }

        public void SetItemData(SimpleItemData itemData, int itemIndex)
        {
            mItemData = itemData;
            mItemDataIndex = itemIndex;
            mNameText.text = itemData.mName;
        }

        public void SetItemSelected(bool isSelected)
        {
            if(mImageSelect != null)
            {
                mImageSelect.gameObject.SetActive(isSelected);
            }  
        }
    }    
}
