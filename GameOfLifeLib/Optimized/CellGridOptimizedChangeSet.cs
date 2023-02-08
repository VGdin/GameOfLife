using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace GameOfLifeLib.Optimized
{
    /// <summary>
    /// Cell Grid optimized with bit-twidling and change set.
    /// </summary>
    internal sealed class CellGridOptimizedChangeSet : CellGridOptimizedAbstract
    {
        /**
         * U - unused
         * N - neigbor count
         * S - state
         * => UUUN'NNNS
         */
        private readonly byte[] _representation;
        private readonly byte[] _tmp;

        private HashSet<(uint x, uint y)> _changeListMain;
        private HashSet<(uint x, uint y)> _changeListBack;

        public CellGridOptimizedChangeSet(uint width, uint height) : base(width, height)
        {
            _representation = new byte[width * height];
            _tmp = new byte[width * height];
            _changeListMain = new();
            _changeListBack = new();
        }

        /// <inheritdoc/>
        public override bool GetAt(uint x, uint y)
        {
            return (_representation[y * _width + x] & 0b00000001) != 0;
        }

        /// <inheritdoc/>
        public override void SetAt(uint x, uint y)
        {
            (uint xl, uint xr, uint yt, uint yb) adjecent = GetAdjecent(x, y);

            // Set state
            _representation[y * _width + x] |= 0b00000001;

            // Update neighbors with data
            _representation[adjecent.yt * _width + adjecent.xl] += 2;
            _representation[adjecent.yt * _width + x] += 2;
            _representation[adjecent.yt * _width + adjecent.xr] += 2;
            _representation[y * _width + adjecent.xl] += 2;
            _representation[y * _width + adjecent.xr] += 2;
            _representation[adjecent.yb * _width + adjecent.xl] += 2;
            _representation[adjecent.yb * _width + x] += 2;
            _representation[adjecent.yb * _width + adjecent.xr] += 2;


            // Add to changelist
            _changeListMain.Add((x, y));

            _changeListMain.Add((adjecent.xl, adjecent.yt));
            _changeListMain.Add((x, adjecent.yt));
            _changeListMain.Add((adjecent.xr, adjecent.yt));
            _changeListMain.Add((adjecent.xl, y));
            _changeListMain.Add((adjecent.xr, y));
            _changeListMain.Add((adjecent.xl, adjecent.yb));
            _changeListMain.Add((x, adjecent.yb));
            _changeListMain.Add((adjecent.xr, adjecent.yb));

            ActiveCells.Add((x, y));
        }

        /// <inheritdoc/>
        public override void ClearAt(uint x, uint y)
        {
            (uint xl, uint xr, uint yt, uint yb) adjecent = GetAdjecent(x, y);
            // Set state
            _representation[y * _width + x] &= 0b11111110;

            // Update neighbors with data
            _representation[adjecent.yt * _width + adjecent.xl] -= 2;
            _representation[adjecent.yt * _width + x] -= 2;
            _representation[adjecent.yt * _width + adjecent.xr] -= 2;
            _representation[y * _width + adjecent.xl] -= 2;
            _representation[y * _width + adjecent.xr] -= 2;
            _representation[adjecent.yb * _width + adjecent.xl] -= 2;
            _representation[adjecent.yb * _width + x] -= 2;
            _representation[adjecent.yb * _width + adjecent.xr] -= 2;

            // Add to changelist
            _changeListMain.Add((x, y));

            _changeListMain.Add((adjecent.xl, adjecent.yt));
            _changeListMain.Add((x, adjecent.yt));
            _changeListMain.Add((adjecent.xr, adjecent.yt));
            _changeListMain.Add((adjecent.xl, y));
            _changeListMain.Add((adjecent.xr, y));
            _changeListMain.Add((adjecent.xl, adjecent.yb));
            _changeListMain.Add((x, adjecent.yb));
            _changeListMain.Add((adjecent.xr, adjecent.yb));

            ActiveCells.Remove((x, y));
        }

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
            Array.Copy(_representation, _tmp, _representation.Length);

            (_changeListMain, _changeListBack) = (_changeListBack, _changeListMain);
            foreach ((uint x, uint y) cell in _changeListBack)
            {
                uint index = cell.y * _width + cell.x;
                int count = _tmp[index] >> 1;
                if ((_tmp[index] & 0b00000001) != 0)
                {
                    if (count != 2 && count != 3)
                    {
                        ClearAt(cell.x, cell.y);
                    }
                }
                else
                {
                    if (count == 3)
                    {
                        SetAt(cell.x, cell.y);
                    }
                }
            }
            _changeListBack.Clear();
        }
    }
}
