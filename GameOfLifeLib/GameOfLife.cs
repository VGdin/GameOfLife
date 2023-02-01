namespace GameOfLifeLib
{
    /// <summary>
    /// Implementation of GameOfLife that uses bool as the cell type
    /// </summary>
    public class GameOfLife : IGameOfLife<bool>
    {

        /// <inheritdoc/>>
        public ICellGrid<bool> Grid { get; private set; }
        private ICellGrid<bool> _newGrid;

        /// <summary>
        /// Creates a new GameOfLife with two given CellGrids, switches between who is the main and tmp when taking a new step
        /// </summary>
        /// <param name="gridOne">First cell grid</param>
        /// <param name="gridTwo">Seconds cell grid</param>
        public GameOfLife(ICellGrid<bool> gridOne, ICellGrid<bool> gridTwo)
        {
            Grid = gridOne;
            _newGrid = gridTwo;
        }

        /// <inheritdoc/>>
        public void Clear()
        {
            ICellGridIEnumerator<bool> enumerator = Grid.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Grid.SetAt(enumerator.X, enumerator.Y, false);
            }
        }

        /// <inheritdoc/>>
        public void Step()
        {
            ICellGridIEnumerator<bool> enumerator = Grid.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int noNeighbors = Grid.GetNoActiveNeighbors(enumerator.X, enumerator.Y, (bool b) => { return b; });
                _newGrid.SetAt(enumerator.X, enumerator.Y, newValue(noNeighbors, enumerator.Current));
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