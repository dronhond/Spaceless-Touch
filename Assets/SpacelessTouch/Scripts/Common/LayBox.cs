using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/**
 * Box class 放置箱子系统 南竹
 * Instance:供其他脚本调用的接口
 * 选中箱子种类:当前放置系统中玩家选中的箱子对象
 * DetectTilemap:地面对应的Tilemap对象，供系统检测放置的位置是否为空
 * InlayMode:是否位于放置模式。
 */

public class LayBox : MonoBehaviour
{
	public static LayBox Instance;
	public GameObject LayingUI;
	public GameObject 选中箱子种类;
	public Tilemap DetectTilemap;
	public bool inLayMode = false;
	public bool InLayMode{
		get{ return inLayMode;}
		set{
            inLayMode = value;
            if(inLayMode){
				if(LayingUI != null) LayingUI.SetActive(true);
			}
			else{
				if(LayingUI != null) LayingUI.SetActive(false);
			}
		}
	}
	private void Awake(){
		Instance = this;
	}
    private void Update(){
		if (Input.GetKeyDown(KeyCode.P)){
			InLayMode = !InLayMode;
		}
		if (InLayMode && Input.GetMouseButtonDown(0)){
			Lay(选中箱子种类);
		}
		
	}
	public void Lay(GameObject 选中的箱子){
		if (DetectTilemap.GetTile(GetMousePosFloor()) == null) Instantiate(选中的箱子, GetMousePos(), Quaternion.identity);
		else Debug.Log($"当前位置存在Tile，不可放置。");
	}
	
	
	public Vector3Int GetMousePosFloor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 层级补偿;
        return Vector3Int.FloorToInt(mousePos);
    }
	public Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 层级补偿;
        return mousePos;
    }

}
