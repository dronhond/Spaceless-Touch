using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SpacelessTouch.Scripts.Common{
   public class GlitteryBox : Box
    {
		private Color TempColor;
		public float Interval = 1f;
		private void Start(){
			InvokeRepeating("NextColor",0f, Interval);
		}
		public void NextColor(){
			switch(ColorType){
				case BoxColorType.红色: Change(BoxColorType.黄色);break;
				case BoxColorType.黄色: Change(BoxColorType.蓝色);break;
				case BoxColorType.蓝色: Change(BoxColorType.红色);break;
			}
		}
		public void Change(BoxColorType color){
			switch(color){
				case BoxColorType.红色: TempColor = Color.red;break;
				case BoxColorType.黄色: TempColor = Color.yellow;break;
				case BoxColorType.蓝色: TempColor = Color.blue;break;
			}
			ColorType = color;
			GetComponent<SpriteRenderer>().color = TempColor;
		}
    }
	
}