//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace cfg.localization
{
public sealed partial class LanguageString :  Bright.Config.BeanBase 
{
    public LanguageString(ByteBuf _buf) 
    {
        Key = _buf.ReadString();
        TextCn = _buf.ReadString();
        TextTw = _buf.ReadString();
        TextEn = _buf.ReadString();
        PostInit();
    }

    public static LanguageString DeserializeLanguageString(ByteBuf _buf)
    {
        return new localization.LanguageString(_buf);
    }

    /// <summary>
    /// 本地化key
    /// </summary>
    public string Key { get; private set; }
    /// <summary>
    /// 简体中文
    /// </summary>
    public string TextCn { get; private set; }
    /// <summary>
    /// 繁體中文
    /// </summary>
    public string TextTw { get; private set; }
    /// <summary>
    /// English
    /// </summary>
    public string TextEn { get; private set; }

    public const int __ID__ = 1752823198;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Key:" + Key + ","
        + "TextCn:" + TextCn + ","
        + "TextTw:" + TextTw + ","
        + "TextEn:" + TextEn + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}