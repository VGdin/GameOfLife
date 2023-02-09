using GameOfLifeLib.FileHandlers;

namespace GameOfLifeLib
{
    /// <summary>
    /// Factory to read and write different type of file types
    /// </summary>
    public static class FileHandlerFactory
    {

        /// <summary>
        /// Given the path to a file, return a filereader for that extension
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>A file reader that can be used to load state into a game</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IGameFileReader CreateFileReader(string path)
        {
            switch (Path.GetExtension(path).ToUpper())
            {
                case ".CELLS":
                    return new FileReaderPlainText(path);
                default:
                    throw new NotImplementedException("File type " + Path.GetExtension(path) + " not implemented yet");
            }
        }
    }
}
