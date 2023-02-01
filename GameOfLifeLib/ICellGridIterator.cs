namespace GameOfLifeLib
{
    public interface ICellGridIterator<T> : IEnumerator<T>
    {
        public uint X { get; }
        public uint Y { get; }
    }
}
