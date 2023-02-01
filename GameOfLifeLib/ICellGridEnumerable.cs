namespace GameOfLifeLib
{
    /// <summary>
    /// Specific enumerable with specific iterator ICellGridIterator
    /// </summary>
    /// <typeparam name="T">Content of a Cell in a CellGrid</typeparam>
    public interface ICellGridEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// Returns specific iterator that also shows coordinates, x and y, for the current element
        /// </summary>
        /// <returns>Returns Enumerator</returns>
        new ICellGridIEnumerator<T> GetEnumerator();
    }
}
