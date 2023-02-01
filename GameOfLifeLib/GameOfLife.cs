namespace GameOfLifeLib
{
    public class GameOfLife : IGameOfLife<bool>
    {

        public ICellGrid<bool> Grid { get; private set; }
        private ICellGrid<bool> _newGrid;

        public GameOfLife(ICellGrid<bool> grid, ICellGrid<bool> tmpgrid)
        {
            Grid = grid;
            _newGrid = tmpgrid;
        }

        public void Clear()
        {
            ICellGridIterator<bool> enumerator = Grid.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Grid.SetAt(enumerator.X, enumerator.Y, false);
            }
        }

        public void Step()
        {
            ICellGridIterator<bool> enumerator = Grid.GetEnumerator();
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