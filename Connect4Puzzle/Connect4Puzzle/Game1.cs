﻿using Connect4Puzzle.Drawing;
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

        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = (int)Sprite.DEF_WIDTH;
            _graphics.PreferredBackBufferHeight = (int)Sprite.DEF_HEIGHT;
            _graphics.ApplyChanges();
            Sprite.graphics = _graphics;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            arial16 = Content.Load<SpriteFont>("Arial16");

            FiniteStateMachineManager.font = arial16;
            UIElementsManager.font = arial16;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            FiniteStateMachineManager.Instance.Update(gameTime);
                    
            Sprite.texture = Content.Load<Texture2D>("SpriteSheet");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: re implement
            _spriteBatch.Begin();                     
            
            FiniteStateMachineManager.Instance.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
