using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DoNetDrive.Protocol.Door.Test.Language
{
    public class ToolLanguage
    {
        private static Dictionary<string, Dictionary<string, string>> UILanguage;

        public static void LoadLanguage(string sFile)
        {
            UILanguage = new Dictionary<string, Dictionary<string, string>>();
            XmlDocument xml = new XmlDocument();
            xml.Load(sFile);
            var xRoot = xml.DocumentElement;
            foreach (XmlNode xElement in xRoot.ChildNodes)
            {
                var lngDic = new Dictionary<string, string>();
                UILanguage.Add(xElement.Name, lngDic);
                foreach (XmlNode xNode in xElement.ChildNodes)
                {
                    lngDic.Add(xNode.Name, xNode.Attributes["value"].Value);
                }

            }
        }

        public static string GetLanguage(string sElementKey, string sNodeKey)
        {
            if (UILanguage.ContainsKey(sElementKey))
            {
                var dic = UILanguage[sElementKey];
                if (dic.ContainsKey(sNodeKey))
                {
                    return dic[sNodeKey];
                }
            }

            return "NULL";
        }
    }
}
