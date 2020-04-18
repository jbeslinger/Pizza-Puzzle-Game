﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pizza_Puzzle_Game.GameObjects
{
    class PanObject : GameObject
    {
        #region Constructors / Destructors
        public PanObject(Vector2 position, Texture2D sprite, Color shade)
        {
            Position = position;
            Sprite = sprite;
            Shade = shade;
            Active = true;
            Rendering = true;

            Game1.m_Renderables.Add(this);
            Game1.m_Updatables.Add(this);
        }

        ~PanObject()
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
        #endregion
    }
}
