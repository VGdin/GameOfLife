namespace GameOfLifeLib
{
    /// <summary>
    /// Extended enumerator to show the corresponding X and Y coordinates for the current element
    /// </summary>
    /// <typeparam name="T">Content of a Cell in a CellGrid</typeparam>
    public interface ICellGridIEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// X coordinate of current element
        /// </summary>
        public uint X { get; }

        /// <summary>
        /// Y coordinate of current element
        /// </summary>
        public uint Y { get; }
    }
}
