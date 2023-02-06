namespace GameOfLifeLib.FileHandlers
{
    /// <summary>
    /// Abstract file reader to shared common methods between all filereaders for different formats
    /// </summary>
    internal abstract class AbstractFileReader : IGameFileReader
    {
        /// <inheritdoc/>
        public string Path { get; private set; }
        /// <inheritdoc/>
        public abstract string Name { get; }
        /// <inheritdoc/>
        public abstract int Width { get; }
        /// <inheritdoc/>
        public abstract int Height { get; }

        protected String[] content;
        protected bool[,]? representation;

        protected AbstractFileReader(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Could mot find file " + path);
            }

            Path = path;
            content = File.ReadAllLines(path);
        }

        /// <inheritdoc/>
        public bool getAt(uint x, uint y)
        {
            if (representation is not null)
            {
                return representation[x, y];
            }
            return false;
        }
    }
}
