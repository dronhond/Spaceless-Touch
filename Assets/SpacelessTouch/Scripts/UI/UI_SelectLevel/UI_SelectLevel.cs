using JKFrame;
using SpacelessTouch.Scripts.System;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

[UIWindowData(typeof(UI_SelectLevel), true, "UI_SelectLevel", 0)]
public class UI_SelectLevel : UI_WindowBase
{
    [SerializeField] private Text textTitle;
    [SerializeField] private LoopListView2 loopListView;
    private int _MaxLevel;

    public override void Init()
    {
        _MaxLevel = TableSystem.Constant.MaxLevel;
        loopListView.InitListView(_MaxLevel, OnGetItemByIndex);
    }

    protected override void OnShow()
    {
        loopListView.MovePanelToItemIndex(0, 0);
    }

    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= _MaxLevel)
            return null;
        var item = listView.NewListViewItem("ItemPrefab");
        var itemSelectLevel = item.GetComponent<Item_SelectLevel>();
        if (item.IsInitHandlerCalled == false) item.IsInitHandlerCalled = true;
        itemSelectLevel.SetItemData(index + 1);
        return item;
    }

    // private void LateUpdate()
    // {
    //     loopListView.UpdateAllShownItemSnapData();
    //     var count = loopListView.ShownItemCount;
    //     for (int i = 0; i < count; ++i)
    //     {
    //         var item = loopListView.GetShownItemByIndex(i);
    //         var itemSelectLevel = item.GetComponent<Item_SelectLevel>();
    //         var scale = 1 - Mathf.Abs(item.DistanceWithViewPortSnapCenter) / 800f;
    //         scale = Mathf.Clamp(scale, 0.4f, 1);
    //         itemSelectLevel.GetComponent<CanvasGroup>().alpha = scale;
    //         itemSelectLevel.transform.localScale = new Vector3(scale, scale, 1);
    //     }
    // }

    public void OnClickReturn()
    {
        UISystem.Show<UI_StartGame>();
        Close();
    }

    public void OnValueLevelChanged(string text)
    {
        if (!int.TryParse(text, out var itemIndex))
            return;
        if (itemIndex < 0 || itemIndex >= _MaxLevel)
            return;
        if (itemIndex < 2)
            loopListView.MovePanelToItemIndex(itemIndex, 0);
        else
            loopListView.MovePanelToItemIndex(itemIndex - 2, 0);
        loopListView.FinishSnapImmediately();
    }

    protected override void OnUpdateLanguage()
    {
        textTitle.text = TableSystem.GetLanguageString("selectLevel");
    }
}