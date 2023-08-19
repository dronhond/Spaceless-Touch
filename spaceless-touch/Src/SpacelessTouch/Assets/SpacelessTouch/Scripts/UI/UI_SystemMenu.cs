using JKFrame;
using SpacelessTouch.Scripts.GameScreen;
using SpacelessTouch.Scripts.Manager;
using SpacelessTouch.Scripts.System;
using UnityEngine;
using UnityEngine.UI;

[UIWindowData(typeof(UI_SystemMenu), true, "UI_SystemMenu", 0)]
public class UI_SystemMenu : UI_WindowBase
{
    [SerializeField] private Text textTitle;
    [SerializeField] private Text textRetry;
    [SerializeField] private Text textReSelect;
    [SerializeField] private Text textSetting;
    [SerializeField] private Text textAchievement;
    [SerializeField] private Text textReMainMenu;
    [SerializeField] private Text textExitGame;
    [SerializeField] private Text textVersion;

    public void OnClickRePlay()
    {
        GameManager.Instance.Retry();
        Close();
    }
    
    public void OnClickReSelect()
    {
        GameManager.isGaming = false;
        GameSystem.PlayBGM("LOOP_Blasting Through the Sky.wav");
        FullScreen.Instance.SetActiveMainBtn(false);
        UISystem.Show<UI_SelectLevel>();
        Close();
    }
    
    public void OnClickSetting()
    {
        UISystem.Show<UI_Setting>();
    }
    
    public void OnClickAchievement()
    {
    }
    
    public void OnClickReMainMenu()
    {
        GameManager.isGaming = false;
        GameSystem.PlayBGM("LOOP_Blasting Through the Sky.wav");
        FullScreen.Instance.SetActiveMainBtn(false);
        UISystem.Show<UI_StartGame>();
        Close();
    }
    
    public void OnClickExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    protected override void OnUpdateLanguage()
    {
        textTitle.text = TableSystem.GetLanguageString("systemMenu");
        textRetry.text = TableSystem.GetLanguageString("retry");
        textReSelect.text = TableSystem.GetLanguageString("returnSelectLevel");
        textSetting.text = TableSystem.GetLanguageString("gameSettings");
        textAchievement.text = TableSystem.GetLanguageString("achievement");
        textReMainMenu.text = TableSystem.GetLanguageString("returnMainMenu");
        textExitGame.text = TableSystem.GetLanguageString("exitGame");
        textVersion.text = TableSystem.GetLanguageStringFormat("versionFormat", Application.version);
    }
}
