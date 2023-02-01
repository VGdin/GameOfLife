using System.Collections;
using System.Diagnostics;

namespace GameOfLifeLib
{
    public partial class CellGridMorton<T> : ICellGrid<T>
    {
        public uint Width { get; }
        public uint Height { get; }

        private T[] _representation;

        public CellGridMorton(uint width, uint height)
        {
            if (width > 60000 || height > 6000)
            {
                throw new ArgumentException(String.Format("Size, width({0}) and height({1}), can't be larger than 6'000", width, height));
            } else if (!Utils.isPowerOf2(width) || !Utils.isPowerOf2(height) )
            {
                throw new ArgumentException(String.Format("Size, width({0}) and height({1}), must be a power of 2, eg. 2,4,8...", width, height));
            }

            Width = width;
            Height = height;
            _representation = new T[Width * Height];
        }

        public T GetAt(uint x, uint y)
        {
            return _representation[Utils.MortonNumber(x, y)];
        }

        public void SetAt(uint x, uint y, T value)
        {
            _representation[Utils.MortonNumber(x, y)] = value;
        }

        public int GetNoActiveNeighbors(uint x, uint y, Predicate<T> test)
        {
            int noNeighborsActive = 0;
            if (x == 0 || y == 0 || !(x < Width - 1) || !(y < Height - 1))
            {
                noNeighborsActive += test(_representation[Utils.MortonNumber(x == 0 ? Width - 1 : x - 1, y == 0 ? Height - 1 : y - 1)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x, y == 0 ? Height - 1 : y - 1)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x == Width - 1 ? 0 : x + 1, y == 0 ? Height - 1 : y - 1)]) ? 1 : 0;

                noNeighborsActive += test(_representation[Utils.MortonNumber(x == 0 ? Width - 1 : x - 1, y)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x == Width - 1 ? 0 : x + 1, y)]) ? 1 : 0;

                noNeighborsActive += test(_representation[Utils.MortonNumber(x == 0 ? Width - 1 : x - 1, y == Height - 1 ? 0 : y + 1)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x, y == Height - 1 ? 0 : y + 1)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x == Width - 1 ? 0 : x + 1, y == Height - 1 ? 0 : y + 1)]) ? 1 : 0;
            }
            else
            {
                noNeighborsActive += test(_representation[Utils.MortonNumber(x - 1, y - 1)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x, y - 1)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x + 1, y - 1)]) ? 1 : 0;

                noNeighborsActive += test(_representation[Utils.MortonNumber(x - 1, y)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x + 1, y)]) ? 1 : 0;

                noNeighborsActive += test(_representation[Utils.MortonNumber(x - 1, y + 1)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x, y + 1)]) ? 1 : 0;
                noNeighborsActive += test(_representation[Utils.MortonNumber(x + 1, y + 1)]) ? 1 : 0;
            }

            return noNeighborsActive;
        }

        ICellGridIterator<T> ICellGridEnumerable<T>.GetEnumerator()
        {
            return new CellGridMortonIterator<T>(ref _representation, Width, Height);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
