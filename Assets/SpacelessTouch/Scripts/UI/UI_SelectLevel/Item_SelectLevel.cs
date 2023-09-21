using JKFrame;
using SpacelessTouch.Scripts.Manager;
using SpacelessTouch.Scripts.System;
using UnityEngine;
using UnityEngine.UI;

public class Item_SelectLevel : MonoBehaviour
{
    [SerializeField] private Text textLevel;
    [SerializeField] private GameObject objLock;
    private int _level;

    public void SetItemData(int level)
    {
        _level = level;
        textLevel.text = TableSystem.GetLanguageStringFormat("levelFormat", level);
        objLock.SetActive(DataManager.Instance.DataInfo.level < level);
    }

    public void OnClickEnterLevel()
    {
        GameSystem.ChangeLevel(_level, delegate
        {
            GameManager.Instance.Init();
            UISystem.Close<UI_SelectLevel>();
        });
    }
}
