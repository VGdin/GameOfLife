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


        public readonly (int widht, int height) GameSize = (256, 256);
        public readonly float DefaultUpdateRate = 0.1f;

        /* Camera and Drawing */
        public readonly (int widht, int height) DefaultResolution = (1000, 1000);
        public readonly int CellSize = 20;
        public readonly float DefaultZoom = 1f;
        public readonly float ZoomMax = 8;
        public readonly float ZoomMin = 1/8;
        public readonly float CameraSpeed = 10f;
    }
}
