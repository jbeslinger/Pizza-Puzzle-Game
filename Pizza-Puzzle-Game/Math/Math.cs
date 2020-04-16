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
            float returnX = Lerp(startValue.X, endValue.X, by);
            float returnY = Lerp(startValue.Y, endValue.Y, by);

            // This ugly, ugly code simply checks for asymptotes and fixes them by returning the value we were trying to reach
            // I'm sorry.
            if ((System.Math.Abs((int)returnX) == System.Math.Abs(endValue.X) - 1 || (int)returnX == endValue.X) && (System.Math.Abs((int)returnY) == System.Math.Abs(endValue.Y) - 1 || (int)returnY == endValue.Y))
            {
                return endValue;
            }

            return new Vector2(returnX, returnY);
        }
    }
}
