/*Draws the game board using Sprites
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using SOCProjectLibrary;

namespace SettlersOfCatan
{
    class DrawGameBoard2D : DrawableGameComponent
    {
        //Port Textures
        Texture2D port;
        Texture2D brickIcon;
        Texture2D grainIcon;
        Texture2D lumberIcon;
        Texture2D oreIcon;
        Texture2D woolIcon;

        //Game Hexes
        Texture2D brick;
        Texture2D desert;
        Texture2D grain;
        Texture2D lumber;
        Texture2D ore;
        Texture2D sea;
        Texture2D wool;

        //Roads
        Texture2D blueRoad;
        Texture2D orangeRoad;
        Texture2D whiteRoad;
        Texture2D roadSquare;

        //Settlements
        Texture2D blueHouse;
        Texture2D orangeHouse;
        Texture2D whiteHouse;
        Texture2D blueSettlement;
        Texture2D orangeSettlement;
        Texture2D whiteSettlement;

        //Numbers
        Texture2D two;
        Texture2D three;
        Texture2D four;
        Texture2D five;
        Texture2D six;
        Texture2D seven;
        Texture2D eight;
        Texture2D nine;
        Texture2D ten;
        Texture2D eleven;
        Texture2D twelve;
        private List<Texture2D> numbers;

        Texture2D robber;

        private BasicEffect effect;
        BasicEffect basicEffect;

        private SettlersOfCatan ourGame;
        private PlayingState playingState;

        //Screen dimension variables
        private int screenWidth;
        private int screenHeight;
        private int halfWidth;
        private int halfHeight;
        Rectangle screenRectangle;

        //Sprite Scaling and positioning variables
        float hexScale = 0.35f;
        float settleScale = 0.8f;
        float roadScale = 0.4f;
        private Vector2 hexMiddle;
        private Vector2 settlePos;
        private Vector2 roadPos;
        private Vector2 portOrigin;

        //Stacks to hold information on settlements, cities, and roads
        List<GameNode> settleToBuild;
        List<GameNode> settleToCity;
        IEnumerable<GameRoad> buildRoad;

        public GameNode initset;

        //Constructor
        public DrawGameBoard2D(Game game, PlayingState state)
            : base(game)
        {
            ourGame = (SettlersOfCatan)game;
            playingState = (PlayingState)state;
            screenWidth = ourGame.width;
            screenHeight = ourGame.height;
            halfWidth = screenWidth / 2;
            halfHeight = screenHeight / 2;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        // Purpose: Import 2D Sprites that make up the sprites to be placed on the gameboard, such as towns
        // Create the basic effect graphics devices
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            ourGame.SpriteBatch = new SpriteBatch(GraphicsDevice);

            //Port Textures
            port = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Port");
            brickIcon = ourGame.Content.Load<Texture2D>(@"Textures\Board\Icon_Brick");
            grainIcon = ourGame.Content.Load<Texture2D>(@"Textures\Board\Icon_Grain");
            lumberIcon = ourGame.Content.Load<Texture2D>(@"Textures\Board\Icon_Lumber");
            oreIcon = ourGame.Content.Load<Texture2D>(@"Textures\Board\Icon_Ore");
            woolIcon = ourGame.Content.Load<Texture2D>(@"Textures\Board\Icon_Wool");

            //Game Hexes
            brick = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Brick");
            desert = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Desert");
            grain = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Grain");
            lumber = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Lumber");
            ore = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Ore");
            sea = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Sea");
            wool = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Wool");

            //Roads
            blueRoad = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_RoadBlue");
            orangeRoad = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_RoadOrange");
            whiteRoad = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_RoadWhite");
            roadSquare = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_RoadSquare");

            //Settlements
            blueHouse = ourGame.Content.Load<Texture2D>(@"Textures\Board\House_Blue");
            orangeHouse = ourGame.Content.Load<Texture2D>(@"Textures\Board\House_Orange");
            whiteHouse = ourGame.Content.Load<Texture2D>(@"Textures\Board\House_White");
            blueSettlement = ourGame.Content.Load<Texture2D>(@"Textures\Board\Settlement_Blue");
            orangeSettlement = ourGame.Content.Load<Texture2D>(@"Textures\Board\Settlement_Orange");
            whiteSettlement = ourGame.Content.Load<Texture2D>(@"Textures\Board\Settlement_White");

            //Numbers
            numbers = new List<Texture2D>();
            numbers.Add(two = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Two"));
            numbers.Add(three = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Three"));
            numbers.Add(four = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Four"));
            numbers.Add(five = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Five"));
            numbers.Add(six = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Six"));
            numbers.Add(seven = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Seven"));
            numbers.Add(eight = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Eight"));
            numbers.Add(nine = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Nine"));
            numbers.Add(ten = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Ten"));
            numbers.Add(eleven = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Eleven"));
            numbers.Add(twelve = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Twelve"));

            robber = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Robber");

            settleToBuild = new List<GameNode>();
            settleToCity = new List<GameNode>();
            buildRoad = new List<GameRoad>();

            hexMiddle.X = brick.Width / 2;
            hexMiddle.Y = brick.Height / 2;

            settlePos.X = blueSettlement.Width / 2;
            settlePos.Y = 3 * blueSettlement.Height / 4;

            roadPos.X = whiteRoad.Width / 2;
            roadPos.Y = whiteRoad.Height / 2;

            portOrigin.X = port.Width / 2;
            portOrigin.Y = port.Height / 2;

            effect = new BasicEffect(GraphicsDevice);

            basicEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //Purpose: Update method that contains all changes that need to be made when an event updates dealing with the gameboard
        public override void Update(GameTime gameTime)
        {

            Point mouseClick = new Point(ourGame.mouseX, ourGame.mouseY);
            //Cycle through settlement rectangles to see if the user added a settlement
            if (playingState.buildingSettlement)
            {
                DrawHud.drawBuild = false;
                //This needs to be edited since placesForSettlements doesn't actually return the necessary info
                if (playingState.gameStartMode)
                    settleToBuild = ourGame.gameBoard.placesForSettlements();
                else
                    settleToBuild = ourGame.gameBoard.placesForSettlements(ourGame.humanPlayer);

                //Code to update the players node list with the appropriate settlement added to it
                foreach (GameNode gn in settleToBuild)
                {
                    //get offset and set positions for the settlements and the rectangles of the settlements that can be built
                    int offset = getOffset(gn);
                    float posY = halfHeight + ((gn.locx - 6) * hexMiddle.Y * 0.75f + (offset * hexMiddle.Y / 4)) * hexScale;
                    float posX = halfWidth - (gn.locy - 11) * (hexMiddle.X / 2) * hexScale;
                    //create the rectangles for the settlements so that the user can click on them
                    Rectangle tmp = new Rectangle((int)posX - (int)(whiteSettlement.Width * settleScale) / 2, (int)posY - (int)(whiteSettlement.Height * settleScale) / 2, (int)(whiteSettlement.Width * settleScale), (int)(whiteSettlement.Height * settleScale));
                    if (tmp.Contains(mouseClick))
                    {
                        playingState.buildingSettlement = false;
                        initset = gn;
                        playingState.AddingSettlement(gn, ourGame.humanPlayer); //added a reference to the initial settlement just placed
                    }
                }
            }
            //Cycle through city rectangles to see if the user added a city
            if (playingState.buildingCity)
            {
                DrawHud.drawBuild = false;
                settleToCity = ourGame.gameBoard.placesForCities(ourGame.humanPlayer);

                //Code to update the players node list with the appropriate city added to it
                foreach (GameNode gn in settleToCity)
                {
                    //get offset and set positions for the cities and rectangles of cities that can be built
                    int offset = getOffset(gn);
                    float posY = halfHeight + ((gn.locx - 6) * hexMiddle.Y * 0.75f + (offset * hexMiddle.Y / 4)) * hexScale;
                    float posX = halfWidth - (gn.locy - 11) * (hexMiddle.X / 2) * hexScale;
                    Rectangle tmp = new Rectangle((int)posX - (int)(whiteHouse.Width * settleScale) / 2, (int)posY - (int)(whiteSettlement.Height * settleScale) / 2, (int)(whiteSettlement.Width * settleScale), (int)(whiteSettlement.Height * settleScale));
                    //create the rectangles for the cities so that user can click on them
                    if (tmp.Contains(mouseClick))
                    {
                        playingState.buildingCity = false;
                        playingState.AddingCity(gn, ourGame.humanPlayer);
                    }
                }
            }
            //Cycle through the available to build road rectangles to see if the user chose a spot to build a road
            if (playingState.buildingRoad)
            {
                DrawHud.drawBuild = false;
                //If we're in startMode, you can only put a road around the current settlemetn
                if (playingState.gameStartMode)
                    buildRoad = ourGame.gameBoard.placesForRoads(ourGame.humanPlayer, initset);
                else
                    buildRoad = ourGame.gameBoard.placesForRoads(ourGame.humanPlayer);

                foreach (GameRoad gr in buildRoad)
                {
                    float posY = halfHeight + ((gr.locx - 6) * hexMiddle.Y * 0.75f * hexScale);
                    float posX = halfWidth - (gr.locy - 11) * (hexMiddle.X / 2) * hexScale;
                    //create the rectangles for the roads so that user can click on them
                    Rectangle tmp = new Rectangle((int)posX - (int)(roadSquare.Width * roadScale) / 2, (int)posY - (int)(roadSquare.Height * roadScale) / 2, (int)(roadSquare.Width * roadScale), (int)(roadSquare.Height * roadScale));
                    if (tmp.Contains(mouseClick))
                    {
                        playingState.buildingRoad = false;
                        playingState.AddingRoad(gr, ourGame.humanPlayer);
                    }
                }
            }
            //Cycles through the hexes to see what hex the player chooses for the robber and then updates appropriately
            if (playingState.movingRobber == true && playingState.humanPlayerTurn )
            {
                ourGame.humanPlayer.canEndTurn = false;
                ourGame.humanPlayer.canBuild = false;
                ourGame.humanPlayer.canTrade = false;
                foreach (GameHex gh in ourGame.gameBoard.gameHexes)
                {
                    //X position of hex (middle)
                    float positionWidth = halfWidth + (gh.column - 5) * hexMiddle.X * hexScale;
                    //Y position of hex (middle)
                    float positionHeight = halfHeight + (gh.row - 3) * brick.Height * 0.75f * hexScale;
                    //Create the rectangles for the hexes that the user can click on to select a hex
                    Rectangle tmp = new Rectangle((int)positionWidth - (int)(brick.Width / 2 * hexScale), (int)positionHeight - (int)(brick.Height / 4 * hexScale), (int)(brick.Width * hexScale), (int)(brick.Height / 2 * hexScale));
                    if (tmp.Contains(mouseClick))
                    {
                        playingState.chooseTheft = true;
                        ourGame.gameBoard.moveRobber(gh, ourGame.humanPlayer);
                    }
                }
            }
            base.Update(gameTime);
        }


        // Draws the board, settlements, cities, roads
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Viewport = ourGame.camera.Viewport;
            DrawScene(gameTime, ourGame.camera);

            base.Draw(gameTime);
        }

        //Purpose: sets the graphics and then calls the methods to draw everything gameboard related
        private void DrawScene(GameTime gameTime, Camera camera)
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            effect.Projection = camera.Projection;
            effect.View = camera.View;

            effect.EnableDefaultLighting();

            ourGame.SpriteBatch.Begin();
            DrawBoard();
            DrawPorts();
            DrawRoads();
            DrawRoadsCanBuild();
            DrawSettlements();
            DrawSettlementsCanBuild();
            ourGame.SpriteBatch.End();
        }

        //Purpose: Draw the hexes that make up the gameboard and the hexNumbers of each hex
        private void DrawBoard()
        {
            Texture2D texture;
            GameHex hexInfo = new GameHex();
            //Draw Ocean Pieces
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (row % 2 == 0 && col % 2 == 0 || (row % 2 == 1 && col % 2 == 1))
                    {
                        hexMiddle.X = sea.Width / 2;
                        hexMiddle.Y = sea.Height / 2;
                        float positionWidth = halfWidth - (col - 7) * hexMiddle.X * hexScale;
                        float positionHeight = halfHeight - (row - 3) * sea.Height * 0.75f * hexScale;
                        Vector2 position = new Vector2(positionWidth, positionHeight);
                        ourGame.SpriteBatch.Draw(sea, position, null, Color.White, 0f, hexMiddle, hexScale, SpriteEffects.None, 0f);
                    }
                }
            }
            //Draw game hexes and hex numbers
            foreach (GameHex gh in ourGame.gameBoard.gameHexes)
            {
                hexInfo = ourGame.gameBoard.getHexInfo(gh);
                if (hexInfo.hexType != null)
                {
                    switch (hexInfo.hexType)
                    {
                        case "Clay":
                            {
                                texture = brick;
                                break;
                            }
                        case "Desert":
                            {
                                texture = desert;
                                break;
                            }
                        case "Grain":
                            {
                                texture = grain;
                                break;
                            }
                        case "Wood":
                            {
                                texture = lumber;
                                break;
                            }
                        case "Rock":
                            {
                                texture = ore;
                                break;
                            }
                        case "Sheep":
                            {
                                texture = wool;
                                break;
                            }
                        case "Ocean":
                            {
                                texture = sea;
                                break;
                            }
                        default:
                            {
                                texture = null;
                                break;
                            }
                    }
                    //X position of hex (middle)
                    float positionWidth = halfWidth + (gh.column - 5) * hexMiddle.X * hexScale;
                    //Y position of hex (middle)
                    float positionHeight = halfHeight + (gh.row - 3) * texture.Height * 0.75f * hexScale;
                    int hexNumber = hexInfo.hexNumber;
                    Vector2 position = new Vector2(positionWidth, positionHeight);
                    //Draw Hexes
                    ourGame.SpriteBatch.Draw(texture, position, null, Color.White, 0f, hexMiddle, hexScale, SpriteEffects.None, 0f);                 
                    //Set position of numbers (middle of hex minus half the width/height of the scaled number texture)
                    Vector2 numPosition = new Vector2(positionWidth + (hexMiddle.X - two.Width / 2) * hexScale, positionHeight + (hexMiddle.Y - two.Height / 2) * hexScale);
                    //Draw Numbers on Hexes
                    if (hexNumber > 0)
                    {
                        ourGame.SpriteBatch.Draw(numbers[hexNumber - 2], numPosition, null, Color.White, 0f, hexMiddle, hexScale, SpriteEffects.None, 0f);
                    }
                    //Draw the Robber                      
                    if (hexInfo.hasRobber)
                    {
                        //Set position of Robber
                        Vector2 robberOrigin = new Vector2(robber.Width / 2, robber.Height / 2);
                        Vector2 robberPosition = new Vector2(positionWidth - 30, positionHeight - 10);
                        ourGame.SpriteBatch.Draw(robber, robberPosition, null, Color.White, 0f, robberOrigin, 0.6f, SpriteEffects.None, 0f);
                    }
                }
            }
        }//end DrawBoard

        //Purpose: Draw the ports and their type at the correct places on the board
        private void DrawPorts()
        {
            for (int i = 0; i < ourGame.gameBoard.randPorts.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            float posX = halfWidth + 4 * hexMiddle.X * hexScale + 15;
                            float posY = halfHeight - 1.5f * hexMiddle.Y * hexScale;
                            Vector2 position = new Vector2(posX, posY);        
                            ourGame.SpriteBatch.Draw(port, position, null, Color.White, (float)DegreeToRadian(65), portOrigin, hexScale, SpriteEffects.None, 0f);
                            DrawPortType(ourGame.gameBoard.randPorts[i], position);
                            break;
                        }
                    case 1:
                        {
                            float posX = halfWidth + 2.5f * hexMiddle.X * hexScale + 10;
                            float posY = halfHeight - 4 * hexMiddle.Y * hexScale + 5;
                            Vector2 position = new Vector2(posX, posY);
                            ourGame.SpriteBatch.Draw(port, position, null, Color.White, 0f, portOrigin, hexScale, SpriteEffects.None, 0f);
                            DrawPortType(ourGame.gameBoard.randPorts[i], position);
                            break;
                        }
                    case 2:
                        {
                            float posX = halfWidth - .5f * hexMiddle.X * hexScale - 10;
                            float posY = halfHeight - 4 * hexMiddle.Y * hexScale + 5;
                            Vector2 position = new Vector2(posX, posY);
                            ourGame.SpriteBatch.Draw(port, position, null, Color.White, (float)DegreeToRadian(-60), portOrigin, hexScale, SpriteEffects.None, 0f);
                            DrawPortType(ourGame.gameBoard.randPorts[i], position);
                            break;
                        }
                    case 3:
                        {
                            float posX = halfWidth - 3.5f * hexMiddle.X * hexScale - 5;
                            float posY = halfHeight - 2.5f * hexMiddle.Y * hexScale + 5;
                            Vector2 position = new Vector2(posX, posY);
                            ourGame.SpriteBatch.Draw(port, position, null, Color.White, (float)DegreeToRadian(-60), portOrigin, hexScale, SpriteEffects.None, 0f);
                            DrawPortType(ourGame.gameBoard.randPorts[i], position);
                            break;
                        }
                    case 4:
                        {
                            float posX = halfWidth - 5 * hexMiddle.X * hexScale - 15;
                            float posY = halfHeight;
                            Vector2 position = new Vector2(posX, posY);
                            ourGame.SpriteBatch.Draw(port, position, null, Color.White, (float)DegreeToRadian(-115), portOrigin, hexScale, SpriteEffects.None, 0f);
                            DrawPortType(ourGame.gameBoard.randPorts[i], position);
                            break;
                        }
                    case 5:
                        {
                            float posX = halfWidth - 3.5f * hexMiddle.X * hexScale - 5;
                            float posY = halfHeight + 2.5f * hexMiddle.Y * hexScale - 5;
                            Vector2 position = new Vector2(posX, posY);
                            ourGame.SpriteBatch.Draw(port, position, null, Color.White, (float)DegreeToRadian(180), portOrigin, hexScale, SpriteEffects.None, 0f);
                            DrawPortType(ourGame.gameBoard.randPorts[i], position);
                            break;
                        }
                    case 6:
                        {
                            float posX = halfWidth - .5f * hexMiddle.X * hexScale - 10;
                            float posY = halfHeight + 4 * hexMiddle.Y * hexScale - 5;
                            Vector2 position = new Vector2(posX, posY);
                            ourGame.SpriteBatch.Draw(port, position, null, Color.White, (float)DegreeToRadian(180), portOrigin, hexScale, SpriteEffects.None, 0f);
                            DrawPortType(ourGame.gameBoard.randPorts[i], position);
                            break;
                        }
                    case 7:
                        {
                            float posX = halfWidth + 2.5f * hexMiddle.X * hexScale + 10;
                            float posY = halfHeight + 4 * hexMiddle.Y * hexScale - 5;
                            Vector2 position = new Vector2(posX, posY);
                            ourGame.SpriteBatch.Draw(port, position, null, Color.White, (float)DegreeToRadian(135), portOrigin, hexScale, SpriteEffects.None, 0f);
                            DrawPortType(ourGame.gameBoard.randPorts[i], position);
                            break;
                        }
                    case 8:
                        {
                            float posX = halfWidth + 4 * hexMiddle.X * hexScale + 15;
                            float posY = halfHeight + 1.5f * hexMiddle.Y * hexScale;
                            Vector2 position = new Vector2(posX, posY);
                            ourGame.SpriteBatch.Draw(port, position, null, Color.White, (float)DegreeToRadian(65), portOrigin, hexScale, SpriteEffects.None, 0f);
                            DrawPortType(ourGame.gameBoard.randPorts[i], position);
                            break;
                        }
                }
            }
        }

        private void DrawPortType(int pt, Vector2 position )
        {
            Vector2 icon = new Vector2 (7, 6);
            switch (pt)
            {
                case 1:
                    {
                        ourGame.SpriteBatch.Draw(lumberIcon, position, null, Color.White, 0f, icon, 1.4f, SpriteEffects.None, 0f);
                        break;
                    }
                case 2:
                    {
                        ourGame.SpriteBatch.Draw(brickIcon, position, null, Color.White, 0f, icon, 1.4f, SpriteEffects.None, 0f);
                        break;
                    }
                case 3:
                    {
                        ourGame.SpriteBatch.Draw(grainIcon, position, null, Color.White, 0f, icon, 1.4f, SpriteEffects.None, 0f);
                        break;

                    }
                case 4:
                    {
                        ourGame.SpriteBatch.Draw(oreIcon, position, null, Color.White, 0f, icon, 1.4f, SpriteEffects.None, 0f);
                        break;
                    }
                case 5:
                    {
                        ourGame.SpriteBatch.Draw(woolIcon, position, null, Color.White, 0f, icon, 1.4f, SpriteEffects.None, 0f);
                        break;
                    }
            }
        }

        //Purpose: Draw the settlements and cities that have been built
        private void DrawSettlements()
        {
            //Draw appropriate Settlements and cities on nodes that players own according to which player owns the node
            List<GameNode> settlements = new List<GameNode>();
            List<GameNode> cities = new List<GameNode>();
            foreach (Player player in ourGame.players)
            {
                settlements = ourGame.gameBoard.placesForCities(player);
                cities = ourGame.gameBoard.getCitiesNodes(player);

                //Draw settlements for each player
                foreach (GameNode gn in settlements)
                {
                    int offset = getOffset(gn);
                    float posY = halfHeight + ((gn.locx - 6) * hexMiddle.Y * 0.75f + (offset * hexMiddle.Y / 4)) * hexScale;
                    float posX = halfWidth - (gn.locy - 11) * (hexMiddle.X / 2) * hexScale;
                    if (player == ourGame.aiPlayer1 && gn.settleType == 0)
                    {
                        Vector2 position = new Vector2(posX, posY);
                        ourGame.SpriteBatch.Draw(blueSettlement, position, null, Color.White, 0f, settlePos, settleScale, SpriteEffects.None, 0f);
                    }
                    if (player == ourGame.aiPlayer2 && gn.settleType == 0)
                    {
                        Vector2 position = new Vector2(posX, posY);
                        ourGame.SpriteBatch.Draw(orangeSettlement, position, null, Color.White, 0f, settlePos, settleScale, SpriteEffects.None, 0f);
                    }
                    if (player == ourGame.humanPlayer && gn.settleType == 0)
                    {
                        Vector2 position = new Vector2(posX, posY);
                        ourGame.SpriteBatch.Draw(whiteSettlement, position, null, Color.White, 0f, settlePos, settleScale, SpriteEffects.None, 0f);
                    }
                }

                //Draw cities for each player
                foreach (GameNode gn in cities)
                {
                    int offset = getOffset(gn);
                    float posY = halfHeight + ((gn.locx - 6) * hexMiddle.Y * 0.75f + (offset * hexMiddle.Y / 4)) * hexScale;
                    float posX = halfWidth - (gn.locy - 11) * (hexMiddle.X / 2) * hexScale;
                    if (player == ourGame.aiPlayer1 && gn.settleType == 1)
                    {
                        Vector2 position = new Vector2(posX, posY);
                        ourGame.SpriteBatch.Draw(blueHouse, position, null, Color.White, 0f, settlePos, settleScale, SpriteEffects.None, 0f);
                    }
                    if (player == ourGame.aiPlayer2 && gn.settleType == 1)
                    {
                        Vector2 position = new Vector2(posX, posY);
                        ourGame.SpriteBatch.Draw(orangeHouse, position, null, Color.White, 0f, settlePos, settleScale, SpriteEffects.None, 0f);
                    }
                    if (player == ourGame.humanPlayer && gn.settleType == 1)
                    {
                        Vector2 position = new Vector2(posX, posY);
                        ourGame.SpriteBatch.Draw(whiteHouse, position, null, Color.White, 0f, settlePos, settleScale, SpriteEffects.None, 0f);
                    }
                }
            }
        }//end DrawSettlements

        //Purpose: Draw the settlements/cities that the player can build when building settlements/cities is allowed
        private void DrawSettlementsCanBuild()
        {
            //Draw Settlements that can be built 
            if (playingState.buildingSettlement)
            {
                foreach (GameNode gn in settleToBuild)
                {
                    //get offset and set positions for the settlements that can be built
                    int offset = getOffset(gn);
                    float posY = halfHeight + ((gn.locx - 6) * hexMiddle.Y * 0.75f + (offset * hexMiddle.Y / 4)) * hexScale;
                    float posX = halfWidth - (gn.locy - 11) * (hexMiddle.X / 2) * hexScale;
                    Vector2 position = new Vector2(posX, posY);
                    //draw the appropriate settlements
                    ourGame.SpriteBatch.Draw(whiteSettlement, position, null, Color.DarkGray, 0f, settlePos, settleScale, SpriteEffects.None, 0f);
                }
            }
            //Draw Cities that can be built 
            if (playingState.buildingCity)
            {
                foreach (GameNode gn in settleToCity)
                {
                    //get offset and set positions for the cities that can be built
                    int offset = getOffset(gn);
                    float posY = halfHeight + ((gn.locx - 6) * hexMiddle.Y * 0.75f + (offset * hexMiddle.Y / 4)) * hexScale;
                    float posX = halfWidth - (gn.locy - 11) * (hexMiddle.X / 2) * hexScale;
                    Vector2 position = new Vector2(posX, posY);
                    //draw the appropriate cities
                    ourGame.SpriteBatch.Draw(whiteHouse, position, null, Color.DarkGray, 0f, settlePos, settleScale, SpriteEffects.None, 0f);
                }
            }
        }

        //Purpose: get offsets so that the settlements/cities can be drawn in the appropriate spots
        private int getOffset(GameNode gn)
        {
            int offset = 0;
            if (Math.Abs(gn.locy - 11) % 4 == 0 && Math.Abs(gn.locx - 6) % 3 == 0)
            {
                if (gn.locx - 6 < 0)
                    offset = 1;
                else
                    offset = -1;
            }
            else if (Math.Abs(gn.locy - 11) % 2 == 0 && Math.Abs(gn.locx - 6) % 3 == 0)
            {
                if (gn.locx - 6 < 0)
                    offset = -1;
                else
                    offset = 1;
            }
            else if (Math.Abs(gn.locy - 11) % 4 == 0 && Math.Abs(gn.locx - 6) % 2 == 1)
            {
                if (gn.locx - 6 < 0)
                    offset = -1;
                else
                    offset = 1;
            }
            else if (Math.Abs(gn.locy - 11) % 2 == 0 && Math.Abs(gn.locx - 6) % 2 == 1)
            {
                if (gn.locx - 6 < 0)
                    offset = 1;
                else
                    offset = -1;
            }
            return offset;
        }//end getOffset

        //Purpose: Draw all roads that have been built by the players
        private void DrawRoads()
        {
            List<GameRoad> roads = new List<GameRoad>();
            foreach (Player player in ourGame.players)
            {
                roads = ourGame.gameBoard.getPlayerRoads(player);

                foreach (GameRoad gr in roads)
                {
                    float roadRot = getRoadRotation(gr);
                    float posY = halfHeight + ((gr.locx - 6) * hexMiddle.Y * 0.75f * hexScale);
                    float posX = halfWidth - (gr.locy - 11) * (hexMiddle.X / 2) * hexScale;
                    if (player == ourGame.aiPlayer1)
                    {
                        Vector2 position = new Vector2(posX, posY);
                        ourGame.SpriteBatch.Draw(blueRoad, position, null, Color.White, roadRot, roadPos, roadScale, SpriteEffects.None, 0f);
                    }
                    if (player == ourGame.aiPlayer2)
                    {
                        Vector2 position = new Vector2(posX, posY);
                        ourGame.SpriteBatch.Draw(orangeRoad, position, null, Color.White, roadRot, roadPos, roadScale, SpriteEffects.None, 0f);
                    }
                    if (player == ourGame.humanPlayer)
                    {
                        Vector2 position = new Vector2(posX, posY);
                        ourGame.SpriteBatch.Draw(whiteRoad, position, null, Color.White, roadRot, roadPos, roadScale, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        //Draw the Roads that a player can build
        private void DrawRoadsCanBuild()
        {
            if (playingState.buildingRoad)
            {
                foreach (GameRoad gr in buildRoad)
                {
                    float roadRot = getRoadRotation(gr);
                    float posY = halfHeight + ((gr.locx - 6) * hexMiddle.Y * 0.75f * hexScale);
                    float posX = halfWidth - (gr.locy - 11) * (hexMiddle.X / 2) * hexScale;
                    Vector2 position = new Vector2(posX, posY);
                    ourGame.SpriteBatch.Draw(whiteRoad, position, null, Color.DarkGray, roadRot, roadPos, roadScale, SpriteEffects.None, 0f);
                    Vector2 posSquare = new Vector2(posX + (whiteRoad.Width / 2 - roadSquare.Width / 2) * roadScale, posY + (whiteRoad.Height / 2 - roadSquare.Height / 2) * roadScale);
                    //draw the appropriate roads
                    ourGame.SpriteBatch.Draw(roadSquare, posSquare, null, Color.White, 0f, roadPos, roadScale, SpriteEffects.None, 0f);
                }
            }
        }

        //Purpose: Determines the rotation of the roads based on the GameRoad location
        private float getRoadRotation(GameRoad gr)
        {
            float rot = 0;

            if (gr.locy % 4 == 2 && gr.locx % 4 == 1)
            {
                rot = (float)DegreeToRadian(60);
            }
            else if (gr.locx % 4 == 3 && gr.locy % 4 == 0)
            {
                rot = (float)DegreeToRadian(60);
            }
            else if (gr.locx % 2 == 0)
            {
                rot = (float)DegreeToRadian(120);
            }
            return rot;
        }

        //Purpose: Converts double in degrees to a radian 
        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

    }
}
