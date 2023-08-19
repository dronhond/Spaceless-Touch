using SpacelessTouch.Scripts.System;
using UnityEngine;
using JKFrame;
using SpacelessTouch.Scripts.GameScreen;
using UnityEngine.UI;

[UIWindowData(typeof(UI_StartGame), true, "UI_StartGame", 0)]
public class UI_StartGame : UI_WindowBase
{
    [SerializeField] private Text textStartGame;
    [SerializeField] private Text textSetting;
    [SerializeField] private Text textTeam;
    [SerializeField] private Text textExitGame;
    [SerializeField] private Text textPress;
    [SerializeField] private Text textVersion;
    [SerializeField] private GameObject objButtonsPanel;
    private bool hasAnyKey;

    public void ShowPress()
    {
        textPress.gameObject.SetActive(true);
        objButtonsPanel.SetActive(false);
        FullScreen.Instance.SetBtnFull(ShowObjBtn);
        hasAnyKey = true;
    }

    private void Update()
    {
        if (hasAnyKey && Input.anyKeyDown) ShowObjBtn();
    }

    public void HideAll()
    {
        textPress.gameObject.SetActive(false);
        textVersion.gameObject.SetActive(false);
        objButtonsPanel.SetActive(false);
    }

    public void ShowObjBtn()
    {
        if (hasAnyKey) hasAnyKey = false;
        textPress.gameObject.SetActive(false);
        textVersion.gameObject.SetActive(true);
        objButtonsPanel.SetActive(true);
    }

    public void OnClickStart()
    {
        //GameSystem.PlaySound("");
        // DataManager.Instance.Init(new DataInfo());
        // GameSystem.NextLevel(Close);
        Close();
        UISystem.Show<UI_SelectLevel>();
    }

    public void OnClickContinue()
    {
        //GameSystem.PlaySound("");
    }
    public void OnClickSetting()
    {
        //GameSystem.PlaySound("");
        UISystem.Show<UI_Setting>();
    }
    public void OnClickTeam()
    {
        //GameSystem.PlaySound("");
        HideAll();
        UISystem.Show<UI_Team>().SetReturnBtnFull();
    }
    public void OnClickQuit()
    {
        //GameSystem.PlaySound("");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    protected override void OnUpdateLanguage()
    {
        base.OnUpdateLanguage();
        textStartGame.text = TableSystem.GetLanguageString("startGame");
        textSetting.text = TableSystem.GetLanguageString("gameSettings");
        textTeam.text = TableSystem.GetLanguageString("productionTeam");
        textExitGame.text = TableSystem.GetLanguageString("exitGame");
        textPress.text = TableSystem.GetLanguageString("pressAnyKey");
        textVersion.text = TableSystem.GetLanguageStringFormat("versionFormat", Application.version);
    }
}
