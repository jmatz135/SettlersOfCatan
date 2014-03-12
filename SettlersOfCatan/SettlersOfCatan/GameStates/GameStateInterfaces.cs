/* Sets up the interfaces for the various Game States
 */

using System;
using System.Collections.Generic;
using SOCProjectLibrary;

namespace SettlersOfCatan
{
    public interface IStartMenuState : IGameState { }
    public interface IFinishedState : IGameState 
    {
        void FinishedGame(Boolean win);
    }
    public interface IPlayingState : IGameState 
    {
        void StartGame();
    }
 
}