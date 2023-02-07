namespace GameOfLifeLib.Optimized
{
    /// <summary>
    /// Cell Grid optimized with bit-twidling etc.
    /// </summary>
    internal sealed class CellGridOptimized : CellGridOptimizedAbstract
    {

        /**
         * U - unused
         * N - neighbor count
         * S - state
         * => UUUN'NNNS
         */
        private readonly byte[] _representation;
        private readonly byte[] _tmp;

        public CellGridOptimized(uint width, uint height) : base(width,height)
        {
            _representation = new byte[Width * height];
            _tmp = new byte[Width * height];
        }

        /// <inheritdoc/>
        public override bool GetAt(uint x, uint y)
        {
            return (_representation[y * Width + x] & 0b00000001) != 0;
        }

        /// <inheritdoc/>
        public override void SetAt(uint x, uint y)
        {
            uint xleft, xright, ytop, ybottom;
            if (x == 0)
            {
                xleft = Width - 1;
                xright = x + 1;
            }
            else if (x == Width - 1)
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
                ytop = Height - 1;
                ybottom = y + 1;
            }
            else if (y == Width - 1)
            {
                ytop = y - 1;
                ybottom = 0;
            }
            else
            {
                ytop = y - 1;
                ybottom = y + 1;
            }

            // Set state
            _representation[y * Width + x] |= 0b00000001;

            // Update neighbors with data
            _representation[ytop * Width + xleft] += 2;
            _representation[ytop * Width + x] += 2;
            _representation[ytop * Width + xright] += 2;
            _representation[y * Width + xleft] += 2;
            _representation[y * Width + xright] += 2;
            _representation[ybottom * Width + xleft] += 2;
            _representation[ybottom * Width + x] += 2;
            _representation[ybottom * Width + xright] += 2;

            ActiveCells.Add((x, y));
        }

        /// <inheritdoc/>
        public override void ClearAt(uint x, uint y)
        {
            uint xleft, xright, ytop, ybottom;
            if (x == 0)
            {
                xleft = Width - 1;
                xright = x + 1;
            }
            else if (x == Width - 1)
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
                ytop = Height - 1;
                ybottom = y + 1;
            }
            else if (y == Width - 1)
            {
                ytop = y - 1;
                ybottom = 0;
            }
            else
            {
                ytop = y - 1;
                ybottom = y + 1;
            }

            // Set state
            _representation[y * Width + x] &= 0b11111110;

            // Update neighbors with data
            _representation[ytop * Width + xleft] -= 2;
            _representation[ytop * Width + x] -= 2;
            _representation[ytop * Width + xright] -= 2;
            _representation[y * Width + xleft] -= 2;
            _representation[y * Width + xright] -= 2;
            _representation[ybottom * Width + xleft] -= 2;
            _representation[ybottom * Width + x] -= 2;
            _representation[ybottom * Width + xright] -= 2;

            ActiveCells.Remove((x, y));
        }

        /// <summary>
        /// Calculates and rewrites the next generation
        /// </summary>
        /// <param name="next">Cell grid of SAME SIZE that will contain next generation</param>
        public override void NextGeneration()
        {
            Array.Copy(_representation, _tmp, _representation.Length);
            uint index, line;
            uint width = Width;
            uint height = Height;
            for (uint y = 0; y < height; y++)
            {
                line = width * y;
                for (uint x = 0; x < width; x++)
                {
                    index = line + x;
                    if (_tmp[index] != 0)
                    {
                        int count = _tmp[index] >> 1;
                        if ((_tmp[index] & 0b00000001) != 0)
                        {
                            if (count != 2 && count != 3)
                            {
                                ClearAt(x, y);
                            }
                        }
                        else
                        {
                            if (count == 3)
                            {
                                SetAt(x, y);
                            }
                        }
                    }
                }
            }
        }
    }
}
