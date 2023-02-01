using GameOfLifeLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameOfLife
{
    public class GameStateDrawer
    {

        private const int _cellSize = 20;
        private const int _cellHalfSize = _cellSize / 2;
        private const float _90DEG = (float)Math.PI / 2;
        private (int x, int y, int width, int height) _visible; 
        private SpriteFont _text;
        private Texture2D _lineTexture;
        private Texture2D _cellTexture;
        private Texture2D _squareTexture;

        private readonly GameState _gameState;
        private readonly InputHandler _inputHandler;
        public GameStateDrawer(GameState gameState, InputHandler inputHandler)
        {
            _gameState = gameState;
            _inputHandler = inputHandler;
        }
        public void LoadContent(ContentManager content)
        {
            _lineTexture = content.Load<Texture2D>("line");
            _cellTexture = content.Load<Texture2D>("circle");
            _squareTexture = content.Load<Texture2D>("square");
            _text = content.Load<SpriteFont>("Text");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawGrid(spriteBatch);
            DrawSelection(spriteBatch);
            DrawCells(spriteBatch);
            DrawStatus(spriteBatch);
        }

        private void DrawStatus(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_text,
                String.Format("{0} - Update rate: {1}Hz\n"
                                + "Commands : {2}"
                                + "{3}",
                    _gameState.Paused ? "Paused" : "Running",
                    1 / _gameState.UpdateRate,
                    InputHandler.AvailableCommandsString,
                    _gameState.EditMode ? ":" + _inputHandler.CurrentCommand : ""),
                new Vector2(10, 10 + _gameState.Size.height * _cellSize), Color.Black);
        }

        private void DrawSelection(SpriteBatch spriteBatch)
        {
            int selectionWidth = _cellSize + _lineTexture.Width * 2;
            int x_pos = _cellSize * _gameState.CurrentSelection.x - _lineTexture.Width;
            int y_pos = _cellSize * _gameState.CurrentSelection.y - _lineTexture.Width;

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
            int drawSize = _cellSize - _lineTexture.Width * 2;
            _gameState.DoActionForACell(delegate (bool cell, int x, int y)
            {
                if (cell)
                {
                    int x_pos = _cellSize * x + _lineTexture.Width;
                    int y_pos = _cellSize * y + _lineTexture.Width;
                    spriteBatch.Draw(_cellTexture,
                        new Rectangle(x_pos, y_pos, drawSize, drawSize),
                        null,
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        SpriteEffects.None,
                        0);
                }
            });
        }

        private void DrawGrid(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _gameState.Size.width + 1; i++)
            {
                spriteBatch.Draw(_lineTexture,
                    new Rectangle(_cellSize * i, _cellHalfSize * _gameState.Size.height, _lineTexture.Width, _cellSize * _gameState.Size.height),
                    null,
                    Color.SlateGray,
                    0,
                    new Vector2(_lineTexture.Width / 2, _lineTexture.Height / 2),
                    SpriteEffects.None,
                    0);
            }
            for (int i = 0; i < _gameState.Size.height + 1; i++)
            {
                spriteBatch.Draw(_lineTexture,
                    new Rectangle(_cellHalfSize * _gameState.Size.width, _cellSize * i, _lineTexture.Width, _cellSize * _gameState.Size.width),
                    null,
                    Color.SlateGray,
                    _90DEG,
                    new Vector2(_lineTexture.Width / 2, _lineTexture.Height / 2),
                    SpriteEffects.None,
                    0);
            }
        }
    }
}
