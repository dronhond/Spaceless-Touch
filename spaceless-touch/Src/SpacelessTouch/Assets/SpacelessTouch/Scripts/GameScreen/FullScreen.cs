using System;
using JKFrame;
using SpacelessTouch.Scripts.Manager;
using SpacelessTouch.Scripts.System;
// using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpacelessTouch.Scripts.GameScreen
{
    public class FullScreen : SingletonMono<FullScreen>
    {
        [SerializeField] private Button btnFull;
        [SerializeField] private GameObject objFull;
        [SerializeField] private GameObject objMainBtn;
        [SerializeField] private Text textLevel;
        private int _unRayFullCount;
    
        public void SetBtnFull(Action action)
        {
            btnFull.onClick.AddListener(delegate
            {
                btnFull.gameObject.SetActive(false);
                action?.Invoke();
                btnFull.onClick.RemoveAllListeners();
            });
            btnFull.gameObject.SetActive(true);
        }

        public void AddBtnFull(UnityAction action)
        {
            btnFull.onClick.AddListener(action);
            btnFull.gameObject.SetActive(true);
        }

        public void CloseBtnFull()
        {
            btnFull.gameObject.SetActive(false);
            btnFull.onClick.RemoveAllListeners();
        }

        public void SetUnRayFull(bool unRay)
        {
            if (objFull.activeInHierarchy == unRay && unRay)
            {
                _unRayFullCount++;
                return;
            }
            if (!unRay && _unRayFullCount > 0)
            {
                _unRayFullCount = Mathf.Max(_unRayFullCount - 1, 0);
                return;
            }
            objFull.SetActive(unRay);
        }

        public void SetActiveMainBtn(bool value)
        {
            objMainBtn.SetActive(value);
        }

        public void OnClickSystemMenu()
        {
            UISystem.Show<UI_SystemMenu>();
        }

        public void OnClickRetry()
        {
            GameManager.Instance.Retry();
        }

        public void UpdateShowLevel()
        {
            textLevel.text = TableSystem.GetLanguageStringFormat("levelFormat", DataManager.Instance.CurrentLevel);
        }
    }
}
