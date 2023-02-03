using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeLib.Optimized
{
    internal class CellGridOptimized : ICellGrid
    {
        public uint Width { get; }
        public uint Height { get; }

        /**
         * U - unused
         * N - Neighbor data
         * S - State
         * => UUUN'NNNS
         */
        private byte[] _representation;

        CellGridOptimized(uint width, uint height)
        {
            Width = width;
            Height = height;

            // Pad representation with on row/line of cells
            _representation = new byte[(Width + 2) * (height + 2)];
        }

        public bool GetAt(uint x, uint y)
        {
            return false; // _representation[(y + 1) * Width + x + 1];
        }

        public void SetAt(uint x, uint y)
        {
            //_representation[(y + 1) * Width + x + 1] = true;
        }

        public void ClearAt(uint x, uint y)
        {
            //_representation[(y + 1) * Width + x + 1] = false;
        }

        public int GetNoActiveNeighbors(uint x, uint y)
        {
            throw new NotImplementedException();
        }

        public (int x, int y)[] getAllActive()
        {
            throw new NotImplementedException();
        }
    }
}
