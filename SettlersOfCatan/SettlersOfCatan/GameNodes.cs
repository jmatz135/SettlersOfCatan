using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    public class GameNode : GameLocation
    {
        public GameHex hex1 { get; set; }
        public GameHex hex2 { get; set; }
        public GameHex hex3 { get; set; }
        public int settleType { get; set; }
        public Boolean hasPort { get; set; }
        public Boolean threePort { get; set; }
        public Boolean woodPort { get; set; }
        public Boolean brickPort { get; set; }
        public Boolean grainPort { get; set; }
        public Boolean orePort { get; set; }
        public Boolean woolPort { get; set; }
        public int player { get; set; }
        public GameNode Next { get; set; }

        public GameNode(int a, int b, int c, GameHex d, GameHex e, GameHex f, int g)
            : base(a, b, c)
        {
            this.hex1 = d;
            this.hex2 = e;
            this.hex3 = f;
            this.settleType = g;
            threePort = false;
            woodPort = false;
            brickPort = false;
            grainPort = false;
            orePort = false;
            woolPort = false;
            hasPort = false;
        }

        public void AddPort(int port)
        {
            if (port == 0)
                this.threePort = true;
            if (port == 1)
                this.woodPort = true;
            if (port == 2)
                this.brickPort = true;
            if (port == 3)
                this.grainPort = true;
            if (port == 4)
                this.orePort = true;
            if (port == 5)
                this.woolPort = true;
            hasPort = true;
        }

        public void buildNode(int player)
        {
            this.settleType++;
            this.player = player;
        }

    }
}