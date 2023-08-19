using System;
using Bright.Serialization;
using cfg;
using SpacelessTouch.Scripts.Manager;
using JKFrame;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if UNITY_EDITOR || !UNITY_STANDALONE
#else
using System.IO;
#endif

namespace SpacelessTouch.Scripts.System
{
    public static class TableSystem
    {
        public static Tables Tables;
        public static Constant Constant;
        private static LanguageType _languageType;

        public static void Init()
        {
#if UNITY_EDITOR || !UNITY_STANDALONE
            Tables = new Tables(LoadByteBuf);
#else
            Tables = new Tables(LoadFileByteBuf);
#endif
            Constant = Tables.TbConstant.DataList[0];
            _languageType = DataManager.Instance.SettingConfig.LanguageType;
            SwitchLanguages(_languageType);
        }

#if UNITY_EDITOR || !UNITY_STANDALONE
        private static ByteBuf LoadByteBuf(string fileName)
        {
            return new ByteBuf(Addressables.LoadAssetAsync<TextAsset>(fileName).WaitForCompletion().bytes);
        }
#else
        private static ByteBuf LoadFileByteBuf(string file)
        {
            return new ByteBuf(File.ReadAllBytes($"{Application.dataPath}/Bytes/{file}.bytes"));
        }
#endif

        public static string GetLanguageString(string key)
        {
            switch (_languageType)
            {
                case LanguageType.SimplifiedChinese:
                    return Tables.TbLanguageString.Get(key).TextCn;
                case LanguageType.TraditionalChinese:
                    return Tables.TbLanguageString.Get(key).TextTw;
                case LanguageType.English:
                    return Tables.TbLanguageString.Get(key).TextEn;
            }

            throw new Exception();
        }

        public static string GetLanguageStringFormat(string key, params object[] args)
        {
            switch (_languageType)
            {
                case LanguageType.SimplifiedChinese:
                    return string.Format(Tables.TbLanguageString.Get(key).TextCn, args);
                case LanguageType.TraditionalChinese:
                    return string.Format(Tables.TbLanguageString.Get(key).TextTw, args);
                case LanguageType.English:
                    return string.Format(Tables.TbLanguageString.Get(key).TextEn, args);
            }

            throw new Exception();
        }

        public static void SwitchLanguages(LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.SimplifiedChinese:
                    Tables.TranslateText((key, originText) =>
                        Tables.TbLanguageString.GetOrDefault(key)?.TextCn ?? originText);
                    break;
                case LanguageType.TraditionalChinese:
                    Tables.TranslateText((key, originText) =>
                        Tables.TbLanguageString.GetOrDefault(key)?.TextTw ?? originText);
                    break;
                case LanguageType.English:
                    Tables.TranslateText((key, originText) =>
                        Tables.TbLanguageString.GetOrDefault(key)?.TextEn ?? originText);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null);
            }

            _languageType = languageType;
            JKEventSystem.EventTrigger("UpdateLanguage");
        }
    }
}