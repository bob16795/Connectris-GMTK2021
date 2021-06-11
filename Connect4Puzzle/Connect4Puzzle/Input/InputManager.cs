﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        DOWN,
        NONE
    }

    class InputManager
    {
        //Singleton
        private static readonly Lazy<InputManager>
            input = new Lazy<InputManager>(() => new InputManager());

        //Fields
        private KeyboardState kb;
        private KeyboardState prevkb;

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

            List<Direction> dir = new List<Direction>();

            if ((kb.IsKeyDown(Keys.Right)
                && SingleKeyPress(Keys.Right))
                || (kb.IsKeyDown(Keys.D) && SingleKeyPress(Keys.D)))
            {
                dir.Add(Direction.RIGHT);
            }

            if ((kb.IsKeyDown(Keys.Left)
                && SingleKeyPress(Keys.Left))
                || (kb.IsKeyDown(Keys.A) && SingleKeyPress(Keys.A)))
            {
                dir.Add(Direction.LEFT);
            }

            if ((kb.IsKeyDown(Keys.Down)
                && SingleKeyPress(Keys.Down))
                || (kb.IsKeyDown(Keys.S) && SingleKeyPress(Keys.S)))
            {
                dir.Add(Direction.DOWN);
            }

            prevkb = kb;

            return dir;
        }

        /// <summary>
        /// Checks if a single key press occurs
        /// </summary>
        /// <returns>True - single key press
        /// false - multiple frames key press</returns>
        public bool SingleKeyPress(Keys k)
        {
            //Key k was down the previous frame, but is now up
            if (prevkb.IsKeyUp(k))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
