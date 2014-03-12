/* A partial class that all GameStates are created off of
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SOCProjectLibrary;

namespace SettlersOfCatan
{
    public partial class BaseGameState : GameState
    {
        protected SettlersOfCatan OurGame;
        protected ContentManager Content;

        public BaseGameState(Game game)
            : base(game)
        {
            Content = game.Content;
            OurGame = (SettlersOfCatan)game;
        }
    }
}
