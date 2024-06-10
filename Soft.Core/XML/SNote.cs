using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

public class SNote
{
    public SNote()
    {
    }
    public SNote(string _Name, string _Value)
    {
        Name = _Name;
        Value = _Value;
    }
    public string Name;
    public string Value;
    public override string ToString()
    {
        return string.Format("\r\n      <{0}>{1}</{0}>", Name.ToUpper(), Value.Trim().Replace("<", "&lt;").Replace(">", "&gt;"));
    }
}