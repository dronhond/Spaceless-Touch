using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Box class 重力箱子 南竹
 * Player:玩家对象
 * 可反重力物体:激活重力箱子后可以被反重力影响的对象
 * Distance:可激活重力箱子的距离
 */
namespace SpacelessTouch.Scripts.Common{
	public class GrivityBox : Box{
		public GameObject Player;
		[SerializeField] public List<Rigidbody2D> 可反重力物体;
		public float Distance = 3f;
		private void Update(){
			if (Vector2.Distance(Player.transform.position, transform.position) <= Distance && Input.GetKeyDown(KeyCode.Q)){
				Player.GetComponent<Rigidbody2D>().gravityScale *= -1; 
				foreach(Rigidbody2D Rb in 可反重力物体){
					Rb.gravityScale *= -1;
				}
			}
			var mPosX = transform.position.x;
            if (Link != null)
            {
                if (Mathf.Abs(transform.position.x - v2PosX) > 0.0001f)
                {
                    var target = Link.transform;
                    var pos = target.position;

                    pos = new Vector2(mPosX - v2PosX + pos.x, pos.y);
                    v2PosX = mPosX;
                    Link.v2PosX = pos.x;
                    target.position = pos;
                }
            }
            else if (Mathf.Abs(v2PosX - mPosX) > 0.0001f) v2PosX = transform.position.x;
		}
		   
	}
}