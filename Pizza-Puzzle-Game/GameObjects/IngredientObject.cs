using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Timers;

namespace Pizza_Puzzle_Game.GameObjects
{
    class IngredientObject : GameObject
    {
        #region Enums
        // These numbers were chosen for a reason: they correspond with the index # of the sprite in the toppings texture
        // The names of the enums and the numbers are interchangeable
        public enum ToppingType { PIZZA_BASE = 0, CHEESE_TOPPING = 1, PEPPERONI = 2, MUSHROOM = 3, BELL_PEPPER = 4, HAM = 5, PINEAPPLE = 6 };
        public enum State { FALLING, SOLID };
        #endregion

        #region Fields
        private MaketableObject m_ParentTableObject;
        private Rectangle m_Rect; // This is used to get the sprite out of the spritesheet
        private Texture2D m_BracketTex;
        private Texture2D m_DestroyTex;
        private Timer m_DestructionTimer;

        public bool isSwapping;
        public bool isToBeDestroyed;
        public State ingredientState;
        #endregion

        #region Properties
        public uint ColumnNumber { get; set; }
        public uint RowNumber { get; set; }
        public ToppingType Type { get; set; }

        public bool IsFalling { get { return ingredientState == State.FALLING; } }
        public bool IsSolidified { get { return ingredientState == State.SOLID; } }
        #endregion

        #region Constructors / Destructors
        /// <param name="spritesheet">The spritesheet that contains all 7 types of ingredients.</param>
        public IngredientObject(MaketableObject parentTableObject, Vector2 position, Texture2D spritesheet, Texture2D bracketTex, Texture2D destroyTex, Color shade, int type, uint columnNumber, uint rowNumber)
            : base()
        {
            if (type < 0 || type > 6)
            {
                throw new Exception("That topping type is invalid.  Pick one between 0-6.");
            }

            m_ParentTableObject = parentTableObject;

            Position = position;
            Sprite = spritesheet;
            m_BracketTex = bracketTex;
            m_DestroyTex = destroyTex;
            Shade = shade;

            Active = true;
            Rendering = true;

            ingredientState = State.FALLING;

            Type = (ToppingType)type;
            m_Rect = new Rectangle((int)Type * ((int)Program.PPU * 3), 0, (int)Program.PPU * 3, (int)Program.PPU * 2);

            ColumnNumber = columnNumber;
            RowNumber = rowNumber;
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            if (isToBeDestroyed)
                PlayDestroyAnimation();

            if (!isSwapping)
                Position = m_ParentTableObject.Position + Program.ToPixelPos(1.5f + 3.0f * ColumnNumber, 1.0f + RowNumber);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (!Rendering)
                return;

            spriteBatch.Draw(Sprite, Position, m_Rect, Shade);

            if (ingredientState == State.SOLID && !isToBeDestroyed)
                spriteBatch.Draw(m_BracketTex, Position, Shade);
        }

        public void PlayDestroyAnimation()
        {
            double m_AnimationDuration = 250;

            Active = false;
            Sprite = m_DestroyTex;
            m_Rect = new Rectangle(0, 0, (int)Program.PPU * 3, (int)Program.PPU * 2);

            m_DestructionTimer = new Timer(m_AnimationDuration);
            m_DestructionTimer.Elapsed += OnDestroy;
            m_DestructionTimer.Enabled = true;
        }
        #endregion

        #region Events
        private void OnDestroy(Object source, ElapsedEventArgs e)
        {
            Destroy();
        }
        #endregion
    }
}
