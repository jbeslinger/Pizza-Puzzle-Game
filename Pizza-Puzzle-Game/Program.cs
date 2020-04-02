using System;

namespace Pizza_Puzzle_Game
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        #region Constants
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
