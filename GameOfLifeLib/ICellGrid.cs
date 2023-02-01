namespace GameOfLifeLib
{
    /// <summary>
    /// Class to hold a implementation of a 2D grid
    /// </summary>
    /// <typeparam name="T">Content of a cell</typeparam>
    public interface ICellGrid<T> :  ICellGridEnumerable<T>
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
        T GetAt(uint x, uint y);

        /// <summary>
        /// Replace the value at coordinates with the new, given, value
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="value">New given value</param>
        void SetAt(uint x, uint y, T value);

        /// <summary>
        /// Given a predicate, return how many neighbors return true
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="test">Test of the content of cell</param>
        /// <returns></returns>
        int GetNoActiveNeighbors(uint x, uint y, Predicate<T> test);
    }
}
