using DG.Tweening;
using JKFrame;
using SpacelessTouch.Scripts.GameScreen;
using SpacelessTouch.Scripts.System;
using UnityEngine;

[UIWindowData(typeof(UI_Team), true, "UI_Team", 0)]
public class UI_Team : UI_WindowBase
{
    [SerializeField] private RectTransform rectText;
    private bool _isShowBtnFull;
    public bool closePlayBGM;

    protected override void OnShow()
    {
        rectText.anchoredPosition = Vector2.zero;
        rectText.DOLocalMoveY(rectText.rect.height / 2, 5f).SetEase(Ease.Linear).OnComplete(SetReturnBtnFull);
    }

    protected override void OnClose()
    {
        rectText.DOKill();
        _isShowBtnFull = false;
    }

    public void SetReturnBtnFull()
    {
        if (_isShowBtnFull) return;
        _isShowBtnFull = true;
        FullScreen.Instance.SetBtnFull(OnClickClose);
    }

    private void OnClickClose()
    {
        UISystem.GetWindow<UI_StartGame>().ShowObjBtn();
        if (closePlayBGM)
        {
            closePlayBGM = false;
            GameSystem.PlayBGM("LOOP_Blasting Through the Sky.wav");
        }
        Close();
    }
}
