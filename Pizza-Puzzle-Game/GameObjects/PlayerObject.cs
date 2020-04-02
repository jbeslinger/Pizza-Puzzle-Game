﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pizza_Puzzle_Game.GameObjects
{
    class PlayerObject : GameObject
    {
        #region Enums
        enum PlayerPosition { LEFT = 0, MIDDLE = 1, RIGHT = 2 };
        #endregion

        #region Fields
        private Color m_arrowOneColor = Color.Red, m_arrowTwoColor = Color.Blue;
        
        private PlayerPosition m_playerPosition;
        private Vector2 m_leftArrowPosition, m_rightArrowPosition;
        private bool m_swapToggle;
        #endregion

        #region Constructors
        public PlayerObject(Vector2 position, Texture2D sprite, Color shade)
        {
            Position = position;
            Sprite = sprite;
            Shade = shade;
            Active = true;
            Rendering = true;
            m_playerPosition = PlayerPosition.MIDDLE;
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            // Put Logic Here
            if (Keyboard.HasBeenPressed(Keys.Z))
                m_swapToggle = !m_swapToggle;
            else if (Keyboard.HasBeenPressed(Keys.Left) && m_playerPosition != (PlayerPosition)0)
                m_playerPosition--;
            else if (Keyboard.HasBeenPressed(Keys.Right) && m_playerPosition != (PlayerPosition)2)
                m_playerPosition++;


            switch (m_playerPosition)
            {
                case PlayerPosition.LEFT:
                    Position = new Vector2(Program.PPU * 6, Program.PPU * 21);
                    break;
                case PlayerPosition.MIDDLE:
                    Position = new Vector2(Program.PPU * 9, Program.PPU * 21);
                    break;
                case PlayerPosition.RIGHT:
                    Position = new Vector2(Program.PPU * 12, Program.PPU * 21);
                    break;
            }


            m_leftArrowPosition = new Vector2(Position.X - (Program.PPU * 2), Position.Y);
            m_rightArrowPosition = new Vector2(Position.X + (Program.PPU * 1), Position.Y);

            if (m_swapToggle)
            {
                m_arrowOneColor = Color.Blue;
                m_arrowTwoColor = Color.Red;
            }
            else
            {
                m_arrowOneColor = Color.Red;
                m_arrowTwoColor = Color.Blue;
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (!Rendering)
                return;

            // Put Draw Code Here
            spriteBatch.Draw(Sprite, m_leftArrowPosition, m_arrowOneColor);
            spriteBatch.Draw(Sprite, m_rightArrowPosition, m_arrowTwoColor);
        }
        #endregion
    }
}