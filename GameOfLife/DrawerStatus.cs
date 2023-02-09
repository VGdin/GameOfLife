using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameOfLife
{
    /// <summary>
    /// Class to draw the status of the game at the bottom of the screen.
    /// </summary>
    internal sealed class DrawerStatus
    {
        private SpriteFont _text;
        private Texture2D _statusBackDrop;
        private Texture2D _statusLogo;

        private readonly GameState _gameState;
        private readonly InputHandler _inputHandler;

        public DrawerStatus(GameState gameState, InputHandler inputHandler)
        {
            _gameState = gameState;
            _inputHandler = inputHandler;
        }

        public void LoadContent(ContentManager content)
        {
            _text = content.Load<SpriteFont>("GohuFont");
            _statusBackDrop = content.Load<Texture2D>("status");
            _statusLogo = content.Load<Texture2D>("logo");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            (int x, int y, int widht, int height) statusDimensions = Config.Instance.StatusDimensions;
            (int x, int y, int widht, int height) logoDimensions = Config.Instance.LogoDimensions;

            spriteBatch.Draw(_statusBackDrop,
                new Rectangle(statusDimensions.x, statusDimensions.y, statusDimensions.widht, statusDimensions.height),
                Color.White);

            spriteBatch.Draw(_statusLogo,
                new Rectangle(logoDimensions.x, logoDimensions.y, logoDimensions.widht, logoDimensions.height),
                Color.White);

            spriteBatch.DrawString(_text,
                String.Format("{0,5} | Possible: {1,8} | Cap: {2,8}\n{3}\n{4}\n{6,-6}| {5,-64}",
                Paused, UpdateRate, UpdateCap, Size, Selection, Command, Mode),
                new Vector2(statusDimensions.x + 10, statusDimensions.y + 10), Color.Black);
        }

        private string Paused => _gameState.Paused ? "Pause" : "Play";
        private string UpdateRate => String.Format("{0,6:f1}Hz", _gameState.UpdateRate == 0 ? 0 : 1 / _gameState.UpdateRate);
        private string UpdateCap => String.Format("{0,6:f1}Hz", _gameState.UpdateCap == 0 ? 0 : 1 / _gameState.UpdateCap);
        private string Size => "Size: " + Config.Instance.GameSize;
        private string Selection => "Sel: " + _gameState.CurrentSelection;
        private string Mode => _inputHandler.CurrentMode switch
        {
            InputHandler.Modes.Normal => "NORMAL",
            InputHandler.Modes.Input => "INPUT",
            InputHandler.Modes.Visual => "VISUAl",
            _ => throw new NotImplementedException("Status drawer not implemented for current mode: " + _inputHandler.CurrentMode)
        };
        private string Command => _inputHandler.CurrentMode == InputHandler.Modes.Input ? _inputHandler.CurrentCommand : "";
    }
}
