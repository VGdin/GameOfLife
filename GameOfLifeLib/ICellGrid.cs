namespace GameOfLifeLib
{
    public interface ICellGrid<T> : ICellGridEnumerable<T>
    {
        uint Width { get; }
        uint Height { get; }
        T GetAt(uint x, uint y);
        void SetAt(uint x, uint y, T value);
        int GetNoActiveNeighbors(uint x, uint y, Predicate<T> test);
    }
}
