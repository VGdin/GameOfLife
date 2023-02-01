using System.Collections;

namespace GameOfLifeLib
{
    /// <summary>
    /// Enumerator over a CellGrid using z-order indexing
    /// </summary>
    /// <typeparam name="T">Content of a cell</typeparam>
    public class CellGridMortonEnumerator<T> : ICellGridIEnumerator<T>
    {
        private readonly T[] _representation;
        private readonly uint _w;
        private readonly uint _h;

        private T Current => _representation[Utils.MortonNumber(X, Y)];
        private bool started;

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        /// <inheritdoc/>
        T IEnumerator<T>.Current => Current;

        /// <inheritdoc/>
        public uint X { get; private set; }

        /// <inheritdoc/>
        public uint Y { get; private set; }

        /// <summary>
        /// Create new Enumerator for a CellGrid using z-order indexing
        /// </summary>
        /// <param name="representation">Reference to the representation of the Cellgrid as a singledimensional array </param>
        /// <param name="w">Width of Cell Grid</param>
        /// <param name="h">Height of Cell Grid</param>
        /// <exception cref="ArgumentException">On wrong input</exception>
        public CellGridMortonEnumerator(ref T[] representation, uint w, uint h)
        {
            if (w * h != representation.Length)
            {
                throw new ArgumentException(String.Format("Width{0} and height{1} does not match input representation", w, h));
            }
            _representation = representation;
            _w = w;
            _h = h;
            Reset();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void Reset()
        {
            started = false;
                X = 0;
                Y = 0;
        }
    }
}
