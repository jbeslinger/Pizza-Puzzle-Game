using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pizza_Puzzle_Game.GameObjects
{
    class PlayerObject : GameObject
    {
        #region Enums
        public enum PlayerNumber { P1 = 0, P2 = 1, P3 = 2, P4 = 3 };
        public enum PlayerPosition { LEFT = 0, MIDDLE = 1, RIGHT = 2 };
        #endregion

        #region Fields
        private PlayerNumber m_PlayerNumber; // An id number associated with each player

        private Color m_ArrowOneColor = Color.Red, m_ArrowTwoColor = Color.Blue;
        private PlayerPosition m_PlayerPos;
        private Vector2 m_MidPos, m_LeftPos, m_RightPos; // The set positions of the player object itself
        private Vector2 m_ArrowOnePos, m_ArrowTwoPos; // The positions of the two arrows to the left and right of the player object

        private bool m_PlayingSwapAnim; // An animation flag to signal when the swap anim starts and stops
        private bool m_Swapped; // A flag to indicate that the player's arrows are or aren't swapped in position

        private PanObject[] m_Pans = new PanObject[4];
        #endregion

        #region Properties
        public PlayerPosition PlayerPos { get { return m_PlayerPos; } set { m_PlayerPos = value; } }
        #endregion

        #region Constructors / Destructors
        public PlayerObject(Vector2 position, Texture2D sprite, Color shade, ContentManager content, PlayerNumber playerNumber)
            : base()
        {
            m_PlayerNumber = playerNumber;

            Position = position;
            Sprite = sprite;
            Shade = shade;
            Active = true;
            Rendering = true;
            m_PlayerPos = PlayerPosition.MIDDLE;

            m_MidPos = position;
            m_LeftPos = new Vector2(position.X - (Program.PPU * 3), position.Y);
            m_RightPos = new Vector2(position.X + (Program.PPU * 3), position.Y);

            Texture2D pizzaPanTex = content.Load<Texture2D>("pan");
            m_Pans[0] = new PanObject(Position + Program.ToPixelPos(-5.5f, 0.0f), pizzaPanTex, Color.White);
            m_Pans[1] = new PanObject(Position + Program.ToPixelPos(-2.5f, 0.0f), pizzaPanTex, Color.White);
            m_Pans[2] = new PanObject(Position + Program.ToPixelPos( 0.5f, 0.0f), pizzaPanTex, Color.White);
            m_Pans[3] = new PanObject(Position + Program.ToPixelPos( 3.5f, 0.0f), pizzaPanTex, Color.White);
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            // Put Logic Here
            if (m_PlayingSwapAnim)
            {
                PlaySwapAnimation();
                return;
            }

            if (Keyboard.HasBeenPressed(Keys.Z) || GamePad.HasBeenPressed(Buttons.X))
            {
                m_PlayingSwapAnim = true;
                return;
            }
            else if ((Keyboard.HasBeenPressed(Keys.Left) || GamePad.HasBeenPressed(Buttons.DPadLeft)) && m_PlayerPos != (PlayerPosition)0)
            {
                m_PlayerPos--;
            }
            else if ((Keyboard.HasBeenPressed(Keys.Right) || GamePad.HasBeenPressed(Buttons.DPadRight)) && m_PlayerPos != (PlayerPosition)2)
            {
                m_PlayerPos++;
            }

            // This switch uses the m_PlayerPos enum type to swap between 3 set positions the player can be
            switch (m_PlayerPos)
            {
                case PlayerPosition.LEFT:
                    Position = m_LeftPos;
                    break;
                case PlayerPosition.MIDDLE:
                    Position = m_MidPos;
                    break;
                case PlayerPosition.RIGHT:
                    Position = m_RightPos;
                    break;
            }

            // Update the arrows positions
            m_ArrowOnePos = new Vector2(Position.X - (Program.PPU * 2), Position.Y);
            m_ArrowTwoPos = new Vector2(Position.X + (Program.PPU * 1), Position.Y);

            // If the arrows are meant to be swapped in position, then flip the colors
            if (!m_Swapped)
            {
                m_ArrowOneColor = Color.Red;
                m_ArrowTwoColor = Color.Blue;
            }
            else
            {
                m_ArrowOneColor = Color.Blue;
                m_ArrowTwoColor = Color.Red;
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (!Rendering)
                return;

            // Put Draw Code Here
            // Here I'm drawing two colored arrows based on the origin point of this object
            spriteBatch.Draw(Sprite, m_ArrowOnePos, m_ArrowOneColor);
            spriteBatch.Draw(Sprite, m_ArrowTwoPos, m_ArrowTwoColor);
        }

        private void PlaySwapAnimation()
        {
            float animationSpeed = 0.4f; // A normalized value to represent how fast the animation plays

            Vector2 playerLeftPos = new Vector2(Position.X - (Program.PPU * 2), Position.Y);
            Vector2 playerRightPos = new Vector2(Position.X + (Program.PPU * 1), Position.Y);

            if (m_ArrowOnePos != playerRightPos && m_ArrowTwoPos != playerLeftPos)
            {
                m_ArrowOnePos = Math.Lerp(m_ArrowOnePos, playerRightPos, animationSpeed);
                m_ArrowTwoPos = Math.Lerp(m_ArrowTwoPos, playerLeftPos, animationSpeed);

                switch(m_PlayerPos)
                {
                    case PlayerPosition.LEFT:
                        m_Pans[0].Position = new Vector2(m_ArrowOnePos.X - (Program.PPU * 0.5f), m_ArrowOnePos.Y);
                        m_Pans[1].Position = new Vector2(m_ArrowTwoPos.X - (Program.PPU * 0.5f), m_ArrowTwoPos.Y);
                        break;
                    case PlayerPosition.MIDDLE:
                        m_Pans[1].Position = new Vector2(m_ArrowOnePos.X - (Program.PPU * 0.5f), m_ArrowOnePos.Y);
                        m_Pans[2].Position = new Vector2(m_ArrowTwoPos.X - (Program.PPU * 0.5f), m_ArrowTwoPos.Y);
                        break;
                    case PlayerPosition.RIGHT:
                        m_Pans[2].Position = new Vector2(m_ArrowOnePos.X - (Program.PPU * 0.5f), m_ArrowOnePos.Y);
                        m_Pans[3].Position = new Vector2(m_ArrowTwoPos.X - (Program.PPU * 0.5f), m_ArrowTwoPos.Y);
                        break;
                }
            }
            else
            {
                m_PlayingSwapAnim = false;
                m_Swapped = !m_Swapped;
                m_Pans[0].Position = m_MidPos + Program.ToPixelPos(-5.5f, 0.0f);
                m_Pans[1].Position = m_MidPos + Program.ToPixelPos(-2.5f, 0.0f);
                m_Pans[2].Position = m_MidPos + Program.ToPixelPos( 0.5f, 0.0f);
                m_Pans[3].Position = m_MidPos + Program.ToPixelPos( 3.5f, 0.0f);
            }
        }
        #endregion
    }
}
