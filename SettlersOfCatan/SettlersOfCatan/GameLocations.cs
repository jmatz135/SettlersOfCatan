using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    public abstract class GameLocation
    {
        public int locx { get; set; }
        public int locy { get; set; }
        public int owner { get; set; }
                
        public GameLocation(int x, int y, int z)
        {
            this.locx = x;
            this.locy = y;
            this.owner = z;
        }
    }
}
