using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
        #endregion

        #region Constructors / Destructors
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
        }

        ~IngredientObject()
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
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (!Rendering)
                return;

            spriteBatch.Draw(Sprite, Position, m_Rect, Shade);
        }
        #endregion
    }
}
