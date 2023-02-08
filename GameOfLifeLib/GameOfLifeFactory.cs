using GameOfLifeLib.Optimized;

namespace GameOfLifeLib
{
    /// <summary>
    /// Factory to return setup GameOfLife's
    /// </summary>
    public static class GameOfLifeFactory
    {
        /// <summary>
        /// Returns a new Game Of Life with optimized bit twidling
        /// </summary>
        /// <param name="width">width of the game</param>
        /// <param name="height">height of the game</param>
        /// <returns>Return the created game</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IGameOfLife CreateGameOfLifeOptimized(uint width, uint height)
        {
            CellGridOptimizedAbstract grid;

            if (width * height > 65536)
            {
                grid = new CellGridOptimizedChangeSet(width, height);
            }
            else
            {
                grid = new CellGridOptimized(width, height);
            }

            return new GameOfLifeOptimized(grid);
        }
    }
}
