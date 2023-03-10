using System.Runtime.CompilerServices;

namespace GameOfLifeLib.Optimized
{
    /// <summary>
    /// Cell Grid optimized with bit-twidling and change set.
    /// </summary>
    internal sealed class CellGridOptimizedChangeSet : CellGridOptimizedAbstract
    {
        private const byte CHANG_MASK = 0b01000000;
        private const byte NEIGH_MASK = 0b00011110;
        private const byte VALUE_MASK = 0b00000001;
        private const byte NEXTV_MASK = 0b00100000;
        private const byte CHANG_MASK_CLEAR = 0b10111111;
        private const byte VALUE_MASK_CLEAR = 0b11111110;
        private const byte NEXTV_MASK_CLEAR = 0b11011111;

        /**
         * TODO: Change so that neigbor count is the 4 lower bits, make more intuitive code, +/1 neighbors and no bitshift
         * U - unused
         * C - Changed
         * X - next value
         * N - neigbor count
         * S - state
         * => UCXN'NNNS
         */
        private readonly byte[] _representation;
        private HashSet<(uint x, uint y)> _changeListMain;
        private HashSet<(uint x, uint y)> _changeListBack;

        public CellGridOptimizedChangeSet(uint width, uint height) : base(width, height)
        {
            _representation = new byte[width * height];
            _changeListMain = new();
            _changeListBack = new();
        }

        public override void ClearAll()
        {
            for(uint i = 0; i< _representation.Length; i++)
            {
                _representation[i] &= 0x00;
            }
            _changeListBack.Clear();
            _changeListMain.Clear();
            ActiveCells.Clear();
        }

        /// <inheritdoc/>
        public override bool GetAt(uint x, uint y)
        {
            return (_representation[y * _width + x] & VALUE_MASK) != 0;
        }

        /// <inheritdoc/>
        public override void SetAt(uint x, uint y)
        {
            (uint xl, uint xr, uint yt, uint yb) = GetAdjecent(x, y);
            // Set state
            _representation[y * _width + x] |= VALUE_MASK;

            // Update neighbors with data
            _representation[yt * _width + xl] += 2;
            _representation[yt * _width + x] += 2;
            _representation[yt * _width + xr] += 2;
            _representation[y * _width + xl] += 2;
            _representation[y * _width + xr] += 2;
            _representation[yb * _width + xl] += 2;
            _representation[yb * _width + x] += 2;
            _representation[yb * _width + xr] += 2;

            AddToChangeList(x, y, xl, xr, yt, yb);

            ActiveCells.Add((x, y));
        }

        /// <inheritdoc/>
        public override void ClearAt(uint x, uint y)
        {
            (uint xl, uint xr, uint yt, uint yb) = GetAdjecent(x, y);
            // Set state
            _representation[y * _width + x] &= VALUE_MASK_CLEAR;

            // Update neighbors with data
            _representation[yt * _width + xl] -= 2;
            _representation[yt * _width + x] -= 2;
            _representation[yt * _width + xr] -= 2;
            _representation[y * _width + xl] -= 2;
            _representation[y * _width + xr] -= 2;
            _representation[yb * _width + xl] -= 2;
            _representation[yb * _width + x] -= 2;
            _representation[yb * _width + xr] -= 2;

            AddToChangeList(x, y, xl, xr, yt, yb);

            ActiveCells.Remove((x, y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddToChangeList(uint x, uint y, uint xl, uint xr, uint yt, uint yb)
        {
            _changeListMain.Add((x, y));
            _changeListMain.Add((xl, yt));
            _changeListMain.Add((x, yt));
            _changeListMain.Add((xr, yt));
            _changeListMain.Add((xl, y));
            _changeListMain.Add((xr, y));
            _changeListMain.Add((xl, yb));
            _changeListMain.Add((x, yb));
            _changeListMain.Add((xr, yb));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private (uint xl, uint xr, uint yt, uint yb) GetAdjecent(uint x, uint y)
        {
            uint xleft, xright, ytop, ybottom;
            if (x == 0)
            {
                xleft = _width - 1;
                xright = x + 1;
            }
            else if (x == _width - 1)
            {
                xleft = x - 1;
                xright = 0;
            }
            else
            {
                xleft = x - 1;
                xright = x + 1;
            }

            if (y == 0)
            {
                ytop = _height - 1;
                ybottom = y + 1;
            }
            else if (y == _width - 1)
            {
                ytop = y - 1;
                ybottom = 0;
            }
            else
            {
                ytop = y - 1;
                ybottom = y + 1;
            }
            return (xleft, xright, ytop, ybottom);
        }

        /// <inheritdoc/>
        public override void NextGeneration()
        {
            (_changeListMain, _changeListBack) = (_changeListBack, _changeListMain);
            foreach ((uint x, uint y) in _changeListBack)
            {
                uint index = y * _width + x;
                int count = (_representation[index] & NEIGH_MASK) >> 1;

                if ((_representation[index] & VALUE_MASK) != 0)
                {
                    if (count != 2 && count != 3)
                    {
                        _representation[index] &= NEXTV_MASK_CLEAR;
                        _representation[index] |= CHANG_MASK;
                    }
                }
                else
                {
                    if (count == 3)
                    {
                        _representation[index] |= NEXTV_MASK;
                        _representation[index] |= CHANG_MASK;
                    }
                }
            }

            foreach ((uint x, uint y) in _changeListBack)
            {
                uint index = y * _width + x;
                if ((_representation[index] & CHANG_MASK) != 0)
                {
                    if ((_representation[index] & NEXTV_MASK) != 0)
                    {
                        SetAt(x, y);
                    }
                    else
                    {
                        ClearAt(x, y);
                    }

                    _representation[index] &= CHANG_MASK_CLEAR;
                }
            }

            _changeListBack.Clear();
        }
    }
}
