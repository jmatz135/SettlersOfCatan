using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{       
    public class GameCard
    {
        // create card array
        //0 is Soldier/Knight
        //1 is University Point Card
        //2 is Palace Point Card
        //3 is Library point card
        //4 is Market Point Card
        //5 is Chapel Point Card
        //6 is Year of Plenty Card
        //7 is Monopoly
        //8 is Roadbuilding
        public int[] InitCardArray = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 6, 7, 7, 8, 8 };
        public int[] RandCardArray;
        public int[] HoldDevCard;
        public int placeHolder;
        public Boolean roadbuilding = false;
        public int CardPlaceHolder = 0;

        SettlersOfCatan OurGame;
        PlayingState playingState;

        public bool knightPlayed=false;
        public bool yearOfPlentyPlayed=false;
        public bool monopolyPlayed=false;


        //Constructor
        public GameCard(SettlersOfCatan game , PlayingState state)
        {
            OurGame = (SettlersOfCatan)game;
            playingState = (PlayingState)state;
            //Randomize the deck
            randomGameCard();
        }
        
        
        //randomize the development card array
        public void randomGameCard()
        {
            Random rnd = new Random();
            RandCardArray = InitCardArray.OrderBy(x => rnd.Next()).ToArray();
        }

        //get a development card from the RandomCardArray
        public Player getCard(Player px)
        {
            px.devCards[RandCardArray[CardPlaceHolder]]++;
            //px.playerCard++;
            if (RandCardArray[CardPlaceHolder] > 0 && RandCardArray[CardPlaceHolder] < 6)
            {
                px.hiddenVictoryPoints++;
            }
            CardPlaceHolder++;

            return px;
        }

        //Playing the monolpoly card, which  gives the player all the resources of one type that other players have.
        public void playMonopolyCard(string resource, Player px)
        {
            int monopoly;
            Player tempPlayer = OurGame.aiPlayer1;
            Player tempPlayer1 = OurGame.aiPlayer2;
            Player tempPlayer2 = OurGame.humanPlayer;
                //get what resources the player wants
                if (resource=="wheat") //player choses wheat
                {
                    monopoly=(tempPlayer.wheat+tempPlayer1.wheat+tempPlayer2.wheat);
                    tempPlayer.wheat=0;
                    tempPlayer1.wheat=0;
                    tempPlayer2.wheat=0;
                    px.wheat=monopoly;
                }
                 if (resource=="ore") //player choses ore
                {
                    monopoly=(tempPlayer.ore+tempPlayer1.ore+tempPlayer2.ore);
                    tempPlayer.ore=0;
                    tempPlayer1.ore=0;
                    tempPlayer2.ore=0;
                    px.ore=monopoly;
                }
                 if (resource=="wool") //player choses wool
                {
                    monopoly=(tempPlayer.wool+tempPlayer1.wool+tempPlayer2.wool);
                    tempPlayer.wool=0;
                    tempPlayer1.wool=0;
                    tempPlayer2.wool=0;
                    px.wool=monopoly;
                }
                 if (resource=="brick") //player choses brick
                {
                    monopoly=(tempPlayer.brick+tempPlayer1.brick+tempPlayer2.brick);
                    tempPlayer.brick=0;
                    tempPlayer1.brick=0;
                    tempPlayer2.brick=0;
                    px.brick=monopoly;
                }
                 if (resource=="wood") //player choses wood
                {
                    monopoly=(tempPlayer.wood+tempPlayer1.wood+tempPlayer2.wood);
                    tempPlayer.wood=0;
                    tempPlayer1.wood=0;
                    tempPlayer2.wood=0;
                    px.wood=monopoly;
                }                            
        }

        //figures out what card is played 
        public Player playCard(Player px, int card)
        {
            Player tempPlayer = OurGame.aiPlayer1;
            Player tempPlayer1 = OurGame.aiPlayer2;
            Player tempPlayer2 = OurGame.humanPlayer; 
            //if knight card check if it largest army time and if player one the game
            //otherwise, set a boolean that forces a move robber call
            if (card == 0)
            {
                // Requirement 2.5.2 
                px.knights++;
                px.devCards[0]--;
                //Requirement 2.5.3
                //Requirement 2.5.4
                if (px.knights > 2)
                {
                    if (OurGame.aiPlayer1.largestArmy == true)
                    {
                        OurGame.aiPlayer1.largestArmy = false;
                        OurGame.aiPlayer1.hiddenVictoryPoints = OurGame.aiPlayer1.hiddenVictoryPoints - 2;
                        OurGame.aiPlayer1.victoryPoints = OurGame.aiPlayer1.victoryPoints - 2;
                    }
                    if (OurGame.aiPlayer2.largestArmy == true)
                    {
                        OurGame.aiPlayer2.largestArmy = false;
                        OurGame.aiPlayer2.hiddenVictoryPoints = OurGame.aiPlayer2.hiddenVictoryPoints - 2;
                        OurGame.aiPlayer2.victoryPoints = OurGame.aiPlayer2.victoryPoints - 2;
                    }
                    if (OurGame.humanPlayer.largestArmy == true)
                    {
                        OurGame.humanPlayer.largestArmy = false;
                        OurGame.humanPlayer.hiddenVictoryPoints = OurGame.humanPlayer.hiddenVictoryPoints - 2;
                        OurGame.humanPlayer.victoryPoints = OurGame.humanPlayer.victoryPoints - 2;
                    }
                    //Requirement 2.5.5
                    px.largestArmy = true;   //need to make sure others get changed to false
                    px.hiddenVictoryPoints = px.hiddenVictoryPoints + 2;
                    px.victoryPoints = px.victoryPoints + 2;
                }

                if (px == OurGame.humanPlayer)
                {
                    //Requirement 2.5.1
                    playingState.movingRobber = true;
                    DrawHud.playedCard = true;
                }

            // change booleans for roadbuilding, year of plenty and monopolies, accordingly.
            }
            if (card == 6)
            {
                if (px == OurGame.humanPlayer)
                {
                    DrawHud.drawCards = false;
                    playingState.yearOfPlentyMode = true;
                }
                else
                    px.devCards[6]--;
            }
            if (card == 7)
            {
                if (px == OurGame.humanPlayer)
                {
                    playingState.monopolyMode = true;
                    DrawHud.drawCards = false;
                }
            }
            if (card == 8)
            {
                if (px == OurGame.humanPlayer)
                {
                    playingState.roadCard = true;
                    playingState.buildingRoad = true;
                    DrawHud.drawCards = false;
                    DrawHud.playedCard = true;
                }
                px.devCards[8]--;
                px.cardsAvailable[8]--;
            }
            return px;
        }

        public void MonopolyChosen(int resource, Player monopolyPlayer)
        {
            foreach (Player px in OurGame.players)
            {
                if (px != monopolyPlayer)
                {
                    int amountOfResource;
                    amountOfResource = px.intToResource(resource, px);
                    for (int i = amountOfResource; i > 0; i--)
                    {
                        px.decResource(resource, px);
                        monopolyPlayer.incResource(resource);
                    }
                }
                px.ResourceSum();
            }
            monopolyPlayer.devCards[7]--;
            monopolyPlayer.cardsAvailable[7]--;
            monopolyPlayer.ResourceSum();
            monopolyPlayer.SetBuildBools();
            if (monopolyPlayer == OurGame.humanPlayer)
                DrawHud.playedCard = true;
        }

    }    
}