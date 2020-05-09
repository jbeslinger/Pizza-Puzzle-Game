﻿using Microsoft.Xna.Framework;
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

        private Timer timer;
        private float m_Interval = 750.0f;
        private float m_SpeedDivisor = 16.0f; // This divisor is used against the interval field when the player is holding down
        private bool m_PiecesAreFalling = true; // Tells the object that the ingredients are falling
        private bool m_PlayingSwapAnim = false;

        private IngredientObject m_FallingIngredient1, m_FallingIngredient2;
        private IngredientObject m_NextIngredient1, m_NextIngredient2;

        private PlayerObject m_Player;
        #endregion

        #region Constructors / Destructors
        public MaketableObject(Vector2 position, Texture2D sprite, Color shade, ContentManager content, PlayerObject.PlayerNumber playerNumber)
        {
            Position = position;
            Sprite = sprite;
            Shade = shade;
            Active = true;
            Rendering = true;

            Game1.m_Renderables.Add(this);
            Game1.m_Updatables.Add(this);

            // Spawn a new player offset to the position of the maketable
            m_Player = new PlayerObject(Position + Program.ToPixelPos(7.0f, 19.0f), content.Load<Texture2D>("arrows"), Color.White, content, playerNumber);

            m_BracketTex = content.Load<Texture2D>("brackets");

            SetSpawnLocations();

            m_IngredientSpriteSheet = content.Load<Texture2D>("toppings");

            SpawnIngredients();

            timer = new Timer();
            timer.Interval = m_Interval;
            timer.Elapsed += OnDrop;
            timer.AutoReset = true;
            timer.Enabled = true;
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
                    timer.Interval = m_Interval / m_SpeedDivisor;
                }
                else if (Keyboard.HasBeenReleased(Keys.Down) || GamePad.HasBeenReleased(Buttons.DPadDown))
                {
                    timer.Interval = m_Interval;
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
                            IngredientObject newIngredient = new IngredientObject(m_SpawnLocations[index], m_IngredientSpriteSheet, m_BracketTex, Color.White, r.Next(0, 7), (uint)index, 0);
                            lock (Game1.m_Updatables) // I have to lock these lists during this while loop to prevent IndexOutOfRangeException?
                                Game1.m_Updatables.Add(newIngredient);
                            lock (Game1.m_Renderables)
                                Game1.m_Renderables.Add(newIngredient);

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
                            IngredientObject newIngredient = new IngredientObject(m_SpawnLocations[index], m_IngredientSpriteSheet, m_BracketTex, Color.White, r.Next(0, 7), (uint)index, 0);
                            lock (Game1.m_Updatables)
                                Game1.m_Updatables.Add(newIngredient);
                            lock (Game1.m_Renderables)
                                Game1.m_Renderables.Add(newIngredient);

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
            //m_PlayingSwapAnim = true;

            IngredientObject[] tempArray;

            switch(m_Player.PlayerPos)
            {
                case PlayerObject.PlayerPosition.LEFT:
                    break;
                case PlayerObject.PlayerPosition.MIDDLE:
                    break;
                case PlayerObject.PlayerPosition.RIGHT:
                    break;
            }

            PlaySwapAnimation();
        }

        private void PlaySwapAnimation()
        {
            float animationSpeed = 0.4f; // A normalized value to represent how fast the animation plays


        }

        private void CheckBoardForMatches()
        {

        }
        #endregion

        #region Events
        private void OnDrop(Object o, ElapsedEventArgs e)
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

                m_FallingIngredient1.Position = origin + Program.ToPixelPos
                    (1.5f + 3.0f * m_FallingIngredient1.ColumnNumber,
                        1.0f + m_FallingIngredient1.RowNumber);
            }
            else if (!nextSpotIsEmpty)
            {
                // TODO: Add code that stops this piece from falling
                m_FallingIngredient1.IsSolidified = true;
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

                m_FallingIngredient2.Position = origin + Program.ToPixelPos
                    (1.5f + 3.0f * m_FallingIngredient2.ColumnNumber,
                        1.0f + m_FallingIngredient2.RowNumber);
            }
            else if (!nextSpotIsEmpty)
            {
                // TODO: Add code that stops this piece from falling
                m_FallingIngredient2.IsSolidified = true;
            }

            CheckBoardForMatches();
        }
        #endregion
    }
}
