using JKFrame;
using SpacelessTouch.Scripts.System;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        GameSystem.PlayBGM("LOOP_Blasting Through the Sky.wav");
        UISystem.Show<UI_StartGame>().ShowPress();
    }
}
