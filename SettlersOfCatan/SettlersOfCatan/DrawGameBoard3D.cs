/*Draws the game board using 3D objects
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
    class DrawGameBoard3D : DrawableGameComponent
    {
        private Model brick;
        private Model desert;
        private Model grain;
        private Model lumber;
        private Model ore;
        private Model wool;
        Texture2D backgroundTexture;
        Texture2D port;

        Texture2D blueRoad;
        Texture2D orangeRoad;
        Texture2D whiteRoad;
        Texture2D blueHouse;
        Texture2D orangeHouse;
        Texture2D whiteHouse;
        Texture2D blueSettlement;
        Texture2D orangeSettlement;
        Texture2D whiteSettlement;

        Texture2D robber;


        private BasicEffect effect;
        BasicEffect basicEffect;

        private SettlersOfCatan ourGame;
        private GameBoard socGameBoard;

        private float hexHeight = 25.2f;
        private float hexWidth = 45;

        private int screenWidth;
        private int screenHeight;
        Rectangle screenRectangle;

        public DrawGameBoard3D(Game game, GameBoard gameBoard)
            : base(game)
        {
            ourGame = (SettlersOfCatan)game;
            socGameBoard = (GameBoard)gameBoard;
            screenWidth = ourGame.width;
            screenHeight = ourGame.height;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        // Import 3D models/textures for the hexagons that make up the game board
        // Import 2D Sprites that make up the sprites to be placed on the gameboard, such as towns
        // Create the basic effect graphics devices
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            ourGame.SpriteBatch = new SpriteBatch(GraphicsDevice);

            desert = ourGame.Content.Load<Model>(@"Models\Desert_Map");
            grain = ourGame.Content.Load<Model>(@"Models\Grain_Map");
            lumber = ourGame.Content.Load<Model>(@"Models\Lumber_Map");
            ore = ourGame.Content.Load<Model>(@"Models\Ore_Map");
            wool = ourGame.Content.Load<Model>(@"Models\Wool_Map");
            brick = ourGame.Content.Load<Model>(@"Models\Brick_Map");
            port = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Port");

            blueRoad = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_RoadBlue");
            orangeRoad = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_RoadOrange");
            whiteRoad = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_RoadWhite");
            blueHouse = ourGame.Content.Load<Texture2D>(@"Textures\Board\House_Blue");
            orangeHouse = ourGame.Content.Load<Texture2D>(@"Textures\Board\House_Orange");
            whiteHouse = ourGame.Content.Load<Texture2D>(@"Textures\Board\House_White");
            blueSettlement = ourGame.Content.Load<Texture2D>(@"Textures\Board\Settlement_Blue");
            orangeSettlement = ourGame.Content.Load<Texture2D>(@"Textures\Board\Settlement_Orange");
            whiteSettlement = ourGame.Content.Load<Texture2D>(@"Textures\Board\Settlement_White");

            robber = ourGame.Content.Load<Texture2D>(@"Textures\Board\Board_Robber");

            backgroundTexture = ourGame.Content.Load<Texture2D>(@"Textures\Board\background");

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

        // This will probably need some stuff for when we actually get the program to place things based on mouse click
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // Draws the board
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Viewport = ourGame.camera.Viewport;
            DrawScene(gameTime, ourGame.camera);

            base.Draw(gameTime);
        }

        // Method to draw the board and sprites, probably will want to separate out the sprite drawing to make
        // this cleaner, but for now it works
        private void DrawScene(GameTime gameTime, Camera camera)
        {
            ourGame.SpriteBatch.Begin();
            ourGame.SpriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
            ourGame.SpriteBatch.End();

            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            effect.Projection = camera.Projection;
            effect.View = camera.View;

            effect.EnableDefaultLighting();
            Matrix world = new Matrix();
            DrawBoard(ref world);

            Vector3 housePosition = new Vector3(-88, 20, 1);

            basicEffect.World = Matrix.CreateScale( 0.5f ) * Matrix.CreateScale(1, -1, 1) * Matrix.CreateTranslation(housePosition);
            basicEffect.View = ourGame.camera.View;
            basicEffect.Projection = ourGame.camera.Projection;

            ourGame.SpriteBatch.Begin(0, BlendState.AlphaBlend, null, DepthStencilState.Default, RasterizerState.CullNone, basicEffect);
            ourGame.SpriteBatch.Draw(orangeSettlement, new Rectangle(0, 0, 75, 75), Color.White);
            ourGame.SpriteBatch.End();
        }

        // Translate and scale the 3d models to be drawn that make up the board (this really needs to be a
        // for loop that cycles through a linked list that has the board tiles and draws based on the information
        // stored about the tiles)
        private void DrawBoard(ref Matrix world)
        {
            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(-2 * hexWidth, 2 * hexHeight, 0));
            DrawModel(ref wool, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(-2 * hexWidth, 0, 0));
            DrawModel(ref lumber, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(-2 * hexWidth, -2 * hexHeight, 0));
            DrawModel(ref grain, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(-hexWidth, -3 * hexHeight, 0));
            DrawModel(ref lumber, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(-hexWidth, -hexHeight, 0));
            DrawModel(ref ore, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(-hexWidth, hexHeight, 0));
            DrawModel(ref grain, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(-hexWidth, 3 * hexHeight, 0));
            DrawModel(ref wool, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(0, 4 * hexHeight, 0));
            DrawModel(ref wool, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(0, 2 * hexHeight, 0));
            DrawModel(ref desert, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(0, 0, 0));
            DrawModel(ref brick, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(0, -2 * hexHeight, 0));
            DrawModel(ref lumber, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(0, -4 * hexHeight, 0));
            DrawModel(ref grain, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(hexWidth, -3 * hexHeight, 0));
            DrawModel(ref wool, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(hexWidth, -hexHeight, 0));
            DrawModel(ref grain, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(hexWidth, hexHeight, 0));
            DrawModel(ref lumber, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(hexWidth, 3 * hexHeight, 0));
            DrawModel(ref brick, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(2 * hexWidth, 2 * hexHeight, 0));
            DrawModel(ref brick, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(2 * hexWidth, 0, 0));
            DrawModel(ref ore, ref world);

            world = Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(new Vector3(2 * hexWidth, -2 * hexHeight, 0));
            DrawModel(ref ore, ref world);

        }

        // This is where the 3D models are actually drawn
        private void DrawModel(ref Model m, ref Matrix world)
        {
            Matrix[] transforms = new Matrix[m.Bones.Count];
            m.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = ourGame.camera.Projection;
                    be.View = ourGame.camera.View;
                    be.World = world * transforms[mesh.ParentBone.Index];
                }

                mesh.Draw();
            }
        }
    }
}

