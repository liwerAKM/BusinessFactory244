using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

public class SAtributre
{
    public SAtributre()
    {
    }
    public SAtributre(string _Name, string _Value)
    {
        Name = _Name;
        Value = _Value;
    }
    public string Name;
    public string Value;
    public override string ToString()
    {
        return " " + Name.ToUpper() + "= \"" + Value.Trim() + "\" ";
    }
}