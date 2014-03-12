using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{

    public class LongestRoad
    {
        public int[,] ForkArray = new int[6, 6];

        SettlersOfCatan OurGame;
        PlayingState playingState;
        GameBoard roadBoard;
        public int leftTotal;
        public int rightTotal;


        public LongestRoad(SettlersOfCatan game, PlayingState state)
        {
            OurGame = (SettlersOfCatan)game;
            playingState = (PlayingState)state;
            roadBoard= OurGame.gameBoard;


            leftTotal = 0;
            rightTotal = 0;
        }
        //Calls Recursive methods to find out the longest road on either side of the road that is placed
        public Player longestRoad(int a, int b, Player px)
        {
            LLongestRoad(a, b, 0, 0, 0, a, b, px);
            RLongestRoad(a, b, 0, 0, 0, a, b, px);
            int totalLength = (leftTotal + rightTotal - 1);
            if (totalLength > px.longestRoad)
            {
                px.longestRoad = totalLength;
            }
            //check to see if player has the longest road
            //Requirement 3.4.4
            if (px.longestRoad > 4)
            {
                //Requirements 3.4.5
                if (OurGame.aiPlayer1.longestRoadPoints == true)
                {
                    OurGame.aiPlayer1.longestRoadPoints = false;
                    OurGame.aiPlayer1.hiddenVictoryPoints = OurGame.aiPlayer1.hiddenVictoryPoints - 2;
                    OurGame.aiPlayer1.victoryPoints = OurGame.aiPlayer1.victoryPoints - 2;
                }
                if (OurGame.aiPlayer2.longestRoadPoints == true)
                {
                    OurGame.aiPlayer2.longestRoadPoints = false;
                    OurGame.aiPlayer2.hiddenVictoryPoints = OurGame.aiPlayer2.hiddenVictoryPoints - 2;
                    OurGame.aiPlayer2.victoryPoints = OurGame.aiPlayer2.victoryPoints - 2;
                }
                if (OurGame.humanPlayer.longestRoadPoints == true)
                {
                    OurGame.humanPlayer.longestRoadPoints = false;
                    OurGame.humanPlayer.hiddenVictoryPoints = OurGame.humanPlayer.hiddenVictoryPoints - 2;
                    OurGame.humanPlayer.victoryPoints = OurGame.humanPlayer.victoryPoints - 2;
                }
                //Requirement 3.4.6
                px.longestRoadPoints = true;   //need to make sure others get changed to false
                px.hiddenVictoryPoints = px.hiddenVictoryPoints + 2;
                px.victoryPoints = px.victoryPoints + 2;
            }
            return px;
        }



        public void LLongestRoad(int a, int b, int tempLength, int lastX, int lastY, int startX, int startY, Player px)
        {
            
            
            int tempX=a;
            int tempY=b;
            int arrayint=0;
            int numberOfPaths = 0;
            GameLocation tempNode= (GameLocation)roadBoard.boardArray[tempX--, tempY];
            GameLocation tempRoad=(GameLocation)roadBoard.boardArray[tempX--, tempY++];
           while (tempLength<15)
           {
            //Check to see board location because it matters for how we adjust are x and y coordinates
            if (tempLength>leftTotal)
            {
                leftTotal=tempLength;
            }
            if (a == 2 || a == 4 || a == 6 || a == 8 || a == 10)
            {
                // Check to make sure that there isn't an opponents settlement in the way on the left
                

                if (tempNode.owner == px.playerNumber ||  tempNode.owner == 0)
                {

                    if (tempRoad.owner == px.playerNumber && tempX--!=lastX && tempY++!=lastY) 
                    {
                        
                        LLongestRoad(tempX--, tempY++, tempLength++, tempX, tempY, startX, startY, px);
                    }
                    tempRoad=(GameLocation)roadBoard.boardArray[tempX--, tempY--];
                    if (tempRoad.owner == px.playerNumber  && tempX--!=lastX && tempY--!=lastY) 
                    {
                        LLongestRoad(tempX--, tempY--, tempLength++, tempX, tempY, startX, startY, px);
                    }
                }
                // Check to make sure that there isn't an opponents settlement in the way on the right
                if (tempX != startX && tempY != startY)
                {

                    {
                        tempNode= (GameLocation)roadBoard.boardArray[tempX++, tempY];
                        if (tempNode.owner == px.playerNumber  || tempNode.owner == 0)
                        {
                            tempRoad=(GameLocation)roadBoard.boardArray[tempX++, tempY++];
                            if (tempRoad.owner == px.playerNumber  && tempX++!=lastX && tempY++!=lastY) 
                            {
                                LLongestRoad(tempX++, tempY++, tempLength++, tempX, tempY, startX, startY, px);
                            }
                            tempRoad=(GameLocation)roadBoard.boardArray[tempX++, tempY--];
                            if (tempRoad.owner == px.playerNumber  && tempX++!=lastX && tempY--!=lastY) 
                            {
                                LLongestRoad(tempX++, tempY--, tempLength++, tempX, tempY, startX, startY, px);
                            }
                        }
                    }
                }
            else if (b == 4 || b == 8 || b == 12 || b == 16 || b == 20)
            {
                // Check to make sure that there isn't an opponents settlement in the way on the top
                tempNode= (GameLocation)roadBoard.boardArray[tempX, tempY--];
                if (tempNode.owner == px.playerNumber  || tempNode.owner == 0)
                {
                     tempRoad=(GameLocation)roadBoard.boardArray[tempX, tempY-2];
                    if (tempRoad.owner == px.playerNumber  && tempX-- != lastX && tempY-2 != lastY)
                    {
                        LLongestRoad(tempX, tempY-2, tempLength++, tempX, tempY, startX, startY, px);
                    }
                    tempRoad=(GameLocation)roadBoard.boardArray[tempX--, tempY--];
                    if (tempRoad.owner == px.playerNumber  && tempX-- != lastX && tempY-- != lastY)
                    {
                        LLongestRoad(tempX--, tempY--, tempLength++, tempX, tempY, startX, startY, px);
                    }
                }
                // Check to make sure that there isn't an opponents settlement in the way on the bottom
                 if (tempX != startX && tempY != startY)
                {
                   tempNode= (GameLocation)roadBoard.boardArray[tempX, tempY++];
                        if (tempNode.owner == px.playerNumber  || tempNode.owner == 0)
                        {
                            tempRoad=(GameLocation)roadBoard.boardArray[tempX, tempY+2];
                            if (tempRoad.owner== px.playerNumber  && tempX != lastX && tempY+2 != lastY)
                            {
                                LLongestRoad(tempX, tempY+2, tempLength++, tempX, tempY, startX, startY, px);
                            }
                            tempRoad=(GameLocation)roadBoard.boardArray[tempX++, tempY++];
                            if (tempRoad.owner == px.playerNumber && tempX++ != lastX && tempY++ != lastY)
                            {
                                LLongestRoad(tempX++, tempY++, tempLength++, tempX, tempY, startX, startY, px);
                            }
                        }
                    
                 }
            }
            else
            {


                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // Check to make sure that there isn't an opponents settlement in the way on the top
                tempNode= (GameLocation)roadBoard.boardArray[tempX, tempY--];
                if (tempNode.owner == px.playerNumber  || tempNode.owner == 0)
                {
                    tempRoad= (GameLocation)roadBoard.boardArray[tempX++, tempY--];
                    if (tempRoad.owner== px.playerNumber  && tempX++ != lastX && tempY-- != lastY)
                    {
                        LLongestRoad(tempX++, tempY--, tempLength++, tempX, tempY, startX, startY, px);
                    }
                    tempRoad= (GameLocation)roadBoard.boardArray[tempX, tempY-2];
                    if (tempRoad.owner == px.playerNumber  && tempX != lastX && tempY-2 != lastY)
                    {
                        LLongestRoad(tempX, tempY-2, tempLength++, tempX, tempY, startX, startY, px);
                    }
                }
                // Check to make sure that there isn't an opponents settlement in the way on the bottom
                if (tempX != startX && tempY != startY)
                {

                    tempNode = (GameLocation)roadBoard.boardArray[tempX, tempY++];
                    if (tempNode.owner == px.playerNumber || tempNode.owner == 0)
                    {
                        tempRoad = (GameLocation)roadBoard.boardArray[tempX--, tempY++];
                        if (tempRoad.owner == px.playerNumber && tempX-- != lastX && tempY++ != lastY)
                        {
                            LLongestRoad(tempX--, tempY++, tempLength++, tempX, tempY, startX, startY, px);
                        }
                        tempRoad = (GameLocation)roadBoard.boardArray[tempX, tempY + 2];
                        if (tempRoad.owner == px.playerNumber && tempX != lastX && tempY + 2 != lastY)
                        {
                            LLongestRoad(tempX, tempY + 2, tempLength++, tempX, tempY, startX, startY, px);
                        }
                    }
                }
  
                 }
            }
        }
    }
        public void RLongestRoad(int a, int b, int tempLength, int lastX, int lastY, int startX, int startY, Player px)
        {
            while (tempLength < 16)
            {
                tempLength = tempLength;
                int tempX = a;
                int tempY = b;
                int arrayint = 0;
                int numberOfPaths = 0;
                lastX = lastX;
                lastY = lastY;
                GameLocation tempNode = (GameLocation)roadBoard.boardArray[tempX, tempY++];
                GameLocation tempRoad = (GameLocation)roadBoard.boardArray[tempX, tempY++];
                if (tempLength > rightTotal)
                {
                    rightTotal = tempLength;
                }
                //Check to see board location because it matters for how we adjust are x and y coordinates
                if (a == 2 || a == 4 || a == 6 || a == 8 || a == 10)
                {
                    if (tempX != startX && tempY != startY)
                    {
                        tempNode = (GameLocation)roadBoard.boardArray[tempX--, tempY++];
                        // Check to make sure that there isn't an opponents settlement in the way on the left
                        if (tempNode.owner == px.playerNumber || tempNode.owner == 0)
                        {
                            tempRoad = (GameLocation)roadBoard.boardArray[tempX--, tempY++];
                            if (tempRoad.owner == px.playerNumber && tempX-- != lastX && tempY++ != lastY)
                            {

                                RLongestRoad(tempX--, tempY++, tempLength++, tempX, tempY, startX, startY, px);
                            }
                            tempRoad = (GameLocation)roadBoard.boardArray[tempX--, tempY--];
                            if (tempRoad.owner == px.playerNumber && tempX-- != lastX && tempY-- != lastY)
                            {
                                RLongestRoad(tempX--, tempY--, tempLength++, tempX, tempY, startX, startY, px);
                            }
                        }

                    }

                    // Check to make sure that there isn't an opponents settlement in the way on the right
                    tempNode = (GameLocation)roadBoard.boardArray[tempX++, tempY];
                    if (tempNode.owner == px.playerNumber || tempNode.owner == 0)
                    {
                        tempRoad = (GameLocation)roadBoard.boardArray[tempX++, tempY++];
                        if (tempRoad.owner == px.playerNumber && tempX++ != lastX && tempY++ != lastY)
                        {
                            RLongestRoad(tempX++, tempY++, tempLength++, tempX, tempY, startX, startY, px);
                        }
                        tempRoad = (GameLocation)roadBoard.boardArray[tempX, tempY--];
                        if (tempRoad.owner == px.playerNumber && tempX++ != lastX && tempY-- != lastY)
                        {
                            RLongestRoad(tempX++, tempY--, tempLength++, tempX, tempY, startX, startY, px);
                        }
                    }

                    else if (b == 4 || b == 8 || b == 12 || b == 16 || b == 20)
                    {
                        if (tempX != startX && tempY != startY)
                        {


                            // Check to make sure that there isn't an opponents settlement in the way on the top
                            tempNode = (GameLocation)roadBoard.boardArray[tempX, tempY--];
                            if (tempNode.owner == px.playerNumber || tempNode.owner == 0)
                            {
                                tempRoad = (GameLocation)roadBoard.boardArray[tempX, tempY = 2];
                                if (tempRoad.owner == px.playerNumber && tempX-- != lastX && tempY - 2 != lastY)
                                {
                                    RLongestRoad(tempX, tempY - 2, tempLength++, tempX, tempY, startX, startY, px);
                                }
                                tempRoad = (GameLocation)roadBoard.boardArray[tempX--, tempY--];
                                if (tempRoad.owner == px.playerNumber && tempX-- != lastX && tempY-- != lastY)
                                {
                                    RLongestRoad(tempX--, tempY--, tempLength++, tempX, tempY, startX, startY, px);
                                }

                            }
                        }
                        // Check to make sure that there isn't an opponents settlement in the way on the bottom
                        tempNode = (GameLocation)roadBoard.boardArray[tempX, tempY++];
                        if (tempNode.owner == px.playerNumber || tempNode.owner == 0)
                        {
                            tempRoad = (GameLocation)roadBoard.boardArray[tempX++, tempY + 2];
                            if (tempRoad.owner == px.playerNumber && tempX != lastX && tempY + 2 != lastY)
                            {
                                RLongestRoad(tempX, tempY + 2, tempLength++, tempX, tempY, startX, startY, px);
                            }
                            tempRoad = (GameLocation)roadBoard.boardArray[tempX++, tempY++];
                            if (tempRoad.owner == px.playerNumber && tempX++ != lastX && tempY++ != lastY)
                            {
                                RLongestRoad(tempX++, tempY++, tempLength++, tempX, tempY, startX, startY, px);
                            }
                        }
                    }

                    else
                    {
                        if (tempX != startX && tempY != startY)
                        {

                            // Check to make sure that there isn't an opponents settlement in the way on the top
                            tempNode = (GameLocation)roadBoard.boardArray[tempX, tempY--];
                            if (tempNode.owner == px.playerNumber || tempNode.owner == 0)
                            {
                                tempRoad = (GameLocation)roadBoard.boardArray[tempX++, tempY--];
                                if (tempRoad.owner == px.playerNumber && tempX++ != lastX && tempY-- != lastY)
                                {
                                    RLongestRoad(tempX++, tempY--, tempLength++, tempX, tempY, startX, startY, px);
                                }
                                tempRoad = (GameLocation)roadBoard.boardArray[tempX, tempY - 2];
                                if (tempRoad.owner == px.playerNumber && tempX != lastX && tempY - 2 != lastY)
                                {
                                    RLongestRoad(tempX, tempY - 2, tempLength++, tempX, tempY, startX, startY, px);
                                }
                            }

                        }
                        // Check to make sure that there isn't an opponents settlement in the way on the bottom
                        tempNode = (GameLocation)roadBoard.boardArray[tempX, tempY++];
                        if (tempNode.owner == px.playerNumber || tempNode.owner == 0)
                        {
                            tempRoad = (GameLocation)roadBoard.boardArray[tempX--, tempY++];
                            if (tempRoad.owner == px.playerNumber && tempX-- != lastX && tempY++ != lastY)
                            {
                                RLongestRoad(tempX--, tempY++, tempLength++, tempX, tempY, startX, startY, px);
                            }
                            tempRoad = (GameLocation)roadBoard.boardArray[tempX, tempY + 2];
                            if (tempRoad.owner == px.playerNumber && tempX != lastX && tempY + 2 != lastY)
                            {
                                RLongestRoad(tempX, tempY + 2, tempLength++, tempX, tempY, startX, startY, px);
                            }
                        }
                    }
                }
            }
        
    }
    }
}