using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gajwa_businfo
{
    public static class CommandManager
    {

        public static List<CommandTemplate> ListCommand = new List<CommandTemplate>() { };

        public static bool RegisterCommand(CommandTemplate c)
        {
            ListCommand.Add(c);
            return true;
        }

    }
}
