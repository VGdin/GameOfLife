namespace GameOfLifeLib.Morton
{
    /// <summary>
    /// Implementation of GameOfLife that implements using Morton numbers
    /// </summary>
    public class GameOfLifeMorton : IGameOfLife
    {

        /// <inheritdoc/>>
        public ICellGrid Grid {
            get
            {
                return _mainGrid;
            }
        }

        public ISet<(uint x, uint y)> AllActiveCells => throw new NotImplementedException();

        private CellGridMorton _newGrid;
        private CellGridMorton _mainGrid;

        /// <summary>
        /// Creates a new GameOfLife with two given CellGrids, switches between who is the main and tmp when taking a new step
        /// </summary>
        /// <param name="gridOne">First cell grid</param>
        /// <param name="gridTwo">Seconds cell grid</param>
        public GameOfLifeMorton(CellGridMorton gridOne, CellGridMorton gridTwo)
        {
            _mainGrid = gridOne;
            _newGrid = gridTwo;
        }

        /// <inheritdoc/>>
        public void Clear()
        {
            for (uint x = 0; x < _mainGrid.Width; x++)
            {
                for (uint y = 0; y < _mainGrid.Height; y++)
                {
                    _mainGrid.ClearAt(x,y);
                }
            }
        }

        /// <inheritdoc/>>
        public void Step()
        {
            for (uint x = 0; x < _mainGrid.Width; x++)
            {
                for (uint y = 0; y < _mainGrid.Height; y++)
                {
                    int noNeighbors = _mainGrid.GetNoActiveNeighbors(x,y);
                    if (Utils.NewValue(noNeighbors,_mainGrid.GetAt(x,y)))
                    {
                        _newGrid.SetAt(x, y);
                    }
                    else
                    {
                        _newGrid.ClearAt(x,y);
                    }
                }
            }

            // Switch the of new and old grid
            (_newGrid, _mainGrid) = (_mainGrid, _newGrid);
        }

        public void Load(IGameFileReader fileHandler, uint x, uint y)
        {
            throw new NotImplementedException();
        }

        public void Save(IGameFileWriter fileHandler, uint x, uint y, uint w, uint h)
        {
            throw new NotImplementedException();
        }
    }
}