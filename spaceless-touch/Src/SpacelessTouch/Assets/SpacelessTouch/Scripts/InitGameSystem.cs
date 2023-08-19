using SpacelessTouch.Scripts.Manager;
using SpacelessTouch.Scripts.System;
using UnityEngine;

namespace SpacelessTouch.Scripts
{
    public class InitGameSystem : MonoBehaviour
    {
        private void Awake()
        {
            if (TableSystem.Tables != null) return;
            Input.multiTouchEnabled = false;
            DataManager.Instance.LoadSettingConfig();
            TableSystem.Init();
            Destroy(this);
        }
    }
}
