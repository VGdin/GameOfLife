namespace GameOfLifeLib
{
    /// <summary>
    /// Interface to read a game file/pattern
    /// </summary>
    public interface IGameFileReader
    {
        /// <summary>
        /// Path to the file
        /// Could be absolute or relative
        /// </summary>
        public String Path { get;}

        /// <summary>
        /// Name of the pattern
        /// </summary>
        public String Name { get;}

        /// <summary>
        /// Width of the pattern
        /// </summary>
        public int Width { get;}

        /// <summary>
        /// Height of the pattern
        /// </summary>
        public int Height { get;}

        /// <summary>
        /// The value of a cell in the pattern
        /// </summary>
        /// <param name="x">x coordinate in the pattern, 0 left</param>
        /// <param name="y">y coordinate in the pattern, 0 top</param>
        /// <returns></returns>
        public bool getAt(uint x, uint y);
    }
}
