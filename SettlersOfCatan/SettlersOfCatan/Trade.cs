using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    public class Trade
    {

        //What requestingPlayer is offering
        public int[] tradeOffer { get; set; }

        //What requestingPlayer is requesting
        public int[] tradeRequest { get; set; }

        public int[] yopRequest { get; set; }

        //The player propsing the trade
        public Player requestingPlayer { get; set; }

        //For AI purposes, the passed in arrays must follow this construct
        //[0] = brick
        //[1] = wheat
        //[2] = wood
        //[3] = wool
        //[4] = ore

        //Zero arg constructor
        public Trade() 
        {
            tradeOffer = new int[5];
            tradeRequest = new int[5];
            yopRequest = new int[5];
        }

        //Construct the Trade object
        public Trade(Player px, int[] offer, int[] request)
        {
            tradeOffer = new int[5];
            tradeRequest = new int[5];

            for (int x = 0; x < 5; x++)
            {
                tradeOffer[x] = offer[x];
            }

            for (int x = 0; x < 5; x++)
            {
                tradeRequest[x] = request[x];
            }

            if (px != null)
                requestingPlayer = px;
        }

        public int countOffers()
        {
            return this.tradeOffer[0] + this.tradeOffer[1] + this.tradeOffer[2] + this.tradeOffer[3] + this.tradeOffer[4];
        }

        public int countRequests()
        {
            return this.tradeRequest[0] + this.tradeRequest[1] + this.tradeRequest[2] + this.tradeRequest[3] + this.tradeRequest[4];
        }
        
        public bool isValid()
        {
            if((this.countRequests() != 0) && (this.countOffers() != 0))
                return true;
            else
                return false;
        }

        //Purpose: Gives resources requested from year of plenty trade
        public void yopTrade(Player px)
        {
            for (int j = 0; j < yopRequest.Length; j++)
            {
                if (yopRequest[j] == 1)
                {
                    px.incResource(j);
                }
                if (yopRequest[j] == 2)
                {
                    px.incResource(j);
                    px.incResource(j);
                }
            }
        }

        //Purpose: increments/decrements appropriate resources for the two players trading
        public void humanTradeWithAI(Player humanPlayer, Player aiTradingWith)
        {
            for (int j = 0; j < 5; j++)
            {
                if (tradeOffer[j] > 0)
                {
                    while (tradeOffer[j] > 0)
                    {
                        humanPlayer.decResource(j, humanPlayer);
                        aiTradingWith.incResource(j);
                        tradeOffer[j]--;
                    }
                }
                if (tradeRequest[j] > 0)
                {
                    while (tradeRequest[j] > 0)
                    {
                        humanPlayer.incResource(j);
                        aiTradingWith.decResource(j, aiTradingWith);
                        tradeRequest[j]--;
                    }
                }
            }
        }

        //Purpose: make sure robber give away is correct amount of cards an if so remove the cards offered
        //Returns: boolean that says if this was valid or not
        public bool robberGiveAway(Player px)
        {
            int[] temp = new int[5];
            yopRequest.CopyTo(temp, 0);
            int amountGive = px.totalResources / 2;
            int counter = 0;
            for (int j = 0; j < yopRequest.Length; j++)
            {
                if (temp[j] > 0)
                {
                    while (temp[j] > 0)
                    {
                        counter++;
                        temp[j]--;
                    }
                }
            }
            if (counter == amountGive)
            {
                for (int j = 0; j < yopRequest.Length; j++)
                {
                    if (yopRequest[j] > 0)
                    {
                        while (yopRequest[j] > 0)
                        {
                            px.decResource(j, px);
                            yopRequest[j]--;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        //Purpose: to see if trade to bank was valid, if so decrements player cards offered and increments player cards desired
        //Returns: boolean to determine whether the offer was a valid trade offer
        public bool isBankTradeValid(Player px)
        {
            bool tradeComplete = false;

            if (px.hasBrick && tradeOffer[0] > 1 && px.brick > 1)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tradeRequest[j] > 0)
                    {
                        if (j != 0)
                        {
                            px.decResource(0, px);
                            px.decResource(0, px);
                            px.incResource(j);
                            tradeComplete = true;
                            return tradeComplete;
                        }
                    }
                }           
            }
            if (px.hasGrain && tradeOffer[1] > 1 && px.wheat > 1)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tradeRequest[j] > 0)
                    {
                        if (j != 1)
                        {
                            px.decResource(1, px);
                            px.decResource(1, px);
                            px.incResource(j);
                            tradeComplete = true;
                            return tradeComplete;
                        }
                    }
                }
            }
            if (px.hasLumber && tradeOffer[2] > 1 && px.wood > 1)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tradeRequest[j] > 0)
                    {
                        if (j != 2)
                        {
                            px.decResource(2, px);
                            px.decResource(2, px);
                            px.incResource(j);
                            tradeComplete = true;
                            return tradeComplete;
                        }
                    }
                } 
            }
            if (px.hasWool && tradeOffer[3] > 1 && px.wool > 1)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tradeRequest[j] > 0)
                    {
                        if (j != 4)
                        {
                            px.decResource(3, px);
                            px.decResource(3, px);
                            px.incResource(j);
                            tradeComplete = true;
                            return tradeComplete;
                        }
                    }
                } 
            }
            if (px.hasOre && tradeOffer[4] > 1 && px.ore > 1)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tradeRequest[j] > 0)
                    {
                        if (j != 3)
                        {
                            px.decResource(4, px);
                            px.decResource(4, px);
                            px.incResource(j);
                            tradeComplete = true;
                            return tradeComplete;
                        }
                    }
                }
            }
            if (px.hasThree)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (tradeOffer[i] > 2)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (tradeRequest[j] > 0)
                            {
                                if (j != i)
                                {
                                    px.decResource(i, px);
                                    px.decResource(i, px);
                                    px.decResource(i, px);
                                    px.incResource(j);
                                    tradeComplete = true;
                                    return tradeComplete;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (tradeOffer[i] > 3)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (tradeRequest[j] > 0)
                            {
                                if (j != i)
                                {
                                    px.decResource(i, px);
                                    px.decResource(i, px);
                                    px.decResource(i, px);
                                    px.decResource(i, px);
                                    px.incResource(j);
                                    tradeComplete = true;
                                    return tradeComplete;
                                }
                            }
                        }
                    }
                }
            }
            return tradeComplete;
        }
    }
}
