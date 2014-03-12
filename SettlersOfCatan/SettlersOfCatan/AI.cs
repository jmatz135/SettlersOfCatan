using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{

    /*    TODO    */
    // 1  Roll Dice, Collect Resources (Should be just calling a function already defined) - CHECK
    // 2  Trade with others
    // 3  Trade with bank
    // 4  Trade on ports
    // 5  Evaluate trades from other players
    // 6  Build - Road, Settlement, City, Dev Card - MOSTLY CHECK
    // 7  Place the Robber - CHECK
    // 12 Discard half of cards when robber is rolled
    // 8  Pick player to take cards from due to placing robber - CHECK
    // 9  Play Cards - Knight Crads, Progress Cards, Victory Points Cards
    // 10 Call the Win
    // 11 End Turn

    public class AI
    {


        /***************************************************************************************
         * 
         *                              Settlers of Catan AI Class 
         *                         
         *      1.) Try to build something every turn
         *          - Settlements and Roads
         *          - Cities and Army
         *          - Hybrid
         *      
         *      2.) If you can't build, try to trade with someone
         *          - Evaluate what you have and what you need 
         *      
         *  
         *    
         ***************************************************************************************/


        //Class Vars
        private int strategyType = 999;

        //Constructor
        public AI()
        {
            //pickStrategy();
            strategyType = 0;
        }

        /***************************************************************************************
        * 
        *                         Strategy Controllers Go Below Here
        *    
        ***************************************************************************************/

        //Strategy 1
        //return false if it built something, true otherwise
        public bool tryToBuild(Player px, SettlersOfCatan game)
        {
            px.SetBuildBools();
            Random rdm = new Random();
            bool successfulBuild = false;

            if (px.canBuildCity)
            {
                buildCity(px, game);
            }
            else if ((px.canBuildSettlement) && (px.canBuildRoad) && (px.canBuildDevCard))
            {
                int option = rdm.Next(0, 3);
                if (option == 0)
                    successfulBuild = buildSettlement(px, game);
                else if (option == 1)
                    successfulBuild = buildRoad(px, game);
                else if (option == 2)
                    successfulBuild = buildDevCard(px, game);
            }
            else if ((px.canBuildSettlement) && (px.canBuildRoad))
            {
                int option = rdm.Next(0, 2);
                if (option == 0)
                    successfulBuild = buildSettlement(px, game);
                else if (option == 1)
                    successfulBuild = buildRoad(px, game);
            }
            else if ((px.canBuildSettlement) && (px.canBuildDevCard))
            {
                int option = rdm.Next(0, 2);
                if (option == 0)
                    successfulBuild = buildSettlement(px, game);
                else if (option == 1)
                    successfulBuild = buildDevCard(px, game);
            }
            else if ((px.canBuildRoad) && (px.canBuildDevCard))
            {
                int option = rdm.Next(0, 2);
                if (option == 0)
                    successfulBuild = buildRoad(px, game);
                else if (option == 1)
                    successfulBuild = buildDevCard(px, game);
            }
            else if (px.canBuildSettlement)
                successfulBuild = buildSettlement(px, game);
            else if (px.canBuildRoad)
                successfulBuild = buildRoad(px, game);
            else if (px.canBuildDevCard)
                successfulBuild = buildDevCard(px, game);

            px.SetBuildBools();
            return !successfulBuild;
        }

        /***************************************************************************************
        * 
        *                     Building Methods Go Below Here....Yikes
        *    
        ***************************************************************************************/


        //Purpose:  Called to build settlements. 
        //Params:   px - the player rolling
        //          game - the game 
        //Return:   None
        public bool buildSettlement(Player px, SettlersOfCatan game)
        {

            List<GameNode> available = game.gameBoard.placesForSettlements(px);
            List<GameNode> otherOptions = new List<GameNode>();
            GameNode buildHere = null;
            int maxProb = 0;
            bool foundOne;
            string resourceType = intToType(indexOfFewestResource(px, 4));

            //Try to find your ideal spot
            foreach (GameNode gn in available)
            {
                //buildHere = gn;                
                GameHex h1 = gn.hex1;
                GameHex h2 = gn.hex2;
                GameHex h3 = gn.hex3;
                foundOne = false;
                int prob = getProbability(h1.hexNumber) + getProbability(h2.hexNumber) + getProbability(h3.hexNumber);

                //Place for Settlements and Roads
                if (strategyType == 0)
                {
                    //Not interested in getting ore, so if any of them is rock then just move on
                    if ((h1.hexType == "Rock") || (h2.hexType == "Rock") || (h3.hexType == "Rock"))
                        foundOne = false;
                    //We've found a Node that has no Rock hexes and at least 1 of our minimum resource
                    else if ((h1.hexType == resourceType) || (h2.hexType == resourceType) || (h3.hexType == resourceType))
                        foundOne = true;
                    else
                        otherOptions.Add(gn);
                }
                //Place for Cities and Armies
                else if (strategyType == 1)
                {
                    //All accepted, emphasis on Ore and Wheat
                    if ((h1.hexType == "Rock") || (h1.hexType == "Grain") || (h2.hexType == "Rock") || (h2.hexType == "Grain") || (h3.hexType == "Rock") || (h3.hexType == "Grain"))
                        foundOne = true;
                    else if (prob > maxProb)
                        foundOne = true;
                    else
                        otherOptions.Add(gn);
                }

                //If we haven't set anything yet, simply set buildHere to the current Node
                //Or if prob is greater than maxProb set it to current Node
                if (foundOne)
                {
                    if ((buildHere == null) || (prob > maxProb))
                    {
                        buildHere = gn;
                        maxProb = prob;
                    }
                }

            }

            //If buildHere is still null, we didn't hit on a desired outcome, randomly choose 
            if (buildHere == null)
            {
                if (otherOptions.Count() > 0)
                {
                    Random rnd = new Random();
                    buildHere = otherOptions[rnd.Next(0, otherOptions.Count())];

                    //Claim that settlement for Player px
                    PlayingState ps = (PlayingState)game.PlayingState;
                    ps.AddingSettlement(buildHere, px);
                    return true; //built something
                }
            }
            return false; //no build
        }

        //Build a city, any city
        public bool buildCity(Player px, SettlersOfCatan game)
        {
            //Get our current settlements
            List<GameNode> settlements = game.gameBoard.placesForCities(px);
            GameNode buildHere = null;
            int maxProb = 0;

            foreach (GameNode gn in settlements)
            {
                string gh1 = gn.hex1.hexType;
                string gh2 = gn.hex2.hexType;
                string gh3 = gn.hex3.hexType;

                if (gh1 == "Ocean" || gh2 == "Ocean" || gh3 == "Ocean") { /*ignore, it sits on a port so no real benefit to improving that one */ }
                else
                {
                    int tempProb = getProbability(gn.hex1.hexNumber) + getProbability(gn.hex2.hexNumber) + getProbability(gn.hex3.hexNumber);
                    if (tempProb > maxProb)
                    {
                        maxProb = tempProb;
                        buildHere = gn;
                    }
                }
            }

            if (buildHere != null)
            {
                PlayingState ps = (PlayingState)game.PlayingState;
                ps.AddingCity(buildHere, px);
                return true;
            }
            return false;
        }

        //Build a road, any road
        public bool buildRoad(Player px, SettlersOfCatan game)
        {
            List<GameRoad> grl = new List<GameRoad>();
            List<object> objList = game.gameBoard.getPlayerNodesAndRoads(px);
            GameRoad buildHere = null;

            foreach (object o in objList)
            {
                if (o is GameNode)
                {
                    GameNode tmp = (GameNode)o;
                    List<GameRoad> options = game.gameBoard.getConnectingRoads(tmp);
                    foreach (GameRoad gr in options)
                    {
                        if (gr.owner == 0)
                            grl.Add(gr);
                    }
                }
                else if (o is GameRoad)
                {
                    GameRoad gr = (GameRoad)o;
                    if (gr.Node1.owner == 0)
                    {
                        List<GameRoad> moreOptions = game.gameBoard.getConnectingRoads(gr.Node1);
                        foreach (GameRoad gr2 in moreOptions)
                        {
                            if (gr2.owner == 0)
                                grl.Add(gr2);
                        }
                    }
                    else if (gr.Node2.owner == 0)
                    {
                        List<GameRoad> moreOptions = game.gameBoard.getConnectingRoads(gr.Node2);
                        foreach (GameRoad gr2 in moreOptions)
                        {
                            if (gr2.owner == 0)
                                grl.Add(gr2);
                        }
                    }
                }
            }

            IEnumerable<GameRoad> distinctRoads = grl.Distinct();
            if (buildHere == null)
            {
                List<GameRoad> distincts = distinctRoads.ToList<GameRoad>();
                Random rnd = new Random();
                buildHere = distincts[rnd.Next(0, distincts.Count())];
                buildHere.owner = px.playerNumber;

                PlayingState ps = (PlayingState)game.PlayingState;
                ps.AddingRoad(buildHere, px);
                return true;
            }
            return false;
        }

        //Buy a dev card
        public bool buildDevCard(Player px, SettlersOfCatan game)
        {
            if (px.canBuildDevCard)
            {
                PlayingState ps = (PlayingState)game.PlayingState;
                px = ps.gameCard.getCard(px); //does game card return somthing if you cant buy? if so check
                return true;
            }
            return false;
        }

        /***************************************************************************************
         * 
         *                      Game Initializing Goes Below Here
         *    
         ***************************************************************************************/


        /*
         * Purpose: Called to place settlements for the first time. Places settlement at
         *          the intersection with the highest combined probabiltiy of rolling the
         *          adjacent hex numbers.
         * Params:  px - the player rolling
         *          game - the SOC object for board access
         * Return:  None
         */
        public void placeSettlement(Player px, SettlersOfCatan game)
        {

            int maxProb = 0;
            GameNode placeHere = null;
            PlayingState ps = (PlayingState)game.PlayingState;

            List<GameNode> gns = game.gameBoard.placesForSettlements();
            foreach (GameNode gn in gns)
            {
                GameHex p1 = gn.hex1;
                GameHex p2 = gn.hex2;
                GameHex p3 = gn.hex3;

                int probCount = getProbability(p1.hexNumber) + getProbability(p2.hexNumber) + getProbability(p3.hexNumber);
                if (probCount > maxProb)
                {
                    placeHere = gn;
                    maxProb = probCount;
                }

            }

            //Claim that spot
            ps.AddingSettlement(placeHere, px);
            placeRoad(px, placeHere, game.gameBoard.boardArray);
        }

        /*
         * Placing the initial roads.  Selecting the road needs improved though.
         */
        public void placeRoad(Player px, GameNode current, object[,] boardArray)
        {
            List<GameRoad> roadOptions = new List<GameRoad>();

            for (int x = 1; x < 12; x++)
            {
                for (int y = 1; y < 22; y++)
                {
                    if (boardArray[x, y] is GameRoad)
                    {
                        GameRoad tmp = (GameRoad)boardArray[x, y];
                        if ((tmp.Node1 == current) || (tmp.Node2 == current))
                            roadOptions.Add(tmp);
                    }
                }
            }

            //Claim the road... THIS NEEDS SOPHISTICATED IT JUST TAKES THE FIRST AVAILABLE NOW
            roadOptions[0].owner = px.playerNumber;
        }


        /***************************************************************************************
         * 
         *                     Robber Related Functions Goes Below Here
         *    
         ***************************************************************************************/

        //Purpose:  Handle all functionality for the AI to move the Robber
        //Params:   px - the Player that is moving the Robber
        //          game - the overall game object
        //Return:   None
        public void handleRobber(Player px, SettlersOfCatan game)
        {
            //Get the Player with the most victory points
            Player highestOpponent = highestScoringOpponent(px.playerNumber, game.players);
            //Get a list of possible locations to move the Robber to
            List<GameHex> locations = possibleLocations(game, px.playerNumber, highestOpponent, false);

            //Randomly pick one of those locations
            Random rdm = new Random();
            game.gameBoard.setRobber(locations[rdm.Next(0, locations.Count())]);

            //Steal resource card
            int resourceType = px.stealRandomCard(highestOpponent);
            px.incResource(resourceType);

            px.SetBuildBools();
            px.ResourceSum();
        }


        //Purpose:  Get a list of GameHexs that are possible places to put the robber. Ideally, you place
        //          the Robber on a hex that won't affect you, only your opponents.
        //Params:   game - the overall game object
        //          aiPlayerNumber - the current AI's playerNumber
        //          highestOpponent - the Player (not including the current AI) with the highest victory points total
        //          leastObjectionable - a boolean indicating whether to accept a non-ideal return list
        //Return:   A List of GameHexs that are possible locations to move the Robber to
        public List<GameHex> possibleLocations(SettlersOfCatan game, int aiPlayerNumber, Player highestOpponent, bool leastObjectionable)
        {
            List<GameHex> possibilities = new List<GameHex>();
            List<GameNode> surroundings = null;
            bool addMe = false;
            bool filled = false;

            while (!filled)
            {
                foreach (GameHex gh in game.gameBoard.gameHexes)
                {
                    addMe = false;
                    surroundings = game.gameBoard.nodesSurroundingHex(gh);
                    foreach (GameNode gn in surroundings)
                    {
                        if (!leastObjectionable)
                        {
                            //Dont place the robber on a hex current AI touches
                            if (gn.owner == aiPlayerNumber)
                            {
                                addMe = false;
                                break;
                            }
                            //Do want to place the robber on a hex with the highest scoring player
                            else if (gn.owner == highestOpponent.playerNumber)
                                addMe = true;
                        }
                        else
                        {
                            //Find any hex with that the highest scorer touches
                            if (gn.owner == highestOpponent.playerNumber)
                            {
                                addMe = true;
                                break;
                            }
                        }
                    }

                    //Met criteria so add it to the list of possible hexes to move to
                    if (addMe)
                        possibilities.Add(gh);
                }


                //Dont return until there is at least 1 hex in the List
                if (possibilities.Count() > 0)
                    filled = true;
                else
                {
                    possibilities = possibleLocations(game, aiPlayerNumber, highestOpponent, true);
                    filled = true;
                }
            }

            return possibilities;
        }

        public void discardHalf(Player px, SettlersOfCatan game)
        {
            int mustDiscard = (px.brick + px.wood + px.wool + px.ore + px.wheat) / 2;
            int discarded = 0;
            int[] discardedCards = new int[5];
            int[] currentResources = new int[5] { px.brick, px.wheat, px.wood, px.wool, px.ore };
            bool keepGoing = true;

            while (keepGoing)
            {
                int idxOfMost = indexOfMostResource(px, currentResources);
                discardedCards[idxOfMost]++;
                currentResources[idxOfMost]--;

                discarded++;

                if (mustDiscard == discarded)
                    keepGoing = false;
            }

            //actually remove everything in the discardedCards
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < discardedCards[x]; y++)
                {
                    px.decResource(x, px);
                }
            }

        }

        /***************************************************************************************
         * 
         *                      Trade Related Functions Below Here
         *    
         ***************************************************************************************/

        //consider returning an int and then decrementing / incrementing resouces in the playing state if thats easier

        //Purpose:	Try to trade for something
        //Param:	px - the Player
        //			game - the Game
        //Return:	False for no trade, true if you're proposing a trade
        public bool tryToTrade(Player px, SettlersOfCatan game)
        {
            int[] currentResources = resourcesToArray(px);
            int least = indexOfFewestResource(px);
            int most = indexOfMostResource(px);

            //Bank Trade First
            if (currentResources[most] > 4) //not perfect, could have a port that would change that...
            {
                tradeWithBank(px, game, least); //straight up trade dont screw arounds with proposeTrade()
                px.SetBuildBools();
                px.ResourceSum();                
                return false;
            }
            else
            {
                Random rdm = new Random();
                int option = rdm.Next(0, 4); //random number to decide which way to trade
                return proposeTrade(px, game, option); //propose trade
            }
            return false;
        }

        //Tries to trade for a resource type depending on int tradeFor
        public bool proposeTrade(Player theTrader, SettlersOfCatan game, int tradeFor)
        {
            Trade myTrade = null;
            switch (tradeFor)
            {
                case 0:
                    myTrade = tradeForSettlement(theTrader);
                    break;
                case 1:
                    myTrade = tradeForCity(theTrader);
                    break;
                case 2:
                    myTrade = tradeForRoad(theTrader);
                    break;
                case 3:
                    myTrade = tradeForCard(theTrader);
                    break;
            }

            if (myTrade != null)
            {
                PlayingState ps = (PlayingState)game.PlayingState;
                myTrade.tradeOffer.CopyTo(ps.currentTrade.tradeOffer, 0);
                myTrade.tradeRequest.CopyTo(ps.currentTrade.tradeRequest, 0);
                ps.currentTrade.requestingPlayer = theTrader;
                return true;
            }
            return false;
        }

        //Purpose:  trade for resources that help build settlements
        //          if you can get rid of ore do it, other wise get rid of
        //          whatever you have the most of
        //Params:   theTrader - the Player that is proposing the trade
        //Return:   A Trade object representing the proposed Trade
        public Trade tradeForSettlement(Player theTrader)
        {
            int[] currentResourceAry = resourcesToArray(theTrader);
            int[] myRequest = fillSettlementRequest(currentResourceAry);
            int[] myOffer = new int[5];

            //You have ore, try to unload it
            if (currentResourceAry[4] > 0)
            {
                if (currentResourceAry[4] == 1)
                    myOffer[4] = 1;
                else if ((currentResourceAry[4] == 2) || (currentResourceAry[4] == 3))
                    myOffer[4] = 2;
                else if (currentResourceAry[4] > 3)
                    myOffer[4] = 3;
            }
            //You don't have ore so try to trade 1:1 or 2:1 with your most plentiful
            else
            {
                int mostIDX = indexOfMostResource(theTrader, new int[1] { 4 });

                //No point in trading 1 resource you need for 1 resource you need, so dont check it
                if (currentResourceAry[mostIDX] == 2)
                    myOffer[mostIDX] = 1;
                else if (currentResourceAry[mostIDX] == 3)
                    myOffer[mostIDX] = 2;
                else if (currentResourceAry[mostIDX] > 3)
                    myOffer[mostIDX] = 3;

            }

            Trade myTrade = new Trade(theTrader, myOffer, myRequest);
            if (myTrade.isValid())
                return myTrade;
            else
                return null;
        }

        public Trade tradeForCity(Player theTrader)
        {
            int[] currentResourceAry = resourcesToArray(theTrader);
            int[] myRequest = fillCityRequest(currentResourceAry);
            int[] myOffer = new int[5];

            //Hang on to your ore [4] and grain [2]
            int mostIDX = indexOfMostResource(theTrader, new int[2] { 2, 4 });

            //No point in trading 1 resource you need for 1 resource you need, so dont check it
            if (currentResourceAry[mostIDX] == 1)
                myOffer[mostIDX] = 1;
            if (currentResourceAry[mostIDX] == 2)
                myOffer[mostIDX] = 1;
            else if (currentResourceAry[mostIDX] == 3)
                myOffer[mostIDX] = 2;
            else if (currentResourceAry[mostIDX] > 3)
                myOffer[mostIDX] = 3;

            Trade myTrade = new Trade(theTrader, myOffer, myRequest);
            if (myTrade.isValid())
                return myTrade;
            else
                return null;
        }

        public Trade tradeForRoad(Player theTrader)
        {
            int[] currentResourceAry = resourcesToArray(theTrader);
            int[] myRequest = fillRoadRequest(currentResourceAry);
            int[] myOffer = new int[5];

            //Road building is more likely to come from strategyType 0 where ore isn't important
            if (currentResourceAry[4] > 0)
            {
                if (currentResourceAry[4] == 1)
                    myOffer[4] = 1;
                else if ((currentResourceAry[4] == 2) || (currentResourceAry[4] == 3))
                    myOffer[4] = 2;
                else if (currentResourceAry[4] > 3)
                    myOffer[4] = 3;
            }
            else
            {
                int mostIDX = indexOfMostResource(theTrader, new int[1] { 4 });

                //No point in trading 1 resource you need for 1 resource you need, so dont check it
                if (currentResourceAry[mostIDX] == 2)
                    myOffer[mostIDX] = 1;
                else if (currentResourceAry[mostIDX] == 3)
                    myOffer[mostIDX] = 2;
                else if (currentResourceAry[mostIDX] > 3)
                    myOffer[mostIDX] = 3;
            }

            Trade myTrade = new Trade(theTrader, myOffer, myRequest);
            if (myTrade.isValid())
                return myTrade;
            else
                return null;
        }

        public Trade tradeForCard(Player theTrader)
        {
            int[] currentResourceAry = resourcesToArray(theTrader);
            int[] myRequest = fillCityRequest(currentResourceAry);
            int[] myOffer = new int[5];

            //Hang on to your ore [4] and grain [2] wool[3]
            int mostIDX = indexOfMostResource(theTrader, new int[3] { 2, 3, 4 });

            //No point in trading 1 resource you need for 1 resource you need, so dont check it
            if (currentResourceAry[mostIDX] == 2)
                myOffer[mostIDX] = 1;
            else if (currentResourceAry[mostIDX] == 3)
                myOffer[mostIDX] = 2;
            else if (currentResourceAry[mostIDX] > 3)
                myOffer[mostIDX] = 3;

            Trade myTrade = new Trade(theTrader, myOffer, myRequest);
            if (myTrade.isValid())
                return myTrade;
            else
                return null;
        }

        //bank idx is what you want to exchange with the bank
        //bank trade shoudl be good all the time, so return false because you
        //dont want to trade with the other players
        public void tradeWithBank(Player theTrader, SettlersOfCatan game, int bankIdx)
        {
            //Get a list of the nodes you own
            List<GameNode> ownedNodes = game.gameBoard.getPlayerNodes(theTrader);
            Trade myTrade = new Trade();
            int[] currentResources = resourcesToArray(theTrader);
            int resourceOffer;

            //Yuck
            foreach (GameNode gn in ownedNodes)
            {
                //Trade 2:1 for a brick
                if ((gn.brickPort) && (bankIdx == 0))
                {
                    resourceOffer = indexOfMostResource(theTrader, new int[1] { 0 });
                    myTrade = buildBankTrade(theTrader, 0, resourceOffer, 2);
                }
                //Trade 2:1 for a wheat
                else if ((gn.grainPort) && (bankIdx == 1))
                {
                    resourceOffer = indexOfMostResource(theTrader, new int[1] { 1 });
                    myTrade = buildBankTrade(theTrader, 1, resourceOffer, 2);
                }
                //Trade 2:1 for a wood 
                else if ((gn.woodPort) && (bankIdx == 2))
                {
                    resourceOffer = indexOfMostResource(theTrader, new int[1] { 2 });
                    myTrade = buildBankTrade(theTrader, 2, resourceOffer, 2);
                }
                //Trade 2:1 for a wool
                else if ((gn.woolPort) && (bankIdx == 3))
                {
                    resourceOffer = indexOfMostResource(theTrader, new int[1] { 3 });
                    myTrade = buildBankTrade(theTrader, 3, resourceOffer, 2);
                }
                //Trade 2:1 for an ore
                else if ((gn.orePort) && (bankIdx == 4))
                {
                    resourceOffer = indexOfMostResource(theTrader, new int[1] { 4 });
                    myTrade = buildBankTrade(theTrader, 4, resourceOffer, 2);
                }
                //Trade 3:1 for anything except what you want
                else if (gn.threePort)
                {
                    resourceOffer = indexOfMostResource(theTrader);
                    if ((currentResources[resourceOffer] > 3) && (resourceOffer != bankIdx))
                        myTrade = buildBankTrade(theTrader, bankIdx, resourceOffer, 3);
                }
                //Trade 4:1 for anything
                else
                {
                    resourceOffer = indexOfMostResource(theTrader);
                    if ((currentResources[resourceOffer] > 4) && (bankIdx != resourceOffer))
                        myTrade = buildBankTrade(theTrader, bankIdx, resourceOffer, 4);
                }

                if (myTrade != null)
                    break;

            }

            if (myTrade != null)
            {
                //Console.WriteLine("Trading With The Bank: " + theTrader.brick.ToString() + " " + theTrader.wheat.ToString() + " " + theTrader.wood.ToString() + " " + theTrader.wool.ToString() + " " + theTrader.ore.ToString());
                for (int x = 0; x < 5; x++)
                {
                    if(myTrade.tradeOffer[x] != 0)
                    {
                        for (int y = 0; y < myTrade.tradeOffer[x]; y++)
                        {
                            theTrader.decResource(x, theTrader);
                        }
                    }
                }

                for(int x = 0; x<5; x++)
                {
                    if (myTrade.tradeRequest[x] != 0)
                    {
                        for (int y = 0; y < myTrade.tradeRequest[x]; y++)
                        {
                            theTrader.incResource(x);
                        }
                    }
                }
                //Console.WriteLine("Traded With The Bank: " + theTrader.brick.ToString() + " " + theTrader.wheat.ToString() + " " + theTrader.wood.ToString() + " " + theTrader.wool.ToString() + " " + theTrader.ore.ToString());
                //PlayingState ps = (PlayingState)game.PlayingState;
                //ps.currentTrade = myTrade;
            }
        }

        public Trade buildBankTrade(Player theTrader, int resourceRequestIdx, int resourceOfferIdx, int offerAmount)
        {
            int[] myOffer = new int[5];
            myOffer[resourceOfferIdx] = offerAmount;
            int[] myReqest = new int[5];
            myReqest[resourceRequestIdx] = 1;
            Trade myTrade = new Trade(theTrader, myOffer, myReqest);
            return myTrade;
        }

        //1 Brick, 1 Wheat, 1 Lumber, 1 Wool
        public int[] fillSettlementRequest(int[] currentResources)
        {
            int[] myRequest = new int[5];
            for (int x = 0; x < 4; x++)
            {
                if (currentResources[x] == 0)
                    myRequest[x] = 1;
            }
            return myRequest;
        }

        //3 ore 2 grain
        public int[] fillCityRequest(int[] currentResources)
        {
            int[] myRequest = new int[5];

            //Fill Wheat Requirement
            if (currentResources[1] == 0)
                myRequest[1] = 2;
            else if (currentResources[1] == 1)
                myRequest[1] = 1;

            //Fill Ore Requirement 
            if (currentResources[4] == 0)
                myRequest[4] = 3;
            else if (currentResources[4] == 1)
                myRequest[4] = 2;
            else if (currentResources[4] == 2)
                myRequest[4] = 1;

            return myRequest;
        }

        //1 Brick, 1 Lumber
        public int[] fillRoadRequest(int[] currentResources)
        {
            int[] myRequest = new int[5];

            //Fill Brick Requirement
            if (currentResources[0] == 0)
                myRequest[0] = 1;

            //Fill Lumber Requirement
            if (currentResources[2] == 0)
                myRequest[2] = 1;

            return myRequest;
        }

        //1 Ore, 1 wool, 1 Grain
        public int[] fillCardRequest(int[] currentResources)
        {
            int[] myRequest = new int[5];

            //Fill Grain Requirment
            if (currentResources[1] == 0)
                myRequest[1] = 1;

            //Fill Wool Requirement
            if (currentResources[3] == 0)
                myRequest[3] = 1;

            //Fill Ore Requirement
            if (currentResources[4] == 0)
                myRequest[4] = 1;

            return myRequest;
        }

        //px is the ai player number
        //t is the trade being offered
        public bool evaluateTradeProposal(Player px, Trade t)
        {
            int[] playersResources = resourcesToArray(px);
            int numberOfRequests = 0;
            int numberOfAccepts = 0;

            for (int x = 0; x < 5; x++)
            {
                //If TradeRequest is greater than zero, the player is requesting that resource
                if (t.tradeRequest[x] > 0)
                {
                    numberOfRequests++;
                    //The Computer <i>can<i> make the trade
                    if (playersResources[x] >= t.tradeRequest[x])
                        numberOfAccepts++;
                }
            }

            bool agree = false;
            if (numberOfRequests == numberOfAccepts)
                agree = doResourcesHelp(t);

            return agree;
        }

        //Purpose:  Determine if it makes sense to do the trade
        //Params:   t - the Trade being offered
        //Return:   true if you agree, false otherwise
        public bool doResourcesHelp(Trade t)
        {
            bool agreeToTrade = true;

            //Roads and Settlements
            //You need everything but ore, so just reject all trades that have that
            if (strategyType == 0)
            {
                if (t.tradeOffer[4] > 0)
                    agreeToTrade = false;
            }
            //Cities and Army
            //Everything benefits you, so agree to all trades
            else if (strategyType == 1)
            {
                agreeToTrade = true;
            }
            //Hybrid
            //Not implemented, so don't agree to trades
            else if (strategyType == 2)
                agreeToTrade = true;
            //If we get here, an error occurred, so don't agree
            else
                agreeToTrade = false;

            return agreeToTrade;
        }

        //Switch around resources between the two AIs
        public void processTrade(Player traderOfferer, Player traderAcceptor, SettlersOfCatan game)
        {
            PlayingState ps = (PlayingState)game.PlayingState;

            //Loop through tradeRequest and add those to tradeOfferer and remove from tradeAccepter
            for (int x = 0; x < 5; x++)
            {
                if (ps.currentTrade.tradeRequest[x] != 0)
                {
                    for(int y=0; y< ps.currentTrade.tradeRequest[x]; y++)
                    {
                        traderOfferer.incResource(x);
                        traderOfferer.decResource(x, traderAcceptor);
                    }
                }
            }

            //Loop through tradeOffer and add those to tradeAccepter and remove from tradeOfferer
            for (int x = 0; x < 5; x++)
            {
                if (ps.currentTrade.tradeOffer[x] != 0)
                {
                    for (int y = 0; y < ps.currentTrade.tradeOffer[x]; y++)
                    {
                        traderAcceptor.incResource(x);
                        traderAcceptor.decResource(x, traderOfferer);
                    }
                }

            }

        }


        /***************************************************************************************
         * 
         *                      Utility Type Function Go Below Here
         *    
         ***************************************************************************************/



        //Purpose: A lazy way to determine the probability of rolling that number            
        //Param:   number - an int representing the number on the hex tile
        //Return:  An int representing the likelihood of rolling that number (Assuming 2 die, with 6 sides each) 
        //         Returns 0 for desert piece or ocean.
        public int getProbability(int number)
        {
            switch (number)
            {
                case 2: return 1;
                case 3: return 2;
                case 4: return 3;
                case 5: return 4;
                case 6: return 5;
                case 8: return 5;
                case 9: return 4;
                case 10: return 3;
                case 11: return 2;
                case 12: return 1;
            }
            return 0;
        }


        //Purpose:  Return the Player with the most victory points, excluding the current AI
        //Params:   currentAI - an int representing the current AI's playerNumber
        //          players - a List of all the Players in the game
        //Return:   The player with the most visible victory points
        public Player highestScoringOpponent(int currentAI, List<Player> players)
        {
            int highVP = 0;
            Player highest = null;

            for (int x = 0; x < players.Count(); x++)
            {
                if ((players[x].victoryPoints > highVP) && (players[x].playerNumber != currentAI))
                {
                    highVP = players[x].victoryPoints;
                    highest = players[x];
                }
            }
            return highest;
        }


        //Purpose:  Format the resources that Player px has into an array for looping
        //Param:    px - the player to get the resources for
        //Return:   An int array with specific resourceTypes corresponding to specific indexes
        public int[] resourcesToArray(Player px)
        {
            int[] resAry = new int[5];
            //[0] = brick
            resAry[0] = px.brick;
            //[1] = wheat
            resAry[1] = px.wheat;
            //[2] = wood
            resAry[2] = px.wood;
            //[3] = wool
            resAry[3] = px.wool;
            //[4] = ore
            resAry[4] = px.ore;

            return resAry;
        }


        //Purpose:  Randomly generate a number and use that to decide on a strategy for that game
        //Params:   None
        //Return:   An int representing the strategy to use
        public void pickStrategy()
        {
            Random rnd = new Random();
            int strategy = rnd.Next(0, 3);

            //Settlements and Roads
            if (strategy == 0)
                strategyType = 0;
            //Cities and Army
            else if (strategy == 1)
                strategyType = 1;
            //Hybrid
            else if (strategy == 2)
                strategyType = 2;
        }

        //Purpose:  Return a string for the resourceType based in the int passed in
        //Param:    resourceIDX - the index for the resource array to get the type of
        //Return:   A string with the resource type based on the index
        public string intToType(int resourceIDX)
        {
            if (resourceIDX == 0)
                return "Clay";
            else if (resourceIDX == 1)
                return "Grain";
            else if (resourceIDX == 2)
                return "Wood";
            else if (resourceIDX == 3)
                return "Sheep";
            else if (resourceIDX == 4)
                return "Rock";
            else
                return "";
        }

        //Purpose:  Return the index to the least plentiful resource, standard index pattern
        //          for this project applies.  You can suppress a value using the optional param
        //Params:   px - the Player having their resources checked
        //          suppress - optional, an index for a resource type you don't want to check
        //Returns:  The index to a resource array of the least plentiful resource type
        public int indexOfFewestResource(Player px, int suppress = 999)
        {
            int[] currentResources = resourcesToArray(px);

            int min = 999;
            int idxOfMin = 999;
            for (int x = 0; x < 5; x++)
            {
                if ((currentResources[x] < min) && (x != suppress))
                {
                    min = currentResources[x];
                    idxOfMin = x;
                }
            }
            return idxOfMin;
        }

        //Purpose:  Return the index to the most plentiful resource, standard index pattern for this
        //          project applies.  You can optionally suppress a certain type
        //Params:   px - the Player who is having their resources checked
        //          suppress - optional param to suppress a value from being marked as most
        //                   helpful for excluding a resource from a list
        //Return:   The index to a resource array of the most plentiful resource type
        public int indexOfMostResource(Player px, int[] suppress = null)
        {
            int[] currentResources = resourcesToArray(px);

            int max = -1;
            int idxOfMax = 999;
            for (int x = 0; x < 5; x++)
            {
                if (currentResources[x] > max)
                {
                    if (suppress == null)
                    {
                        max = currentResources[x];
                        idxOfMax = x;
                    }
                    else
                    {
                        for (int y = 0; y < suppress.Length; y++)
                        {
                            if (x != suppress[y])
                            {
                                max = currentResources[x];
                                idxOfMax = x;
                            }
                        }
                    }

                }
            }
            return idxOfMax;
        }

        //Purpose:  overload for getting the max of an array
        //          project applies.  You can optionally suppress a certain type
        //Params:   px - the Player who is having their resources checked
        //          suppress - optional param to suppress a value from being marked as most
        //                   helpful for excluding a resource from a list
        //Return:   The index to a resource array of the most plentiful resource type
        public int indexOfMostResource(Player px, int[] currentResources, int[] suppress = null)
        {
            int max = -1;
            int idxOfMax = 999;
            for (int x = 0; x < 5; x++)
            {
                if (currentResources[x] > max)
                {
                    if (suppress == null)
                    {
                        max = currentResources[x];
                        idxOfMax = x;
                    }
                    else
                    {
                        for (int y = 0; y < suppress.Length; y++)
                        {
                            if (x != suppress[y])
                            {
                                max = currentResources[x];
                                idxOfMax = x;
                            }
                        }
                    }

                }
            }
            return idxOfMax;
        }

        //Purpose:  Use this to evaluate when the player is ready to stop
        //Params:   px
        //Return:   True if done with turn, false otherwise
        public bool endTurn(Player px)
        {
            if (px.hiddenVictoryPoints == 10)
                return true;
            return false;
        }

    }
}