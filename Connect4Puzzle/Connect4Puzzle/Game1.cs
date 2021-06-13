using Connect4Puzzle.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Connect4Puzzle.Graphics;
using Connect4Puzzle.Tiles;
using Connect4Puzzle.FSM;
using Connect4Puzzle.UI;
using Connect4Puzzle.Music;
using Connect4Puzzle.Input;

namespace Connect4Puzzle
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont arial16;

        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Window.AllowUserResizing = true;
            _graphics.PreferredBackBufferWidth = (int)Sprite.DEF_WIDTH;
            _graphics.PreferredBackBufferHeight = (int)Sprite.DEF_HEIGHT;
            Window.Title = "Connectris";
            _graphics.ApplyChanges();
            Sprite.graphics = _graphics;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            arial16 = Content.Load<SpriteFont>("Arial16");
            Sprite.texture = Content.Load<Texture2D>("SpriteSheet");
            FiniteStateMachineManager.font = arial16;
            UIElementsManager.font = arial16;
            SoundManager.Instance.content = Content;
            SoundManager.Instance.LoadContent();

            FiniteStateMachineManager.Instance.bgTexture = Content.Load<Texture2D>("Backdrop");
            FiniteStateMachineManager.Instance.tutTexture = Content.Load<Texture2D>("Tutorial");
        }

        protected override void Update(GameTime gameTime)
        {
            if (_graphics.PreferredBackBufferWidth != Window.ClientBounds.Width || _graphics.PreferredBackBufferHeight != Window.ClientBounds.Height)
            {
                _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                _graphics.ApplyChanges();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (InputManager.Instance.TrackInput().Contains(Direction.FS)) {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }
            FiniteStateMachineManager.Instance.Update(gameTime);
            if (FiniteStateMachineManager.Instance.CurrentState == GameState.GAME && (MapManager.Instance.lost || MapManager.Instance.Score < 0))
            {
                FiniteStateMachineManager.Instance.CurrentState = GameState.GAME_OVER;
                SoundManager.Instance.StopMusic();
                SoundManager.Instance.PlaySFX("gameover");
                UIElementsManager.menuButton.IsActive = false;
                UIElementsManager.ScoreText.IsActive = false;
            }
                
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: re implement
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            FiniteStateMachineManager.Instance.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
