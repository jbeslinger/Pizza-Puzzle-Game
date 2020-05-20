using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Timers;

namespace Pizza_Puzzle_Game.GameObjects
{
    class MaketableObject : GameObject
    {
        #region Fields
        private IngredientObject[,] m_IngredientsOnTheBoard = new IngredientObject[4,17];

        private Texture2D m_IngredientSpriteSheet;
        private Texture2D m_BracketTex;

        private Vector2[] m_SpawnLocations = new Vector2[4]; // These are the spots that the ingredients will drop from
        
        private double m_TimerInterval = 0.75; // The interval that IngredientObjects "drop" in seconds
        private double m_ElapsedTime = 0.0;
        private float m_SpeedDivisor = 16.0f; // This divisor is used against the interval field when the player is holding down

        private bool m_SpeedUp = false;
        private bool m_PiecesAreFalling = true; // Tells the object that the ingredients are falling
        private bool m_PlayingSwapAnim = false;

        private IngredientObject m_FallingIngredient1, m_FallingIngredient2;
        private IngredientObject m_NextIngredient1, m_NextIngredient2;

        private PlayerObject m_Player;
        #endregion

        #region Constructors / Destructors
        public MaketableObject(Vector2 position, Texture2D sprite, Color shade, ContentManager content, PlayerObject.PlayerNumber playerNumber)
            : base()
        {
            Position = position;
            Sprite = sprite;
            Shade = shade;
            Active = true;
            Rendering = true;

            // Spawn a new player offset to the position of the maketable
            m_Player = new PlayerObject(Position + Program.ToPixelPos(7.0f, 19.0f), content.Load<Texture2D>("arrows"), Color.White, content, playerNumber);

            m_BracketTex = content.Load<Texture2D>("brackets");

            SetSpawnLocations();

            m_IngredientSpriteSheet = content.Load<Texture2D>("toppings");

            SpawnIngredients();
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            if (m_PlayingSwapAnim)
            {
                PlaySwapAnimation();
                return;
            }

            if (m_PiecesAreFalling)
            {
                // If the player presses/releases Down, then adjust the falling speed
                if (Keyboard.HasBeenPressed(Keys.Down) || GamePad.HasBeenPressed(Buttons.DPadDown))
                {
                    m_SpeedUp = true;
                }
                else if (Keyboard.HasBeenReleased(Keys.Down) || GamePad.HasBeenReleased(Buttons.DPadDown))
                {
                    m_SpeedUp = false;
                }

                m_ElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (!m_SpeedUp)
                {
                    if (m_ElapsedTime >= m_TimerInterval)
                    {
                        OnDrop();
                        m_ElapsedTime = 0.0;
                    }
                }
                else
                {
                    if (m_ElapsedTime >= m_TimerInterval / m_SpeedDivisor)
                    {
                        OnDrop();
                        m_ElapsedTime = 0.0;
                    }
                }
            }

            if (Keyboard.HasBeenPressed(Keys.Z) || GamePad.HasBeenPressed(Buttons.X))
            {
                SwapColumns();
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (!Rendering)
                return;
            
            spriteBatch.Draw(Sprite, Position, Shade);
        }

        private void SetSpawnLocations()
        {
            // Set spawn locations based on local position
            Vector2 origin = Position;

            m_SpawnLocations[0] = origin + Program.ToPixelPos(1.5f, 1.0f);
            m_SpawnLocations[1] = origin + Program.ToPixelPos(4.5f, 1.0f);
            m_SpawnLocations[2] = origin + Program.ToPixelPos(7.5f, 1.0f);
            m_SpawnLocations[3] = origin + Program.ToPixelPos(10.5f, 1.0f);
        }

        private void SpawnIngredients()
        {
            Random r = new Random();
            bool finished = false;

            while (!finished)
            {
                if (m_FallingIngredient1 == null & m_NextIngredient1 == null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int index = r.Next(0, 4);
                        if (m_IngredientsOnTheBoard[index, 0] == null)
                        {
                            // Oooh, an empty spot; I'll spawn now
                            IngredientObject newIngredient = new IngredientObject(this, m_SpawnLocations[index], m_IngredientSpriteSheet, m_BracketTex, Color.White, r.Next(0, 7), (uint)index, 0);

                            m_IngredientsOnTheBoard[index, 0] = newIngredient;

                            if (i == 0)
                            {
                                m_FallingIngredient1 = newIngredient;
                            }
                            else if (i == 1)
                            {
                                m_FallingIngredient2 = newIngredient;
                                finished = true;
                            }
                        }
                        else
                        {
                            // "This spot was full, I'll try to spawn again."
                            --i;
                        }
                    }
                }
                else if (m_FallingIngredient1 != null && m_NextIngredient1 == null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int index = r.Next(0, 4);
                        if (m_IngredientsOnTheBoard[index, 0] == null)
                        {
                            // Oooh, an empty spot; I'll spawn now
                            IngredientObject newIngredient = new IngredientObject(this, m_SpawnLocations[index], m_IngredientSpriteSheet, m_BracketTex, Color.White, r.Next(0, 7), (uint)index, 0);

                            m_IngredientsOnTheBoard[index, 0] = newIngredient;

                            if (i == 0)
                            {
                                m_NextIngredient1 = newIngredient;
                            }
                            else if (i == 1)
                            {
                                m_NextIngredient2 = newIngredient;
                                finished = true;
                            }
                        }
                        else
                        {
                            // "This spot was full, I'll try to spawn again."
                            --i;
                        }
                    }
                }
            }
        }

