using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Connect4Puzzle.UI;
using Connect4Puzzle.Graphics;
using Connect4Puzzle.Tiles;
using Connect4Puzzle.Drawing;
using Connect4Puzzle.Music;
using Connect4Puzzle.Input;

namespace Connect4Puzzle.FSM
{
    //Header=========================================
    //Names: sciencedoge, prestosilver
    //Date: 6/11/2021
    //Purpose: Handles all state changes in the game
    //===============================================

    /// <summary>
    /// Describes the various gameStates of the game
    /// </summary>
    public enum GameState
    {
        MAIN_MENU,
        MENU,
        GAME,
        INSTRUCTIONS,
        WIN,
        GAME_OVER,
        CREDITS
    }

    class FiniteStateMachineManager
    {
        //Singleton
        public static readonly Lazy<FiniteStateMachineManager>
            fsmManager = new Lazy<FiniteStateMachineManager>(() => new FiniteStateMachineManager());

        /// <summary>
        /// returns the instance of the finite state machine manager
        /// </summary>
        public static FiniteStateMachineManager Instance { get { return fsmManager.Value; } }

        private GameState currentState;
        public static SpriteFont font;
        public Texture2D bgTexture, tutTexture;

        private Sprite titleSprite, pausedSprite, gameOverSprite;
        private RenderMap rm;
        private int frames;

        private UIElementsManager uiMan;

        /// <summary>
        /// gets or sets currentState
        /// </summary>
        public GameState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }


        /// <summary>
        /// Creates a new Finite state manager object
        /// </summary>
        public FiniteStateMachineManager() 
        {
            this.currentState = GameState.MAIN_MENU;
            uiMan = new UIElementsManager();

            frames = 0;

            rm = new RenderMap();

            titleSprite = new Sprite(new Rectangle(22, 0, 116, 22), new Vector2(0, 0), Color.White);
            gameOverSprite = new Sprite(new Rectangle(24, 43, 111, 23), new Vector2(0, 0), Color.White);
            pausedSprite = new Sprite(new Rectangle(24, 22, 76, 21), new Vector2(0, 0), Color.White);
        }

        /// <summary>
        /// Draws assets to the screen depending on the state of the game
        /// </summary>
        public void Draw(SpriteBatch sb, GameTime gt)
        {
            sb.Draw(bgTexture, new Vector2(0), Color.White);
            switch (currentState)
            {                
                case GameState.MAIN_MENU:
                    titleSprite.Draw(sb, new Point((int)((Sprite.DEF_WIDTH - 116 * 4) / 2), 100), 0, new Vector2(116, 22) * 4);                  
                    break;
                case GameState.INSTRUCTIONS:
                    Point pos = new Point((int)((Sprite.DEF_WIDTH * Sprite.GetScale() - tutTexture.Bounds.Width) / 2),(int)(Sprite.DEF_HEIGHT * Sprite.GetScale() - tutTexture.Bounds.Height) / 2);
                    sb.Draw(tutTexture, new Rectangle(0, 0, Sprite.graphics.PreferredBackBufferWidth, Sprite.graphics.PreferredBackBufferHeight), Color.White);
                    break;
                case GameState.MENU:
                    pausedSprite.Draw(sb, new Point((int)((Sprite.DEF_WIDTH - 76 * 4) / 2), 100), 0, new Vector2(76, 21) * 4);                  
                    break;
                case GameState.GAME:
                    rm.Draw(sb);
                    Tile.ps.Draw(sb);
                    break;
                case GameState.GAME_OVER:
                    gameOverSprite.Draw(sb, new Point((int)((Sprite.DEF_WIDTH - 111 * 4) / 2), 100), 0, new Vector2(111, 23) * 4);                  
                    break;
            }
            UIManager.Instance.Draw(gt, sb);
        }

        /// <summary>
        /// Updates assets depending on the state of the game
        /// </summary>
        public void Update(GameTime gt)
        {
            UIManager.Instance.Update(gt);
            InputManager.Instance.TrackInput();

            switch (currentState)
            {
                case GameState.MAIN_MENU:
                    SoundManager.Instance.PlayMusic("menu");
                    UIElementsManager.playButton.IsActive = true;
                    UIElementsManager.playButton.Text.Text = "Start";
                    break;
                case GameState.INSTRUCTIONS:
                    UIElementsManager.nextButton.IsActive = true;
                    break;
                case GameState.MENU:
                    SoundManager.Instance.PlayMusic("pause");
                    UIElementsManager.okButton.IsActive = true;
                    break;
                case GameState.GAME:
                    MapManager.Instance.Update(gt);
                    SoundManager.Instance.PlayMusic("game");
                    SoundManager.Instance.ResumeMusic();
                    
                    if (frames++ % MapManager.Instance.Speed == 0)
                    {
                        MapManager.Instance.DropTiles();
                    }
                    Tile.ps.Update(gt);
                    
                    
                    break;
                case GameState.GAME_OVER:
                    UIElementsManager.ScoreText2.IsActive = true;
                    UIElementsManager.playButton.IsActive = true;
                    UIElementsManager.playButton.Text.Text = "Play Again";
                    break;
                case GameState.WIN:
                    break;
            }
        } 
        
        public void Reset()
        {
            
            MapManager.Instance.Score = 0;

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Tile.Map[x, y] = new Tile(new Point(x, y));
                    Tile.ps = new ParticleSystem();
                }
            }
        }
    }
}
