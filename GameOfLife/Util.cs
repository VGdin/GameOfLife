using Microsoft.Xna.Framework;

namespace GameOfLife
{
    static class Util
    {
        public static Vector2 TupleToVector2((int t1, int t2) tuple)
        {
            return new Vector2(tuple.t1 * Config.Instance.CellSize, tuple.t2 * Config.Instance.CellSize);
        }
    }
}
