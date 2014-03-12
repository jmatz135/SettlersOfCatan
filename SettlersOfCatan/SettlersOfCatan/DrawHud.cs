/* Draws the game Hud
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
    class DrawHud : BaseGameState
    {
        private PlayingState playingState;

        //Textures for the main Hud menu
        private Texture2D hudBackground;
        private Texture2D hudActions;
        private Texture2D hudResources;
        private Texture2D hudLongests;
        private Texture2D hudVictoryPoints;

        //Textures for the dice
        private Texture2D hudDiceOne;
        private Texture2D hudDiceTwo;
        private Texture2D hudDiceThree;
        private Texture2D hudDiceFour;
        private Texture2D hudDiceFive;
        private Texture2D hudDiceSix;

        //Textures for the different menus that pop up when an action on the main hud is taken
        private Texture2D hudTradeMenu;
        private Texture2D hudAITradeMenu;
        private Texture2D hudBuildRoad;
        private Texture2D hudBuildSettlement;
        private Texture2D hudBuildCity;
        private Texture2D hudBuildDevCard;
        private Texture2D hudCardPopup;
        private Texture2D hudYearOfPlenty;

        private List<Texture2D> developmentCards;
        private List<Rectangle> devCardRectangles;
        private List<Rectangle> devCardRectanglesOwned;

        //AI Huds textures
        private Texture2D aiOne;
        private Texture2D aiTwo;
        private Texture2D aiOneSteal;
        private Texture2D aiTwoSteal;
        private Texture2D aiAccept;
        private Texture2D aiReject;

        private SpriteFont largeFont;

        Color brown = new Color(92, 65, 0);

        //Bools for drawing different aspects of the hud
        public static bool drawDice;
        public static bool drawTrade;
        public static bool drawBuild;
        public static bool drawCards;

        //Bools for different Hud Menus
        public bool tradingMode;
        public static bool playedCard;
        private bool turnCountZero;

        //Hud text positions
        private Vector2 brickTextPosition;
        private Vector2 wheatTextPosition;
        private Vector2 woodTextPosition;
        private Vector2 woolTextPosition;
        private Vector2 oreTextPosition;
        private Vector2 knightsTextPosition;
        private Vector2 roadsTextPosition;
        private Vector2 victoryPointTextPosition;
        private Vector2 aiOneResourcesPos;
        private Vector2 aiOneDevCardsPos;
        private Vector2 aiOneKnightsPos;
        private Vector2 aiOneRoadsPos;
        private Vector2 aiOneVPPos;
        private Vector2 aiTwoResourcesPos;
        private Vector2 aiTwoDevCardsPos;
        private Vector2 aiTwoKnightsPos;
        private Vector2 aiTwoRoadsPos;
        private Vector2 aiTwoVPPos;
        private Rectangle aiOneStealPos;
        private Rectangle aiTwoStealPos;
        private Rectangle aiOneResponse;
        private Rectangle aiTwoResponse;
        private Rectangle playerRejects;

        //Temporary mouse positioning information
        private string mouseXText = string.Empty;
        private Vector2 mouseXTextPosition;

        private string mouseYText = string.Empty;
        private Vector2 mouseYTextPosition;

        //screen information for easy access
        private int screenWidth;
        private int screenHeight;

        //Rectangles for various Hud pieces
        Rectangle diceRectangle;
        Rectangle tradeRectangle;
        Rectangle stopRectangle;
        Rectangle buildRectangle;
        Rectangle cardPopupRectangle;

        //Rectangles for build cards
        Rectangle roadCardRectangle;
        Rectangle settleCardRectangle;
        Rectangle cityCardRectangle;
        Rectangle devCardRectangle;

        //Rectangles and Text positions for trading menu
        int[] tradeNumbers;
        String[] tradeMenuText;
        List<Vector2> tradeTextPos;
        List<Rectangle> increaseAmount;
        List<Rectangle> decreaseAmount;
        Rectangle acceptTrade;
        Rectangle declineTrade;
        Rectangle bankTrade;

        int[] aiTradeNumbers;
        String aiOffering;
        Vector2 aiOfferingPos;
        String[] aiTradeMenuText;
        List<Vector2> aiTradeTextPos;
        Rectangle acceptAITrade;
        Rectangle rejectAITrade;

        //Rectangles and Text positions for YearOfPlenty Menu
        int yopCounter;
        int[] yopNumbers;
        String[] yopMenuText;
        List<Vector2> yopTextPos;
        List<Rectangle> yopUp;
        List<Rectangle> yopDown;
        Rectangle yopAccept;
        Rectangle yopDecline;

        int amountGive;
        Vector2 amountGivePos;

        //Monopoly Card variables
        List<Rectangle> monopolyRectangles;
        List<Texture2D> monopolyTextures;

        //Robber Lists
        IEnumerable<Player> aiCanSteal;
        GameHex robberHex;

        public DrawHud(Game game, PlayingState state)
            : base(game)
        {
            screenWidth = OurGame.width;
            screenHeight = OurGame.height;

            playingState = (PlayingState)state;
        }

        public override void Initialize()
        {
            base.Initialize();
        }


        // Purpose: Import 2D Sprites that make up the HUD and different menus that are opened up
        protected override void LoadContent()
        {
            //Sprites for the default HUD
            hudBackground = OurGame.Content.Load<Texture2D>(@"Textures\Hud\Hud_Background");
            hudActions = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_Actions2");
            hudResources = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_Resources");
            hudLongests = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_Longests");
            hudVictoryPoints = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_VictoryPoints");

            //Sprites to show Dice roll
            hudDiceOne = OurGame.Content.Load<Texture2D>(@"Textures\Hud\Dice_One");
            hudDiceTwo = OurGame.Content.Load<Texture2D>(@"Textures\Hud\Dice_Two");
            hudDiceThree = OurGame.Content.Load<Texture2D>(@"Textures\Hud\Dice_Three");
            hudDiceFour = OurGame.Content.Load<Texture2D>(@"Textures\Hud\Dice_Four");
            hudDiceFive = OurGame.Content.Load<Texture2D>(@"Textures\Hud\Dice_Five");
            hudDiceSix = OurGame.Content.Load<Texture2D>(@"Textures\Hud\Dice_Six");

            //Sprites that are opened up after choosing different actions on the main HUD
            hudTradeMenu = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_TradingBlock");
            hudAITradeMenu = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_aiTrade");
            hudYearOfPlenty = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_RobberGiveaway");
            hudBuildRoad = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_BuildRoad");
            hudBuildSettlement = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_BuildSettlement");
            hudBuildCity = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_BuildCity");
            hudBuildDevCard = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_BuildDevCard");
            hudCardPopup = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_CardPopup");

            //The AI player HUD Information
            aiOne = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_MaleAI");
            aiTwo = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_FemaleAI");
            aiOneSteal = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_StealFrom1");
            aiTwoSteal = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_StealFrom2");
            aiAccept = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_Accept");
            aiReject = OurGame.Content.Load<Texture2D>(@"Textures\Hud\HUD_Reject");


            //Fill the developmentCards List with card textures
            developmentCards = new List<Texture2D>();
            developmentCards.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Knight"));
            developmentCards.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_University"));
            developmentCards.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Palace"));
            developmentCards.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Library"));
            developmentCards.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Market"));           
            developmentCards.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Chapel"));
            developmentCards.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_YearOfPlenty"));                     
            developmentCards.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Monopoly"));           
            developmentCards.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_RoadBuilding"));
            
            //Monopoly textures
            monopolyTextures = new List<Texture2D>();
            monopolyTextures.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Brick"));
            monopolyTextures.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Grain"));
            monopolyTextures.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Lumber"));
            monopolyTextures.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Wool"));
            monopolyTextures.Add(OurGame.Content.Load<Texture2D>(@"Textures\Cards\Card_Ore"));
            


            //Set the font for the text on the HUD
            largeFont = Content.Load<SpriteFont>(@"Fonts\hudlarge");

            //Set Bools for draw actions
            drawDice = false;
            drawTrade = false;
            drawBuild = false;
            drawCards = false;

            //Set Bools for Hud Menus
            tradingMode = false;
            playedCard = false;
            turnCountZero = true;

            //Set Score Text Positions for humanPlayer
            brickTextPosition = new Vector2((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 170, 
                screenHeight - (TitleSafeArea.Bottom + 40));
            wheatTextPosition = new Vector2((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 234,
                screenHeight - (TitleSafeArea.Bottom + 40));
            woodTextPosition = new Vector2((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 295,
                screenHeight - (TitleSafeArea.Bottom + 40));
            woolTextPosition = new Vector2((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 358,
                screenHeight - (TitleSafeArea.Bottom + 40));
            oreTextPosition = new Vector2((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 420,
                screenHeight - (TitleSafeArea.Bottom + 40));
            knightsTextPosition = new Vector2((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 550,
                screenHeight - (TitleSafeArea.Bottom + 100));
            roadsTextPosition = new Vector2((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 550,
                screenHeight - (TitleSafeArea.Bottom + 50));
            victoryPointTextPosition = new Vector2((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 642,
                screenHeight - (TitleSafeArea.Bottom + 50));

            //Set Score Text Positions for AIOne
            aiOneResourcesPos = new Vector2(TitleSafeArea.Left + 64, TitleSafeArea.Top + 162);
            aiOneDevCardsPos = new Vector2(TitleSafeArea.Left + 64, TitleSafeArea.Top + 195);
            aiOneKnightsPos = new Vector2(TitleSafeArea.Left + 114, TitleSafeArea.Top + 162);
            aiOneRoadsPos = new Vector2(TitleSafeArea.Left + 114, TitleSafeArea.Top + 195);
            aiOneVPPos = new Vector2(TitleSafeArea.Left + 90, TitleSafeArea.Top + 225);
            
            //Set Score Text Positions for AITwo
            aiTwoResourcesPos = new Vector2(screenWidth - (aiTwo.Width + TitleSafeArea.Right - 64), TitleSafeArea.Top + 162);
            aiTwoDevCardsPos = new Vector2(screenWidth - (aiTwo.Width + TitleSafeArea.Right - 64), TitleSafeArea.Top + 195);
            aiTwoKnightsPos = new Vector2(screenWidth - (aiTwo.Width + TitleSafeArea.Right - 114), TitleSafeArea.Top + 162);
            aiTwoRoadsPos = new Vector2(screenWidth - (aiTwo.Width + TitleSafeArea.Right - 114), TitleSafeArea.Top + 195);
            aiTwoVPPos = new Vector2(screenWidth - (aiTwo.Width + TitleSafeArea.Right - 90), TitleSafeArea.Top + 225);

            //Rectangles for Accept/Reject AI Icons
            aiOneResponse = new Rectangle(TitleSafeArea.Left + 150, TitleSafeArea.Top + 160, aiAccept.Width, aiAccept.Height);
            aiTwoResponse = new Rectangle(screenWidth - (TitleSafeArea.Right + 150 + aiAccept.Width), TitleSafeArea.Top + 160, aiAccept.Width, aiAccept.Height);
            playerRejects = new Rectangle(screenWidth / 5 + 20, screenHeight - (180 + TitleSafeArea.Bottom), aiReject.Width, aiReject.Height);

            //Rectangles for Steal from AI Icons
            aiOneStealPos = new Rectangle(TitleSafeArea.Left + 142, TitleSafeArea.Top + 160, aiOneSteal.Width, aiOneSteal.Height);
            aiTwoStealPos = new Rectangle(screenWidth - (TitleSafeArea.Right + 142 + aiTwoSteal.Width), TitleSafeArea.Top + 160, aiTwoSteal.Width, aiTwoSteal.Height);

            aiCanSteal = new List<Player>();
            robberHex = new GameHex();

            //Temporary Mouse Position information
            mouseXTextPosition = new Vector2(OurGame.width / 2 + 300, OurGame.height - (TitleSafeArea.Bottom + 40));
            mouseYTextPosition = new Vector2(OurGame.width / 2 + 400, OurGame.height - (TitleSafeArea.Bottom + 40));

            //Rectangles for main Hud Action Pieces
            diceRectangle = new Rectangle(screenWidth / 2 - 502, screenHeight - (hudActions.Height + TitleSafeArea.Bottom - 10), 60, 54);
            tradeRectangle = new Rectangle(screenWidth / 2 - 442, screenHeight - (hudActions.Height + TitleSafeArea.Bottom - 10), 60, 54);
            stopRectangle = new Rectangle(screenWidth / 2 - 502, screenHeight - (hudActions.Height + TitleSafeArea.Bottom - 64), 60, 54);
            buildRectangle = new Rectangle(screenWidth / 2 - 442, screenHeight - (hudActions.Height + TitleSafeArea.Bottom - 64), 60, 54);
            cardPopupRectangle = new Rectangle((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + hudActions.Width / 2 + hudResources.Width + hudLongests.Width + hudVictoryPoints.Width + 10,
                    screenHeight - (hudCardPopup.Height + TitleSafeArea.Bottom - 10),
                    hudCardPopup.Width - 10,
                    hudCardPopup.Height - 15);

            //Rectangles for building cards
            roadCardRectangle = new Rectangle(screenWidth / 4,
                screenHeight - (240 + TitleSafeArea.Bottom),
                hudBuildRoad.Width,
                hudBuildRoad.Height);
            settleCardRectangle = new Rectangle(screenWidth / 4 + hudBuildRoad.Width + 10,
                screenHeight - (240 + TitleSafeArea.Bottom),
                hudBuildRoad.Width,
                hudBuildRoad.Height);
            cityCardRectangle = new Rectangle(screenWidth / 4 + hudBuildRoad.Width * 2 + 20,
                screenHeight - (240 + TitleSafeArea.Bottom),
                hudBuildRoad.Width,
                hudBuildRoad.Height);
            devCardRectangle = new Rectangle(screenWidth / 4 + hudBuildRoad.Width * 3 + 30,
                screenHeight - (240 + TitleSafeArea.Bottom),
                hudBuildRoad.Width,
                hudBuildRoad.Height);

            //Rectangles, text numbers, and text position vectors for trade menu
            tradeNumbers = new int[10];
            tradeTextPos = new List<Vector2>();
            tradeMenuText = new String[10];
            increaseAmount = new List<Rectangle>();
            decreaseAmount = new List<Rectangle>();

            //Rectangles for the Up/Down buttons to increase or decrease the amount to trade
            for (int i = 0; i < 10; i++)
            {
                if (i < 5)
                {
                    tradeTextPos.Add(new Vector2(screenWidth / 8 + 20,
                        screenHeight - (60 - i * 34 + hudTradeMenu.Height + TitleSafeArea.Bottom)));
                    increaseAmount.Add(new Rectangle(screenWidth / 8 + 47,
                        screenHeight - (67 - i * 34 + hudTradeMenu.Height + TitleSafeArea.Bottom),
                        37,
                        17));
                    decreaseAmount.Add(new Rectangle(screenWidth / 8 + 47,
                        screenHeight - (47 - i * 34 + hudTradeMenu.Height + TitleSafeArea.Bottom),
                        37,
                        13));
                }
                else
                {
                    tradeTextPos.Add(new Vector2(screenWidth / 8 + 192,
                        screenHeight - (60 - (i - 5) * 34 + hudTradeMenu.Height + TitleSafeArea.Bottom)));
                    increaseAmount.Add(new Rectangle(screenWidth / 8 + 138,
                        screenHeight - (67 - (i - 5) * 34 + hudTradeMenu.Height + TitleSafeArea.Bottom),
                        40,
                        17));
                    decreaseAmount.Add(new Rectangle(screenWidth / 8 + 138,
                        screenHeight - (47 - (i - 5) * 34 + hudTradeMenu.Height + TitleSafeArea.Bottom),
                        40,
                        13));
                }
                tradeNumbers[i] = 0;
            }
            //rectangles for accept trade and bank trade
            acceptTrade = new Rectangle(screenWidth / 8 + 47,
                        screenHeight - (hudTradeMenu.Height + TitleSafeArea.Bottom - 106),
                        38,
                        50);
            bankTrade = new Rectangle(screenWidth / 8 + 92,
                screenHeight - (hudTradeMenu.Height + TitleSafeArea.Bottom - 106),
                38,
                50);
            declineTrade = new Rectangle(screenWidth / 8 + 137,
                screenHeight - (hudTradeMenu.Height + TitleSafeArea.Bottom - 106),
                38,
                50);

            //Initialize aiTrade information
            aiTradeNumbers = new int[10];
            aiTradeMenuText = new String[10];
            aiOffering = "";
            aiTradeTextPos = new List<Vector2>();

            aiOfferingPos = new Vector2(screenWidth / 8 + 92,
                        screenHeight - (115 + hudTradeMenu.Height + TitleSafeArea.Bottom));
            for (int i = 0; i < 10; i++)
            {
                if (i < 5)
                {
                    aiTradeTextPos.Add(new Vector2(screenWidth / 8 + 40,
                        screenHeight - (60 - i * 34 + hudTradeMenu.Height + TitleSafeArea.Bottom)));
                }
                else
                {
                    aiTradeTextPos.Add(new Vector2(screenWidth / 8 + 177,
                        screenHeight - (60 - (i - 5) * 34 + hudTradeMenu.Height + TitleSafeArea.Bottom)));
                }
                aiTradeNumbers[i] = 0;
            }
            acceptAITrade = new Rectangle(screenWidth / 8 + 70,
                        screenHeight - (hudTradeMenu.Height + TitleSafeArea.Bottom - 126),
                        30,
                        30);
            rejectAITrade = new Rectangle(screenWidth / 8 + 128,
                        screenHeight - (hudTradeMenu.Height + TitleSafeArea.Bottom - 126),
                        30,
                        30);


            //YearOfPlenty Menu
            yopCounter = 0;
            yopNumbers = new int[5];
            yopTextPos = new List<Vector2>();
            yopMenuText = new String[5];
            yopUp = new List<Rectangle>();
            yopDown = new List<Rectangle>();

            for (int i = 0; i < 15; i++)
            {
                yopTextPos.Add(new Vector2(6 * screenWidth / 8 + 70,
                    screenHeight - (60 - i * 34 + hudYearOfPlenty.Height + TitleSafeArea.Bottom)));
                yopUp.Add(new Rectangle(6 * screenWidth / 8 + 102,
                    screenHeight - (67 - i * 34 + hudYearOfPlenty.Height + TitleSafeArea.Bottom),
                    20,
                    17));
                yopDown.Add(new Rectangle(6 * screenWidth / 8 + 102,
                    screenHeight - (47 - i * 34 + hudYearOfPlenty.Height + TitleSafeArea.Bottom),
                    20,
                    13));
            }
            yopAccept = new Rectangle(6 * screenWidth / 8 + 58,
                        screenHeight - (hudYearOfPlenty.Height + TitleSafeArea.Bottom - 126),
                        50,
                        30);
            yopDecline = new Rectangle(6 * screenWidth / 8 + 120,
                screenHeight - (hudYearOfPlenty.Height + TitleSafeArea.Bottom - 126),
                50,
                30);

            //rectangles for devCards
            devCardRectangles = new List<Rectangle>();
            devCardRectanglesOwned = new List<Rectangle>();
            for (int i = 0; i < 9; i++)
            {                  
                devCardRectangles.Add(new Rectangle(screenWidth - 5 * developmentCards[i].Width + (i * developmentCards[i].Width / 2),
                    screenHeight - (developmentCards[i].Height + TitleSafeArea.Bottom),
                    developmentCards[i].Width / 2,
                    developmentCards[i].Height / 2));
            }

            monopolyRectangles = new List<Rectangle>();
            for (int i = 0; i < 5; i++)
            {
                monopolyRectangles.Add(new Rectangle(screenWidth - 4 * monopolyTextures[i].Width + (i * monopolyTextures[i].Width / 2),
                    screenHeight - (monopolyTextures[i].Height + TitleSafeArea.Bottom),
                    monopolyTextures[i].Width / 2,
                    monopolyTextures[i].Height / 2));
            }

            amountGivePos = new Vector2((6 * screenWidth / 8 + 162), screenHeight - (114 + hudYearOfPlenty.Height + TitleSafeArea.Bottom));

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //Update the mouse spot when left click is hit. (this will be altered, more of a test to see if left click worked)
        public override void Update(GameTime gameTime)
        {
            //Temporary Mouse Information write out
            mouseXText = "X: " + OurGame.mouseX.ToString();
            mouseYText = "Y: " + OurGame.mouseY.ToString();
            //OurGame.humanPlayer.SetBuildBools();

            Point mouseClick = new Point( OurGame.mouseX, OurGame.mouseY );

            //Mouse updates for hudActions menu
            //Updates the dice roll for the player if the dice menu button is pressed
            if ( OurGame.humanPlayer.canRollDice && diceRectangle.Contains(mouseClick) )
            {
                OurGame.humanPlayer.canRollDice = false;
                drawDice = true;
                OurGame.gameDice.rollDice();
                playingState.rolledDice = true;
                OurGame.humanPlayer.canTrade = true;
                OurGame.humanPlayer.canBuild = true;
                OurGame.humanPlayer.canEndTurn = true;
            }
            //Updates information if the trade menu button is pressed
            if (OurGame.humanPlayer.canTrade && tradeRectangle.Contains(mouseClick))
            {
                drawDice = false;
                drawBuild = false;
                drawCards = false;
                playingState.buildingSettlement = false;
                playingState.buildingCity = false;
                playingState.buildingRoad = false;
                tradingMode = true;
                drawTrade = true;

            }
            //Updates information if the build menu button is pressed
            if (OurGame.humanPlayer.canBuild && buildRectangle.Contains(mouseClick))
            {
                drawDice = false;
                drawTrade = false;
                drawCards = false;
                playingState.buildingSettlement = false;
                playingState.buildingCity = false;
                playingState.buildingRoad = false;
                if (!drawBuild)
                    drawBuild = true;
                else
                    drawBuild = false;               
            }
            //Updates information if the endturn menu button is pressed
            if (OurGame.humanPlayer.canEndTurn && stopRectangle.Contains(mouseClick))
            {
                drawDice = false;
                drawTrade = false;
                drawBuild = false;
                drawCards = false;
                playedCard = false;
                OurGame.humanPlayer.devCards.CopyTo(OurGame.humanPlayer.cardsAvailable, 0);
                playingState.turnWaitCounter = 0;
                playingState.rolledDice = false;
                playingState.buildingSettlement = false;
                playingState.buildingCity = false;
                playingState.buildingRoad = false;
                OurGame.humanPlayer.canEndTurn = false;
                OurGame.humanPlayer.canTrade = false;
                OurGame.humanPlayer.canBuild = false;
                playingState.humanPlayerTurn = false;
                playingState.aiTurn = true;
            }
            //Updates information if the card menu button is pressed
            if (playingState.humanPlayerTurn && cardPopupRectangle.Contains(mouseClick) && !playingState.gameStartMode)
            {
                drawDice = false;
                drawBuild = false;
                drawTrade = false;
                playingState.buildingSettlement = false;
                playingState.buildingCity = false;
                playingState.buildingRoad = false;
                drawCards = true;
            }

            //Mouse updates for buildCards
            //Updates the build bools if the buildCards are pressed
            if (OurGame.humanPlayer.canBuildRoad && roadCardRectangle.Contains(mouseClick) && drawBuild)
            {
                playingState.buildingRoad = true;
            }

            if (OurGame.humanPlayer.canBuildSettlement && settleCardRectangle.Contains(mouseClick) && drawBuild)
            {
                playingState.buildingSettlement = true;
            }

            if (OurGame.humanPlayer.canBuildCity && cityCardRectangle.Contains(mouseClick) && drawBuild)
            {
                playingState.buildingCity = true;
            }

            if (OurGame.humanPlayer.canBuildDevCard && devCardRectangle.Contains(mouseClick) && drawBuild)
            {
                playingState.BoughtDevCard(OurGame.humanPlayer);
            }

            //Controls the trade menu
            if (tradingMode)
            {
                bool test;
                for (int i = 0; i < 10; i++)
                {
                    if (increaseAmount[i].Contains(mouseClick))
                    {
                        test = testTradeIncrement(i, tradeNumbers[i]);
                        if (test)
                            tradeNumbers[i]++;
                    }
                    if (decreaseAmount[i].Contains(mouseClick))
                    {
                        if (tradeNumbers[i] > 0)
                            tradeNumbers[i]--;
                    }
                }
                if (acceptTrade.Contains(mouseClick))
                {
                    tradingMode = false;
                    drawTrade = false;
                    for (int i = 0; i < 5; i++)
                    {
                        playingState.currentTrade.tradeOffer[i] = tradeNumbers[i];
                        playingState.currentTrade.tradeRequest[i] = tradeNumbers[i + 5];
                    }
                    playingState.PlayerRequestedTrade(OurGame.humanPlayer);
                    Array.Clear(tradeNumbers, 0, tradeNumbers.Length);
                }
                if (bankTrade.Contains(mouseClick))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        playingState.currentTrade.tradeOffer[i] = tradeNumbers[i];
                        playingState.currentTrade.tradeRequest[i] = tradeNumbers[i + 5];
                    }
                    if (playingState.currentTrade.isBankTradeValid(OurGame.humanPlayer))
                    {
                        tradingMode = false;
                        drawTrade = false;
                        Array.Clear(tradeNumbers, 0, tradeNumbers.Length);
                    }
                }
                if (declineTrade.Contains(mouseClick))
                {
                    tradingMode = false;
                    drawTrade = false;
                    Array.Clear(tradeNumbers, 0, tradeNumbers.Length);
                }
                OurGame.humanPlayer.SetBuildBools();
            }

            //Controls the yearOfPlenty Menu
            if (playingState.yearOfPlentyMode)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (yopUp[i].Contains(mouseClick) && yopCounter < 2)
                    {                     
                        yopNumbers[i]++;
                        yopCounter++;
                    }
                    if (yopDown[i].Contains(mouseClick))
                    {
                        if (yopNumbers[i] > 0)
                        {
                            yopNumbers[i]--;
                            yopCounter--;
                        }
                    }
                }
                if (yopAccept.Contains(mouseClick))
                {
                    yopCounter = 0;
                    playingState.yearOfPlentyMode = false;
                    OurGame.humanPlayer.devCards[6]--;
                    OurGame.humanPlayer.cardsAvailable[6]--;
                    playingState.currentTrade.yopRequest = yopNumbers;
                    playingState.currentTrade.yopTrade(OurGame.humanPlayer);
                    playedCard = true;
                    Array.Clear(yopNumbers, 0, yopNumbers.Length);
                }
                if (yopDecline.Contains(mouseClick))
                {
                    yopCounter = 0;
                    playingState.yearOfPlentyMode = false;
                    Array.Clear(yopNumbers, 0, yopNumbers.Length);
                }
                OurGame.humanPlayer.SetBuildBools();
            }

            //Control DevCards
            if (drawCards)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (OurGame.humanPlayer.cardsAvailable[i] > 0)
                    {
                        if (devCardRectangles[i].Contains(mouseClick) && !playedCard)
                        {
                            playingState.PlayDevCard(i, OurGame.humanPlayer);
                            drawCards = false;
                        }
                    }
                }
            }

            //Control choosing which AI gets stolen from when robber moved
            if (playingState.chooseTheft)
            {               
                robberHex = OurGame.gameBoard.findRobber();
                aiCanSteal = OurGame.gameBoard.eligibleVictims(robberHex, OurGame, OurGame.humanPlayer);
                foreach (Player px in aiCanSteal)
                {
                    if (px == OurGame.aiPlayer1)
                    {
                        if (aiOneStealPos.Contains(mouseClick))
                        {
                            int stolenResource = OurGame.humanPlayer.stealRandomCard(OurGame.aiPlayer1);
                            if (stolenResource != -1)
                                OurGame.humanPlayer.incResource(stolenResource);
                            OurGame.humanPlayer.canEndTurn = true;
                            OurGame.humanPlayer.canBuild = true;
                            OurGame.humanPlayer.canTrade = true;
                            OurGame.humanPlayer.SetBuildBools();
                            playingState.movingRobber = false;
                            playingState.chooseTheft = false;
                        }
                    }
                    if (px == OurGame.aiPlayer2)
                    {
                        if (aiTwoStealPos.Contains(mouseClick))
                        {
                            int stolenResource = OurGame.humanPlayer.stealRandomCard(OurGame.aiPlayer2);
                            if (stolenResource != -1)
                                OurGame.humanPlayer.incResource(stolenResource);
                            OurGame.humanPlayer.canEndTurn = true;
                            OurGame.humanPlayer.canBuild = true;
                            OurGame.humanPlayer.canTrade = true;
                            OurGame.humanPlayer.SetBuildBools();
                            playingState.movingRobber = false;
                            playingState.chooseTheft = false;
                        }
                    }
                }
            }

            //control monopoly mode cards
            if (playingState.monopolyMode)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (monopolyRectangles[i].Contains(mouseClick))
                    {
                        playingState.gameCard.MonopolyChosen(i, OurGame.humanPlayer);
                        playingState.monopolyMode = false;
                    }
                }
            }

            //control player reject or accept of trade offered to AI players
            if (playingState.aiResponded)
            {
                if (playingState.aiOneResponse)
                {
                    if (playerRejects.Contains(mouseClick))
                    {
                        playingState.aiResponded = false;
                    }
                    if (aiOneResponse.Contains(mouseClick))
                    {
                        playingState.currentTrade.humanTradeWithAI(OurGame.humanPlayer, OurGame.aiPlayer1);
                        playingState.aiResponded = false;
                    }
                }
                if (playingState.aiTwoResponse)
                {
                    if (playerRejects.Contains(mouseClick))
                    {
                        playingState.aiResponded = false;
                    }
                    if (aiTwoResponse.Contains(mouseClick))
                    {
                        playingState.currentTrade.humanTradeWithAI(OurGame.humanPlayer, OurGame.aiPlayer2);
                        playingState.aiResponded = false;
                    }
                }
                if (!playingState.aiOneResponse && !playingState.aiTwoResponse)
                {
                    if (turnCountZero)
                    {
                        playingState.turnWaitCounter = 0;
                        turnCountZero = false;
                    }
                    if (playingState.turnWaitCounter > 2)
                    {
                        playingState.aiResponded = false;
                        turnCountZero = true;
                    }
                }

            }

            //Control menu for when AI requests trade
            if (playingState.aiRequestsTradeFromHuman)
            {
                for (int i = 0; i < 5; i++)
                {
                    aiTradeNumbers[i] = playingState.currentTrade.tradeOffer[i];
                    aiTradeNumbers[i + 5] = playingState.currentTrade.tradeRequest[i];
                }
                for (int i = 0; i < 3; i++)
                {
                    if (playingState.playerTurn == OurGame.players[i].playerNumber)
                    {
                        if (OurGame.players[i] == OurGame.aiPlayer1)
                            aiOffering = "Jeff";
                        else if (OurGame.players[i] == OurGame.aiPlayer2)
                            aiOffering = "Mary";
                    }
                }

                if (acceptAITrade.Contains(mouseClick))
                {
                    playingState.humanAcceptsTrade = true;
                    playingState.humanResponded = true;
                    playingState.aiRequestsTradeFromHuman = false;
                }
                if (rejectAITrade.Contains(mouseClick))
                {
                    playingState.humanAcceptsTrade = false;
                    playingState.humanResponded = true;
                    playingState.aiRequestsTradeFromHuman = false;
                }
            }

            //this controls the robber give away menu (uses year of plenty menu rectangles)
            if (playingState.robberGiveAway)
            {
                bool test;
                amountGive = OurGame.humanPlayer.totalResources / 2;
                for (int i = 0; i < 5; i++)
                {
                    if (yopUp[i].Contains(mouseClick))
                    {
                        test = testTradeIncrement(i, yopNumbers[i]);
                        if (test)
                            yopNumbers[i]++;
                    }
                    if (yopDown[i].Contains(mouseClick))
                    {
                        if (yopNumbers[i] > 0)
                        {
                            yopNumbers[i]--;
                        }
                    }
                }
                if (yopAccept.Contains(mouseClick))
                {
                    bool isValid;
                    playingState.currentTrade.yopRequest = yopNumbers;
                    isValid = playingState.currentTrade.robberGiveAway(OurGame.humanPlayer);
                    if (isValid)
                    {
                        playingState.robberGiveAway = false;
                        if (playingState.humanPlayerTurn)
                            playingState.movingRobber = true;
                        Array.Clear(yopNumbers, 0, yopNumbers.Length);
                    }
                }
                if (yopDecline.Contains(mouseClick))
                {                  
                    Array.Clear(yopNumbers, 0, yopNumbers.Length);
                }
                OurGame.humanPlayer.SetBuildBools();
                OurGame.humanPlayer.ResourceSum();
            }

            base.Update(gameTime);
        }//End Update

        //Purpose: Limit the trade possibilities in the trade menu to the amount of resources the player has
        //Params: index - index of tradeNumbers array which corresponds to the different resources, amount - amount of resources the player has
        //Returns: boolean true/false, true if the player can increment the resource, false if the player doesn't have the resources to increment
        private bool testTradeIncrement(int index, int amount)
        {
            switch (index)
            {
                case 0:
                    {
                        if (OurGame.humanPlayer.brick == amount)
                            return false;
                        break;
                    }
                case 1:
                    {
                        if (OurGame.humanPlayer.wheat == amount)
                            return false;
                        break;
                    }
                case 2:
                    {
                        if (OurGame.humanPlayer.wood == amount)
                            return false;
                        break;
                    }
                case 3:
                    {
                        if (OurGame.humanPlayer.wool == amount)
                            return false;
                        break;
                    }
                case 4:
                    {
                        if (OurGame.humanPlayer.ore == amount)
                            return false;
                        break;
                    }
                default:
                    {
                        return true;
                    }
            }
            return true;
        }

        //Draw the sprites for the Hud where they need to be
        public override void Draw(GameTime gameTime)
        {                  
            //Draw Sprites Begin
            OurGame.SpriteBatch.Begin();

            DrawMainHud();
            DrawAIHuds();
            DrawAISteal();
            DrawAITradeResponse();
            DrawTradeMenu();
            DrawAITradeMenu();
            DrawDice(OurGame.gameDice);
            DrawBuild();
            DrawDevCards();
            DrawYearOfPlenty();
            DrawMonopolyMode();

            //Write X, Y coord of mouseclick for testing
            OurGame.SpriteBatch.DrawString(OurGame.Font, mouseXText,
                mouseXTextPosition, brown);
            OurGame.SpriteBatch.DrawString(OurGame.Font, mouseYText,
                mouseYTextPosition, brown);

            OurGame.SpriteBatch.End();

            base.Draw(gameTime);
        }//End Draw

        //Purpose: draws the main player hud
        private void DrawMainHud()
        {
            //Assign humanPlayer scores text
            //Requirement 4.3.0 and 4.3.1
            String brickText = "x" + OurGame.humanPlayer.brick.ToString();
            String wheatText = "x" + OurGame.humanPlayer.wheat.ToString();
            String woodText = "x" + OurGame.humanPlayer.wood.ToString();
            String woolText = "x" + OurGame.humanPlayer.wool.ToString();
            String oreText = "x" + OurGame.humanPlayer.ore.ToString();
            //Requirement 4.2.0
            String knightsText = "x" + OurGame.humanPlayer.knights.ToString();
            //Requirement 4.1.0
            String roadsText = "x" + OurGame.humanPlayer.longestRoad.ToString();
            //Requirement 4.0.1
            String victoryPointText = "x" + OurGame.humanPlayer.hiddenVictoryPoints.ToString();

            //Draw the main Hud for the player to interact with
            OurGame.SpriteBatch.Draw(hudBackground,
                new Rectangle(screenWidth / 2 - hudBackground.Width / 2,
                    screenHeight - (hudBackground.Height / 2 + TitleSafeArea.Bottom),
                    hudBackground.Width,
                    hudBackground.Height / 2),
                new Rectangle(0, 256, hudBackground.Width, hudBackground.Height / 2),
                    Color.White);
            DrawHudActions();
            OurGame.SpriteBatch.Draw(hudResources,
                new Rectangle((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + hudActions.Width / 2 + 10,
                    screenHeight - (hudResources.Height + TitleSafeArea.Bottom - 10),
                    hudResources.Width - 10,
                    hudResources.Height - 15),
                new Rectangle(0, 0, hudResources.Width, hudResources.Height),
                    Color.White);
            OurGame.SpriteBatch.Draw(hudLongests,
                new Rectangle((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + hudActions.Width / 2 + hudResources.Width + 10,
                    screenHeight - (hudLongests.Height + TitleSafeArea.Bottom - 10),
                    hudLongests.Width - 10,
                    hudLongests.Height - 15),
                new Rectangle(0, 0, hudLongests.Width, hudLongests.Height), Color.White);
            OurGame.SpriteBatch.Draw(hudVictoryPoints,
                new Rectangle((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + hudActions.Width / 2 + hudResources.Width + hudLongests.Width + 10,
                    screenHeight - (hudVictoryPoints.Height + TitleSafeArea.Bottom - 10),
                    hudVictoryPoints.Width - 10,
                    hudVictoryPoints.Height - 15),
                new Rectangle(0, 0, hudVictoryPoints.Width, hudVictoryPoints.Height),
                    Color.White);
            OurGame.SpriteBatch.Draw(hudCardPopup,
                cardPopupRectangle,
                new Rectangle(0, 0, hudCardPopup.Width, hudCardPopup.Height),
                    Color.White);

            //Write out player information scores on the Hud
            //Requirement 4.0.0
            //Requirement 4.3.0 and 4.3.1
            OurGame.SpriteBatch.DrawString(OurGame.Font, brickText,
                brickTextPosition, brown);
            OurGame.SpriteBatch.DrawString(OurGame.Font, wheatText,
                wheatTextPosition, brown);
            OurGame.SpriteBatch.DrawString(OurGame.Font, woodText,
                woodTextPosition, brown);
            OurGame.SpriteBatch.DrawString(OurGame.Font, woolText,
                woolTextPosition, brown);
            OurGame.SpriteBatch.DrawString(OurGame.Font, oreText,
                oreTextPosition, brown);
            //Requirement 4.2.0
            OurGame.SpriteBatch.DrawString(largeFont, knightsText,
                knightsTextPosition, brown);
            //Requirement 4.1.0
            OurGame.SpriteBatch.DrawString(largeFont, roadsText,
                roadsTextPosition, brown);
            //Requirement 4.0.1
            OurGame.SpriteBatch.DrawString(largeFont, victoryPointText,
                victoryPointTextPosition, brown);
        }

        private void DrawAIHuds()
        {
            //Assign AIOne scores text
            String aiOneResourceText = OurGame.aiPlayer1.totalResources.ToString();
            String aiOneKnightsText = OurGame.aiPlayer1.TotalDevCards().ToString();
            String aiOneDevCardsText = OurGame.aiPlayer1.knights.ToString();
            String aiOneRoadsText = OurGame.aiPlayer1.longestRoad.ToString();
            String aiOneVPText = OurGame.aiPlayer1.victoryPoints.ToString();

            //Assign AITwo scores text
            String aiTwoResourceText = OurGame.aiPlayer2.totalResources.ToString();
            String aiTwoKnightsText = OurGame.aiPlayer2.TotalDevCards().ToString();
            String aiTwoDevCardsText = OurGame.aiPlayer2.knights.ToString();
            String aiTwoRoadsText = OurGame.aiPlayer2.longestRoad.ToString();
            String aiTwoVPText = OurGame.aiPlayer2.victoryPoints.ToString();

            //Draw AI Huds
            OurGame.SpriteBatch.Draw(aiOne, new Rectangle(TitleSafeArea.Left, TitleSafeArea.Top, 256, 256), new Rectangle(0, 0, 256, 256), Color.White);
            OurGame.SpriteBatch.Draw(aiTwo, new Rectangle(screenWidth - (aiTwo.Width + TitleSafeArea.Right), TitleSafeArea.Top, 256, 256), new Rectangle(0, 0, 256, 256), Color.White);

            //Write out AI player information on the AI Huds
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiOneResourceText,
               aiOneResourcesPos, Color.Black);
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiOneDevCardsText,
               aiOneDevCardsPos, Color.Black);
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiOneKnightsText,
               aiOneKnightsPos, Color.Black);
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiOneRoadsText,
               aiOneRoadsPos, Color.Black);
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiOneVPText,
               aiOneVPPos, Color.Black);
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiTwoResourceText,
                aiTwoResourcesPos, Color.Black);
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiTwoDevCardsText,
               aiTwoDevCardsPos, Color.Black);
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiTwoKnightsText,
               aiTwoKnightsPos, Color.Black);
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiTwoRoadsText,
               aiTwoRoadsPos, Color.Black);
            OurGame.SpriteBatch.DrawString(OurGame.Font, aiTwoVPText,
               aiTwoVPPos, Color.Black);
        }

        //Purpose: To draw the trade menu after the trade menu button has been pressed
        //Requirement 4.5.1
        private void DrawTradeMenu()
        {
            //Draw trade Menu
            if (drawTrade)
            {
                OurGame.SpriteBatch.Draw(hudTradeMenu,
                    new Rectangle(screenWidth / 8,
                        screenHeight - (130 + hudTradeMenu.Height + TitleSafeArea.Bottom),
                        hudTradeMenu.Width,
                        hudTradeMenu.Height),
                    new Rectangle(0, 0, hudTradeMenu.Width, hudTradeMenu.Height),
                        Color.White);

                //Write out trade menu offer and request numbers
                for (int i = 0; i < 10; i++)
                {
                    tradeMenuText[i] = tradeNumbers[i].ToString();
                    OurGame.SpriteBatch.DrawString(OurGame.Font, tradeMenuText[i],
                         tradeTextPos[i], brown);
                }
            }
        }

        //Purpose: To draw the trade menu when the ai requests a trade
        //Requirement 4.5.1 continued
        private void DrawAITradeMenu()
        {
            if (playingState.aiRequestsTradeFromHuman)
            {
                OurGame.SpriteBatch.Draw(hudAITradeMenu,
                    new Rectangle(screenWidth / 8,
                        screenHeight - (130 + hudAITradeMenu.Height + TitleSafeArea.Bottom),
                        hudAITradeMenu.Width,
                        hudAITradeMenu.Height),
                    new Rectangle(0, 0, hudAITradeMenu.Width, hudAITradeMenu.Height),
                        Color.White);

                OurGame.SpriteBatch.DrawString(OurGame.Font, aiOffering,
                         aiOfferingPos, brown);
                //Write out trade menu offer and request numbers
                for (int i = 0; i < 10; i++)
                {
                    aiTradeMenuText[i] = aiTradeNumbers[i].ToString();
                    OurGame.SpriteBatch.DrawString(OurGame.Font, aiTradeMenuText[i],
                         aiTradeTextPos[i], brown);
                }
            }
        }

        //Purpose: To draw the player Hud Actions menu with the proper areas grayed out or highlighted depending on which state the Hud is in
        private void DrawHudActions()
        {
            //Draw the Hud Actions Menu
            OurGame.SpriteBatch.Draw(hudActions,
                new Rectangle((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 10,
                    screenHeight - (hudActions.Height + TitleSafeArea.Bottom - 10),
                    hudActions.Width / 2 - 10,
                    hudActions.Height - 15),
                new Rectangle(0, 0, hudActions.Width / 2, hudActions.Height),
                    Color.White);

            Color backGroundColor = new Color();
            //Draw the Dice sprite on the Hud Actions menu highlighted/grayed out as appropriate
            //Requirement 4.4.1
            if (OurGame.humanPlayer.canRollDice)
                backGroundColor = Color.White;
            else
                backGroundColor = Color.DarkGray;
            OurGame.SpriteBatch.Draw(hudActions,
                new Rectangle((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 12,
                    screenHeight - (hudActions.Height + TitleSafeArea.Bottom - 10),
                    hudActions.Width / 4 - 5,
                    hudActions.Height / 2 - 7),
                new Rectangle(hudActions.Width / 2, 0, hudActions.Width / 4, hudActions.Height / 2),
                    backGroundColor);
            //Draw the trade menu sprite on the Hud Actions menu highlighted/grayed out as appropriate
            //Requirement 4.5.0
            if (OurGame.humanPlayer.canTrade)
                backGroundColor = Color.White;
            else
                backGroundColor = Color.DarkGray;
            OurGame.SpriteBatch.Draw(hudActions,
                new Rectangle((screenWidth - hudBackground.Width) / 2 + hudActions.Width / 4 + TitleSafeArea.Left + 7,
                    screenHeight - (hudActions.Height + TitleSafeArea.Bottom - 10),
                    hudActions.Width / 4 - 5,
                    hudActions.Height / 2 - 7),
                new Rectangle(3 * hudActions.Width / 4, 0, hudActions.Width / 4, hudActions.Height / 2),
                    backGroundColor);
            //Draw the build menu sprite on the Hud Actions menu highlighted/grayed out as appropriate
            if (OurGame.humanPlayer.canBuild)
                backGroundColor = Color.White;
            else
                backGroundColor = Color.DarkGray;
            OurGame.SpriteBatch.Draw(hudActions,
                new Rectangle((screenWidth - hudBackground.Width) / 2 + hudActions.Width / 4 + TitleSafeArea.Left + 7,
                    screenHeight - (hudActions.Height / 2 + TitleSafeArea.Bottom - 3),
                    hudActions.Width / 4 - 5,
                    hudActions.Height / 2 - 7),
                new Rectangle(3 * hudActions.Width / 4, hudActions.Height / 2, hudActions.Width / 4, hudActions.Height / 2),
                    backGroundColor);
            //Draw the endturn button sprite on the Hud Actions menu highlighted/grayed out as appropriate
            //Requirement 4.6.0
            if (OurGame.humanPlayer.canEndTurn)
                backGroundColor = Color.White;
            else
                backGroundColor = Color.DarkGray;
            OurGame.SpriteBatch.Draw(hudActions,
                new Rectangle((screenWidth - hudBackground.Width) / 2 + TitleSafeArea.Left + 12,
                    screenHeight - (hudActions.Height / 2 + TitleSafeArea.Bottom - 3),
                    hudActions.Width / 4 - 5,
                    hudActions.Height / 2 - 7),
                new Rectangle(hudActions.Width / 2, hudActions.Height / 2, hudActions.Width / 4, hudActions.Height / 2),
                    backGroundColor);          
        }//End DrawHudActions

        //Purpose: To draw the dice on the screen that the player has rolled
        //Requirement 4.4.0
        private void DrawDice(Dice dice)
        {            
            if (drawDice)
            {
                int[] diceRoll = dice.getDiceRoll();
                Texture2D Die = hudDiceOne;
                for (int i = 1; i < 3; i++)
                {
                    int die;
                    if (i == 1)
                        die = diceRoll[0];
                    else
                        die = diceRoll[1];
                    switch (die)
                    {
                        case 1:
                            {
                                Die = hudDiceOne;
                                break;
                            }
                        case 2:
                            {
                                Die = hudDiceTwo;
                                break;
                            }
                        case 3:
                            {
                                Die = hudDiceThree;
                                break;
                            }
                        case 4:
                            {
                                Die = hudDiceFour;
                                break;
                            }
                        case 5:
                            {
                                Die = hudDiceFive;
                                break;
                            }
                        case 6:
                            {
                                Die = hudDiceSix;
                                break;
                            }
                    }
                    //Draw die one
                    if (i == 1)
                    {
                        OurGame.SpriteBatch.Draw(Die,
                            new Rectangle(screenWidth / 8 - 40,
                            screenHeight - (190 + TitleSafeArea.Bottom),
                            hudDiceOne.Width,
                            hudDiceOne.Height),
                            new Rectangle(0, 0, hudDiceOne.Width, hudDiceOne.Height),
                            Color.White);
                    }
                    else             //draw die two
                    {
                        OurGame.SpriteBatch.Draw(Die,
                            new Rectangle(screenWidth / 6 - 40,
                            screenHeight - (190 + TitleSafeArea.Bottom),
                            hudDiceOne.Width,
                            hudDiceOne.Height),
                            new Rectangle(0, 0, hudDiceOne.Width, hudDiceOne.Height),
                            Color.White);
                    }
                }
            }
        }//End DrawDice

        //Purpose: To draw build cards if the player has clicked the build menu button with the build options that the player
        //         can do highlighted, and ones that they cannot do grayed out
        private void DrawBuild()
        {
            if (drawBuild)
            {
                Color backGroundColor = new Color();
                //Draw road build card
                if (OurGame.humanPlayer.canBuildRoad)
                    backGroundColor = Color.White;
                else
                    backGroundColor = Color.DarkGray;
                OurGame.SpriteBatch.Draw(hudBuildRoad,
                    roadCardRectangle,
                    new Rectangle(0, 0, hudBuildRoad.Width, hudBuildRoad.Height),
                backGroundColor);
                //draw settlement build card
                if (OurGame.humanPlayer.canBuildSettlement)
                    backGroundColor = Color.White;
                else
                    backGroundColor = Color.DarkGray;
                OurGame.SpriteBatch.Draw(hudBuildSettlement,
                    settleCardRectangle,
                    new Rectangle(0, 0, hudBuildRoad.Width, hudBuildRoad.Height),
                backGroundColor);
                //draw city build card
                if (OurGame.humanPlayer.canBuildCity)
                    backGroundColor = Color.White;
                else
                    backGroundColor = Color.DarkGray;
                OurGame.SpriteBatch.Draw(hudBuildCity,
                    cityCardRectangle,
                    new Rectangle(0, 0, hudBuildRoad.Width, hudBuildRoad.Height),
                backGroundColor);
                //draw devcard build card
                if (OurGame.humanPlayer.canBuildDevCard)
                    backGroundColor = Color.White;
                else
                    backGroundColor = Color.DarkGray;
                OurGame.SpriteBatch.Draw(hudBuildDevCard,
                    devCardRectangle,
                    new Rectangle(0, 0, hudBuildRoad.Width, hudBuildRoad.Height),
                backGroundColor);
            }           
        }//End DrawBuild

        //Purpose: To draw the developement Cards if the devCard button has been pressed
        private void DrawDevCards()
        {
            //Draw development cards that the player owns if the devCards button is pressed
            if (drawCards)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (OurGame.humanPlayer.devCards[i] > 0)
                    {
                        OurGame.SpriteBatch.Draw(developmentCards[i],
                        devCardRectangles[i],
                        new Rectangle(0, 0, developmentCards[i].Width, developmentCards[i].Height),
                        Color.White);
                    }
                }
            }
        }//End DrawDevCards

        //Purpose: Draw Yearofplenty menu/robber give away menu
        private void DrawYearOfPlenty()
        {
            if (playingState.yearOfPlentyMode || playingState.robberGiveAway)
            {
                OurGame.SpriteBatch.Draw(hudYearOfPlenty,
                    new Rectangle( 6 * screenWidth / 8,
                        screenHeight - (130 + hudYearOfPlenty.Height + TitleSafeArea.Bottom),
                        hudYearOfPlenty.Width,
                        hudYearOfPlenty.Height),
                    new Rectangle(0, 0, hudYearOfPlenty.Width, hudYearOfPlenty.Height),
                        Color.White);
                //Write out trade menu offer and request numbers
                for (int i = 0; i < 5; i++)
                {
                    yopMenuText[i] = yopNumbers[i].ToString();
                    OurGame.SpriteBatch.DrawString(OurGame.Font, yopMenuText[i],
                         yopTextPos[i], brown);
                }
                if (playingState.robberGiveAway)
                {
                    OurGame.SpriteBatch.DrawString(OurGame.Font, amountGive.ToString(),
                         amountGivePos, brown);
                }
            }
        }

        //Purpose: Draw AI steal from buttons
        private void DrawAISteal()
        {
            if (playingState.chooseTheft)
            {
                foreach (Player px in aiCanSteal)
                {
                    if (px == OurGame.aiPlayer1)
                    {
                        OurGame.SpriteBatch.Draw(aiOneSteal,
                            aiOneStealPos,
                            new Rectangle(0, 0, aiOneSteal.Width, aiOneSteal.Height),
                                Color.White);
                    }
                    if (px == OurGame.aiPlayer2)
                    {
                        OurGame.SpriteBatch.Draw(aiTwoSteal,
                            aiTwoStealPos,
                            new Rectangle(0, 0, aiTwoSteal.Width, aiTwoSteal.Height),
                                Color.White);
                    }
                }
            }
        }

        //Purpose: Draw monopoly mode menu
        private void DrawMonopolyMode()
        {
            if (playingState.monopolyMode)
            {
                for (int i = 0; i < 5; i++)
                {
                    OurGame.SpriteBatch.Draw(monopolyTextures[i],
                    monopolyRectangles[i],
                    new Rectangle(0, 0, monopolyTextures[i].Width, monopolyTextures[i].Height),
                    Color.White);
                }
            }
        }

        //Purpose: Draw whether ai accepted/rejected trade offer
        private void DrawAITradeResponse()
        {
            if (playingState.aiResponded)
            {
                if (playingState.aiOneResponse)
                {
                    OurGame.SpriteBatch.Draw(aiAccept,
                           aiOneResponse,
                           new Rectangle(0, 0, aiAccept.Width, aiAccept.Height),
                               Color.White);
                    OurGame.SpriteBatch.Draw(aiReject,
                           playerRejects,
                           new Rectangle(0, 0, aiAccept.Width, aiAccept.Height),
                               Color.White);
                }
                else
                {
                    OurGame.SpriteBatch.Draw(aiReject,
                           aiOneResponse,
                           new Rectangle(0, 0, aiAccept.Width, aiAccept.Height),
                               Color.White);
                }
                if (playingState.aiTwoResponse)
                {
                    OurGame.SpriteBatch.Draw(aiAccept,
                           aiTwoResponse,
                           new Rectangle(0, 0, aiAccept.Width, aiAccept.Height),
                               Color.White);
                    OurGame.SpriteBatch.Draw(aiReject,
                       playerRejects,
                       new Rectangle(0, 0, aiAccept.Width, aiAccept.Height),
                           Color.White);
                }
                else
                {
                    OurGame.SpriteBatch.Draw(aiReject,
                           aiTwoResponse,
                           new Rectangle(0, 0, aiAccept.Width, aiAccept.Height),
                               Color.White);
                }
            }          
        }


    }
}
