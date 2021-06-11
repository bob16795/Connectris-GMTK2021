using Connect4Puzzle.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Connect4Puzzle.Graphics;
using Connect4Puzzle.Tiles;
using Connect4Puzzle.FSM;
using Connect4Puzzle.UI;

namespace Connect4Puzzle
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont arial16;

        private int frames;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = (int)Sprite.DEF_WIDTH;
            _graphics.PreferredBackBufferHeight = (int)Sprite.DEF_HEIGHT;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            arial16 = Content.Load<SpriteFont>("Arial16");

            FiniteStateMachineManager.font = arial16;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            frames++;

            if(frames % 60 == 0)
            {
                MapManager.Instance.Update(gameTime);
                frames = 0;
            }

            FiniteStateMachineManager.Instance.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if(frames++ % 60 == 0)
            {
                MapManager.Instance.DropTiles();
            }
            MapManager.Instance.Update(gameTime);
            Sprite.graphics = _graphics;
            Sprite.texture = Content.Load<Texture2D>("SpriteSheet");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: re implement
            _spriteBatch.Begin();          
            RenderMap rm = new RenderMap(Tile.Map);
            rm.Draw(_spriteBatch);
            FiniteStateMachineManager.Instance.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
