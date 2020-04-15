using System;

namespace Pizza_Puzzle_Game
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        #region Constants
        // This is the constant "Pixels Per Unit".  A "Unit" can be defined as an 8x8 pixel "square".  Use this when determining object positions.
        public const float PPU = 8.0f;
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
