using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameOfLife
{
    /// <summary>
    /// Class to draw the current state of the game
    /// </summary>
    internal sealed class DrawerGameState
    {
        private Texture2D _lineTexture;
        private Texture2D _cellTexture;
        private Texture2D _squareTexture;

        private Vector2 _middleOfLine;

        private readonly GameState _gameState;
        private readonly Camera _camera;

        public DrawerGameState(GameState gameState, Camera camera)
        {
            _gameState = gameState;
            _camera = camera;
        }

        public void LoadContent(ContentManager content)
        {
            _lineTexture = content.Load<Texture2D>("line");
            _cellTexture = content.Load<Texture2D>("circle");
            _squareTexture = content.Load<Texture2D>("square");
            _middleOfLine = new Vector2(_lineTexture.Width / 2, _lineTexture.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawOutline(spriteBatch);
            if (!Config.Instance.DisableGridAboveZoom || (_camera.Zoom > Config.Instance.ZoomGridThreshold))
            {
                // if above zoom, dont draw grid lines
                DrawGrid(spriteBatch);
            }
            DrawSelection(spriteBatch);
            DrawCells(spriteBatch);
        }

        private void DrawSelection(SpriteBatch spriteBatch)
        {
            int cellSize = Config.Instance.CellSize;
            int selectionWidth = cellSize + _lineTexture.Width * 4;
            int x_pos = cellSize * _gameState.CurrentSelection.x - _lineTexture.Width * 2;
            int y_pos = cellSize * _gameState.CurrentSelection.y - _lineTexture.Width * 2;

            spriteBatch.Draw(_squareTexture,
                new Rectangle(x_pos, y_pos, selectionWidth, selectionWidth),
                null,
                Color.White,
                0,
                new Vector2(0, 0),
                SpriteEffects.None,
                0);
        }

        private void DrawCells(SpriteBatch spriteBatch)
        {
            int cellSize = Config.Instance.CellSize;
            int drawSize = cellSize - _lineTexture.Width * 2;

            foreach (var (x, y) in _gameState.getAllActiveCells())
            {
                int x_pos = cellSize * (int)x + _lineTexture.Width;
                int y_pos = cellSize * (int)y + _lineTexture.Width;
                spriteBatch.Draw(_cellTexture,
                    new Rectangle(x_pos, y_pos, drawSize, drawSize),
                    null,
                    Color.Black,
                    0,
                    new Vector2(0, 0),
                    SpriteEffects.None,
                    0);
            }
        }

        private void DrawOutline(SpriteBatch spriteBatch)
        {
            int cellSize = Config.Instance.CellSize;
            int cellSizeHalf = cellSize / 2;

            int lineLengthDiag = cellSize * Config.Instance.GameSize.height;
            int midDiag = cellSizeHalf * Config.Instance.GameSize.height;

            int lineLengthHorz = cellSize * Config.Instance.GameSize.widht;
            int midHorz = cellSizeHalf * Config.Instance.GameSize.widht;

            int width = _lineTexture.Width * 4;

            spriteBatch.Draw(_lineTexture,
                new Rectangle(0, midDiag, width, lineLengthDiag),
                null,
                Color.DarkGreen,
                0,
                _middleOfLine,
                SpriteEffects.None,
                0);

            spriteBatch.Draw(_lineTexture,
                new Rectangle(lineLengthHorz, midDiag, width, lineLengthDiag),
                null,
                Color.DarkGreen,
                0,
                _middleOfLine,
                SpriteEffects.None,
                0);

            spriteBatch.Draw(_lineTexture,
                new Rectangle(midHorz, 0, width, lineLengthHorz),
                null,
                Color.DarkGreen,
                MathHelper.PiOver2,
                _middleOfLine,
                SpriteEffects.None,
                0);

            spriteBatch.Draw(_lineTexture,
                new Rectangle(midHorz, lineLengthDiag, width, lineLengthHorz),
                null,
                Color.DarkGreen,
                MathHelper.PiOver2,
                _middleOfLine,
                SpriteEffects.None,
                0);
        }

        private void DrawGrid(SpriteBatch spriteBatch)
        {
            int cellSize = Config.Instance.CellSize;
            int cellSizeHalf = cellSize / 2;

            int lineLengthDiag = cellSize * Config.Instance.GameSize.height;
            int midDiag = cellSizeHalf * Config.Instance.GameSize.height;

            int lineLengthHorz = cellSize * Config.Instance.GameSize.widht;
            int midHorz = cellSizeHalf * Config.Instance.GameSize.widht;

            // Horizontal Lines
            for (int i = 1; i < Config.Instance.GameSize.widht; i++)
            {
                spriteBatch.Draw(_lineTexture,
                    new Rectangle(cellSize * i, midDiag, _lineTexture.Width, lineLengthDiag),
                    null,
                    Color.SlateGray,
                    0,
                    _middleOfLine,
                    SpriteEffects.None,
                    0);
            }

            // Vertical Lines
            for (int i = 1; i < Config.Instance.GameSize.height; i++)
            {
                spriteBatch.Draw(_lineTexture,
                    new Rectangle(midHorz, cellSize * i, _lineTexture.Width, lineLengthHorz),
                    null,
                    Color.SlateGray,
                    MathHelper.PiOver2,
                    _middleOfLine,
                    SpriteEffects.None,
                    0) ;
            }
        }
    }
}
