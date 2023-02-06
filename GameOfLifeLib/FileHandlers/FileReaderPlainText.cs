using System.Text.RegularExpressions;

namespace GameOfLifeLib.FileHandlers
{
    /// <summary>
    /// File Reader to read a plain text file with .cells suffix.
    /// </summary>
    internal sealed class FileReaderPlainText : AbstractFileReader
    {
        private readonly Regex _nameLineFormat = new Regex(@"^!Name:(.+)$", RegexOptions.Compiled);
        private readonly Regex _commentLineFormat = new Regex(@"^!(.*)$", RegexOptions.Compiled);
        private readonly Regex _gameLineFormat = new Regex(@"^([.O]*)$", RegexOptions.Compiled);

        /// <inheritdoc/>
        public override string Name { get; } = String.Empty;
        /// <inheritdoc/>
        public override int Width { get; }
        /// <inheritdoc/>
        public override int Height { get; }

        public FileReaderPlainText(string path) : base(path)
        {

            for (int y = 0; y < content.Length; y++)
            {
                string line = content[y];
                if (_nameLineFormat.IsMatch(line))
                {
                    Name = _nameLineFormat.Match(line).Groups[1].Value.Trim();
                }
                else if (_commentLineFormat.IsMatch(line))
                {
                    //Do nothing with comments
                }
                else if (_gameLineFormat.IsMatch(line))
                {
                    // Get Widht and Height
                    Height++;
                    if (line.Length > Width)
                    {
                        Width = line.Length;
                    }
                }
                else
                {
                    throw new FormatException(String.Format("Error parsing file at line {}. Malformatted line: {}", y, line));
                }
            }

            representation = new bool[Width, Height];

            //Populate matrix with data, i = line in file, y = line representation
            // Makes it possible to intersperse comments between two lines (why not?)
            int y_matrix = -1;
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                if (_gameLineFormat.IsMatch(line))
                {
                    y_matrix++;
                    char[] cells = line.ToCharArray();
                    for (int x_matrix = 0; x_matrix < line.Length; x_matrix++)
                    {
                        representation[x_matrix, y_matrix] = cells[x_matrix] == 'O';
                    }
                }
            }
        }
    }
}
