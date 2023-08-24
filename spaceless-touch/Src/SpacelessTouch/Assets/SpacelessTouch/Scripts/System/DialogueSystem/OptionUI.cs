using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OptionUI : MonoBehaviour
{
    public Data data;
    public GameObject MainUI, Text;
    public bool IsSecond;
    public void CallMainUI() => MainUI.GetComponent<MainUI>().Call(data, IsSecond);
    public void Enable(string text, Data data)
    {
        gameObject.SetActive(true);
        Text.GetComponent<TextMeshProUGUI>().text = text;
        this.data = data;
    }
    public void Close() => gameObject.SetActive(false);
}
