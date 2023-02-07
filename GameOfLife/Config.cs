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


        public readonly (int widht, int height) GameSize = (2000, 2000);
        public readonly float DefaultUpdateRate = (float)1 / 8;

        /* Camera and Drawing */
        public readonly (int widht, int height) DefaultResolution = (1920, 1080);
        public readonly int CellSize = 20;
        public readonly float DefaultZoom = 1f;
        public readonly float ZoomMax = 4f;
        public readonly float ZoomMin = (float)1 / 8;
        public readonly float ZoomGridThreshold = (float)1 / 2;
        public readonly float CameraSpeed = 10f;

        public readonly bool DisableGridAboveZoom = true;

        /*  Derived */
        public (int widht, int height) GameResolution => (GameSize.widht * CellSize, GameSize.height * CellSize);
        public (int x, int y, int widht, int height) StatusDimensions => (0, DefaultResolution.height - 100, 400, 100);
        public (int x, int y, int widht, int height) LogoDimensions => (DefaultResolution.widht-400, DefaultResolution.height - 100, 400, 100);
    }
}
