using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pizza_Puzzle_Game.GameObjects
{
    class IngredientObject : GameObject
    {
        #region Enums
        // These numbers were chosen for a reason: they correspond with the index # of the sprite in the toppings texture
        // The names of the enums and the numbers are interchangeable
        enum ToppingType { PIZZA_BASE = 0, CHEESE_TOPPING = 1, PEPPERONI = 2, MUSHROOM = 3, BELL_PEPPER = 4, HAM = 5, PINEAPPLE = 6 };
        #endregion

        #region Fields
        private ToppingType m_Type;
        private Rectangle m_Rect; // This is the sprite in the spritesheet; it's passed to the Draw() function
        
        private Timer timer;
        private float m_SpeedDivisor = 16.0f; // The divisor for determining the speed at which ingredients fall when player is holding Down

        private bool m_IsFalling = true;
        #endregion

        #region Properties
        public float Interval { get; set; } = 750.0f;
        #endregion

        #region Constructors
        /// <param name="spritesheet">The spritesheet that contains all 7 types of ingredients.</param>
        public IngredientObject(Vector2 position, Texture2D spritesheet, Color shade, int type)
        {
            if (type < 0 || type > 6)
            {
                throw new Exception("That topping type is invalid.  Pick one between 0-6.");
            }

            Position = position;
            Sprite = spritesheet;
            Shade = shade;
            Active = true;
            Rendering = true;

            m_Type = (ToppingType)type;
            m_Rect = new Rectangle((int)m_Type * ((int)Program.PPU * 3), 0, (int)Program.PPU * 3, (int)Program.PPU * 2);

            timer = new Timer();
            timer.Interval = Interval;
            timer.Elapsed += OnFall;
            timer.AutoReset = true;
            timer.Enabled = true;
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
                if (Keyboard.HasBeenPressed(Keys.Down))
                {
                    timer.Interval = Interval / m_SpeedDivisor;
                }
                else if (Keyboard.HasBeenReleased(Keys.Down))
                {
                    timer.Interval = Interval;
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (!Rendering)
                return;

            spriteBatch.Draw(Sprite, Position, m_Rect, Shade);
        }
        #endregion

        #region Events
        private void OnFall(Object o, System.Timers.ElapsedEventArgs e)
        {
            Position = new Vector2(Position.X, Position.Y + Program.PPU);
        }
        #endregion
    }
}
