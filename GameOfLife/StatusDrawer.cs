﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameOfLife
{
    public sealed class StatusDrawer
    {
        private SpriteFont _text;
        private Texture2D _statusBackDrop;

        private readonly GameState _gameState;
        private readonly InputHandler _inputHandler;

        public StatusDrawer(GameState gameState, InputHandler inputHandler)
        {
            _gameState = gameState;
            _inputHandler = inputHandler;
        }


        public void LoadContent(ContentManager content)
        {
            _text = content.Load<SpriteFont>("GohuFont");
            _statusBackDrop = content.Load<Texture2D>("status");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            (int x, int y, int widht, int height) statusDimensions = Config.Instance.StatusDimensions;
            spriteBatch.Draw(_statusBackDrop,
                new Rectangle(statusDimensions.x, statusDimensions.y, statusDimensions.widht, statusDimensions.height),
                Color.White);

            spriteBatch.DrawString(_text,
                String.Format("{0,6} | {1,6} | {2,14} | {3,14} |" +
                "\n{4,-64}",
                paused, updateRate, size, selection, command),
                new Vector2(statusDimensions.x + 10, statusDimensions.y + 10), Color.Black);
        }

        private string paused => _gameState.Paused ? "Pause" : "Play";
        private string updateRate => String.Format("{0,4:f1}Hz", 1 / _gameState.UpdateRate);
        private string size => "Size: " + Config.Instance.GameSize;
        private string selection => "Sel: " + _gameState.CurrentSelection;
        private string command => _inputHandler.EditMode ? ": " + _inputHandler.CurrentCommand : "";

    }
}
