using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeLib.Optimized
{
    internal class GameOfLifeOptimized : IGameOfLife
    {
        /// <inheritdoc/>
        public ICellGrid Grid
        {
            get
            {
                return _mainGrid;
            }
        }

        public ISet<(uint x, uint y)> AllActiveCells => _mainGrid.ActiveCells;

        public CellGridOptimized _mainGrid;

        /// <summary>
        /// Create new game of life using two optimized grids
        /// </summary>
        /// <param name="grid"></param>
        public GameOfLifeOptimized(CellGridOptimized grid)
        {
            _mainGrid = grid;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _mainGrid.Clear();
        }

        /// <inheritdoc/>
        public void Step()
        {
            _mainGrid.NextGeneration();
        }
    }
}
