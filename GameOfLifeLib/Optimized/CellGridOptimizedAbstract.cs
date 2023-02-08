using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeLib.Optimized
{
    internal abstract class CellGridOptimizedAbstract : ICellGrid
    {
        protected uint _width;
        protected uint _height;

        /// <inheritdoc/>
        public HashSet<(uint x, uint y)> ActiveCells { get; }
        /// <inheritdoc/>
        public uint Width
        {
            get
            {
                return _width;
            }
        }
        /// <inheritdoc/>
        public uint Height
        {
            get { return _height; }
        }

        protected CellGridOptimizedAbstract(uint widht, uint height)
        {
            _width = widht;
            _height = height;
            ActiveCells = new HashSet<(uint x, uint y)>();
        }

        public abstract void ClearAll();

        /// <inheritdoc/>
        public abstract void ClearAt(uint x, uint y);

        /// <inheritdoc/>
        public abstract bool GetAt(uint x, uint y);

        /// <inheritdoc/>
        public abstract void SetAt(uint x, uint y);

        /// <summary>
        /// Calculates the next generation
        /// </summary>
        public abstract void NextGeneration();
    }
}
