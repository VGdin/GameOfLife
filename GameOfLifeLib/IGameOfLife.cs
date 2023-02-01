namespace GameOfLifeLib
{
    /// <summary>
    /// A game of life, with a generic cell type
    /// </summary>
    /// <typeparam name="T">Content of a Cell in a CellGrid</typeparam>
    public interface IGameOfLife<T>
    {
        /// <summary>
        /// A grid containing all cells in a game
        /// </summary>
        ICellGrid<T> Grid { get; }

        /// <summary>
        /// Take one step in the game producing the next state
        /// </summary>
        public void Step();

        /// <summary>
        /// Clear all cells, restore to original state
        /// </summary>
        public void Clear();
    }
}
