namespace GameOfLifeLib
{
    /// <summary>
    /// A game of life, with a generic cell type
    /// </summary>
    public interface IGameOfLife
    {
        /// <summary>
        /// A grid containing all cells in a game
        /// </summary>
        ICellGrid Grid { get; }

        /// <summary>
        /// An set of all active cells
        /// </summary>
        ISet<(uint x, uint y)> AllActiveCells { get; }

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
