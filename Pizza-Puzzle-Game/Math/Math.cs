using Microsoft.Xna.Framework;

namespace Pizza_Puzzle_Game
{
    class Math
    {
        /// <summary>
        /// Linearly interpolates between two float values using a "by" step parameter.  "by" should be normalized.
        /// </summary>
        public static float Lerp(float startValue, float endValue, float by)
        {
            return startValue + (endValue - startValue) * by;
        }

        /// <summary>
        /// Linearly interpolates between two Vector2 position using a "by" step parameter.  "by" should be normalized.
        /// </summary>
        public static Vector2 Lerp(Vector2 startValue, Vector2 endValue, float by)
        {
            float returnX = (int)Lerp(startValue.X, endValue.X, by);
            float returnY = (int)Lerp(startValue.Y, endValue.Y, by);
            return new Vector2(returnX, returnY);
        }
    }
}
