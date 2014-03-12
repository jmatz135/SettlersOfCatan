using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    public class GameHex
    {
        public int hexNumber { get; set; }
        public string hexType { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        public Boolean hasRobber { get; set; }

        public GameHex()
        {
        }

        public GameHex(string z, int zz, int r, int c)
        {
            this.hexNumber = zz;
            this.hexType = z;
            this.row = r;
            this.column = c;
            this.hasRobber = false;
        }
    }
}
