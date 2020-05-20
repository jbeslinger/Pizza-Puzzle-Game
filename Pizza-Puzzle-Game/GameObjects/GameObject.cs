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

        #region Constructors / Destructors
        public GameObject()
        {
            Game1.m_Updatables.Add(this);
            Game1.m_Renderables.Add(this);
        }
        #endregion

        #region Abstract Methods
        public abstract void Update(GameTime gameTime);
        public abstract void Render(SpriteBatch spriteBatch);
        #endregion

        #region Methods
        public void Destroy()
        {
            Game1.m_Updatables.Remove(this);
            Game1.m_Renderables.Remove(this);
        }
        #endregion
    }
}
