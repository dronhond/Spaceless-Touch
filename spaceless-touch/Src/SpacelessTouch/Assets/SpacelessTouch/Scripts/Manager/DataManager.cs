using System.Linq;
using SpacelessTouch.Scripts.Model;
using SpacelessTouch.Scripts.System;
using JKFrame;
using UnityEngine;

namespace SpacelessTouch.Scripts.Manager
{
    public class DataManager : Singleton<DataManager>
    {
        public int CurrentLevel = 1;
        public DataInfo DataInfo;

        public SettingConfig SettingConfig;

        public void Save()
        {
            if (DataInfo.level < CurrentLevel)
                DataInfo.level = CurrentLevel;
            if (SaveSystem.GetSaveItem(0) == null) SaveSystem.CreateSaveItem();
            SaveSystem.SaveObject(DataInfo);
        }

        private void Load()
        {
            if (SaveSystem.GetSaveItem(0) == null)
            {
                DataInfo = new DataInfo();
                return;
            }
            DataInfo = SaveSystem.LoadObject<DataInfo>();
        }

        public bool IsMaxLevel => CurrentLevel >= TableSystem.Constant.MaxLevel;

        public void SaveSettingConfig()
        {
            SaveSystem.SaveSetting(SettingConfig);
        }

        public void LoadSettingConfig()
        {
            Load();
            var config = SaveSystem.LoadSetting<SettingConfig>();
            if (config != null)
                SettingConfig = config;
            else
            {
                SettingConfig = new SettingConfig();
                SaveSystem.SaveSetting(SettingConfig);
            }

            Application.targetFrameRate = GameSystem.fpsInts[SettingConfig.FPS];
            AudioSystem.GlobalVolume = SettingConfig.GlobalVolume;
            AudioSystem.BGVolume = SettingConfig.BGMVolume;
            AudioSystem.EffectVolume = SettingConfig.EffectVolume;
            AudioSystem.IsMute = SettingConfig.IsMute;
#if UNITY_EDITOR || UNITY_STANDALONE
            var resolutions = Screen.resolutions.Distinct(new ResolutionEqualityComparer()).ToArray();
            if (SettingConfig.ResolutionRatio < resolutions.Length) //链接屏幕数量或屏幕型号发生改变时数组越界的处理
            {
                var resolution = resolutions[SettingConfig.ResolutionRatio];
                Screen.SetResolution(resolution.width, resolution.height, SettingConfig.IsFullScreen);
            }
            else
            {
                for (var i = 0; i < resolutions.Length; i++)
                {
                    if (resolutions[i].width != Screen.currentResolution.width ||
                        resolutions[i].height != Screen.currentResolution.height) continue;
                    SettingConfig.ResolutionRatio = i;
                    SaveSystem.SaveSetting(SettingConfig);
                    break;
                }
            }
#endif
        }
    }
}