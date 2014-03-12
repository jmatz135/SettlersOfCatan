using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    public class GameRoad : GameLocation
    {
        public GameNode Node1 {get; set; }
        public GameNode Node2 { get; set; }


        public GameRoad(int a, int b, int c, GameNode x, GameNode y)
        : base(a, b, c)
        {
            this.Node1 = x;
            this.Node2 = y;
        }
      
    }
}
