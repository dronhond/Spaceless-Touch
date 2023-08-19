using JKFrame;
using SpacelessTouch.Scripts.Manager;
using SpacelessTouch.Scripts.System;
using UnityEngine;

namespace MirMirror
{
    public class EndDetector : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;
            GameManager.Instance.Reset();
            if (!DataManager.Instance.IsMaxLevel)
                GameSystem.NextLevel();
            else
            {
                GameManager.isGaming = false;
                GameManager.Instance.Close();
                UISystem.Show<UI_StartGame>().HideAll();
                GameSystem.PlayBGM("LOOP_Feeling Lucky!.wav");
                UISystem.Show<UI_Team>().closePlayBGM = true;
            }
        }
    }
}
