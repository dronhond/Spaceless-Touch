using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    private  GridLayoutGroup gridLayoutObject; // 参考 GridLayout Group 的游戏对象
    public GameObject objectsToAdd; // 要添加的游戏对象数组
    

    private void Start()
    {
        gridLayoutObject = GetComponent<GridLayoutGroup>();
        
        for (int i = 0; i < 6; i++)
        {
            GameObject newObj = Instantiate(objectsToAdd, gridLayoutObject.transform); // 创建游戏对象副本
            newObj.transform.SetParent(gridLayoutObject.transform); // 设置游戏对象的父对象为 GridLayout Group
        }
    }

    public void AddObjects()
    {

        for (int i = 0; i < 6; i++)
        {
            GameObject newObj = Instantiate(objectsToAdd, gridLayoutObject.transform); // 创建游戏对象副本
            newObj.transform.SetParent(gridLayoutObject.transform); // 设置游戏对象的父对象为 GridLayout Group
        }
    }
}