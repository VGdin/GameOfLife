using Microsoft.Xna.Framework;
using System;

namespace GameOfLife
{
    public enum CameraDirections
    {
        Up, Down, Left, Right
    }
    public enum ZoomActions
    {
        ZoomIn, ZoomOut, ZoomMax, ZoomMin
    }
    internal sealed class Camera
    {
        public Vector2 Position { get; private set; } = new Vector2(Config.Instance.GameResolution.widht / 2, Config.Instance.GameResolution.height / 2);
        public float Zoom { get; private set; } = Config.Instance.DefaultZoom;
        public int ViewPortWidth { get; set; } = Config.Instance.DefaultResolution.widht;
        public int ViewPortHeight { get; set; } = Config.Instance.DefaultResolution.height;
        public Vector2 ViewPortCenter => new Vector2(ViewPortWidth / 2, ViewPortHeight / 2);

        private bool _isDirty = true;
        private Matrix _backingMatrix;
        public Matrix TranslationMatrix
        {
            get
            {
                if (_isDirty)
                {
                    _isDirty = false;
                    _backingMatrix =  Matrix.CreateTranslation(-Position.X, -Position.Y, 0) * Matrix.CreateScale(Zoom, Zoom, 1) * Matrix.CreateTranslation(ViewPortCenter.X, ViewPortCenter.Y, 0);
                }
                return _backingMatrix;
            }
        }

        public Camera()
        {
        }

        public void ZoomCamera(ZoomActions action)
        {
            _isDirty = true;
            Zoom = action switch
            {
                ZoomActions.ZoomMin => Config.Instance.ZoomMin,
                ZoomActions.ZoomMax => Config.Instance.ZoomMax,
                ZoomActions.ZoomIn when Zoom < Config.Instance.ZoomMax => Zoom * 2,
                ZoomActions.ZoomOut when Zoom > Config.Instance.ZoomMin => Zoom / 2,
                _ => Zoom
            };
        }

        public void MoveCameraTo(Vector2 newPosition)
        {
            _isDirty = true;
            Position = newPosition;
        }

        public void MoveCamera(CameraDirections direction)
        {
            _isDirty = true;
            Position += direction switch
            {
                CameraDirections.Up => new Vector2(0, -Config.Instance.CameraSpeed),
                CameraDirections.Down => new Vector2(0, Config.Instance.CameraSpeed),
                CameraDirections.Left => new Vector2(-Config.Instance.CameraSpeed, 0),
                CameraDirections.Right => new Vector2(Config.Instance.CameraSpeed, 0),
                _ => throw new NotImplementedException()
            };
        }
    }
}