        private void SwapColumns()
        {
            m_PlayingSwapAnim = true;

            int leftColumnIndex = (int)m_Player.PlayerPos,
                rightColumnIndex = leftColumnIndex + 1;
            IngredientObject[] leftColumnCopy = new IngredientObject[m_IngredientsOnTheBoard.GetLength(1)],
                rightColumnCopy = new IngredientObject[m_IngredientsOnTheBoard.GetLength(1)];

            // Copy the left and right columns of Ingredient objects
            for (int rowIndex = 0; rowIndex < m_IngredientsOnTheBoard.GetLength(1); rowIndex++)
            {
                leftColumnCopy[rowIndex] = m_IngredientsOnTheBoard[leftColumnIndex, rowIndex];
                rightColumnCopy[rowIndex] = m_IngredientsOnTheBoard[rightColumnIndex, rowIndex];
            }

            // Swap them based on several conditions
            for (int rowIndex = 1; rowIndex < m_IngredientsOnTheBoard.GetLength(1); rowIndex++)
            {
                IngredientObject leftIngredient = leftColumnCopy[rowIndex], rightIngredient = rightColumnCopy[rowIndex];

                if (leftIngredient == null && rightIngredient == null)
                {
                    continue;
                }

                if (leftIngredient != null && rightIngredient == null)
                {
                    if (leftIngredient.IsSolidified)
                    {
                        leftIngredient.ColumnNumber = (uint)rightColumnIndex;
                        leftIngredient.isSwapping = true;
                        m_IngredientsOnTheBoard[rightColumnIndex, rowIndex] = leftIngredient;
                        m_IngredientsOnTheBoard[leftColumnIndex, rowIndex] = null;
                    }
                    else if (leftIngredient.IsFalling)
                    {
                        if (m_IngredientsOnTheBoard[rightColumnIndex, rowIndex + 1] != null)
                        {
                            leftIngredient.ColumnNumber = (uint)rightColumnIndex;
                            leftIngredient.isSwapping = true;
                            m_IngredientsOnTheBoard[rightColumnIndex, rowIndex] = leftIngredient;
                            m_IngredientsOnTheBoard[leftColumnIndex, rowIndex] = null;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else if (leftIngredient == null && rightIngredient != null)
                {
                    if (rightIngredient.IsSolidified)
                    {
                        rightIngredient.ColumnNumber = (uint)leftColumnIndex;
                        rightIngredient.isSwapping = true;
                        m_IngredientsOnTheBoard[leftColumnIndex, rowIndex] = rightIngredient;
                        m_IngredientsOnTheBoard[rightColumnIndex, rowIndex] = null;
                    }
                    else if (rightIngredient.IsFalling)
                    {
                        if (m_IngredientsOnTheBoard[leftColumnIndex, rowIndex + 1] != null)
                        {
                            rightIngredient.ColumnNumber = (uint)leftColumnIndex;
                            rightIngredient.isSwapping = true;
                            m_IngredientsOnTheBoard[leftColumnIndex, rowIndex] = rightIngredient;
                            m_IngredientsOnTheBoard[rightColumnIndex, rowIndex] = null;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else if (leftIngredient != null && rightIngredient != null)
                {
                    if ((leftIngredient.IsSolidified && rightIngredient.IsSolidified) || (leftIngredient.IsSolidified && rightIngredient.IsFalling || leftIngredient.IsFalling && rightIngredient.IsSolidified))
                    {
                        leftIngredient.ColumnNumber = (uint)rightColumnIndex;
                        rightIngredient.ColumnNumber = (uint)leftColumnIndex;
                        leftIngredient.isSwapping = true;
                        rightIngredient.isSwapping = true;
                        m_IngredientsOnTheBoard[leftColumnIndex, rowIndex] = rightIngredient;
                        m_IngredientsOnTheBoard[rightColumnIndex, rowIndex] = leftIngredient;
                    }
                }
            }

            PlaySwapAnimation();
        }

        private void PlaySwapAnimation()
        {
            float animationSpeed = 0.4f; // A normalized value to represent how fast the animation plays
            int leftColumnIndex = (int)m_Player.PlayerPos;
            int rightColumnIndex = leftColumnIndex + 1;
            bool animationIsDone = true;

            for (int rowIndex = 1; rowIndex < m_IngredientsOnTheBoard.GetLength(1); rowIndex++)
            {
                IngredientObject leftIngredient = m_IngredientsOnTheBoard[leftColumnIndex, rowIndex], rightIngredient = m_IngredientsOnTheBoard[rightColumnIndex, rowIndex];

                if (leftIngredient != null && leftIngredient.isSwapping)
                {
                    Vector2 dest = Position + Program.ToPixelPos(1.5f + 3.0f * leftIngredient.ColumnNumber, 1.0f + leftIngredient.RowNumber);

                    if (leftIngredient.Position != dest)
                    {
                        leftIngredient.Position = Math.Lerp(leftIngredient.Position, dest, animationSpeed);
                        animationIsDone = false;
                    }
                    else
                    {
                        leftIngredient.isSwapping = false;
                    }
                }

                if (rightIngredient != null && rightIngredient.isSwapping)
                {
                    Vector2 dest = Position + Program.ToPixelPos(1.5f + 3.0f * rightIngredient.ColumnNumber, 1.0f + rightIngredient.RowNumber);

                    if (rightIngredient.Position != dest)
                    {
                        rightIngredient.Position = Math.Lerp(rightIngredient.Position, dest, animationSpeed);
                        animationIsDone = false;
                    }
                    else
                    {
                        rightIngredient.isSwapping = false;
                    }
                }
            }

            if (animationIsDone)
                m_PlayingSwapAnim = false;
        }

        private void CheckBoardForMatches()
        {

        }
        
        private void OnDrop()
        {
            Vector2 origin = Position;
            bool nextSpotIsEmpty = false;

            if (m_FallingIngredient1.IsSolidified && m_FallingIngredient2.IsSolidified)
            {
                m_FallingIngredient1 = m_NextIngredient1;
                m_NextIngredient1 = null;
                m_FallingIngredient2 = m_NextIngredient2;
                m_NextIngredient2 = null;
            }

            if (m_FallingIngredient1.RowNumber == 1)
            {
                SpawnIngredients();
            }

            // Drop the first ingredient & update its array position
            if (m_FallingIngredient1.RowNumber < m_IngredientsOnTheBoard.GetLength(1) - 1)
            {
                nextSpotIsEmpty = false;

                // Look ahead and see if there are any empty spots
                if (m_FallingIngredient1.RowNumber + 2 <= m_IngredientsOnTheBoard.GetLength(1) - 1)
                {
                    if (m_IngredientsOnTheBoard[m_FallingIngredient1.ColumnNumber, m_FallingIngredient1.RowNumber + 2] == null)
                        nextSpotIsEmpty = true;
                    else if (m_IngredientsOnTheBoard[m_FallingIngredient1.ColumnNumber, m_FallingIngredient1.RowNumber + 2] != null)
                        nextSpotIsEmpty = false;
                }
                else
                {
                    if (m_IngredientsOnTheBoard[m_FallingIngredient1.ColumnNumber, m_FallingIngredient1.RowNumber + 1] == null)
                        nextSpotIsEmpty = true;
                    else if (m_IngredientsOnTheBoard[m_FallingIngredient1.ColumnNumber, m_FallingIngredient1.RowNumber + 1] != null)
                        nextSpotIsEmpty = false;
                }
            }

            if (nextSpotIsEmpty)
            {
                m_IngredientsOnTheBoard[m_FallingIngredient1.ColumnNumber, m_FallingIngredient1.RowNumber] = null;
                m_FallingIngredient1.RowNumber++;
                m_IngredientsOnTheBoard[m_FallingIngredient1.ColumnNumber, m_FallingIngredient1.RowNumber] = m_FallingIngredient1;
            }
            else if (!nextSpotIsEmpty)
            {
                m_FallingIngredient1.ingredientState = IngredientObject.State.SOLID;
            }

            // Drop the second ingredient & update its array position
            if (m_FallingIngredient2.RowNumber < m_IngredientsOnTheBoard.GetLength(1) - 1)
            {
                nextSpotIsEmpty = false;

                // Look ahead and see if there are any empty spots
                if (m_FallingIngredient2.RowNumber + 2 <= m_IngredientsOnTheBoard.GetLength(1) - 1)
                {
                    if (m_IngredientsOnTheBoard[m_FallingIngredient2.ColumnNumber, m_FallingIngredient2.RowNumber + 2] == null)
                        nextSpotIsEmpty = true;
                    else if (m_IngredientsOnTheBoard[m_FallingIngredient2.ColumnNumber, m_FallingIngredient2.RowNumber + 2] != null)
                        nextSpotIsEmpty = false;
                }
                else
                {
                    if (m_IngredientsOnTheBoard[m_FallingIngredient2.ColumnNumber, m_FallingIngredient2.RowNumber + 1] == null)
                        nextSpotIsEmpty = true;
                    else if (m_IngredientsOnTheBoard[m_FallingIngredient2.ColumnNumber, m_FallingIngredient2.RowNumber + 1] != null)
                        nextSpotIsEmpty = false;
                }
            }

            if (nextSpotIsEmpty)
            {
                m_IngredientsOnTheBoard[m_FallingIngredient2.ColumnNumber, m_FallingIngredient2.RowNumber] = null;
                m_FallingIngredient2.RowNumber++;
                m_IngredientsOnTheBoard[m_FallingIngredient2.ColumnNumber, m_FallingIngredient2.RowNumber] = m_FallingIngredient2;
            }
            else if (!nextSpotIsEmpty)
            {
                m_FallingIngredient2.ingredientState = IngredientObject.State.SOLID;
            }

            CheckBoardForMatches();
        }
        #endregion
    }
}
