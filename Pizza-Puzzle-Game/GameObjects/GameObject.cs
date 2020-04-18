using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pizza_Puzzle_Game.GameObjects
{
    /// <summary>
    /// This is a parent class for interactible objects in the game.
    /// </summary>
    public abstract class GameObject
    {
        #region Properties
        public Vector2 Position { get; set; }
        public Texture2D Sprite { get; set; }
        public Color Shade { get; set; }
        public bool Active { get; set; }
        public bool Rendering { get; set; }
        #endregion

        #region Abstract Methods
        public abstract void Update(GameTime gameTime);
        public abstract void Render(SpriteBatch spriteBatch);
        #endregion
    }
}
