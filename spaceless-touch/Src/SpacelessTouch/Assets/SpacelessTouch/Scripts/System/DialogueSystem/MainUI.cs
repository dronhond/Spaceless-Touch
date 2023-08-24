using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Data.Common;
using UnityEngine.Networking;

public class MainUI : MonoBehaviour
{
    public GameObject Background, Text, Name, Image, Option1, Option2;
    public Data data;
    public string TextFile, SpriteFile, NameFile, OptionFile, OptionInfoFile;
    public int MinUpdateChar, CharPrintSpeed;
    private Sprite[] Sprites;
    private int[] count;
    private string[] texts, nameTexts, option1Texts, option2Texts;
    private int line = -1, imageIndex = 0, imageCount = 0, nameIndex = 0, nameCount = 0, optionIndex = 0, charIndex = 0, charPrintCount = 0;
    private string[][,] optionInfoTexts;
    private int[,] optionLineIndex;

    public void Enable() => gameObject.SetActive(true);
    public void Enable(Data data)
    {
        gameObject.SetActive(true);
        CopyData(data);
        this.data = data;
    }
    public void Enable(string textFile, string nameFile, Sprite[] sprites)
    {
        gameObject.SetActive(true);
        TextFile = textFile;
        NameFile = nameFile;
        Sprites = sprites;
    }

    private void OnEnable()
    {
        Background.SetActive(true);
        Text.SetActive(true);
        Name.SetActive(true);
        Image.SetActive(true);
        texts = System.IO.File.Exists(TextFile) ? System.IO.File.ReadAllLines(TextFile) : new string[0];
        nameTexts = System.IO.File.Exists(NameFile) ? System.IO.File.ReadAllLines(NameFile) : new string[0];
        if (OptionFile != "")
        {
            string[] _optionTexts = System.IO.File.ReadAllLines(OptionFile);
            option1Texts = new string[_optionTexts.Length];
            option2Texts = new string[_optionTexts.Length];
            for (int i = 0; i < _optionTexts.Length; i++)
            {
                option1Texts[i] = _optionTexts[i][.._optionTexts[i].IndexOf('`')];
                option2Texts[i] = _optionTexts[i][(_optionTexts[i].IndexOf('`') + 1)..];
            }
        }
        optionInfoTexts = new string[System.IO.File.ReadAllLines(OptionInfoFile).Length][,];
        optionLineIndex = new int[2, optionInfoTexts.Length];
        for (int i = 0; i < optionInfoTexts.Length; i++)
        {
            optionInfoTexts[i] = new string[,]
            {
                {
                    System.IO.File.ReadAllLines(OptionInfoFile)[i][..System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`')],

                    System.IO.File.ReadAllLines(OptionInfoFile)[i][(System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`') + 1)
                    ..
                    System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`', System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`') + 1)]
                },
                {
                    System.IO.File.ReadAllLines(OptionInfoFile)[i][(System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`') + 1) + 1)
                    ..
                    System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`') + 1) + 1)],

                    System.IO.File.ReadAllLines(OptionInfoFile)[i][(System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`') + 1) + 1) + 1)
                    ..
                    (System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`') + 1) + 1) + 1))]
                }
            };
            optionLineIndex[0, i] = Convert.ToInt32(System.IO.File.ReadAllLines(OptionInfoFile)[i][(System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`') + 1) + 1) + 1) + 1)..]);
            optionLineIndex[1, i] = Convert.ToInt32(System.IO.File.ReadAllLines(OptionInfoFile)[i][(System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`'
                    , System.IO.File.ReadAllLines(OptionInfoFile)[i].IndexOf('`') + 1) + 1) + 1) + 1) + 1)..]);
        }
        Sprites = new Sprite[System.IO.File.ReadAllLines(SpriteFile).Length];
        count = new int[System.IO.File.ReadAllLines(SpriteFile).Length];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i] = Sprite.Create(DownloadHandlerTexture.GetContent(new UnityWebRequest(System.IO.File.ReadAllLines(SpriteFile)[i][..System.IO.File.ReadAllLines(SpriteFile)[i].IndexOf(',')])), new Rect(0, 0, DownloadHandlerTexture.GetContent(new UnityWebRequest(System.IO.File.ReadAllLines(SpriteFile)[i][..System.IO.File.ReadAllLines(SpriteFile)[i].IndexOf(',')])).width, DownloadHandlerTexture.GetContent(new UnityWebRequest(System.IO.File.ReadAllLines(SpriteFile)[i][..System.IO.File.ReadAllLines(SpriteFile)[i].IndexOf(',')])).height), new Vector2(0.5f, 0.5f));
            count[i] = Convert.ToInt32(System.IO.File.ReadAllLines(SpriteFile)[i][(System.IO.File.ReadAllLines(SpriteFile)[i].IndexOf(',') + 1)..]);
        }

