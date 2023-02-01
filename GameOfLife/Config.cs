using System;

namespace GameOfLife
{
    internal sealed class Config
    {
        private static readonly Lazy<Config> lazy = new Lazy<Config>(() => new Config());
        public static Config Instance { get { return lazy.Value; } }

        private Config()
        {
        }

        public (int widht, int height) GameSize { get; private set; } = (256, 256);
        public (int x, int y, int widht, int height) VisibleGameSize { get; private set; } = (103, 103, 50, 50);
        public int CellSize { get; private set; } = 20;
        public int StatusHeight { get; private set; } = 100;
        public float DefaultUpdateRate { get; private set; } = 0.1f;
    }
}
