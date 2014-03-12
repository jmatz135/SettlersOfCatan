/* When the game is being played this is what runs the game.  
 * This class calls the necessary classes to build the game board, the hud etc..
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SOCProjectLibrary;


namespace SettlersOfCatan
{
    public sealed class PlayingState : BaseGameState, IPlayingState
    {
        private DrawGameBoard2D gameBoardGraphics;
        private DrawHud gameHud;
        private AI ai;

        //game playing state booleans
        public Boolean gameStartMode;
        public Boolean movingRobber;

        //Bools for determining whether the a button on the Hud has been pressed to enter a state
        public bool rolledDice;
        public bool buildingSettlement;
        public bool buildingCity;
        public bool buildingRoad;
        public bool buildingDevCard;

        public bool humanPlayerTurn;
        public bool endHumanTurn;
        public bool aiTurn;
        private bool firstTimeThrough;
        public int playerTurn;
        public float turnWaitCounter;
        private int roadCardCounter;

        public PlayingState( Game game) 
            : base(game)
        {           
            game.Services.AddService(typeof(IPlayingState), this);
        }

        // Detect if state changed i.e. going to start menu (perhaps a pause later)
        protected override void StateChanged(object sender, EventArgs e)
        {
            base.StateChanged(sender, e);

            if (GameManager.State != this.Value)
            {
                Visible = true;
                Enabled = false;
                OurGame.camera.UpdateInput = false;
            }
            else
            {
                OurGame.camera.UpdateInput = false;
            }
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //OurGame.SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // Controls the input interrupts for keyboard, gamepad, mouse
        public override void Update(GameTime gameTime)
        {
            //Playing state keyboard and mouse interrupts to update input information
            if (OurGame.input.WasPressed(0, Buttons.Back, Keys.Escape))
            {
                GameManager.PushState(OurGame.StartMenuState.Value);
            }

#if !XBOX360
            OurGame.mouseX = 0;
            OurGame.mouseY = 0;
            if (Input.MouseState.LeftButton == ButtonState.Pressed && Input.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                OurGame.mouseX = OurGame.input.MouseState.X;
                OurGame.mouseY = OurGame.input.MouseState.Y;
            }
#endif
            //Set pause duration info
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            turnWaitCounter = turnWaitCounter + timeDelta;
             
            //For loop to determing what player is going and make the player's move
            foreach (Player px in OurGame.players)
            {
                if (px.playerNumber == playerTurn)
                {
                    PlayerMove(px);
                }
            }      

            //To Do: Game Updates for playing state
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        //Purpose: Set up the start of the game, including game board, dice, playingstates, etc..
        public void StartGame()
        {
            //Create gameDoard
            OurGame.gameBoard = new GameBoard(OurGame.gameType);
            //Create gameDice
            OurGame.gameDice = new Dice();
            ai = new AI();

            //Create players array and randomize
            OurGame.players = new List<Player>();
            OurGame.players.Add(new Player(1));
            OurGame.players.Add(new Player(2));
            OurGame.players.Add(new Player(3));

            //Randomize players List
            Random rnd = new Random();
            int n = OurGame.players.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Player value = OurGame.players[k];
                OurGame.players[k] = OurGame.players[n];
                OurGame.players[n] = value;
            }  
            //Assign players
            OurGame.aiPlayer1 = OurGame.players[0];
            OurGame.aiPlayer2 = OurGame.players[1];
            OurGame.humanPlayer = OurGame.players[2];

            rolledDice = false;
            humanPlayerTurn = false;
            aiTurn = false;
            turnWaitCounter = 0;
            roadCardCounter = 0;

            //Create the GUIs for the game
            gameBoardGraphics = new DrawGameBoard2D(Game, this);
            Game.Components.Add(gameBoardGraphics);

            gameHud = new DrawHud(Game, this);
            Game.Components.Add(gameHud);

            //Set game playing state booleans
            movingRobber = false;
            gameStartMode = true;
            firstTimeThrough = true;

            //Set Bools for building actions
            buildingSettlement = false;
            buildingCity = false;
            buildingRoad = false;

            playerTurn = 1;
        }

        //Purpose: set all settlement information and remove resources when a settlement is built
        //Param: gn - gamenode that is getting the settlement, px - player that is placing settlement
        public void AddingSettlement(GameNode gn, Player px)
        {
            px.hiddenVictoryPoints++;
            px.victoryPoints++;
            px.settlementPieces++;

            //Not changing settlement type from 0 to 1 because Dallas and I assumed settlementType 0 is settlement and settlementType 1 is City
            OurGame.gameBoard.setNodeInfo(gn, 0, px);

            if (!gameStartMode)
            {
                px.wheat--;
                px.brick--;
                px.wood--;
                px.wool--;
            }

            px.SetBuildBools();
            //CheckLongestRoad();  Not implemented yet
            //CheckVictoryPoints();  Not implemented yet
        }

        //Purpose: set all city information and remove resources when a city is built
        //Param: gn - gamenode that is getting the city, px - player that is placing city
        public void AddingCity(GameNode gn, Player px)
        {
            px.hiddenVictoryPoints++;
            px.victoryPoints++;
            px.cityPieces++;
            px.settlementPieces--;

            OurGame.gameBoard.setNodeInfo(gn, 1, px);

            px.ore = px.ore - 3;
            px.wheat = px.wheat - 2;

            px.SetBuildBools();

            //CheckVictoryPoints();  Not implemented yet
        }

        //Purpose: set all road information and remove resources when a settlement is road
        //Param: gr - gamenode that is getting the road, px - player that is placing road
        public void AddingRoad(GameRoad gr, Player px)
        {

            if (!gameStartMode) //need to also check against Road Card
            {
                OurGame.gameBoard.setRoadInfo(gr, px);
                px.wood--;
                px.brick--;              
                px.roadPieces++;
                px.SetBuildBools();
                //CheckLongestRoad(); Not implemented yet
            }
            //else if(roadCard)
           // {
           //     
            //    if (roadCardCounter < 2)
            //        {
                //      buildingRoad = true;
           //         OurGame.gameBoard.setRoadInfo(gr, px);
             //            }
          //  }
            else if (gameStartMode)
            {
                OurGame.gameBoard.setRoadInfo(gr, px);
                px.roadPieces--;
            }
        }

        //Purpose: remove resouces when a devcard is bought and update the player's devcard info with the new card they received
        //Params: px - player buying the devcard
        public void BoughtDevCard(Player px)
        {
            px.wool--;
            px.wheat--;
            px.ore--;

            px.SetBuildBools();
            //Some Code to figure out just what dev card the player got and assign it to the player
        }

        //Purpose: call correct card method to do what the devcard says when a devcard is played
        //Params: index - index that corresponds to the card being played, px - player playing card
        public void PlayDevCard(int index, Player px)
        {

        }

        //Purpose: Call appropriate player move method based on what player is going
        private void PlayerMove(Player px)
        {
            if (px == OurGame.humanPlayer)
            {
                HumanMove(px);
            }
            else if (px == OurGame.aiPlayer1)
            {
                AIMove(px);
            }
            else if (px == OurGame.aiPlayer2)
            {
                AIMove(px);
            }
        }

        //Purpose: Control the actions of a human player's turn
        //Params: px - the player (humanPlayer) that is taking the turn
        private void HumanMove(Player px)
        {
            //human player's actions when in regular game mode
            if (!gameStartMode)
            {
                //Start of human player turn, set up menus for player
                if (humanPlayerTurn == false && aiTurn == false)
                {
                    humanPlayerTurn = true;
                    OurGame.humanPlayer.canRollDice = true;
                }
                //when the human player's menus have been set up see if they have rolled and update the player information accordingly
                else if (humanPlayerTurn == true && aiTurn == false)
                {
                    if (rolledDice)
                    {
                        OurGame.gameBoard.getResources(OurGame.players, OurGame.gameDice.sumDice());                     
                        //Update player turn information so next player can go
                        if (px.playerNumber == 3)
                            playerTurn = 1;
                        else
                            playerTurn++;
                        rolledDice = false;
                    }
                }
            }
            //human player's actions when in gamestart mode
            else if (gameStartMode)
            {
                if (!buildingRoad && !humanPlayerTurn)
                {
                    buildingSettlement = true;
                    humanPlayerTurn = true;
                }
                if (!buildingSettlement && !endHumanTurn)
                {
                    buildingRoad = true;
                    endHumanTurn = true;
                }
                if (endHumanTurn && !buildingRoad)
                {
                    endHumanTurn = false;
                    humanPlayerTurn = false;
                    if (px.playerNumber == 1)
                    {
                        if (firstTimeThrough)
                            playerTurn++;
                        else
                            gameStartMode = false;
                    }
                    else if (px.playerNumber == 2)
                    {
                        if (firstTimeThrough)
                            playerTurn++;
                        else
                            playerTurn--;
                        aiTurn = true;
                    }
                    else if (px.playerNumber == 3)
                    {
                        if (firstTimeThrough)
                            firstTimeThrough = false;
                        else
                            playerTurn--;
                        aiTurn = true;
                    }
                }
            }
            turnWaitCounter = 0;
        }

        //Purpose: Control the actions of the AI for the AI player turn
        private void AIMove(Player px)
        {
            //Small pause so that AI turns aren't super duper fast
            if (turnWaitCounter > 1)
            {
                //During regular game if it's the aiTurn
                if (!gameStartMode && aiTurn)
                {
                    //AI move during regular turn
                    if (!rolledDice)
                    {
                        OurGame.gameDice.rollDice();
                        DrawHud.drawDice = true;
                        rolledDice = true;
                    }
                    if (turnWaitCounter > 2)
                    {
                        OurGame.gameBoard.getResources(OurGame.players, OurGame.gameDice.sumDice());

                        //Start Turn
                        ai.takeTurn(px, OurGame);

                        //Check what player is next and set playerTurn and aiTurn based on that
                        if (px.playerNumber == 3)
                        {
                            playerTurn = 1;
                            if (OurGame.humanPlayer.playerNumber == 1)
                            {
                                aiTurn = false;
                            }
                        }
                        else
                        {
                            playerTurn++;
                            if ((px.playerNumber + 1) == OurGame.humanPlayer.playerNumber)
                            {
                                aiTurn = false;
                            }
                        }
                        rolledDice = false;
                        turnWaitCounter = 0;
                    }
                }
                //During start of game mode AI turn
                else if (gameStartMode)
                {
                    //Set settlement and road
                    ai.placeSettlement(px, OurGame);
                    px.victoryPoints++;
                    //Check what turn is going and determine what player is next based on start of game progression
                    if (px.playerNumber == 1)
                    {
                        if (firstTimeThrough)
                            playerTurn++;
                        else
                            gameStartMode = false;
                    }
                    else if (px.playerNumber == 2)
                    {
                        if (firstTimeThrough)
                            playerTurn++;
                        else
                            playerTurn--;
                    }
                    else if (px.playerNumber == 3)
                    {
                        if (firstTimeThrough)
                            firstTimeThrough = false;
                        else
                            playerTurn--;
                    }
                    turnWaitCounter = 0;
                }
            }
        }
    }
}