        Next(false);
    }

    private void Close()
    {
        OnClose();
        Background.SetActive(false);
        Text.SetActive(false);
        Name.SetActive(false);
        Image.SetActive(false);
        gameObject.SetActive(false);
    }
    private void OnClose()
    {
        line = -1;
        imageIndex = 0;
        imageCount = 0;
        nameIndex = 0;
        nameCount = 0;
        charIndex = 0;
        charPrintCount = 0;
    }
    public void Call(Data data, bool IsSecond)
    {
        optionIndex = optionLineIndex[IsSecond ? 1 : 0, optionIndex - 1];
        this.data = data;
        CopyData();
        Option1.SetActive(false);
        Option2.SetActive(false);
        OnClose();
        Enable(data);
        OnEnable();
    }

    public void Next()
    {
        if ((texts.Length > 0 && line >= 0 && (texts[line].Length < MinUpdateChar || charIndex >= MinUpdateChar) && charIndex == texts[line].Length) || line == texts.Length)
        {
            if (line < texts.Length - 1)
            {
                Text.GetComponent<TextMeshProUGUI>().text = "";
                charIndex = 0;
                line++;
            }
            else
            {
                Text.GetComponent<TextMeshProUGUI>().text = texts[line];
                if (OptionFile != "")
                {
                    if (!Option1.activeSelf)
                    {
                        if (optionIndex < option1Texts.Length)
                        {
                            Option1.GetComponent<OptionUI>().Enable(option1Texts[optionIndex], new Data(option1Texts, optionInfoTexts[optionIndex][0, 0], optionInfoTexts[optionIndex][0, 1]));
                            Option2.GetComponent<OptionUI>().Enable(option2Texts[optionIndex], new Data(option2Texts, optionInfoTexts[optionIndex][1, 0], optionInfoTexts[optionIndex][1, 1]));
                        }
                        else
                            Close();
                    }
                }
                else
                    Close();
            }
            if (nameIndex < nameTexts.Length)
            {
                if (texts == new string[0])
                    Name.GetComponent<TextMeshProUGUI>().text = nameTexts[nameIndex][..nameTexts[nameIndex].IndexOf('`')];
                if (nameCount == Convert.ToInt32(nameTexts[nameIndex][(nameTexts[nameIndex].IndexOf('`') + 1)..]))
                    if (nameIndex < nameTexts.Length)
                    {
                        nameIndex++;
                        Name.GetComponent<TextMeshProUGUI>().text = nameTexts[nameIndex - 1][..nameTexts[nameIndex - 1].IndexOf('`')];
                        nameCount = 0;
                    }
                    else
                        goto ImageUpdate;
                nameCount++;
            }
            else if (nameCount < Convert.ToInt32(nameTexts[nameIndex - 1][(nameTexts[nameIndex - 1].IndexOf('`') + 1)..]))
            {
                Name.GetComponent<TextMeshProUGUI>().text = nameTexts[nameIndex - 1][..nameTexts[nameIndex - 1].IndexOf('`')];
                nameCount++;
            }
        ImageUpdate:
            if (imageIndex < Sprites?.Length)
            {
                Image.GetComponent<Image>().sprite = Sprites[imageIndex];
                if (imageCount == count[imageIndex])
                    if (imageIndex < Sprites.Length)
                    {
                        imageIndex++;
                        Image.GetComponent<Image>().sprite = Sprites[imageIndex - (imageIndex == count.Length ? 1 : 0)];
                        imageCount = 0;
                    }
                imageCount++;
            }
            else if (imageCount < ((count != null && count.Length > 0) ? count[imageIndex - (imageIndex == count.Length ? 1 : 0)] : 0))
            {
                Image.GetComponent<Image>().sprite = Sprites[imageIndex - (imageIndex == count.Length ? 1 : 0)];
                imageCount++;
            }
        }
        else if (charIndex >= MinUpdateChar)
        {
            Text.GetComponent<TextMeshProUGUI>().text = texts[line];
            charIndex = texts[line].Length;
        }
    }
    public void Next(bool UseCondition)
    {
        if (!UseCondition || (texts.Length > 0 && line >= 0 && (texts[line].Length < MinUpdateChar || charIndex >= MinUpdateChar) && charIndex == texts[line].Length) || line == texts.Length)
        {
            if (line < texts.Length - 1)
            {
                Text.GetComponent<TextMeshProUGUI>().text = "";
                charIndex = 0;
                line++;
            }
            else
            {
                if (texts == new string[0])
                    Text.GetComponent<TextMeshProUGUI>().text = texts[line];
                if (OptionFile != "")
                {
                    if (!Option1.activeSelf)
                    {
                        if (optionIndex < option1Texts.Length)
                        {
                            Option1.GetComponent<OptionUI>().Enable(option1Texts[optionIndex], new Data(option1Texts, optionInfoTexts[optionIndex][0, 0], optionInfoTexts[optionIndex][0, 1]));
                            Option2.GetComponent<OptionUI>().Enable(option2Texts[optionIndex], new Data(option2Texts, optionInfoTexts[optionIndex][1, 0], optionInfoTexts[optionIndex][1, 1]));
                        }
                        else
                            Close();
                    }
                }
                else
                    Close();
            }
            if (nameIndex < nameTexts.Length)
            {
                Name.GetComponent<TextMeshProUGUI>().text = nameTexts[nameIndex][..nameTexts[nameIndex].IndexOf('`')];
                if (nameCount == Convert.ToInt32(nameTexts[nameIndex][(nameTexts[nameIndex].IndexOf('`') + 1)..]))
                    if (nameIndex < nameTexts.Length)
                    {
                        nameIndex++;
                        Name.GetComponent<TextMeshProUGUI>().text = nameTexts[nameIndex - 1][..nameTexts[nameIndex - 1].IndexOf('`')];
                        nameCount = 0;
                    }
                    else
                        goto ImageUpdate;
                nameCount++;
            }
            else if (nameCount < Convert.ToInt32(nameTexts[nameIndex - 1][(nameTexts[nameIndex - 1].IndexOf('`') + 1)..]))
            {
                Name.GetComponent<TextMeshProUGUI>().text = nameTexts[nameIndex - 1][..nameTexts[nameIndex - 1].IndexOf('`')];
                nameCount++;
            }
        ImageUpdate:
            if (imageIndex < Sprites?.Length)
            {
                Image.GetComponent<Image>().sprite = Sprites[imageIndex];
                if (imageCount == count[imageIndex])
                    if (imageIndex < Sprites.Length)
                    {
                        imageIndex++;
                        Image.GetComponent<Image>().sprite = Sprites[imageIndex - (imageIndex == count.Length ? 1 : 0)];
                        imageCount = 0;
                    }
                imageCount++;
            }
            else if (imageCount < ((count != null && count.Length > 0) ? count[imageIndex - (imageIndex == count.Length ? 1 : 0)] : 0))
            {
                Image.GetComponent<Image>().sprite = Sprites[imageIndex - (imageIndex == count.Length ? 1 : 0)];
                imageCount++;
            }
        }
        else if (charIndex >= MinUpdateChar)
        {
            Text.GetComponent<TextMeshProUGUI>().text = texts[line];
            charIndex = texts[line].Length;
        }
    }
    public void CopyData()
    {
        TextFile = data.MainUITextFile;
        NameFile = data.NameFile;
        SpriteFile = data.SpriteFiles;
        count = data.SpriteCount;
        if (TextFile != "")
            texts = System.IO.File.ReadAllLines(TextFile);
        if (NameFile != "")
            nameTexts = System.IO.File.ReadAllLines(NameFile);
    }
    public void CopyData(Data data)
    {
        TextFile = data.MainUITextFile;
        NameFile = data.NameFile;
        SpriteFile = data.SpriteFiles;
        count = data.SpriteCount;
        if (TextFile != "")
            texts = System.IO.File.ReadAllLines(TextFile);
        if (NameFile != "")
            nameTexts = System.IO.File.ReadAllLines(NameFile);
    }
    private void Update()
    {
        if (line >= 0 && line < texts.Length)
        {
            charPrintCount++;
            if (charIndex < texts[line].Length && charPrintCount % CharPrintSpeed == 0)
            {
                Text.GetComponent<TextMeshProUGUI>().text += texts[line][charIndex];
                charIndex++;
            }
        }
    }
}
public class Data
{
    public string MainUITextFile;
    public string NameFile;
    public string SpriteFiles;
    public int[] SpriteCount;
    protected string TextFile;
    public string[] texts;
    public Data(string[] texts, string mainUITextFile = "", string nameFile = "", string spriteFiles = "", int[] spriteCount = default)
    {
        this.texts = texts;
        MainUITextFile = mainUITextFile;
        NameFile = nameFile;
        SpriteFiles = spriteFiles;
        SpriteCount = spriteCount;
    }
}
