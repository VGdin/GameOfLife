namespace GameOfLifeLib
{
    public interface IGameOfLife<T>
    {
        ICellGrid<T> Grid { get; }
        public void Step();
        public void Clear();
    }
}
