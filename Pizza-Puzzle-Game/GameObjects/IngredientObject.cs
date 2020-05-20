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
        public enum State { FALLING, SOLID };
        #endregion

        #region Fields
        private ToppingType m_Type;
        private Rectangle m_Rect; // This is used to get the sprite out of the spritesheet
        private Texture2D m_BracketTex;

        public bool isSwapping;
        public State ingredientState;
        #endregion

        #region Properties
        public uint ColumnNumber { get; set; }
        public uint RowNumber { get; set; }
        
        public bool IsFalling { get { return ingredientState == State.FALLING; } }
        public bool IsSolidified { get { return ingredientState == State.SOLID; } }
        #endregion

        #region Constructors / Destructors
        /// <param name="spritesheet">The spritesheet that contains all 7 types of ingredients.</param>
        public IngredientObject(Vector2 position, Texture2D spritesheet, Texture2D bracketTex, Color shade, int type, uint columnNumber, uint rowNumber)
        {
            if (type < 0 || type > 6)
            {
                throw new Exception("That topping type is invalid.  Pick one between 0-6.");
            }

            Position = position;
            Sprite = spritesheet;
            m_BracketTex = bracketTex;
            Shade = shade;

            Active = true;
            Rendering = true;

            ingredientState = State.FALLING;

            m_Type = (ToppingType)type;
            m_Rect = new Rectangle((int)m_Type * ((int)Program.PPU * 3), 0, (int)Program.PPU * 3, (int)Program.PPU * 2);

            ColumnNumber = columnNumber;
            RowNumber = rowNumber;
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

            if (ingredientState == State.SOLID)
                spriteBatch.Draw(m_BracketTex, Position, Shade);
        }
        #endregion
    }
}
