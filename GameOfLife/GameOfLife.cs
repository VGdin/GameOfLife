using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOfLife
{
    public class GameOfLife : Microsoft.Xna.Framework.Game
    {

        private GameState _gameState;
        private InputHandler _inputHandler;
        private GameStateDrawer _drawer;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public GameOfLife()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Config.Instance.VisibleGameSize.widht * Config.Instance.CellSize;
            _graphics.PreferredBackBufferHeight = Config.Instance.VisibleGameSize.height * Config.Instance.CellSize + Config.Instance.StatusHeight;
            _graphics.ApplyChanges();

            _gameState = new GameState(Config.Instance.GameSize, Config.Instance.DefaultUpdateRate);
            _inputHandler = new InputHandler(_gameState);
            _drawer = new GameStateDrawer(_gameState, _inputHandler);

            Window.KeyDown += _inputHandler.HandleInput;
            Window.TextInput += _inputHandler.HandleText;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _drawer.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            _gameState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();
            _drawer.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}