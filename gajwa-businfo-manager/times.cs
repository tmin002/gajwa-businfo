using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gajwa_businfo_manager
{
    class times
    {
        public int StartHour;
        public int StartMinute;
        public int EndHour;
        public int EndMinute;

        public times(int starth, int startm, int endh, int endm)
        {
            StartHour = starth;
            StartMinute = startm;
            EndHour = endh;
            EndMinute = endm;
        }

    }
}
