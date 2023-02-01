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
        public static IGameOfLife<bool> CreateGameOfLife(uint width, uint height)
        {
            ICellGrid<bool> gridOne = new CellGridMorton<bool>(width, height);
            ICellGrid<bool> gridTwo = new CellGridMorton<bool>(width, height);

            return new GameOfLife(gridOne, gridTwo);
        }
    }
}
