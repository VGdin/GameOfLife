namespace GameOfLifeLib
{
    /// <summary>
    /// Class to hold a implementation of a 2D grid
    /// </summary>
    public interface ICellGrid
    {
        /// <summary>
        /// Width of the grid
        /// </summary>
        uint Width { get; }

        /// <summary>
        /// Height of the grid
        /// </summary>
        uint Height { get; }

        /// <summary>
        /// Returns value at coordinates
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns></returns>
        bool GetAt(uint x, uint y);

        /// <summary>
        /// Marks cell at coordinates as alive
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        void SetAt(uint x, uint y);

        /// <summary>
        /// Marks cell at coordinates as dead
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        void ClearAt(uint x, uint y);
    }
}
