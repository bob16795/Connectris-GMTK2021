using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Connect4Puzzle.UI;

namespace Connect4Puzzle.Input
{
    //Header=================================
    //Names: sciencedoge, prestosilver
    //Date: 6/11/2021
    //Purpose: handles all input in the game
    //=======================================

    /// <summary>
    /// Directions of input
    /// </summary>
    public enum Direction
    {
        LEFT,
        RIGHT,
        DOWN
    }

    class InputManager
    {
        //Singleton
        private static readonly Lazy<InputManager>
            input = new Lazy<InputManager>(() => new InputManager());

        /// <summary>
        /// returns the instance of map manager
        /// </summary>
        public static InputManager Instance { get { return input.Value; } }

        //Fields
        private KeyboardState kb;
        private KeyboardState prevkb;

        private MouseState ms;
        private MouseState prevMs;

        //Constructor

        /// <summary>
        /// Creates an InputManager object class
        /// </summary>
        public InputManager() { }

        /// <summary>
        /// Tracks user input
        /// </summary>
        public List<Direction> TrackInput()
        {
            kb = Keyboard.GetState();
            ms = Mouse.GetState();

            List<Direction> dir = new List<Direction>();

            if ((kb.IsKeyDown(Keys.Right))
                || (kb.IsKeyDown(Keys.D)))
            {
                dir.Add(Direction.RIGHT);
            }

            if ((kb.IsKeyDown(Keys.Left))
                || (kb.IsKeyDown(Keys.A)))
            {
                dir.Add(Direction.LEFT);
            }

            if ((kb.IsKeyDown(Keys.Down))
                || (kb.IsKeyDown(Keys.S)))
            {
                dir.Add(Direction.DOWN);
            }

            if(ms.LeftButton == ButtonState.Pressed && SingleMousePress())
            {
                UIManager.Instance.ProcessClick(ms.Position);
            }

            prevkb = kb;
            prevMs = ms;

            return dir;
        }

        /// <summary>
        /// Checks for a single mouse press
        /// </summary>
        /// <returns></returns>
        private bool SingleMousePress()
        {
            if(prevMs.LeftButton == ButtonState.Pressed)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
