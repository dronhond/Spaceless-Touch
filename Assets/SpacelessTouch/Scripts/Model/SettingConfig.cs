using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;

namespace SpacelessTouch.Scripts.Model
{
    [Serializable]
    public class SettingConfig
    {
        public float GlobalVolume = 1;
        public float BGMVolume = 1;
        public float EffectVolume = 1;
        public bool IsMute;
        public LanguageType LanguageType = LanguageType.SimplifiedChinese;
        public int FPS;
        public bool Vibration;
        public bool CanSkipStory;
#if UNITY_EDITOR || UNITY_STANDALONE
        public int ResolutionRatio;
        public bool IsFullScreen = true;
#endif
        
        public SettingConfig()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            FPS = 2;
            var resolution = Screen.resolutions.Distinct(new ResolutionEqualityComparer()).ToArray();
            for (var i = 0; i < resolution.Length; i++)
            {
                if (resolution[i].width != Screen.currentResolution.width ||
                    resolution[i].height != Screen.currentResolution.height) continue;
                ResolutionRatio = i;
                break;
            }
#else
            FPS = 1;
#endif
        }

        public SettingConfig(SettingConfig settingConfig)
        {
            GlobalVolume = settingConfig.GlobalVolume;
            BGMVolume = settingConfig.BGMVolume;
            EffectVolume = settingConfig.EffectVolume;
            IsMute = settingConfig.IsMute;
            LanguageType = settingConfig.LanguageType;
            FPS = settingConfig.FPS;
            Vibration = settingConfig.Vibration;
            CanSkipStory = settingConfig.CanSkipStory;
#if UNITY_EDITOR || UNITY_STANDALONE
            ResolutionRatio = settingConfig.ResolutionRatio;
            IsFullScreen = settingConfig.IsFullScreen;
#endif
        }
    }
}

public class ResolutionEqualityComparer : IEqualityComparer<Resolution>
{
    public bool Equals(Resolution x, Resolution y)
    {
        return x.width == y.width && x.height == y.height;
    }

    public int GetHashCode(Resolution obj)
    {
        return obj.width.GetHashCode() ^ obj.height.GetHashCode();
    }
}