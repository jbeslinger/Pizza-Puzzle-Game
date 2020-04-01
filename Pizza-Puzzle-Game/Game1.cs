using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pizza_Puzzle_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Constants
        const float PPU = 8.0f;
        #endregion

        #region Enums
        enum PlayerPosition { LEFT = 0, MIDDLE = 1, RIGHT = 2 };
        #endregion

        #region Textures & Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D maketableTex;
        Texture2D pizzaPanTex;
        Texture2D playerArrowsTex;
        #endregion

        #region Game Variables
        // The pixel coordinates of the pans on the bottom
        Vector2[] panPositions =
            { new Vector2(PPU * 4, PPU * 21),
              new Vector2(PPU * 7, PPU * 21),
              new Vector2(PPU * 10, PPU * 21),
              new Vector2(PPU * 13, PPU * 21) };

        PlayerPosition playerPos = PlayerPosition.MIDDLE; // Based on the index of the panPositions variable
        bool playerSwapToggle; // A flag to indicate that the player pressed the swap button
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = (int)PPU * 19;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = (int)PPU * 26;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            maketableTex = Content.Load<Texture2D>("maketable");
            pizzaPanTex = Content.Load<Texture2D>("pan");
            playerArrowsTex = Content.Load<Texture2D>("arrows");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Keyboard.GetState();
            if (Keyboard.HasBeenPressed(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.HasBeenPressed(Keys.Z))
                playerSwapToggle = !playerSwapToggle;
            else if (Keyboard.HasBeenPressed(Keys.Left) && playerPos != (PlayerPosition)0)
                playerPos--;
            else if (Keyboard.HasBeenPressed(Keys.Right) && playerPos != (PlayerPosition)2)
                playerPos++;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(maketableTex, new Vector2(PPU * 2, PPU * 2), Color.White);

            foreach (Vector2 pan in panPositions)
            {
                spriteBatch.Draw(pizzaPanTex, pan, Color.White);
            }

            switch (playerPos)
            {
                case PlayerPosition.LEFT:
                    if (playerSwapToggle)
                    {
                        spriteBatch.Draw(playerArrowsTex, panPositions[0], Color.Red);
                        spriteBatch.Draw(playerArrowsTex, panPositions[1], Color.Blue);
                    }
                    else
                    {
                        spriteBatch.Draw(playerArrowsTex, panPositions[0], Color.Blue);
                        spriteBatch.Draw(playerArrowsTex, panPositions[1], Color.Red);
                    }
                    break;
                case PlayerPosition.MIDDLE:
                    if (playerSwapToggle)
                    {
                        spriteBatch.Draw(playerArrowsTex, panPositions[1], Color.Red);
                        spriteBatch.Draw(playerArrowsTex, panPositions[2], Color.Blue);
                    }
                    else
                    {
                        spriteBatch.Draw(playerArrowsTex, panPositions[1], Color.Blue);
                        spriteBatch.Draw(playerArrowsTex, panPositions[2], Color.Red);
                    }
                    break;
                case PlayerPosition.RIGHT:
                    if (playerSwapToggle)
                    {
                        spriteBatch.Draw(playerArrowsTex, panPositions[2], Color.Red);
                        spriteBatch.Draw(playerArrowsTex, panPositions[3], Color.Blue);
                    }
                    else
                    {
                        spriteBatch.Draw(playerArrowsTex, panPositions[2], Color.Blue);
                        spriteBatch.Draw(playerArrowsTex, panPositions[3], Color.Red);
                    }
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
