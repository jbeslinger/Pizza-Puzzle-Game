using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Timers;

namespace Pizza_Puzzle_Game.GameObjects
{
    class MaketableObject : GameObject
    {
        #region Fields
        private static IngredientObject[] m_IngredientColumn0 = new IngredientObject[17], m_IngredientColumn1 = new IngredientObject[17], m_IngredientColumn2 = new IngredientObject[17], m_IngredientColumn3 = new IngredientObject[17];
        private IngredientObject[][] m_Columns = new IngredientObject[][] { m_IngredientColumn0, m_IngredientColumn1, m_IngredientColumn2, m_IngredientColumn3 };

        private Texture2D m_IngredientSpriteSheet;

        private Vector2[] m_SpawnLocations = new Vector2[4]; // These are the spots that the ingredients will drop from

        private Timer timer;
        private float m_Interval = 750.0f;
        private float m_SpeedDivisor = 16.0f; // This divisor is used against the interval field when the player is holding down
        private bool m_IsFalling = true; // Tells the object that the ingredients are falling

        IngredientObject m_FallingIngredient1, m_FallingIngredient2;
        #endregion

        #region Constructors / Destructors
        public MaketableObject(Vector2 position, Texture2D sprite, Color shade, ContentManager content)
        {
            Position = position;
            Sprite = sprite;
            Shade = shade;
            Active = true;
            Rendering = true;

            Game1.m_Renderables.Add(this);
            Game1.m_Updatables.Add(this);

            // Spawn a new player offset to the position of the maketable
            new PlayerObject(Position + Program.ToPixelPos(7.0f, 19.0f), content.Load<Texture2D>("arrows"), Color.White, PlayerObject.PlayerNumber.P1);

            SetSpawnLocations();

            m_IngredientSpriteSheet = content.Load<Texture2D>("toppings");
            SpawnIngredients();

            timer = new Timer();
            timer.Interval = m_Interval;
            timer.Elapsed += OnDrop;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        ~MaketableObject()
        {
            Game1.m_Renderables.Remove(this);
            Game1.m_Updatables.Remove(this);
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            if (m_IsFalling)
            {
                // If the player presses/releases Down, then adjust the falling speed
                if (Keyboard.HasBeenPressed(Keys.Down) || GamePad.HasBeenPressed(Buttons.DPadDown))
                {
                    timer.Interval = m_Interval / m_SpeedDivisor;
                }
                else if (Keyboard.HasBeenReleased(Keys.Down) || GamePad.HasBeenReleased(Buttons.DPadDown))
                {
                    timer.Interval = m_Interval;
                }
            }

            CheckBoard();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (!Rendering)
                return;
            
            spriteBatch.Draw(Sprite, Position, Shade);
        }

        private void SetSpawnLocations()
        {
            // Set spawn locations based on local position
            Vector2 origin = Position;

            m_SpawnLocations[0] = origin + Program.ToPixelPos(1.5f, 1.0f);
            m_SpawnLocations[1] = origin + Program.ToPixelPos(4.5f, 1.0f);
            m_SpawnLocations[2] = origin + Program.ToPixelPos(7.5f, 1.0f);
            m_SpawnLocations[3] = origin + Program.ToPixelPos(10.5f, 1.0f);
        }

        private void SpawnIngredients()
        {
            Random r = new Random();

            for (int i = 0; i < 2; i++)
            {
                int index = r.Next(0, 4);
                if (m_Columns[index][0] == null)
                {
                    // Oooh, an empty spot; I'll spawn now
                    IngredientObject newIngredient = new IngredientObject(m_SpawnLocations[index], m_IngredientSpriteSheet, Color.White, r.Next(0, 7), (uint)index, 0);
                    Game1.m_Updatables.Add(newIngredient);
                    Game1.m_Renderables.Add(newIngredient);
                    
                    m_Columns[index][0] = newIngredient;

                    if (i == 0)
                        m_FallingIngredient1 = newIngredient;
                    else if (i == 1)
                        m_FallingIngredient2 = newIngredient;
                }
                else
                {
                    // "This spot was full, I'll try to spawn again."
                    --i;
                }
            }
        }

        private void CheckBoard()
        {

        }
        #endregion

        #region Events
        private void OnDrop(Object o, System.Timers.ElapsedEventArgs e)
        {
            //TODO: Make ingredients fall
            m_FallingIngredient1.Position = new Vector2(m_FallingIngredient1.Position.X, m_FallingIngredient1.Position.Y + Program.PPU);
            m_FallingIngredient2.Position = new Vector2(m_FallingIngredient2.Position.X, m_FallingIngredient2.Position.Y + Program.PPU);
        }
        #endregion
    }
}
