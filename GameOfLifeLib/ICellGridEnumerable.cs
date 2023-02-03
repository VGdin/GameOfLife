namespace GameOfLifeLib
{
    /// <summary>
    /// Specific enumerable with specific iterator ICellGridIterator
    /// </summary>
    public interface ICellGridEnumerable : IEnumerable<bool>
    {
        /// <summary>
        /// Returns specific iterator that also shows coordinates, x and y, for the current element
        /// </summary>
        /// <returns>Returns Enumerator</returns>
        new ICellGridIEnumerator GetEnumerator();
    }
}
