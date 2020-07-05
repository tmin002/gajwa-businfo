using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gajwa_businfo
{
    public abstract class CommandTemplate
    {

        public abstract string CommandName
        { get; set; }

        //private abstract bool RegisterState = CommandManager.RegisterCommand(this);
        public abstract string Execute(List<string> args);

    }
}
