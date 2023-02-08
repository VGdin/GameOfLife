using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOfLife
{
    public class GameOfLife : Microsoft.Xna.Framework.Game
    {

        private readonly GraphicsDeviceManager _graphics;

        private GameState _gameState;
        private InputHandler _inputHandler;
        
        private SpriteBatch _statusSpriteBatch;
        private StatusDrawer _statusDrawer;

        private SpriteBatch _gameSpriteBatch;
        private GameStateDrawer _gameStateDrawer;
        private Camera _camera;

        public GameOfLife()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Config.Instance.DefaultResolution.widht;
            _graphics.PreferredBackBufferHeight = Config.Instance.DefaultResolution.height;
            _graphics.ApplyChanges();

            _gameState = new GameState();
            _camera= new Camera();
            _gameStateDrawer = new GameStateDrawer(_gameState, _camera);
            _inputHandler = new InputHandler(Window, _gameState, _camera);
            _statusDrawer = new StatusDrawer(_gameState, _inputHandler);

            Window.KeyDown += _inputHandler.HandleInput;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _gameSpriteBatch = new SpriteBatch(GraphicsDevice);
            _statusSpriteBatch = new SpriteBatch(GraphicsDevice);
            _gameStateDrawer.LoadContent(Content);
            _statusDrawer.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            _gameState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _gameSpriteBatch.Begin(SpriteSortMode.Deferred,null,null,null,null,null,_camera.TranslationMatrix);
            _gameStateDrawer.Draw(_gameSpriteBatch);
            _gameSpriteBatch.End();

            _statusSpriteBatch.Begin();
            _statusDrawer.Draw(_statusSpriteBatch);
            _statusSpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}