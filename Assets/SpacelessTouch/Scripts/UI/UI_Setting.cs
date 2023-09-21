using System;
using System.Linq;
using cfg;
using UnityEngine;
using UnityEngine.UI;
using JKFrame;
using SpacelessTouch.Scripts.Manager;
using SpacelessTouch.Scripts.Model;
using SpacelessTouch.Scripts.System;

[UIWindowData(typeof(UI_Setting), true, "UI_Setting", 1)]
public class UI_Setting : UI_WindowBase
{
    [SerializeField] private Text textTitle;
    [SerializeField] private Text textGlobal;
    [SerializeField] private Text textBg;
    [SerializeField] private Text textEffect;
    [SerializeField] private Text textMute;
    [SerializeField] private Text textLanguage;
    [SerializeField] private Text textFPS;
    [SerializeField] private Text textBtnSave;
    [SerializeField] private Text textBtnCancel;
    [SerializeField] private Slider sliderGlobalVolume;
    [SerializeField] private Slider sliderBgVolume;
    [SerializeField] private Slider sliderEffectVolume;
    [SerializeField] private Toggle toggleMute;
    [SerializeField] private Dropdown dropdownLanguage;
    [SerializeField] private Toggle[] togglesFPS;
#if UNITY_STANDALONE || UNITY_EDITOR
    private Dropdown dropdownResolutionRatio;
    private Toggle toggleFullScreen;
    private Text textResolutionRatio;
    private Text textFullScreenMode;
#endif

    private SettingConfig settingConfig;
    private SettingConfig settingTemp;

    public override void Init()
    {
        dropdownLanguage.AddOptions(TableSystem.Tables.TbLanguage.DataList.Select(language => language.Name).ToList());
#if UNITY_STANDALONE || UNITY_EDITOR
        textResolutionRatio = transform.Find("Background/Panel/ResolutionRatioPanel/DropdownResolutionRatio/TextResolutionRatio").GetComponent<Text>();
        textFullScreenMode = transform.Find("Background/Panel/TogglePanel/ToggleFullScreen/TextFullScreen").GetComponent<Text>();
        transform.Find("Background/Panel/ResolutionRatioPanel").gameObject.SetActive(true);
        transform.Find("Background/Panel/TogglePanel/ToggleFullScreen").gameObject.SetActive(true);
        dropdownResolutionRatio = transform.Find("Background/Panel/ResolutionRatioPanel/DropdownResolutionRatio").GetComponent<Dropdown>();
        toggleFullScreen = transform.Find("Background/Panel/TogglePanel/ToggleFullScreen").GetComponent<Toggle>();
        dropdownResolutionRatio.AddOptions(Screen.resolutions.Distinct(new ResolutionEqualityComparer()).ToArray().Select(resolution => $"{resolution.width}×{resolution.height}").ToList());
#endif
    }

    public override void ShowGeneralLogic()
    {
        settingConfig = DataManager.Instance.SettingConfig;
        settingTemp = new SettingConfig(settingConfig);
        base.ShowGeneralLogic();
    }

    protected override void OnShow()
    {
        toggleMute.isOn = settingConfig.IsMute;
        sliderGlobalVolume.value = settingConfig.GlobalVolume;
        sliderBgVolume.value = settingConfig.BGMVolume;
        sliderEffectVolume.value = settingConfig.EffectVolume;
        dropdownLanguage.value = (int)settingConfig.LanguageType;
        togglesFPS[settingConfig.FPS].isOn = true;
#if UNITY_STANDALONE || UNITY_EDITOR
        dropdownResolutionRatio.value = settingConfig.ResolutionRatio;
        toggleFullScreen.isOn = settingConfig.IsFullScreen;
#endif
        sliderGlobalVolume.onValueChanged.AddListener(SetAudioGlobalVolume);
        sliderBgVolume.onValueChanged.AddListener(SetAudioBgVolume);
        sliderEffectVolume.onValueChanged.AddListener(SetAudioEffectVolume);
        toggleMute.onValueChanged.AddListener(SetAudioMute);
        dropdownLanguage.onValueChanged.AddListener(SetLanguage);
        togglesFPS[0].onValueChanged.AddListener(SetFPS30);
        togglesFPS[1].onValueChanged.AddListener(SetFPS60);
        togglesFPS[2].onValueChanged.AddListener(SetFPS120);
#if UNITY_STANDALONE || UNITY_EDITOR
        dropdownResolutionRatio.onValueChanged.AddListener(SetResolutionRatio);
        toggleFullScreen.onValueChanged.AddListener(SetFullScreen);
#endif
    }
    
    bool HasChange =>
        Math.Abs(settingTemp.GlobalVolume - settingConfig.GlobalVolume) > 0.01f || Math.Abs(settingTemp.BGMVolume - settingConfig.BGMVolume) > 0.01f ||
        Math.Abs(settingTemp.EffectVolume - settingConfig.EffectVolume) > 0.01f || settingTemp.IsMute != settingConfig.IsMute || 
        settingTemp.LanguageType != settingConfig.LanguageType || settingTemp.FPS != settingConfig.FPS
#if UNITY_STANDALONE || UNITY_EDITOR
        || settingTemp.ResolutionRatio != settingConfig.ResolutionRatio || settingTemp.IsFullScreen != settingConfig.IsFullScreen
#endif
        ;

