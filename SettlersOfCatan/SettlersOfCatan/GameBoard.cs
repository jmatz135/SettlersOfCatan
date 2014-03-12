using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    public class GameBoard
    {
        public List<GameHex> gameHexes = new List<GameHex>();
        public object[,] boardArray;
        public int[] randPorts;

        public GameBoard(string gameType)
        {
            switch (gameType)
            {
                case "BaseGame":
                    {                                 
                        string[] boardNumbers = { "5", "2", "6", "3", "8", "10", "9", "12", "11", "4", "8", "10", "9", "4", "5", "6", "3", "11" };
                        string[] boardHexes = {"Desert", "Sheep", "Sheep","Sheep","Sheep", "Clay", "Clay", "Clay", "Rock", "Rock", "Rock",
                            "Grain", "Grain", "Grain", "Grain","Wood", "Wood", "Wood", "Wood"};
                        Random rnd = new Random();
                        string[] randHex = boardHexes.OrderBy(x => rnd.Next()).ToArray();
                        string[,] finishedHexes = new String[19, 2];
                        int y = 0;
                        for (int x = 0; x < 19; x++)
                        {
                            string Desert = "Desert";
                            finishedHexes[x, 0] = randHex[x];
                            if (string.ReferenceEquals(randHex[x], Desert))
                            {
                                finishedHexes[x, 1] = "0";
                            }
                            else
                            {
                                finishedHexes[x, 1] = boardNumbers[y];
                                y++;
                            }
                        }
                        int[] ports = { 1, 2, 3, 4, 5, 0, 0, 0, 0 };
                        randPorts = ports.OrderBy(x => rnd.Next()).ToArray();

                        //Game Hexes (hexType, hexNumber, row, columns
                        GameHex GH1 = new GameHex(finishedHexes[0, 0], int.Parse(finishedHexes[0, 1]), 1, 7);
                        GameHex GH2 = new GameHex(finishedHexes[1, 0], int.Parse(finishedHexes[1, 1]), 1, 5);
                        GameHex GH3 = new GameHex(finishedHexes[2, 0], int.Parse(finishedHexes[2, 1]), 1, 3);
                        GameHex GH4 = new GameHex(finishedHexes[3, 0], int.Parse(finishedHexes[3, 1]), 2, 2);
                        GameHex GH5 = new GameHex(finishedHexes[4, 0], int.Parse(finishedHexes[4, 1]), 3, 1);
                        GameHex GH6 = new GameHex(finishedHexes[5, 0], int.Parse(finishedHexes[5, 1]), 4, 2);
                        GameHex GH7 = new GameHex(finishedHexes[6, 0], int.Parse(finishedHexes[6, 1]), 5, 3);
                        GameHex GH8 = new GameHex(finishedHexes[7, 0], int.Parse(finishedHexes[7, 1]), 5, 5);
                        GameHex GH9 = new GameHex(finishedHexes[8, 0], int.Parse(finishedHexes[8, 1]), 5, 7);
                        GameHex GH10 = new GameHex(finishedHexes[9, 0], int.Parse(finishedHexes[9, 1]), 4, 8);
                        GameHex GH11 = new GameHex(finishedHexes[10, 0], int.Parse(finishedHexes[10, 1]), 3, 9);
                        GameHex GH12 = new GameHex(finishedHexes[11, 0], int.Parse(finishedHexes[11, 1]), 2, 8);
                        GameHex GH13 = new GameHex(finishedHexes[12, 0], int.Parse(finishedHexes[12, 1]), 2, 6);
                        GameHex GH14 = new GameHex(finishedHexes[13, 0], int.Parse(finishedHexes[13, 1]), 2, 4);
                        GameHex GH15 = new GameHex(finishedHexes[14, 0], int.Parse(finishedHexes[14, 1]), 3, 3);
                        GameHex GH16 = new GameHex(finishedHexes[15, 0], int.Parse(finishedHexes[15, 1]), 4, 4);
                        GameHex GH17 = new GameHex(finishedHexes[16, 0], int.Parse(finishedHexes[16, 1]), 4, 6);
                        GameHex GH18 = new GameHex(finishedHexes[17, 0], int.Parse(finishedHexes[17, 1]), 3, 7);
                        GameHex GH19 = new GameHex(finishedHexes[18, 0], int.Parse(finishedHexes[18, 1]), 3, 5);

                        GameHex Ocean = new GameHex("Ocean", -1, -5, -5);

                        gameHexes.Add(GH1);
                        gameHexes.Add(GH2);
                        gameHexes.Add(GH3);
                        gameHexes.Add(GH4);
                        gameHexes.Add(GH5);
                        gameHexes.Add(GH6);
                        gameHexes.Add(GH7);
                        gameHexes.Add(GH8);
                        gameHexes.Add(GH9);
                        gameHexes.Add(GH10);
                        gameHexes.Add(GH11);
                        gameHexes.Add(GH12);
                        gameHexes.Add(GH13);
                        gameHexes.Add(GH14);
                        gameHexes.Add(GH15);
                        gameHexes.Add(GH16);
                        gameHexes.Add(GH17);
                        gameHexes.Add(GH18);
                        gameHexes.Add(GH19);
                        gameHexes.Add(Ocean);

                        //Place the Robber on the Desert
                        initializeRobber();

                        //Game Nodes (locx, locy, player, hex1, hex2, hex4, settleType)
                        GameNode A5 = new GameNode(1, 5, 0, Ocean, Ocean, GH1, 0);
                        GameNode A7 = new GameNode(1, 7, 0, Ocean, Ocean, GH1, 0);
                        GameNode A9 = new GameNode(1, 9, 0, GH1, Ocean, GH2, 0);
                        GameNode A11 = new GameNode(1, 11, 0, Ocean, Ocean, GH2, 0);
                        GameNode A13 = new GameNode(1, 13, 0, GH2, Ocean, GH3, 0);
                        GameNode A15 = new GameNode(1, 15, 0, Ocean, Ocean, GH3, 0);
                        GameNode A17 = new GameNode(1, 17, 0, GH3, Ocean, Ocean, 0);

                        GameNode C3 = new GameNode(3, 3, 0, Ocean, Ocean, GH12, 0);
                        GameNode C5 = new GameNode(3, 5, 0, Ocean, GH1, GH12, 0);
                        GameNode C7 = new GameNode(3, 7, 0, GH12, GH1, GH13, 0);
                        GameNode C9 = new GameNode(3, 9, 0, GH1, GH2, GH13, 0);
                        GameNode C11 = new GameNode(3, 11, 0, GH13, GH2, GH14, 0);
                        GameNode C13 = new GameNode(3, 13, 0, GH2, GH3, GH14, 0);
                        GameNode C15 = new GameNode(3, 15, 0, GH14, GH3, GH4, 0);
                        GameNode C17 = new GameNode(3, 17, 0, GH3, Ocean, GH4, 0);
                        GameNode C19 = new GameNode(3, 19, 0, GH4, Ocean, Ocean, 0);

                        GameNode E1 = new GameNode(5, 1, 0, Ocean, Ocean, GH11, 0);
                        GameNode E3 = new GameNode(5, 3, 0, Ocean, GH12, GH11, 0);
                        GameNode E5 = new GameNode(5, 5, 0, GH11, GH12, GH18, 0);
                        GameNode E7 = new GameNode(5, 7, 0, GH12, GH13, GH18, 0);
                        GameNode E9 = new GameNode(5, 9, 0, GH18, GH13, GH19, 0);
                        GameNode E11 = new GameNode(5, 11, 0, GH13, GH14, GH19, 0);
                        GameNode E13 = new GameNode(5, 13, 0, GH19, GH14, GH15, 0);
                        GameNode E15 = new GameNode(5, 15, 0, GH14, GH4, GH15, 0);
                        GameNode E17 = new GameNode(5, 17, 0, GH15, GH4, GH5, 0);
                        GameNode E19 = new GameNode(5, 19, 0, GH4, Ocean, GH5, 0);
                        GameNode E21 = new GameNode(5, 21, 0, GH5, Ocean, Ocean, 0);

                        GameNode G1 = new GameNode(7, 1, 0, Ocean, GH11, Ocean, 0);
                        GameNode G3 = new GameNode(7, 3, 0, Ocean, GH11, GH10, 0);
                        GameNode G5 = new GameNode(7, 5, 0, GH11, GH18, GH10, 0);
                        GameNode G7 = new GameNode(7, 7, 0, GH10, GH18, GH17, 0);
                        GameNode G9 = new GameNode(7, 9, 0, GH18, GH19, GH17, 0);
                        GameNode G11 = new GameNode(7, 11, 0, GH17, GH19, GH16, 0);
                        GameNode G13 = new GameNode(7, 13, 0, GH19, GH15, GH16, 0);
                        GameNode G15 = new GameNode(7, 15, 0, GH16, GH15, GH6, 0);
                        GameNode G17 = new GameNode(7, 17, 0, GH15, GH5, GH6, 0);
                        GameNode G19 = new GameNode(7, 19, 0, GH6, GH5, Ocean, 0);
                        GameNode G21 = new GameNode(7, 21, 0, GH5, Ocean, Ocean, 0);

                        GameNode I3 = new GameNode(9, 3, 0, Ocean, GH10, Ocean, 0);
                        GameNode I5 = new GameNode(9, 5, 0, Ocean, GH10, GH9, 0);
                        GameNode I7 = new GameNode(9, 7, 0, GH10, GH17, GH9, 0);
                        GameNode I9 = new GameNode(9, 9, 0, GH9, GH17, GH8, 0);
                        GameNode I11 = new GameNode(9, 11, 0, GH17, GH16, GH8, 0);
                        GameNode I13 = new GameNode(9, 13, 0, GH8, GH16, GH7, 0);
                        GameNode I15 = new GameNode(9, 15, 0, GH16, GH6, GH7, 0);
                        GameNode I17 = new GameNode(9, 17, 0, GH7, GH6, Ocean, 0);
                        GameNode I19 = new GameNode(9, 19, 0, GH6, Ocean, Ocean, 0);

                        GameNode K5 = new GameNode(11, 5, 0, Ocean, GH9, Ocean, 0);
                        GameNode K7 = new GameNode(11, 7, 0, Ocean, GH9, Ocean, 0);
                        GameNode K9 = new GameNode(11, 9, 0, GH9, GH8, Ocean, 0);
                        GameNode K11 = new GameNode(11, 11, 0, Ocean, GH8, Ocean, 0);
                        GameNode K13 = new GameNode(11, 13, 0, GH8, GH7, Ocean, 0);
                        GameNode K15 = new GameNode(11, 15, 0, Ocean, GH7, Ocean, 0);
                        GameNode K17 = new GameNode(11, 17, 0, GH7, Ocean, Ocean, 0);
                        C3.AddPort(randPorts[0]);
                        E3.AddPort(randPorts[0]);
                        A5.AddPort(randPorts[1]);
                        A7.AddPort(randPorts[1]);
                        A11.AddPort(randPorts[2]);
                        A13.AddPort(randPorts[2]);
                        C17.AddPort(randPorts[3]);
                        C19.AddPort(randPorts[3]);
                        E21.AddPort(randPorts[4]);
                        G21.AddPort(randPorts[4]);
                        I19.AddPort(randPorts[5]);
                        I17.AddPort(randPorts[5]);
                        K11.AddPort(randPorts[6]);
                        K13.AddPort(randPorts[6]);
                        K5.AddPort(randPorts[7]);
                        K7.AddPort(randPorts[7]);
                        G3.AddPort(randPorts[8]);
                        I3.AddPort(randPorts[8]);

                        //Game Roads (locx, locy, player, Node1, Node2)
                        GameRoad A6 = new GameRoad(1, 6, 0, A5, A7);
                        GameRoad A8 = new GameRoad(1, 8, 0, A7, A9);
                        GameRoad A10 = new GameRoad(1, 10, 0, A9, A11);
                        GameRoad A12 = new GameRoad(1, 12, 0, A11, A13);
                        GameRoad A14 = new GameRoad(1, 14, 0, A13, A15);
                        GameRoad A16 = new GameRoad(1, 16, 0, A15, A17);

                        GameRoad B5 = new GameRoad(2, 5, 0, A5, C5);
                        GameRoad B9 = new GameRoad(2, 9, 0, A9, C9);
                        GameRoad B13 = new GameRoad(2, 13, 0, A13, C13);
                        GameRoad B17 = new GameRoad(2, 17, 0, A17, C17);

                        GameRoad C4 = new GameRoad(3, 4, 0, C3, C5);
                        GameRoad C6 = new GameRoad(3, 6, 0, C5, C7);
                        GameRoad C8 = new GameRoad(3, 8, 0, C7, C9);
                        GameRoad C10 = new GameRoad(3, 10, 0, C9, C11);
                        GameRoad C12 = new GameRoad(3, 12, 0, C11, C13);
                        GameRoad C14 = new GameRoad(3, 14, 0, C13, C15);
                        GameRoad C16 = new GameRoad(3, 16, 0, C15, C17);
                        GameRoad C18 = new GameRoad(3, 18, 0, C17, C19);

                        GameRoad D3 = new GameRoad(4, 3, 0, C3, E3);
                        GameRoad D7 = new GameRoad(4, 7, 0, C7, E7);
                        GameRoad D11 = new GameRoad(4, 11, 0, C11, E11);
                        GameRoad D15 = new GameRoad(4, 15, 0, C15, E15);
                        GameRoad D19 = new GameRoad(4, 19, 0, C19, E19);

                        GameRoad E2 = new GameRoad(5, 2, 0, E1, E3);
                        GameRoad E4 = new GameRoad(5, 4, 0, E3, E5);
                        GameRoad E6 = new GameRoad(5, 6, 0, E5, E7);
                        GameRoad E8 = new GameRoad(5, 8, 0, E7, E9);
                        GameRoad E10 = new GameRoad(5, 10, 0, E9, E11);
                        GameRoad E12 = new GameRoad(5, 12, 0, E11, E13);
                        GameRoad E14 = new GameRoad(5, 14, 0, E13, E15);
                        GameRoad E16 = new GameRoad(5, 16, 0, E15, E17);
                        GameRoad E18 = new GameRoad(5, 18, 0, E17, E19);
                        GameRoad E20 = new GameRoad(5, 20, 0, E19, E21);

                        GameRoad F1 = new GameRoad(6, 1, 0, E1, G1);
                        GameRoad F5 = new GameRoad(6, 5, 0, E5, G5);
                        GameRoad F9 = new GameRoad(6, 9, 0, E9, G9);
                        GameRoad F13 = new GameRoad(6, 13, 0, E13, G13);
                        GameRoad F17 = new GameRoad(6, 17, 0, E17, G17);
                        GameRoad F21 = new GameRoad(6, 21, 0, E21, G21);

                        GameRoad G2 = new GameRoad(7, 2, 0, G1, G3);
                        GameRoad G4 = new GameRoad(7, 4, 0, G3, G5);
                        GameRoad G6 = new GameRoad(7, 6, 0, G5, G7);
                        GameRoad G8 = new GameRoad(7, 8, 0, G7, G9);
                        GameRoad G10 = new GameRoad(7, 10, 0, G9, G11);
                        GameRoad G12 = new GameRoad(7, 12, 0, G11, G13);
                        GameRoad G14 = new GameRoad(7, 14, 0, G13, G15);
                        GameRoad G16 = new GameRoad(7, 16, 0, G15, G17);
                        GameRoad G18 = new GameRoad(7, 18, 0, G17, G19);
                        GameRoad G20 = new GameRoad(7, 20, 0, G19, G21);

                        GameRoad H3 = new GameRoad(8, 3, 0, G3, I3);
                        GameRoad H7 = new GameRoad(8, 7, 0, G7, I7);
                        GameRoad H11 = new GameRoad(8, 11, 0, G11, I11);
                        GameRoad H15 = new GameRoad(8, 15, 0, G15, I15);
                        GameRoad H19 = new GameRoad(8, 19, 0, G19, I19);

                        GameRoad I4 = new GameRoad(9, 4, 0, I3, I5);
                        GameRoad I6 = new GameRoad(9, 6, 0, I5, I7);
                        GameRoad I8 = new GameRoad(9, 8, 0, I7, I9);
                        GameRoad I10 = new GameRoad(9, 10, 0, I9, I11);
                        GameRoad I12 = new GameRoad(9, 12, 0, I11, I13);
                        GameRoad I14 = new GameRoad(9, 14, 0, I13, I15);
                        GameRoad I16 = new GameRoad(9, 16, 0, I15, I17);
                        GameRoad I18 = new GameRoad(9, 18, 0, I17, I19);

                        GameRoad J5 = new GameRoad(10, 5, 0, I5, K5);
                        GameRoad J9 = new GameRoad(10, 9, 0, I9, K9);
                        GameRoad J13 = new GameRoad(10, 13, 0, I13, K13);
                        GameRoad J17 = new GameRoad(10, 17, 0, I17, K17);

                        GameRoad K6 = new GameRoad(11, 6, 0, K5, K7);
                        GameRoad K8 = new GameRoad(11, 8, 0, K7, K9);
                        GameRoad K10 = new GameRoad(11, 10, 0, K9, K11);
                        GameRoad K12 = new GameRoad(11, 12, 0, K11, K13);
                        GameRoad K14 = new GameRoad(11, 14, 0, K13, K15);
                        GameRoad K16 = new GameRoad(11, 16, 0, K15, K17);

                        //Board Array, array of all GameNodes and GameRoads
                        boardArray = new object[12, 22];
                        boardArray[1, 5] = A5;
                        boardArray[1, 6] = A6;
                        boardArray[1, 7] = A7;
                        boardArray[1, 8] = A8;
                        boardArray[1, 9] = A9;
                        boardArray[1, 10] = A10;
                        boardArray[1, 11] = A11;
                        boardArray[1, 12] = A12;
                        boardArray[1, 13] = A13;
                        boardArray[1, 14] = A14;
                        boardArray[1, 15] = A15;
                        boardArray[1, 16] = A16;
                        boardArray[1, 17] = A17;
                        boardArray[2, 5] = B5;
                        boardArray[2, 9] = B9;
                        boardArray[2, 13] = B13;
                        boardArray[2, 17] = B17;
                        boardArray[3, 3] = C3;
                        boardArray[3, 4] = C4;
                        boardArray[3, 5] = C5;
                        boardArray[3, 6] = C6;
                        boardArray[3, 7] = C7;
                        boardArray[3, 8] = C8;
                        boardArray[3, 9] = C9;
                        boardArray[3, 10] = C10;
                        boardArray[3, 11] = C11;
                        boardArray[3, 12] = C12;
                        boardArray[3, 13] = C13;
                        boardArray[3, 14] = C14;
                        boardArray[3, 15] = C15;
                        boardArray[3, 16] = C16;
                        boardArray[3, 17] = C17;
                        boardArray[3, 18] = C18;
                        boardArray[3, 19] = C19;
                        boardArray[4, 3] = D3;
                        boardArray[4, 7] = D7;
                        boardArray[4, 11] = D11;
                        boardArray[4, 15] = D15;
                        boardArray[4, 19] = D19;
                        boardArray[5, 1] = E1;
                        boardArray[5, 2] = E2;
                        boardArray[5, 3] = E3;
                        boardArray[5, 4] = E4;
                        boardArray[5, 5] = E5;
                        boardArray[5, 6] = E6;
                        boardArray[5, 7] = E7;
                        boardArray[5, 8] = E8;
                        boardArray[5, 9] = E9;
                        boardArray[5, 10] = E10;
                        boardArray[5, 11] = E11;
                        boardArray[5, 12] = E12;
                        boardArray[5, 13] = E13;
                        boardArray[5, 14] = E14;
                        boardArray[5, 15] = E15;
                        boardArray[5, 16] = E16;
                        boardArray[5, 17] = E17;
                        boardArray[5, 18] = E18;
                        boardArray[5, 19] = E19;
                        boardArray[5, 20] = E20;
                        boardArray[5, 21] = E21;
                        boardArray[6, 1] = F1;
                        boardArray[6, 5] = F5;
                        boardArray[6, 9] = F9;
                        boardArray[6, 13] = F13;
                        boardArray[6, 17] = F17;
                        boardArray[6, 21] = F21;
                        boardArray[7, 1] = G1;
                        boardArray[7, 2] = G2;
                        boardArray[7, 3] = G3;
                        boardArray[7, 4] = G4;
                        boardArray[7, 5] = G5;
                        boardArray[7, 6] = G6;
                        boardArray[7, 7] = G7;
                        boardArray[7, 8] = G8;
                        boardArray[7, 9] = G9;
                        boardArray[7, 10] = G10;
                        boardArray[7, 11] = G11;
                        boardArray[7, 12] = G12;
                        boardArray[7, 13] = G13;
                        boardArray[7, 14] = G14;
                        boardArray[7, 15] = G15;
                        boardArray[7, 16] = G16;
                        boardArray[7, 17] = G17;
                        boardArray[7, 18] = G18;
                        boardArray[7, 19] = G19;
                        boardArray[7, 20] = G20;
                        boardArray[7, 21] = G21;
                        boardArray[8, 3] = H3;
                        boardArray[8, 7] = H7;
                        boardArray[8, 11] = H11;
                        boardArray[8, 15] = H15;
                        boardArray[8, 19] = H19;
                        boardArray[9, 3] = I3;
                        boardArray[9, 4] = I4;
                        boardArray[9, 5] = I5;
                        boardArray[9, 6] = I6;
                        boardArray[9, 7] = I7;
                        boardArray[9, 8] = I8;
                        boardArray[9, 9] = I9;
                        boardArray[9, 10] = I10;
                        boardArray[9, 11] = I11;
                        boardArray[9, 12] = I12;
                        boardArray[9, 13] = I13;
                        boardArray[9, 14] = I14;
                        boardArray[9, 15] = I15;
                        boardArray[9, 16] = I16;
                        boardArray[9, 17] = I17;
                        boardArray[9, 18] = I18;
                        boardArray[9, 19] = I19;
                        boardArray[10, 5] = J5;
                        boardArray[10, 9] = J9;
                        boardArray[10, 13] = J13;
                        boardArray[10, 17] = J17;
                        boardArray[11, 5] = K5;
                        boardArray[11, 6] = K6;
                        boardArray[11, 7] = K7;
                        boardArray[11, 8] = K8;
                        boardArray[11, 9] = K9;
                        boardArray[11, 10] = K10;
                        boardArray[11, 11] = K11;
                        boardArray[11, 12] = K12;
                        boardArray[11, 13] = K13;
                        boardArray[11, 14] = K14;
                        boardArray[11, 15] = K15;
                        boardArray[11, 16] = K16;
                        boardArray[11, 17] = K17;

                        break;
                    }
                case "CustomGame":
                    {

                        break;
                    }
            }
        }

        //                                 Quick Reference
        //placesForSettlement() - call to find out where anyone can build settlements
        //placesForCities() - call to find what settlements you own, which is where you can build cities
        //placesForRoads() - call to find our what roads are available for building
        //surroundingSettlements() - a helper to determine if a GameNode has any neighboring GameNodes built
        //getConnectingRoads() - a helper to get the 2 or 3 GameRoads that connect to a supplied GameNode
        //getPlayerNodes() - a helper to get all the nodes that a Player owns
        //getPlayerNodesAndRoads() - a helper to get all the owned property of a Player

        //Purpose: Places the robber during initialization of the board
        //Params: None
        //Return: None
        public void initializeRobber()
        {
            foreach (GameHex gh in gameHexes)
            {
                if (gh.hexType == "Desert")
                {
                    gh.hasRobber = true;
                    return;
                }
            }
        }

        //Not finished yet, right now it's set up more for automated taking
        //Need more information on wiring it up to the GUI.
        //But the logic is there
        public void moveRobber(GameHex moveHere, Player px)
        {
            //Move the robber
            setRobber(moveHere);

            //Get the 6 nodes surrounding it
            List<GameNode> buildingAround = nodesSurroundingHex(moveHere);

            bool keepGoing = true;
            Random rdm = new Random();
            while (keepGoing)
            {
                int random = rdm.Next(0, 6);
                GameNode tmp = buildingAround[random];
                if ((tmp.owner != 0) && (tmp.owner != px.playerNumber))
                {

                    //steal a random card from tmp.owner
                    keepGoing = false;
                }
            }
        }

        //Purpose: Moves the Robber from its current location to moveToHere
        //Params: moveToHere - a GameHex where the robber is to be moved
        //Return: None
        public void setRobber(GameHex moveToHere)
        {
            GameHex currentRobber = findRobber();
            foreach (GameHex gh in gameHexes)
            {
                //We can place it when we find it, but it can't be moved to where it already is
                if ((gh == moveToHere) && (gh != currentRobber))
                {
                    currentRobber.hasRobber = false;
                    gh.hasRobber = true;
                    return;
                }
            }
        }

        //Purpose: Find the current location of the robber
        //Param: None
        //Return: The GameHex where the Robber resides, null if there is a problem
        public GameHex findRobber()
        {
            foreach (GameHex gh in gameHexes)
            {
                if (gh.hasRobber)
                    return gh;
            }
            return null;
        }

        //Purpose:  Get the 6 GameNodes that surround GameHex gh
        //          Pass this the robber's hex to get the nodes that surround the robber
        //Params: gh - the GameHex that we're requesting the nodes for
        //Return: A list of the 6 GameNodes that surround the GameHex gh
        public List<GameNode> nodesSurroundingHex(GameHex gh)
        {
            List<GameNode> gn = new List<GameNode>();

            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    if (boardArray[x, y] is GameNode)
                    {
                        GameNode tmp = (GameNode)boardArray[x, y];
                        if ((tmp.hex1 == gh) || (tmp.hex2 == gh) || (tmp.hex3 == gh))
                            gn.Add(tmp);
                    }
                }
            }
            return gn;
        }

        //Purpose: To return a stack of GameNodes that show the nodes that are able to have settlements built on
        //Params: None
        //Return: A Stack of GameNodes with all possible build locations
        public List<GameNode> placesForSettlements()
        {
            List<GameNode> gns = new List<GameNode>();

            for (int x = 1; x < 12; x++)
            {
                for (int y = 1; y < 22; y++)
                {
                    if (boardArray[x, y] is GameNode)
                    {
                        GameNode tmp = (GameNode)boardArray[x, y];

                        //If owner is 0, and there are no surrounding settlements then its not owned and available for settlment
                        if ((tmp.owner == 0) && (!surroundingSettlements(tmp)))
                            gns.Add(tmp);
                    }
                }
            }
            return gns;
        }

        //Purpose: To return a stack of GameNodes that show where the player can build
        //         can only build where they have roads right now
        //Params: px - the player you're getting ownership for
        //Return: A Stack of GameNodes with all possible build locations
        public List<GameNode> placesForSettlements(Player px)
        {
            List<GameNode> gns = new List<GameNode>();
            List<GameRoad> grs = getPlayerRoads(px);

            foreach (GameRoad gr in grs)
            {
                GameNode gn1 = gr.Node1;
                GameNode gn2 = gr.Node2;

                if (gn1.owner == 0)
                {
                    if (!surroundingSettlements(gn1))
                        gns.Add(gn1);
                }
                if (gn2.owner == 0)
                    if (!surroundingSettlements(gn2))
                        gns.Add(gn2);
            }
            return gns;
        }

        //Purpose: To return a Stack of GameNodes that Player px owns (has settlements on), which means they can become cities
        //Params: px - a player object representing who is trying to build a city
        //Return: A Stack of GameNodes with the players current settlements
        public List<GameNode> placesForCities(Player px)
        {
            List<GameNode> allNodes = getPlayerNodes(px);
            List<GameNode> justSettlements = new List<GameNode>();
            foreach (GameNode gn in allNodes)
            {
                //ASSUMING SETTLETYPE 0 FOR SETTLEMENT AND 1 FOR CITIES
                if (gn.settleType == 0)
                    justSettlements.Add(gn);
            }
            return justSettlements;
        }

        //Purpose: To return a Stack of GameNodes that Player px has cities on
        //Params: px - a player object representing who is trying to build a city
        //Return: A Stack of GameNodes with the players current cities
        public List<GameNode> getCitiesNodes(Player px)
        {
            List<GameNode> allNodes = getPlayerNodes(px);
            List<GameNode> justSettlements = new List<GameNode>();
            foreach (GameNode gn in allNodes)
            {
                //ASSUMING SETTLETYPE 0 FOR SETTLEMENT AND 1 FOR CITIES
                if (gn.settleType == 1)
                    justSettlements.Add(gn);
            }
            return justSettlements;
        }

        //Purpose: To return a list of GameRoads that are available for Player px to build
        //Params: px - a player object representing who is trying to build a road
        //Return: An IEnumerable of distinct GameRoads where Player px can build.
        public IEnumerable<GameRoad> placesForRoads(Player px, GameNode settlement = null)
        {
            List<GameRoad> grs = new List<GameRoad>();

            if (settlement != null)
            {
                List<GameRoad> grl = getConnectingRoads(settlement);
                foreach (GameRoad gr in grl)
                {
                    if (gr.owner == 0)
                        grs.Add(gr);
                }
            }
            else
            {
                List<object> gns = getPlayerNodesAndRoads(px);
                foreach (object obj in gns)
                {
                    if (obj is GameNode)
                    {
                        GameNode tmp = (GameNode)obj;
                        List<GameRoad> tmpStk = getConnectingRoads(tmp);

                        foreach (GameRoad gr in tmpStk)
                        {
                            if (gr.owner == 0)
                                grs.Add(gr);
                        }

                    }
                    else if (obj is GameRoad)
                    {
                        GameRoad grt = (GameRoad)obj;
                        GameNode[] endPoints = new GameNode[2] { grt.Node1, grt.Node2 };

                        for (int i = 0; i < 2; i++)
                        {
                            if (endPoints[i].owner != 0) { /*do nothing*/}
                            else
                            {
                                List<GameRoad> connectedTo = getConnectingRoads(endPoints[i]);
                                foreach (GameRoad tmp in connectedTo)
                                {
                                    if (tmp.owner == 0)
                                        grs.Add(tmp);
                                }
                            }
                        }
                    }
                }
            }

            IEnumerable<GameRoad> distinctRoads = grs.Distinct();
            return distinctRoads;
        }


        //Purpose: Call to determine if the passed in node, gn, is surrounded by any settlements
        //Params: gn - the GameNode that we are checking for neighbors
        //Return: true if there is a settlement or city within 1 road of gn
        //        false if it is okay to build a settlement at gn
        public bool surroundingSettlements(GameNode gn)
        {
            List<GameRoad> connectingRoads = getConnectingRoads(gn);
            
            foreach (GameRoad gr in connectingRoads)
            {
                if ((gr.Node1.owner != 0) || (gr.Node2.owner != 0))
                    return true;
            }
            return false;
        } 

        //Purpose: To return a Stack of the GameRoads that connect to the given GameNode, gn
        //Params: gn - the GameNode to find the connecting roads to
        //Return: A Stack of the 2 or 3 roads that connect to gn.  Ownership is not considered.
        public List<GameRoad> getConnectingRoads(GameNode gn)
        {

            List<GameRoad> grs = new List<GameRoad>();

            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    if (boardArray[x, y] is GameRoad)
                    {
                        GameRoad grTmp = (GameRoad)boardArray[x, y];
                        if ((grTmp.Node1 == gn) || (grTmp.Node2 == gn))
                            grs.Add(grTmp);
                    }
                }
            }
            return grs;
        }

        //Purpose: Used to get a Stack of all the GameNodes that Player px owns
        //Params: px - the Player who we are getting all GameNodes for
        //Return: A Stack of all the GameNodes that Player px owns
        public List<GameNode> getPlayerNodes(Player px)
        {
            List<GameNode> gns = new List<GameNode>();

            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    if (boardArray[x, y] is GameNode)
                    {
                        GameNode tmp = (GameNode)boardArray[x, y];
                        if (tmp.owner == px.playerNumber)
                            gns.Add(tmp);
                    }
                }
            }
            return gns;
        }

        //Purpose: Craetes a Stack of all the GameNodes and GameRoads that Player px owns
        //Params: px - the Player we're getting owned pieces for
        //Return: A Stack of all the GameNode and GameRoad objects that Player px owns.
        public List<object> getPlayerNodesAndRoads(Player px)
        {
            List<object> nnr = new List<object>();

            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    if (boardArray[x, y] is GameNode)
                    {
                        GameNode tmp = (GameNode)boardArray[x, y];
                        if (tmp.owner == px.playerNumber)
                            nnr.Add(tmp);
                    }
                    else if (boardArray[x, y] is GameRoad)
                    {
                        GameRoad tmp = (GameRoad)boardArray[x, y];
                        if (tmp.owner == px.playerNumber)
                            nnr.Add(tmp);
                    }
                }
            }
            return nnr;
        }

        //Purpose: Craetes a Stack of all the GameRoads that Player px owns
        //Params: px - the Player we're getting owned pieces for
        //Return: A Stack of all the GameRoad objects that Player px owns.
        public List<GameRoad> getPlayerRoads(Player px)
        {
            List<GameRoad> gr = new List<GameRoad>();

            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    if (boardArray[x, y] is GameRoad)
                    {
                        GameRoad tmp = (GameRoad)boardArray[x, y];
                        if (tmp.owner == px.playerNumber)
                            gr.Add(tmp);
                    }
                }
            }
            return gr;
        }

        //Purpose: To return hex based upon the row and column given
        //Params: r - the row the hex is in, c - the column the hex is in
        //Return: Hexagon corresponding to the row and column given
        public GameHex getHexInfo(GameHex gh)
        {
            GameHex boardHex = new GameHex();
            for (int j = 0; j < 20; j++)
            {
                if (gameHexes[j].row == gh.row && gameHexes[j].column == gh.column)
                {
                    boardHex.hexType = gameHexes[j].hexType;
                    boardHex.hexNumber = gameHexes[j].hexNumber;
                    boardHex.hasRobber = gameHexes[j].hasRobber;
                    return boardHex;
                }
            }
            return boardHex;
        }

        //Purpose: To set settlement/City information for the given player on node
        //Params: gn - GameNode, type - settlement type to be set, px - player to be set
        public void setNodeInfo(GameNode gn, int type, Player px)
        {
            gn.settleType = type;
            gn.player = px.playerNumber;
            gn.owner = px.playerNumber;
        }

        //Purpose: To set settlement/City information for the given player on node
        //Params: gn - GameNode, type - settlement type to be set, px - player to be set
        public void setRoadInfo(GameRoad gr, Player px)
        {
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    if (boardArray[x, y] == gr)
                    {
                        gr.owner = px.playerNumber;
                    }
                }
            }
        }

        //Get a list of players that have settlements around the hex provided
        public IEnumerable<Player> eligibleVictims(GameHex gh, SettlersOfCatan game, Player mover)
        {
            List<Player> playerList = new List<Player>();
            List<GameNode> surroundSettles = nodesSurroundingHex(gh);
            foreach (GameNode gn in surroundSettles)
            {
                foreach (Player px in game.players)
                {
                    if ((px.playerNumber == gn.owner) && (mover.playerNumber != px.playerNumber))
                    {
                        playerList.Add(px);
                    }
                }
            }
            IEnumerable<Player> players = playerList.Distinct();
            return players;
        }

        //Purpose: Get resources for based on the dice roll for all of the players
        public void getResources(List<Player> players, int a)
        {
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 18; y++)
                {
                    if (boardArray[x, y] is GameNode)
                    {
                        GameNode Temp = (GameNode)boardArray[x, y];
                        int implement = 1;
                        if (Temp.settleType == 1)
                        {
                            implement = 2;
                        }
                        if (Temp.owner != 0 && Temp.hex1.hexNumber == a && !Temp.hex1.hasRobber)
                        {
                            if (Temp.hex1.hexType == "Wood")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].wood = players[i].wood + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex1.hexType == "Sheep")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].wool = players[i].wool + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex1.hexType == "Rock")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].ore = players[i].ore + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex1.hexType == "Grain")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].wheat = players[i].wheat + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex1.hexType == "Clay")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].brick = players[i].brick + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                        }
                        if (Temp.owner != 0 && Temp.hex2.hexNumber == a && !Temp.hex2.hasRobber)
                        {
                            if (Temp.hex2.hexType == "Wood")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].wood = players[i].wood + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex2.hexType == "Sheep")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].wool = players[i].wool + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex2.hexType == "Rock")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].ore = players[i].ore + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex2.hexType == "Grain")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].wheat = players[i].wheat + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex2.hexType == "Clay")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].brick = players[i].brick + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                        }
                        if (Temp.owner != 0 && Temp.hex3.hexNumber == a && !Temp.hex3.hasRobber)
                        {
                            if (Temp.hex3.hexType == "Wood")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].wood = players[i].wood + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex3.hexType == "Sheep")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].wool = players[i].wool + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex3.hexType == "Rock")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].ore = players[i].ore + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex3.hexType == "Grain")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].wheat = players[i].wheat + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                            if (Temp.hex3.hexType == "Clay")
                            {
                                for (int i = 0; i < players.Count; i++)
                                {
                                    if (Temp.owner == players[i].playerNumber)
                                    {
                                        players[i].brick = players[i].brick + implement;
                                        players[i].ResourceSum();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //Purpose: Get resources from surrounding nodes after settlement is placed during game startup
        public void GameStartSettlementUpdate(GameNode gn, Player px)
        {
            if (gn.hex1.hexType == "Wood")
            {
                px.wood++;
                px.ResourceSum();
            }
            if (gn.hex1.hexType == "Sheep")
            {
                px.wool++;
                px.ResourceSum();
            }
            if (gn.hex1.hexType == "Rock")
            {
                px.ore++;
                px.ResourceSum();
            }
            if (gn.hex1.hexType == "Grain")
            {
                px.wheat++;
                px.ResourceSum();
            }
            if (gn.hex1.hexType == "Clay")
            {
                px.brick++;
                px.ResourceSum();
            }
            if (gn.hex2.hexType == "Wood")
            {
                px.wood++;
                px.ResourceSum();
            }
            if (gn.hex2.hexType == "Sheep")
            {
                px.wool++;
                px.ResourceSum();
            }
            if (gn.hex2.hexType == "Rock")
            {
                px.ore++;
                px.ResourceSum();
            }
            if (gn.hex2.hexType == "Grain")
            {
                px.wheat++;
                px.ResourceSum();
            }
            if (gn.hex2.hexType == "Clay")
            {
                px.brick++;
                px.ResourceSum();
            }
            if (gn.hex3.hexType == "Wood")
            {
                px.wood++;
                px.ResourceSum();
            }
            if (gn.hex3.hexType == "Sheep")
            {
                px.wool++;
                px.ResourceSum();
            }
            if (gn.hex3.hexType == "Rock")
            {
                px.ore++;
                px.ResourceSum();
            }
            if (gn.hex3.hexType == "Grain")
            {
                px.wheat++;
                px.ResourceSum();
            }
            if (gn.hex3.hexType == "Clay")
            {
                px.brick++;
                px.ResourceSum();
            }
        }
                           

    }//End Class GameBoard
}