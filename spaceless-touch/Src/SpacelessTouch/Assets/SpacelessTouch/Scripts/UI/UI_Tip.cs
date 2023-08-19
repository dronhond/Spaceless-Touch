using System;
using DG.Tweening;
using JKFrame;
using SpacelessTouch.Scripts.System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum TipType
{
    /// <summary>
    /// Information Dialog with OK button
    /// </summary>
    Information = 1,

    /// <summary>
    /// Confirm Dialog whit OK and Cancel buttons
    /// </summary>
    Confirm = 2,

    /// <summary>
    /// Warning Dialog whit OK and Cancel buttons
    /// </summary>
    Warning = 3,

    /// <summary>
    /// Error Dialog with OK buttons
    /// </summary>
    Error = 4
}

[UIWindowData(typeof(UI_Tip), true, "UI_Tip", 1)]
public class UI_Tip : UI_WindowBase
{
    [SerializeField] private CanvasGroup cGroup;
    [SerializeField] private Text textTitle;
    [SerializeField] private Text textMessage;
    [SerializeField] private GameObject objBtnConfirm;
    [SerializeField] private GameObject objBtnError;
    [SerializeField] private GameObject objBtnCancel;
    [SerializeField] private Text textBtnConfirm;
    [SerializeField] private Text textBtnError;
    [SerializeField] private Text textBtnCancel;

    private event UnityAction OnYesAction;
    private event UnityAction OnNoAction;
    private event UnityAction OnCloseAction;

    public override void Init()
    {
        cGroup.alpha = 0;
    }

    public UI_Tip Show(string messageContentKey, TipType type = TipType.Information, UnityAction yesOnClick = null,
        UnityAction noOnClick = null, string textTitleContentKey = "tip",
        string textBtnYesContentKey = "confirm", string textBtnNoContentKey = "cancel", UnityAction closeOnClick = null)
    {
        ShowTip(TableSystem.GetLanguageString(messageContentKey), TableSystem.GetLanguageString(textTitleContentKey),
            type, yesOnClick,
            noOnClick, textBtnYesContentKey, "cancel", closeOnClick);
        return this;
    }

    public UI_Tip ShowTip(string messageContent, string textTitleContent, TipType type = TipType.Information,
        UnityAction yesOnClick = null,
        UnityAction noOnClick = null,
        string textBtnYesContentKey = "confirm", string textBtnNoContentKey = "cancel", UnityAction closeOnClick = null)
    {
        var hasCancel = type is TipType.Confirm or TipType.Warning;
        textTitle.text = textTitleContent;
        switch (type)
        {
            case TipType.Information:
                textBtnConfirm.text = TableSystem.GetLanguageString(textBtnYesContentKey);
                break;
            case TipType.Confirm:
                textBtnConfirm.text = TableSystem.GetLanguageString(textBtnYesContentKey);
                break;
            case TipType.Warning:
                textBtnError.text = TableSystem.GetLanguageString(textBtnYesContentKey);
                break;
            case TipType.Error:
                textBtnError.text = TableSystem.GetLanguageString(textBtnYesContentKey);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        if (hasCancel) textBtnCancel.text = TableSystem.GetLanguageString(textBtnNoContentKey);

        objBtnConfirm.SetActive(type == TipType.Confirm || type == TipType.Information);
        //objBtnError.SetActive(type == TipType.Warning || type == TipType.Error);
        objBtnCancel.SetActive(hasCancel);

        textMessage.text = messageContent;
        if (yesOnClick != null) OnYesAction = yesOnClick;
        if (noOnClick != null) OnNoAction = noOnClick;
        if (closeOnClick != null) OnCloseAction = closeOnClick;
        cGroup.DOFade(1, 0.5f);
        return this;
    }

    public void Format(params object[] args)
    {
        textMessage.text = string.Format(textMessage.text, args);
    }

    protected override void OnClose()
    {
        cGroup.DOFade(0, 0.5f);
    }

    public void OnClickYes()
    {
        Close();
        OnYesAction?.Invoke();
        ResetAction();
    }

    public void OnClickNo()
    {
        Close();
        OnNoAction?.Invoke();
        ResetAction();
    }

    public void OnClickClose()
    {
        Close();
        if (OnCloseAction != null)
        {
            OnCloseAction.Invoke();
            return;
        }

        OnNoAction?.Invoke();
    }

    private void ResetAction()
    {
        OnYesAction = null;
        OnNoAction = null;
        OnCloseAction = null;
    }
}