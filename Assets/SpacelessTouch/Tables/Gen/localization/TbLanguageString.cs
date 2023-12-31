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
   
public partial class TbLanguageString
{
    private readonly Dictionary<string, localization.LanguageString> _dataMap;
    private readonly List<localization.LanguageString> _dataList;
    
    public TbLanguageString(ByteBuf _buf)
    {
        _dataMap = new Dictionary<string, localization.LanguageString>();
        _dataList = new List<localization.LanguageString>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            localization.LanguageString _v;
            _v = localization.LanguageString.DeserializeLanguageString(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Key, _v);
        }
        PostInit();
    }

    public Dictionary<string, localization.LanguageString> DataMap => _dataMap;
    public List<localization.LanguageString> DataList => _dataList;

    public localization.LanguageString GetOrDefault(string key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public localization.LanguageString Get(string key) => _dataMap[key];
    public localization.LanguageString this[string key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}