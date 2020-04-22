using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pizza_Puzzle_Game.GameObjects;
using System.Collections.Generic;

namespace Pizza_Puzzle_Game
{
    public class Game1 : Game
    {
        #region Fields
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private SpriteBatch m_TargetBatch;
        private RenderTarget2D m_Target;

        private uint m_Scale = 4;
        private uint m_ResWidth = (uint)Program.PPU * 32;
        private uint m_ResHeight = (uint)Program.PPU * 28;

        public static readonly List<GameObject> m_Renderables = new List<GameObject>(); // A list of every object that contains an Render method
        public static readonly List<GameObject> m_Updatables = new List<GameObject>(); // A list of every object that contains an Update method
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
            new BackgroundObject(Vector2.Zero, Content.Load<Texture2D>("bg"), Color.White);
            new MaketableObject(Program.ToPixelPos(1.0f, 3.0f), Content.Load<Texture2D>("maketable"), Color.White, Content);
            new MaketableObject(Program.ToPixelPos(16.0f, 3.0f), Content.Load<Texture2D>("maketable"), Color.White, Content);
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
            m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

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
