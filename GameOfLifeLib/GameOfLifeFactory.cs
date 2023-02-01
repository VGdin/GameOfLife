namespace GameOfLifeLib
{
    public static class GameOfLifeFactory
    {
        public static IGameOfLife<bool> CreateGameOfLife(uint width, uint height)
        {
            ICellGrid<bool> gridOne = new CellGridMorton<bool>(width, height);
            ICellGrid<bool> gridTwo = new CellGridMorton<bool>(width, height);

            return new GameOfLife(gridOne, gridTwo);
        }
    }
}
