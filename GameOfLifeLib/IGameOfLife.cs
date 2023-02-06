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

        /// <summary>
        /// Loads a pattern into game and places its top left corner at x, y
        /// </summary>
        /// <param name="fileHandler">Filehandler handling loading file</param>
        /// <param name="x">x coordinate in game</param>
        /// <param name="y">y coordinate in game</param>
        public void Load(IGameFileReader fileHandler, uint x, uint y);

        /// <summary>
        /// Saves a pattern from game from top left corner of x,y with a width and height
        /// </summary>
        /// <param name="fileHandler">Filehandler handling saving the file</param>
        /// <param name="x">x coordinate in game</param>
        /// <param name="y">y coordinate in game</param>
        /// <param name="w">Width of pattern in game</param>
        /// <param name="h">Height of pattern in game</param>
        public void Save(IGameFileWriter fileHandler, uint x, uint y, uint w, uint h);
    }
}
