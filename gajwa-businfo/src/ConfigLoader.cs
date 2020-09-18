using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace gajwa_businfo
{
    public static class ConfigLoader //todo: config 파일에 아무것도 없는 상태로 저장하거나 불러오면 어떤 일이 일어날지 모름.
    {

        public static ConfigContainer LoadConfigFromFile(string FileLocation)
        {

            List<ConfigGroup> config = new List<ConfigGroup>();

            //for loop
            int groupCnt = 0;
            bool first = false;
            string gname = "";
            ConfigGroup cgroup = new ConfigGroup();

            StringReader reader;
            if (File.Exists(FileLocation))
            {
                reader = new StringReader(File.ReadAllText(FileLocation));
            }
            else
            {
                return null;
            }


            //loop
            while (reader.Peek() >= 0)
            {

                string readline = reader.ReadLine();

                if (readline.Length == 0) continue;

                if (readline[0] != '#')
                {

                    if (readline[0] == '[' && readline[readline.Length - 1] == ']')
                    {
                        gname = readline.Replace("[", "").Replace("]", "");
                        cgroup = new ConfigGroup();
                        cgroup.GroupName = gname;

                        if (!first) config.Add(cgroup);

                        first = false;
                        groupCnt += 1;

                    }

                    if (gname != "" && !readline.Contains("[") && !readline.Contains("]") && readline.Contains("="))
                    {
                        cgroup.AddItem(readline.Split('=')[0], readline.Split('=')[1]);
                    }

                }

            }

            //if (groupCnt > 0) config.Add(cgroup);

            return new ConfigContainer() { Items = config };

        }

        public static void SaveConfigToFile(ConfigContainer config, string filename)
        {

            if (File.Exists(filename)) File.Delete(filename);

            File.AppendAllText(filename, $"{base_.CONFIG_SAVE_TITLE_STRING}\n");

            foreach (ConfigGroup i in config.Items)
            {
                File.AppendAllText(filename, $"\n[{i.GroupName}]\n");

                foreach(string j in i.GroupItems.Keys)
                {
                    File.AppendAllText(filename, $"{j}={i.FindItem(j)}\n");// yo mama nigga sista
                }
            }
        }

    }
}
