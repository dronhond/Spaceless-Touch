using System;
using SpacelessTouch.Scripts.Controller;
using SpacelessTouch.Scripts.Manager;
using JKFrame;
using SpacelessTouch.Scripts.Common;
using SpacelessTouch.Scripts.GameScreen;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpacelessTouch.Scripts.System
{
    public static class GameSystem
    {
        public static readonly int[] fpsInts =
        {
            30,
            60,
            120
        };

        private static string bgmClipName;
        private static float bgmClipVolume;

        public static void InitPlayer()
        {
            Object.Instantiate(ResSystem.LoadAsset<GameObject>("Events/Unit/Player.prefab"))
                .GetComponent<PlayerController>();
            CameraController.Instance.SetFollowPlayer(true);
        }

        public static void ChangeScene(string scenePath, Action callBack = null)
        {
            MonoSystem.StopAllCoroutine();
            SceneSystem.LoadSceneAsyncByPath(scenePath, callBack);
        }

        public static void NextLevel(Action callBack = null)
        {
            ChangeLevel(++DataManager.Instance.CurrentLevel, callBack);
            DataManager.Instance.Save();
        }

        public static void ChangeLevel(int level, Action callBack = null)
        {
            DataManager.Instance.CurrentLevel = level;
            FullScreen.Instance.SetUnRayFull(true);
            SceneSystem.LoadSceneAsyncByPath($"Level/Level{level}.unity", delegate
            {
                FullScreen.Instance.SetUnRayFull(false);
                callBack?.Invoke();
            });
        }

        #region 音乐音效封装

        public static void PlayBGM(string clipName)
        {
            AudioSystem.PlayBGAudio(ResSystem.LoadAsset<AudioClip>($"BGM/{clipName}"));
        }

        public static void PlaySound(string soundName, float volume = 1f, float pitch = 1f, Action callBack = null)
        {
            AudioSystem.PlayOneShot(ResSystem.LoadAsset<AudioClip>($"Sound/{soundName}"), null, true,
                DataManager.Instance.SettingConfig.EffectVolume * volume, false, callBack);
        }

        #endregion
    }
}