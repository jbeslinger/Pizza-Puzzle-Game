﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pizza_Puzzle_Game.GameObjects;
using System;
using System.Collections.Generic;

namespace Pizza_Puzzle_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Fields
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private SpriteBatch m_TargetBatch;
        private RenderTarget2D m_Target;

        private Texture2D m_BackgroundTex;

        private uint m_Scale = 4;
        private uint m_ResWidth = (uint)Program.PPU * 32;
        private uint m_ResHeight = (uint)Program.PPU * 28;

        private List<GameObject> m_Renderables = new List<GameObject>();
        private List<GameObject> m_Updatables = new List<GameObject>();
        #endregion

        #region Constructors
        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);

            m_Graphics.PreferredBackBufferWidth = (int)m_ResWidth * (int)m_Scale;  // set this value to the desired width of your window
            m_Graphics.PreferredBackBufferHeight = (int)m_ResHeight * (int)m_Scale;   // set this value to the desired height of your window
            m_Graphics.PreferMultiSampling = false;
            m_Graphics.ApplyChanges();

            m_TargetBatch = new SpriteBatch(GraphicsDevice);
            m_Target = new RenderTarget2D(GraphicsDevice, (int)m_ResWidth, (int)m_ResHeight);

            Content.RootDirectory = "Content";
        }
        #endregion

        #region Methods
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            m_BackgroundTex = Content.Load<Texture2D>("bg");

            m_Renderables.Add(new MaketableObject(new Vector2(Program.PPU * 8, Program.PPU * 3), Content.Load<Texture2D>("maketable"), Color.White));

            Texture2D pizzaPanTex = Content.Load<Texture2D>("pan");
            m_Renderables.Add(new PanObject(new Vector2(Program.PPU * 9.5f, Program.PPU * 22f), pizzaPanTex, Color.White));
            m_Renderables.Add(new PanObject(new Vector2(Program.PPU * 12.5f, Program.PPU * 22f), pizzaPanTex, Color.White));
            m_Renderables.Add(new PanObject(new Vector2(Program.PPU * 15.5f, Program.PPU * 22f), pizzaPanTex, Color.White));
            m_Renderables.Add(new PanObject(new Vector2(Program.PPU * 18.5f, Program.PPU * 22f), pizzaPanTex, Color.White));

            PlayerObject newPlayer = new PlayerObject(new Vector2(Program.PPU * 15, Program.PPU * 22), Content.Load<Texture2D>("arrows"), Color.White);
            m_Renderables.Add(newPlayer);
            m_Updatables.Add(newPlayer);

            Texture2D ingredientsTex = Content.Load<Texture2D>("toppings");

            Random r = new Random();
            IngredientObject ing1 = new IngredientObject(new Vector2(Program.PPU * 9.5f, Program.PPU * 4f), ingredientsTex, Color.White, r.Next(0, 7));
            m_Renderables.Add(ing1);
            m_Updatables.Add(ing1);
            IngredientObject ing2 = new IngredientObject(new Vector2(Program.PPU * 15.5f, Program.PPU * 4f), ingredientsTex, Color.White, r.Next(0, 7));
            m_Renderables.Add(ing2);
            m_Updatables.Add(ing2);
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            Keyboard.GetState();
            GamePad.GetState();
            if (Keyboard.HasBeenPressed(Keys.Escape) || GamePad.HasBeenPressed(Buttons.Start))
                Exit();

            // TODO: Add your update logic here
            foreach (GameObject updatable in m_Updatables)
                updatable.Update(gameTime);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            GraphicsDevice.SetRenderTarget(m_Target);
            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(m_BackgroundTex, new Vector2(0, 0), Color.White);
            foreach (GameObject renderable in m_Renderables)
                renderable.Render(m_SpriteBatch);
            m_SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            m_TargetBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            m_TargetBatch.Draw(m_Target, new Rectangle(0, 0, m_Graphics.PreferredBackBufferWidth, m_Graphics.PreferredBackBufferHeight), Color.White);
            m_TargetBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
