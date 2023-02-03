namespace GameOfLifeLib
{
    /// <summary>
    /// Implementation of GameOfLife that uses bool as the cell type
    /// </summary>
    public class GameOfLife : IGameOfLife {
        public ICellGrid Grid { get; private set; }
        private ICellGrid _newGrid;

        /// <summary>
        /// Creates a new GameOfLife with two given CellGrids, switches between who is the main and tmp when taking a new step
        /// </summary>
        /// <param name="gridOne">First cell grid</param>
        /// <param name="gridTwo">Seconds cell grid</param>
        public GameOfLife(ICellGrid gridOne, ICellGrid gridTwo)
        {
            Grid = gridOne;
            _newGrid = gridTwo;
        }

        /// <inheritdoc/>>
        public void Clear()
        {
            ICellGridIEnumerator enumerator = Grid.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Grid.ClearAt(enumerator.X, enumerator.Y);
            }
        }

        /// <inheritdoc/>>
        public void Step()
        {
            ICellGridIEnumerator enumerator = Grid.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int noNeighbors = Grid.GetNoActiveNeighbors(enumerator.X, enumerator.Y, (bool b) => { return b; });
                if (newValue(noNeighbors, enumerator.Current))
                {
                    _newGrid.SetAt(enumerator.X, enumerator.Y);
                }
                else
                {
                    _newGrid.ClearAt(enumerator.X, enumerator.Y);
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