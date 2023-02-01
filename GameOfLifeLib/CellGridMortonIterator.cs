using System.Collections;

namespace GameOfLifeLib
{
    public class CellGridMortonIterator<T> : ICellGridIterator<T>
    {
        private readonly T[] _representation;
        private readonly uint _w;
        private readonly uint _h;

        private T Current => _representation[Utils.MortonNumber(X, Y)];
        private bool started;

        public uint X { get; private set; }
        public uint Y { get; private set; }

        public CellGridMortonIterator(ref T[] representation, uint w, uint h)
        {
            _representation = representation;
            _w = w;
            _h = h;
            Reset();
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        T IEnumerator<T>.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            if (!started)
            {
                started = true;
                return true;
            }
            if (++X < _w)
            {
                return true;
            }
            else if (++Y < _h)
            {
                X = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            started = false;
            X = 0;
            Y = 0;
        }
    }
}
