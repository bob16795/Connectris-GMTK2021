using System;
using System.Collections.Generic;
using System.Text;

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

        private GameState currentState;

        /// <summary>
        /// Creates a new Finite state manager object
        /// </summary>
        public FiniteStateMachineManager() 
        {
            this.currentState = GameState.MAIN_MENU;
        }

        /// <summary>
        /// Draws assets to the screen depending on the state of the game
        /// </summary>
        public void Draw()
        {
            switch (currentState)
            {
                case GameState.MAIN_MENU:
                    break;
                case GameState.INSTRUCTIONS:
                    break;
                case GameState.MENU:
                    break;
                case GameState.GAME:
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
        public void Update()
        {
            SwitchState();

            switch (currentState)
            {
                case GameState.MAIN_MENU:
                    break;
                case GameState.INSTRUCTIONS:
                    break;
                case GameState.MENU:
                    break;
                case GameState.GAME:
                    break;
                case GameState.GAME_OVER:
                    break;
                case GameState.WIN:
                    break;
            }
        }

        /// <summary>
        /// Switches states given a condition
        /// </summary>
        public void SwitchState()
        {

        }
    }
}
