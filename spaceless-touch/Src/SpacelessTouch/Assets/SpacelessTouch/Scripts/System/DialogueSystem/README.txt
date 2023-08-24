1 提供的接口有：
    MainUI.Next()执行下一行
    *ALL.Close()正常关闭整个对话系统UI
    MainUI.Enable(各种数据，详见MainUI.cs -> MainUI.Enable()重载[2])正常打开整个对话系统UI，前两个重载是为了各UI间的调度，不要使用
2 公共成员
    MainUI：
        Background (GameObject)背景，可有可无但是不能为空
        Text (GameObject)显示内容的文本
        Name (GameObject)显示名字的文本
        Option1 Option2 (GameObject)选项1和2
        ...File (string)各类文件的路径
        MinUpdateChar (int)显示至少MinUpdateChar个字符后可以直接单击UI显示此行所有文本
        CharPrintSpeed (int)每隔CharPrintSpeed帧显示1个字符
    OptionUI：
        MainUI (GameObject)指向MainUI所在的GameObject就行
        Text (GameObject)显示内容的文本
        IsSecond (bool)第二个选项勾选就行
3 文本格式
    .1 以行为单位，以`符号为间隔（例：NameFile的某行：Qures`2）
    .2 文件路径从Asset开始计算
    .3 具体格式：
        OptionInfoFile：Option1对应的文本文件，Option1对应的名字文件，Option1对应的文本文件，Option1对应的名字文件，Option1目标行（int），Option2目标行（int）
        TextFile：文本
        OptionFIle：Option1文本，Option2文本
        NameFile：文本，持续行数（int）
        SpriteFile：文本，持续行数（int）
    .4 注意事项：
        OptionFile的行数要比文本少一行
        OptionInfoFile TextFile的行数要相同，NameFile SpriteFile的行数总和要与前面的行数相同
        不要对文本使用空行
        Option1 Option2默认是禁用的，其他是启用的，不要更改，不要直接使用gameObject.SetActive()，换成GetComponent<MainUI>().Enable()重载[2]
    .5 其他：
        可能存在其他问题，必要时更改源文件
        包中Canvas为标准UI