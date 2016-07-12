using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackmydick
{
    class GameInfo
    {
        string GameName { get; set; }
        public int LeftID { get; set; }
        public int RightID { get; set; }
        public int LeverID { get; set; }
        public int XDeviation { get; set; }
        public int YDeviation { get; set; }

        public GameInfo(string gameName)
        {
            GameName = gameName;
        }

        public override string ToString()
        {
            return GameName;
        }
    }
}
