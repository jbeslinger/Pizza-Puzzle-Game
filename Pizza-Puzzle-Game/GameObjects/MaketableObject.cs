using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pizza_Puzzle_Game.GameObjects
{
    class MaketableObject : GameObject
    {
        #region Fields
        private static IngredientObject[] m_IngredientColumn0 = new IngredientObject[17], m_IngredientColumn1 = new IngredientObject[17], m_IngredientColumn2 = new IngredientObject[17], m_IngredientColumn3 = new IngredientObject[17];
        private IngredientObject[][] m_Columns = new IngredientObject[][] { m_IngredientColumn0, m_IngredientColumn1, m_IngredientColumn2, m_IngredientColumn3 };

        private Texture2D m_IngredientSpriteSheet;

        private Vector2[] m_SpawnLocations =
            { Program.ToPixelPos(9.5f, 4.0f),
              Program.ToPixelPos(12.5f, 4.0f),
              Program.ToPixelPos(15.5f, 4.0f),
              Program.ToPixelPos(18.5f, 4.0f) }; // These are the spots that the ingredients will drop from
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

            m_IngredientSpriteSheet = content.Load<Texture2D>("toppings");
            SpawnIngredients();
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
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (!Rendering)
                return;
            
            spriteBatch.Draw(Sprite, Position, Shade);
        }

        public void SpawnIngredients()
        {
            Random r = new Random();

            for (int i = 0; i < 2; i++)
            {
                int index = r.Next(0, 4);
                if (m_Columns[index][0] == null)
                {
                    IngredientObject newIngredient = new IngredientObject(m_SpawnLocations[index], m_IngredientSpriteSheet, Color.White, r.Next(0, 7));
                    Game1.m_Updatables.Add(newIngredient);
                    Game1.m_Renderables.Add(newIngredient);

                    m_Columns[index][0] = newIngredient;
                }
                else
                {
                    // "This spot was full, I'll try to spawn again."
                    --i;
                }
            }
        }
        #endregion
    }
}
