namespace GameOfLifeLib
{
    public interface ICellGridEnumerable<T> : IEnumerable<T>
    {
        new ICellGridIterator<T> GetEnumerator();
    }
}
