using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gajwa_businfo
{
    public class ConfigGroup
    {
        public string GroupName = "";
        public Dictionary<string, string> GroupItems = new Dictionary<string, string>() { };

        public string FindItem(string key)
        {
            bool ex = false;
            foreach (string i in GroupItems.Keys) if (i == key) ex = true;
            if (ex) return GroupItems[key];
            return null;
        }

        public void AddItem(string key, string str)
        {
            GroupItems.Add(key, str);
        }
    }
}
