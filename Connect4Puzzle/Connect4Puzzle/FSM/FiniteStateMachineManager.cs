using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Connect4Puzzle.UI;
using Connect4Puzzle.Graphics;
using Connect4Puzzle.Tiles;
using Connect4Puzzle.Drawing;

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
        GAME_OVER
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

        private UIButton playButton;

        private RenderMap rm;
        private int frames;

        /// <summary>
        /// Creates a new Finite state manager object
        /// </summary>
        public FiniteStateMachineManager() 
        {
            this.currentState = GameState.MAIN_MENU;
            playButton = new UIButton(font,
                new Rectangle((Sprite.graphics.PreferredBackBufferWidth / 2) - 130, 
                (2 * Sprite.graphics.PreferredBackBufferHeight / 3) - 100,
                200, 100));

            frames = 0;

            rm = new RenderMap(Tile.Map);
            playButton.Text.Text = "Play Game";
            playButton.onClick = new UIAction((i) =>
            {
                System.Diagnostics.Debug.WriteLine("test");
                currentState = GameState.INSTRUCTIONS;
            });

            UIManager.Instance.Add(playButton);
        }

        /// <summary>
        /// Draws assets to the screen depending on the state of the game
        /// </summary>
        public void Draw(SpriteBatch sb, GameTime gt)
        {
            UIManager.Instance.Draw(gt, sb);

            switch (currentState)
            {                
                case GameState.MAIN_MENU:                    
                    break;
                case GameState.INSTRUCTIONS:
                    break;
                case GameState.MENU:
                    break;
                case GameState.GAME:
                    rm.Draw(sb);
                    break;
                case GameState.GAME_OVER:
                    break;
                case GameState.WIN:
                    break;

            }
        }

        /// <summary>
        /// Updates assets depending on the state of the game
        /// </summary>
        public void Update(GameTime gt)
        {
            UIManager.Instance.Update(gt);
            MapManager.Instance.Update(gt);

            switch (currentState)
            {
                case GameState.MAIN_MENU:
                    playButton.IsActive = true;
                    break;
                case GameState.INSTRUCTIONS:
                    playButton.IsActive = false;
                    break;
                case GameState.MENU:
                    break;
                case GameState.GAME:
                    if (frames++ % 15 == 0)
                    {
                        MapManager.Instance.DropTiles();
                    }          
                    break;
                case GameState.GAME_OVER:
                    break;
                case GameState.WIN:
                    break;
            }
        }   
    }
}
