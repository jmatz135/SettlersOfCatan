using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    public class Player
    {
        GameBoard gameBoard;

        public int playerNumber;
        public int hiddenVictoryPoints;
        public int victoryPoints;
        public int totalResources;
        public int roads;
        public int longestRoad;
        public int knights;
        public int brick;
        public int wheat;
        public int wood;
        public int wool;
        public int ore;
        public int settlementPieces;
        public int cityPieces;
        public int roadPieces;
        //public int playerCard;
        public int[] devCards = new int[9];
        public int[] cardsAvailable = new int[9];
        public bool largestArmy;
        public bool longestRoadPoints;

        //Port Booleans
        public bool hasThree;
        public bool hasBrick;
        public bool hasWool;
        public bool hasLumber;
        public bool hasOre;
        public bool hasGrain;

        //Player State Booleans
        public bool canRollDice;
        public bool canTrade;
        public bool canBuild;
        public bool canEndTurn;

        public bool canBuildRoad;
        public bool canBuildSettlement;
        public bool canBuildCity;
        public bool canBuildDevCard;

        public Player(int num, GameBoard gb)
        {
            gameBoard = gb;

            playerNumber = num;
            hiddenVictoryPoints = 0;
            victoryPoints = 0;
            roads = 0;
            longestRoad = 0;
            knights = 0;
            brick = 0;
            wheat = 0;
            wood = 0;
            wool = 0;
            ore = 0;
            for (int i = 0; i < 9; i++)
            {
                devCards[i] = 0;
            }
            for (int i = 0; i < 9; i++)
            {
                cardsAvailable[i] = 0;
            }

            canRollDice = false;
            canTrade = false;
            canBuild = false;
            canEndTurn = false;

            settlementPieces = 0;
            cityPieces = 0;
            roadPieces = 0;

            totalResources = ResourceSum();
            canBuildRoad = CanBuildRoad();
            canBuildSettlement = CanBuildSettlement();
            canBuildCity = CanBuildCity();
            canBuildDevCard = CanBuildDevCard();

            hasThree = false;
            hasBrick = false;
            hasWool = false;
            hasGrain = false;
            hasLumber = false;
            hasOre = false;

            largestArmy = false;
        }

        //Purpose: Steals a random card from the player.  Used for taking a card
        //         when moving the robber
        //Params: None
        //Return: An int that represents which resource to increment for the stealing player.
        //        0 = brick
        //        1 = wheat
        //        2 = wood
        //        3 = wool
        //        4 = ore
        public int stealRandomCard(Player takeFromMe)
        {
            Random rdm = new Random();
            bool cont = true;
            int rdmRes = -1;
            if (takeFromMe.totalResources > 0)
            {
                while (cont)
                {
                    rdmRes = rdm.Next(0, 5);
                    int takeThis = intToResource(rdmRes, takeFromMe);
                    if (takeThis > 0)
                    {
                        cont = false;
                        decResource(rdmRes, takeFromMe);
                        takeFromMe.ResourceSum();
                        takeFromMe.SetBuildBools();
                    }
                }
            }           
            return rdmRes;
        }

        //Purpose: Increments a resource, as denoted by param x, by 1
        //Params: x - an int that represents which resource type to increase
        //Return: None
        public void incResource(int x)
        {
            if (x == 0)
                this.brick++;
            else if (x == 1)
                this.wheat++;
            else if (x == 2)
                this.wood++;
            else if (x == 3)
                this.wool++;
            else if (x == 4)
                this.ore++;
        }

        //Purpose: This decrements the players resource, as determined by x, by 1
        //Params: x - an int that determines which resource to dec. See above.
        //Return: None
        public void decResource(int x, Player takeFromMe)
        {
            if (x == 0)
                takeFromMe.brick--;
            else if (x == 1)
                takeFromMe.wheat--;
            else if (x == 2)
                takeFromMe.wood--;
            else if (x == 3)
                takeFromMe.wool--;
            else if (x == 4)
                takeFromMe.ore--;
        }

        //Purpose: Return the amount of a resource, as designated by x, that the player has
        //Params: x - an int that determines which resource count to return. See above.
        //Return: An int that is the count of the resource type you are looking for
        public int intToResource(int x, Player takeFromMe)
        {
            if (x == 0)
                return takeFromMe.brick;
            else if (x == 1)
                return takeFromMe.wheat;
            else if (x == 2)
                return takeFromMe.wood;
            else if (x == 3)
                return takeFromMe.wool;
            else if (x == 4)
                return takeFromMe.ore;
            else
                return 999;
        }

        public int ResourceSum()
        {
            int resources = brick + wheat + wool + wood + ore;
            totalResources = resources;
            return resources;
        }

        private Boolean CanBuildRoad()
        {
            Boolean canBuild = false;
            if (brick > 0 && wood > 0 && roadPieces < 15)
                canBuild = true;
            return canBuild;
        }

        private Boolean CanBuildSettlement()
        {
            Boolean canBuild = false;
            if (brick > 0 && wood > 0 && wheat > 0 && wool > 0 && settlementPieces < 5)
                canBuild = true;
            return canBuild;
        }

        private Boolean CanBuildSettlement(GameBoard gameBoard)
        {
            Boolean canBuild = false;
            if (brick > 0 && wood > 0 && wheat > 0 && wool > 0 && settlementPieces < 5)
            {
                List<GameNode> settleToBuild = gameBoard.placesForSettlements(this);
                if (settleToBuild.Count > 0)
                    canBuild = true;
            }
            return canBuild;
        }

        private Boolean CanBuildCity()
        {
            Boolean canBuild = false;
            if (settlementPieces > 0 && ore > 2 && wheat > 1 && cityPieces < 4)
                canBuild = true;
            return canBuild;
        }

        private Boolean CanBuildCity(GameBoard gameBoard)
        {
            Boolean canBuild = false;
            if (settlementPieces > 0 && ore > 2 && wheat > 1 && cityPieces < 4)
            {
                List<GameNode> cityToBuild = gameBoard.placesForCities(this);
                if (cityToBuild.Count > 0)
                    canBuild = true;
            }
            return canBuild;
        }

        private Boolean CanBuildDevCard()
        {
            Boolean canBuild = false;
            if (wool > 0 && wheat > 0 && ore > 0)
                canBuild = true;
            return canBuild;
        }

        public int TotalDevCards()
        {
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum = sum + devCards[i];
            }
            return sum;
        }

        public void SetBuildBools()
        {
            bool test;
            test = CanBuildSettlement(gameBoard);
            if (test)
                canBuildSettlement = true;
            else
                canBuildSettlement = false;
            test = CanBuildCity(gameBoard);
            if (test)
                canBuildCity = true;
            else
                canBuildCity = false;
            test = CanBuildRoad();
            if (test)
                canBuildRoad = true;
            else
                canBuildRoad = false;
            test = CanBuildDevCard();
            if (test)
                canBuildDevCard = true;
            else
                canBuildDevCard = false;
        }
    }
}
