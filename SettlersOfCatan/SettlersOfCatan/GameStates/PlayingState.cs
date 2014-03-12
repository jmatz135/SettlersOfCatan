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
        private LongestRoad lr;

        public GameCard gameCard;
        public Trade currentTrade;

        //game playing state booleans
        public Boolean gameStartMode;
        public Boolean movingRobber;
        public bool chooseTheft;

        //Trading Booleans
        public bool aiOneResponse;
        public bool aiTwoResponse;
        public bool aiResponded;

        public bool aiDesiresTrade;
        public bool aiRequestsTradeFromHuman;
        public bool humanAcceptsTrade;
        public bool humanResponded;

        public bool robberGiveAway;

        //Bools for determining whether the a button on the Hud has been pressed to enter a state
        public bool rolledDice;
        public bool buildingSettlement;
        public bool buildingCity;
        public bool buildingRoad;
        public bool buildingDevCard;
        public bool monopolyMode;
        public bool yearOfPlentyMode;
        public bool roadCard;

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

            foreach (Player px in OurGame.players)
            {
                px.ResourceSum();
                px.SetBuildBools();
                if (px.hiddenVictoryPoints >= 10)
                {
                    if (px == OurGame.humanPlayer)
                    {
                        GameManager.ChangeState(OurGame.FinishedState.Value);
                        OurGame.FinishedState.FinishedGame(true);
                    }
                    else
                    {
                        GameManager.ChangeState(OurGame.FinishedState.Value);
                        OurGame.FinishedState.FinishedGame(false);
                    }
                }
            }
             
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
            currentTrade = new Trade();
            gameCard = new GameCard(OurGame, this);
            lr = new LongestRoad(OurGame, this);

            //Create players array and randomize
            OurGame.players = new List<Player>();
            OurGame.players.Add(new Player(1, OurGame.gameBoard));
            OurGame.players.Add(new Player(2, OurGame.gameBoard));
            OurGame.players.Add(new Player(3, OurGame.gameBoard));

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
            roadCardCounter = 1;

            //Create the GUIs for the game
            gameBoardGraphics = new DrawGameBoard2D(Game, this);
            Game.Components.Add(gameBoardGraphics);

            gameHud = new DrawHud(Game, this);
            Game.Components.Add(gameHud);

            //Set game playing state booleans
            movingRobber = false;
            chooseTheft = false;
            gameStartMode = true;
            firstTimeThrough = true;
            monopolyMode = false;
            yearOfPlentyMode = false;
            roadCard = false;

            //Set trading booleans
            aiOneResponse = false;
            aiTwoResponse = false;
            aiResponded = false;

            aiDesiresTrade = false;
            aiRequestsTradeFromHuman = false;
            humanResponded = false;
            humanAcceptsTrade = false;

            robberGiveAway = false;

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
            px.victoryPoints++;
            px.hiddenVictoryPoints++;
            px.settlementPieces++;

            //Not changing settlement type from 0 to 1 because Dallas and I assumed settlementType 0 is settlement and settlementType 1 is City
            OurGame.gameBoard.setNodeInfo(gn, 0, px);

            if (gn.hasPort)
            {
                if (gn.threePort)
                    px.hasThree = true;
                else if (gn.brickPort)
                    px.hasBrick = true;
                else if (gn.grainPort)
                    px.hasGrain = true;
                else if (gn.woodPort)
                    px.hasLumber = true;
                else if (gn.orePort)
                    px.hasOre = true;
                else if (gn.woolPort)
                    px.hasWool = true;
            }

            if (!gameStartMode)
            {
                px.wheat--;
                px.brick--;
                px.wood--;
                px.wool--;
            }

            if (gameStartMode && !firstTimeThrough)
            {
                OurGame.gameBoard.GameStartSettlementUpdate(gn, px);
            }

            px.SetBuildBools();
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
        }

        //Purpose: set all road information and remove resources when a settlement is road
        //Param: gr - gamenode that is getting the road, px - player that is placing road
        public void AddingRoad(GameRoad gr, Player px)
        {
            px.roadPieces++;
            if (gameStartMode)
            {
                OurGame.gameBoard.setRoadInfo(gr, px);        
            }
            else if(roadCard)
            {
                if (roadCardCounter != 2)
                {
                    buildingRoad = true;
                    OurGame.gameBoard.setRoadInfo(gr, px);
                    roadCardCounter++;
                }
                else
                {
                    OurGame.gameBoard.setRoadInfo(gr, px);
                    buildingRoad = false;
                    roadCard = false;
                    roadCardCounter = 1;
                }
            }
            else if (!gameStartMode) //need to also check against Road Card
            {
                OurGame.gameBoard.setRoadInfo(gr, px);
                px.wood--;
                px.brick--;              
                px.SetBuildBools();
                //lr.longestRoad(gr.locx, gr.locy, px);
            }
        }

        public void PlayerRequestedTrade(Player px)
        {
            if (px == OurGame.humanPlayer)
            {
                aiOneResponse = ai.evaluateTradeProposal(OurGame.aiPlayer1, currentTrade);
                aiTwoResponse = ai.evaluateTradeProposal(OurGame.aiPlayer2, currentTrade);
                aiResponded = true;
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
            gameCard.getCard(px);       
        }

        //Purpose: call correct card method to do what the devcard says when a devcard is played
        //Params: index - index that corresponds to the card being played, px - player playing card
        public void PlayDevCard(int index, Player px)
        {
            gameCard.playCard(px, index);
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
                px.SetBuildBools();
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
                        if (OurGame.gameDice.sumDice() == 7)
                        {
                            movingRobber = true;
                            if (OurGame.aiPlayer1.totalResources > 7)
                                ai.discardHalf(OurGame.aiPlayer1, OurGame);

                            if (OurGame.aiPlayer2.totalResources > 7)
                                ai.discardHalf(OurGame.aiPlayer2, OurGame);

                            if (OurGame.humanPlayer.totalResources > 7)
                            {
                                robberGiveAway = true;
                                movingRobber = false;
                            }
                        }
                        px.SetBuildBools();
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
                    if (!aiRequestsTradeFromHuman && !robberGiveAway)  //skip all this if requesting trade (not actually necessary but speeds it up a bit)
                    {
                        if (turnWaitCounter > 2)
                        {

                            OurGame.gameBoard.getResources(OurGame.players, OurGame.gameDice.sumDice());
                            if (OurGame.gameDice.sumDice() == 7)
                            {
                                ai.handleRobber(px, OurGame);
                                if (OurGame.humanPlayer.totalResources > 7)
                                    robberGiveAway = true;

                                if (OurGame.aiPlayer1.totalResources > 7)
                                    ai.discardHalf(OurGame.aiPlayer1, OurGame);

                                if (OurGame.aiPlayer2.totalResources > 7)
                                    ai.discardHalf(OurGame.aiPlayer2, OurGame);
                            }
                            px.SetBuildBools();
                            px.ResourceSum();

                            if (!robberGiveAway)
                            {
                                //Start Turn
                                if (!humanResponded)  //check to make sure this isn't run again if a trade was requested
                                {
                                    //Try to build, if unsuccessful we want to trade
                                    aiDesiresTrade = ai.tryToBuild(px, OurGame);
                                    if (aiDesiresTrade)
                                        aiRequestsTradeFromHuman = ai.tryToTrade(px, OurGame);  //If AI wants to trade with human set requestsTradeFromHuman = true
                                }

                                //If the human has responded and it accepts try to build again
                                if (humanResponded && humanAcceptsTrade)
                                {
                                    Player otherAI = null;
                                    if (px == OurGame.aiPlayer1)
                                        otherAI = OurGame.aiPlayer2;
                                    else
                                        otherAI = OurGame.aiPlayer1;

                                    //Get the response from the other AI to the trade
                                    bool doesOtherAIAccept = ai.evaluateTradeProposal(otherAI, currentTrade);

                                    //If both the Human and AI accept the trade
                                    if (humanAcceptsTrade && doesOtherAIAccept)
                                    {
                                        //If the Human has more VP, trade with AI
                                        if (OurGame.humanPlayer.victoryPoints > otherAI.victoryPoints)
                                            ai.processTrade(px, otherAI, OurGame);
                                        //If the AI has more VP, trade with Human
                                        else if (otherAI.victoryPoints < OurGame.humanPlayer.victoryPoints)
                                            ai.processTrade(px, OurGame.humanPlayer, OurGame);
                                        //Equal VP, just do AI for now
                                        else
                                            ai.processTrade(px, otherAI, OurGame);
                                    }
                                    //If just the human accepts the trade
                                    else if (humanAcceptsTrade)
                                        ai.processTrade(px, OurGame.humanPlayer, OurGame);
                                    //If just the AI accepts the trade
                                    else if (doesOtherAIAccept)
                                        ai.processTrade(px, otherAI, OurGame);

                                    px.ResourceSum();
                                    otherAI.ResourceSum();
                                    OurGame.humanPlayer.ResourceSum();

                                    px.SetBuildBools();
                                    otherAI.SetBuildBools();
                                    OurGame.humanPlayer.SetBuildBools();

                                    aiRequestsTradeFromHuman = ai.tryToBuild(px, OurGame);
                                    humanResponded = false;
                                    humanAcceptsTrade = false;  //Reset booleans
                                }
                                else if (humanResponded && !humanAcceptsTrade) // if the human has responded but rejects offer, just reset booleans
                                {
                                    humanResponded = false;
                                    humanAcceptsTrade = false;  //Reset booleans
                                }


                                px.SetBuildBools();
                                px.ResourceSum();

                                if (!aiRequestsTradeFromHuman) //Make sure the playerTurn isn't updated unless there is no trade requested
                                {
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
                        }
                    }
                }
                //During start of game mode AI turn
                else if (gameStartMode)
                {
                    //Set settlement and road
                    ai.placeSettlement(px, OurGame);
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