    private void SetAudioGlobalVolume(float volume)
    {
        AudioSystem.GlobalVolume = volume;
        settingTemp.GlobalVolume = volume;
    }

    private void SetAudioBgVolume(float volume)
    {
        AudioSystem.BGVolume = volume;
        settingTemp.BGMVolume = volume;
    }

    private void SetAudioEffectVolume(float volume)
    {
        AudioSystem.EffectVolume = volume;
        settingTemp.EffectVolume = volume;
    }
    
    private void SetAudioMute(bool isMute)
    {
        AudioSystem.IsMute = isMute;
        settingTemp.IsMute = isMute;
    }

    private void SetLanguage(int index)
    {
        settingTemp.LanguageType = (LanguageType)index;
        TableSystem.SwitchLanguages(settingTemp.LanguageType);
    }

    private void SetFPS30(bool isOn)
    {
        if (!isOn) return;
        Application.targetFrameRate = 30;
        settingTemp.FPS = 0;
    }

    private void SetFPS60(bool isOn)
    {
        if (!isOn) return;
        Application.targetFrameRate = 60;
        settingTemp.FPS = 1;
    }

    private void SetFPS120(bool isOn)
    {
        if (!isOn) return;
        Application.targetFrameRate = 120;
        settingTemp.FPS = 2;
    }
    
#if UNITY_STANDALONE || UNITY_EDITOR
    private void SetResolutionRatio(int index)
    {
        var resolution = Screen.resolutions.Distinct(new ResolutionEqualityComparer()).ToArray()[index];
        settingTemp.ResolutionRatio = index;
        Screen.SetResolution(resolution.width, resolution.height, settingTemp.IsFullScreen);
    }

    private void SetFullScreen(bool isOn)
    {
        settingTemp.IsFullScreen = isOn;
        var resolution = Screen.resolutions.Distinct(new ResolutionEqualityComparer()).ToArray()[settingTemp.ResolutionRatio];
        Screen.SetResolution(resolution.width, resolution.height, settingTemp.IsFullScreen);
    }
#endif

    private void ResetAllSetting()
    {
        AudioSystem.IsMute = settingConfig.IsMute;
        AudioSystem.GlobalVolume = settingConfig.GlobalVolume;
        AudioSystem.BGVolume = settingConfig.BGMVolume;
        AudioSystem.EffectVolume = settingConfig.EffectVolume;
        AudioSystem.IsMute = settingConfig.IsMute;
        TableSystem.SwitchLanguages(settingConfig.LanguageType);
#if UNITY_STANDALONE || UNITY_EDITOR
        var resolution = Screen.resolutions.Distinct(new ResolutionEqualityComparer()).ToArray()[settingConfig.ResolutionRatio];
        Screen.SetResolution(resolution.width, resolution.height, settingConfig.IsFullScreen);
#endif
    }

    public void OnClickSave()
    {
        if (HasChange)
        {
            DataManager.Instance.SettingConfig = settingTemp;
            DataManager.Instance.SaveSettingConfig();
        }
        Close();
    }

    public void OnClickCancel()
    {
        if (HasChange)
        {
            UISystem.Show<UI_Tip>().Show("tipSettingChanged", TipType.Confirm, delegate
            {
                DataManager.Instance.SettingConfig = settingTemp;
                DataManager.Instance.SaveSettingConfig();
                Close();
            }, delegate
            {
                ResetAllSetting();
                Close();
            }, "tip", "save");
            return;
        }
        Close();
    }

    protected override void OnClose()
    {
        settingConfig = null;
        settingTemp = null;
        sliderGlobalVolume.onValueChanged.RemoveListener(SetAudioGlobalVolume);
        sliderBgVolume.onValueChanged.RemoveListener(SetAudioBgVolume);
        sliderEffectVolume.onValueChanged.RemoveListener(SetAudioEffectVolume);
        toggleMute.onValueChanged.RemoveListener(SetAudioMute);
        dropdownLanguage.onValueChanged.RemoveListener(SetLanguage);
        togglesFPS[0].onValueChanged.RemoveListener(SetFPS30);
        togglesFPS[1].onValueChanged.RemoveListener(SetFPS60);
        togglesFPS[2].onValueChanged.RemoveListener(SetFPS120);
#if UNITY_STANDALONE || UNITY_EDITOR
        dropdownResolutionRatio.onValueChanged.RemoveListener(SetResolutionRatio);
        toggleFullScreen.onValueChanged.RemoveListener(SetFullScreen);
#endif
    }

    protected override void OnUpdateLanguage()
    {
        textTitle.text = TableSystem.GetLanguageString("gameSettings");
        textGlobal.text = TableSystem.GetLanguageString("volumeGlobal");
        textBg.text = TableSystem.GetLanguageString("music");
        textEffect.text = TableSystem.GetLanguageString("soundEffect");
        textMute.text = TableSystem.GetLanguageString("mute");
        textLanguage.text = TableSystem.GetLanguageString("language");
        textFPS.text = TableSystem.GetLanguageString("fps");
        textBtnSave.text = TableSystem.GetLanguageString("save");
        textBtnCancel.text = TableSystem.GetLanguageString("cancel");
#if UNITY_STANDALONE || UNITY_EDITOR
        textResolutionRatio.text = TableSystem.GetLanguageString("resolutionRatio");
        textFullScreenMode.text = TableSystem.GetLanguageString("fullScreenMode");
#endif
    }
}