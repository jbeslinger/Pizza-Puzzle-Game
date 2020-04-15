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
        
        private Timer timer;
        private float m_Interval = 750.0f; // The amount of time (ms) it takes for the ingredient to "update" it's falling
        private float m_SpeedDivisor = 16.0f; // The divisor for determining the speed at which ingredients fall when player is holding Down

        private bool m_IsFalling;
        private bool m_Speedup = false;
        #endregion

        #region Constructors
        public IngredientObject(Vector2 position, Texture2D sprite, Color shade)
        {
            Position = position;
            Sprite = sprite;
            Shade = shade;
            Active = true;
            Rendering = true;

            m_Type = (ToppingType)new Random().Next(0, 7);

            timer = new Timer();
            timer.Interval = m_Interval;
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

            if (Keyboard.HasBeenPressed(Keys.Down))
            {
                Console.WriteLine("Pressed, boi.");
            }
            if (Keyboard.HasBeenReleased(Keys.Down))
            {
                Console.WriteLine("Released, boi.");
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (!Rendering)
                return;

            spriteBatch.Draw(Sprite, Position, Shade);
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
