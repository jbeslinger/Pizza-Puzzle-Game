using Microsoft.Xna.Framework.Input;

namespace Pizza_Puzzle_Game
{
    class GamePad
    {
        static GamePadState currentPadState;
        static GamePadState previousPadState;

        public static GamePadState GetState()
        {
            previousPadState = currentPadState;
            currentPadState = Microsoft.Xna.Framework.Input.GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
            return currentPadState;
        }

        public static bool IsPressed(Buttons button)
        {
            return currentPadState.IsButtonDown(button);
        }

        public static bool HasBeenPressed(Buttons button)
        {
            return currentPadState.IsButtonDown(button) && !previousPadState.IsButtonDown(button);
        }

        public static bool HasBeenReleased(Buttons button)
        {
            return !currentPadState.IsButtonDown(button) && previousPadState.IsButtonDown(button);
        }
    }
}
