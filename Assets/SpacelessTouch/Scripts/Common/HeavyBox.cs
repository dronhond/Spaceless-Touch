using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
/**
 * Box class 沉重箱子 南竹
 * 可被砸碎的Sprite:箱子砸到Sprite列表中的任一Sprite对应的Tile后删除该Tile
 */
namespace SpacelessTouch.Scripts.Common{
   public class HeavyBox : Box
    {
		[SerializeField] public List<Sprite> 可被砸碎的Sprite;
	    private void OnCollisionEnter2D(Collision2D coll){
			float Force = coll.relativeVelocity.magnitude;
            ContactPoint2D[] contacts = coll.contacts;
            Tilemap TheTilemap = coll.gameObject.GetComponent<Tilemap>();
            if(TheTilemap != null) foreach(ContactPoint2D contact in contacts){
                Vector3Int Center = Vector3Int.RoundToInt(new Vector3(contact.point.x, contact.point.y - 1, 0));
				Sprite Spr = TheTilemap.GetSprite(Center);
			    // print($"Sprite:{Spr} {可被砸碎的Sprite.Exists(e => e == Spr)} {Force}");
                if(Spr != null) if(Force > 5f && 可被砸碎的Sprite.Exists(e => e == Spr)){
                    TheTilemap.SetTile(Center, null);
			    }
		    }
	    }	
    }
}