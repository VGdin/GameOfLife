namespace GameOfLifeLib.Optimized
{
    internal class GameOfLifeOptimized : IGameOfLife
    {
        /// <inheritdoc/>
        public ICellGrid Grid
        {
            get
            {
                return _mainGrid;
            }
        }

        public ISet<(uint x, uint y)> AllActiveCells => _mainGrid.ActiveCells;

        public CellGridOptimized _mainGrid;

        /// <summary>
        /// Create new game of life using two optimized grids
        /// </summary>
        /// <param name="grid"></param>
        public GameOfLifeOptimized(CellGridOptimized grid)
        {
            _mainGrid = grid;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _mainGrid.Clear();
        }

        /// <inheritdoc/>
        public void Step()
        {
            _mainGrid.NextGeneration();
        }

        /// <inheritdoc/>
        public void Load(IGameFileReader fileHandler, uint x, uint y)
        {
            for (uint j = 0; j < fileHandler.Height; j++)
            {
                if (j + y > Grid.Height)
                {
                    break;
                }

                for (uint i = 0; i < fileHandler.Width; i++)
                {
                    if (i + x > Grid.Width)
                    {
                        break;
                    }

                    if (fileHandler.getAt(i, j) && !Grid.GetAt(i+x,j+y))
                    {
                        Grid.SetAt(i + x, j + y);
                    }
                    else if (!fileHandler.getAt(i, j) && Grid.GetAt(i+x,j+y))
                    {
                        Grid.ClearAt(i + x, j + y);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Save(IGameFileWriter fileHandler, uint x, uint y, uint w, uint h)
        {
            throw new NotImplementedException();
        }
    }
}
