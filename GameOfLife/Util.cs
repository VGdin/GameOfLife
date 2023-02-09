using Microsoft.Xna.Framework;

namespace GameOfLife
{
    /// <summary>
    /// Util class to hold static help methods
    /// </summary>
    internal static class Util
    {

        /// <summary>
        /// Convert from game coordinates to vector
        /// </summary>
        /// <param name="tuple">Coorinates in x and y</param>
        /// <returns></returns>
        public static Vector2 TupleToVector2((int x, int y) tuple)
        {
            return new Vector2(tuple.x * Config.Instance.CellSize, tuple.x * Config.Instance.CellSize);
        }
    }
}
