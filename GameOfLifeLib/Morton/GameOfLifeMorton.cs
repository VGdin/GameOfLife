namespace GameOfLifeLib.Morton
{
    /// <summary>
    /// Implementation of GameOfLife that uses bool as the cell type
    /// </summary>
    public class GameOfLifeMorton : IGameOfLife
    {

        /// <inheritdoc/>>
        public ICellGrid Grid { get; private set; }
        private ICellGrid _newGrid;

        /// <summary>
        /// Creates a new GameOfLife with two given CellGrids, switches between who is the main and tmp when taking a new step
        /// </summary>
        /// <param name="gridOne">First cell grid</param>
        /// <param name="gridTwo">Seconds cell grid</param>
        public GameOfLifeMorton(ICellGrid gridOne, ICellGrid gridTwo)
        {
            Grid = gridOne;
            _newGrid = gridTwo;
        }

        /// <inheritdoc/>>
        public void Clear()
        {
            for (uint x = 0; x < Grid.Width; x++)
            {
                for (uint y = 0; y < Grid.Height; y++)
                {
                    Grid.ClearAt(x,y);
                }
            }
        }

        /// <inheritdoc/>>
        public void Step()
        {
            for (uint x = 0; x < Grid.Width; x++)
            {
                for (uint y = 0; y < Grid.Height; y++)
                {
                    int noNeighbors = Grid.GetNoActiveNeighbors(x,y, (b) => { return b; });
                    if (newValue(noNeighbors,Grid.GetAt(x,y)))
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
            (_newGrid, Grid) = (Grid, _newGrid);
        }

        private static bool newValue(int neigbors, bool current) => neigbors switch
        {
            2 => current,
            3 => true,
            _ => false
        };
    }
}