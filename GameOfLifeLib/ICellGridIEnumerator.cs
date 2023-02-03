namespace GameOfLifeLib
{
    /// <summary>
    /// Extended enumerator to show the corresponding X and Y coordinates for the current element
    /// </summary>
    public interface ICellGridIEnumerator : IEnumerator<bool>
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
