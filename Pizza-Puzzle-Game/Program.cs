using Microsoft.Xna.Framework;
using System;

namespace Pizza_Puzzle_Game
{
    public static class Program
    {
        #region Constants
        public const float PPU = 8.0f; // This is the constant "Pixels Per Unit".  A "Unit" in this context can be defined as an 8x8 pixel square.  Use this when determining object positions.
        #endregion

        /// <summary>
        /// Converts a desired Vector2 "unit" position to Vector2 pixel position; saves me from writing PPU * X over and over.
        /// </summary>
        /// <param name="unitXPos">The desired 'x' position in "units".</param>
        /// <param name="unitYPos">The desired 'y' position in "units".</param>
        public static Vector2 ToPixelPos(float unitXPos, float unitYPos)
        {
            return new Vector2(PPU * unitXPos, PPU * unitYPos);
        }

        #region Entry Point
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
        #endregion
    }
}
