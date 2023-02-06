using GameOfLifeLib.Morton;
using GameOfLifeLib.Optimized;

namespace GameOfLifeLib
{
    /// <summary>
    /// Factory to return setup GameOfLife's
    /// </summary>
    public static class GameOfLifeFactory
    {
        /// <summary>
        /// Returns a Game of Life with morton grid with bool cells
        /// </summary>
        /// <param name="width">width of the game</param>
        /// <param name="height">height of the game</param>
        /// <returns>Return the created game</returns>
        public static IGameOfLife CreateGameOfLifeMorton(uint width, uint height)
        {
            CellGridMorton gridOne = new CellGridMorton(width, height);
            CellGridMorton gridTwo = new CellGridMorton(width, height);

            return new GameOfLifeMorton(gridOne, gridTwo);
        }

        /// <summary>
        /// Returns a new Game Of Life with bool cells
        /// </summary>
        /// <param name="width">width of the game</param>
        /// <param name="height">height of the game</param>
        /// <returns>Return the created game</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IGameOfLife CreateGameOfLifeOptimized(uint width, uint height)
        {
            CellGridOptimized gridOne = new CellGridOptimized(width, height);

            return new GameOfLifeOptimized(gridOne);
        }
    }
}
