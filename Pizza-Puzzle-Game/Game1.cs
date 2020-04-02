using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pizza_Puzzle_Game.GameObjects;
using System.Collections.Generic;

namespace Pizza_Puzzle_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Textures & Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<GameObject> renderables = new List<GameObject>();
        List<GameObject> updatables = new List<GameObject>();
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = (int)Program.PPU * 19;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = (int)Program.PPU * 26;   // set this value to the desired height of your window
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
            renderables.Add(new MaketableObject(new Vector2(Program.PPU * 2, Program.PPU * 2), Content.Load<Texture2D>("maketable"), Color.White));

            Texture2D pizzaPanTex = Content.Load<Texture2D>("pan");
            renderables.Add(new PanObject(new Vector2(Program.PPU * 4, Program.PPU * 21), pizzaPanTex, Color.White));
            renderables.Add(new PanObject(new Vector2(Program.PPU * 7, Program.PPU * 21), pizzaPanTex, Color.White));
            renderables.Add(new PanObject(new Vector2(Program.PPU * 10, Program.PPU * 21), pizzaPanTex, Color.White));
            renderables.Add(new PanObject(new Vector2(Program.PPU * 13, Program.PPU * 21), pizzaPanTex, Color.White));

            PlayerObject newPlayer = new PlayerObject(new Vector2(Program.PPU * 9, Program.PPU * 21), Content.Load<Texture2D>("arrows"), Color.White);
            renderables.Add(newPlayer);
            updatables.Add(newPlayer);
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
            foreach (GameObject updatable in updatables)
                updatable.Update(gameTime);

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

            foreach (GameObject renderable in renderables)
                renderable.Render(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
