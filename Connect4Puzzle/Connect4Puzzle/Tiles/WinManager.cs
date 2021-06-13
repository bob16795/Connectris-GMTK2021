﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Connect4Puzzle.Tiles
{
    //HEADER====================================================
    //names: sciencedoge, prestosilver
    //date: 6/11/2021
    //purpose: tracks four in a row connections between tiles
    //==========================================================

    public enum Directions
    {
        NORTH,
        SOUTH,
        EAST,
        WEST,
        NW,
        NE,
        SW,
        SE,
        NONE
    }
    class WinManager
    {
        //fields
        private Directions direction;
        private int numSearched;
        private Random random;

        private List<List<Tile>> removedTiles;

        private bool canSearchNorth;
        private bool canSearchWest;
        private bool canSearchNW;
        private bool canSearchSW;
        private bool canSearchSouth;
        private bool canSearchSouthEast;
        private bool canSearchNE;
        private bool canSearchEast;


        public static readonly Lazy<WinManager>
            win = new Lazy<WinManager>(() => new WinManager());

        /// <summary>
        /// Returns singleton instance
        /// </summary>
        public static WinManager Instance { get { return win.Value; } }

        //properties


        //ctor

        /// <summary>
        /// Creates a win manager object 
        /// </summary>
        public WinManager()
        {
            direction = default;
            numSearched = 0;

            random = new Random();
            direction = Directions.NONE;

            canSearchNorth = false;
            canSearchWest= false;
            canSearchNW = false;
            canSearchSW = false;
            canSearchSouth = false;
            canSearchSouthEast = false;
            canSearchNE = false;

            removedTiles = new List<List<Tile>>();
            removedTiles.Capacity = 10;

        }

        //methods
        /// <summary>
        /// performs breadth first search to find 
        /// potential four in a rows
        /// </summary>
        public List<Tile> BFSearch(Tile t)
        {
            Tile initialTile = t;

            if(t  == default || t.controlled)
            {
                return null;
            }

            ResetBooleans();
            Stack<Tile> tileQueue = new Stack<Tile>();
            Point location = t.Position;


            //pushes the first tile to the queue
            tileQueue.Push(t);

            SearchForDirections(location, tileQueue, initialTile);
            

            //adds sequence of tiles to list
            List<Tile> tiles = new List<Tile>();
            List<Tile> reds = new List<Tile>();
            tiles.AddRange(tileQueue);

            
            if (tiles.Count >= 4)
            {
                foreach(Tile tile in Tile.Map)
                {
                    if(tile.Type == TileType.RED_TILE && tile.controlled == false)
                    {
                        reds.Add(tile);
                    }
                }

                if (reds.Count <= 4)
                    return tiles;

                for(int i = 0; i < 4; i++)
                {
                    int index = random.Next(0, reds.Count);

                    reds[index].Remove(false);
                }
                return tiles;
            }

            return null;
        }

        /// <summary>
        /// resets the boolean array
        /// </summary>
        public void ResetBooleans()
        {
            direction = Directions.NONE;
            numSearched = 0;
            canSearchWest = true;
            canSearchSW = true;
            canSearchNW = true;
            canSearchNorth = true;
            canSearchSouth = true;
            canSearchSouthEast = true;
        }

        public bool isTileValid(Tile t)
        {
            if(t != null && !t.controlled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// checks if a tile is in bounds
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool CheckInBounds(int x, int y)
        {
            Rectangle r = new Rectangle(0, 0, 8, 20);

            if (r.Contains(new Point(x, y)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SearchForDirections(Point location, Stack<Tile> tileQueue, Tile initialTile)
        {
            //loops until three MORE tiles are searched
            while (numSearched < 3)
            {
                Tile currentVertex = null;

                //peeks to the first tile in the stack
                if (tileQueue.Count > 0)
                {
                    currentVertex = initialTile;
                    location = currentVertex.Position;
                }

                removedTiles.Add(new List<Tile>());

                int initialStackCapacity = tileQueue.Count;

                //finds direction needed to search
                if (direction == Directions.NONE && isTileValid(currentVertex))
                {
                    if (CheckInBounds(location.X - 1, location.Y)
                        && Tile.Map[location.X - 1, location.Y].Type == initialTile.Type
                        && canSearchWest)
                    {
                        removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X - 1, location.Y]);
                        direction = Directions.WEST;
                        tileQueue.Push(Tile.Map[location.X - 1, location.Y]);
                    }
                    else if (CheckInBounds(location.X, location.Y - 1)
                        && Tile.Map[location.X, location.Y - 1].Type == initialTile.Type
                        && canSearchNorth)
                    {
                        removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X, location.Y - 1]);
                        direction = Directions.NORTH;
                        tileQueue.Push(Tile.Map[location.X, location.Y - 1]);
                    }
                    else if (CheckInBounds(location.X, location.Y + 1)
                        && Tile.Map[location.X, location.Y + 1].Type == initialTile.Type
                        && canSearchSouth)
                    {
                        removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X, location.Y + 1]);
                        direction = Directions.SOUTH;
                        tileQueue.Push(Tile.Map[location.X, location.Y + 1]);
                    }
                    else if (CheckInBounds(location.X + 1, location.Y + 1)
                        && Tile.Map[location.X + 1, location.Y + 1].Type == initialTile.Type
                        && canSearchSouthEast)
                    {
                        removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X + 1, location.Y + 1]);
                        direction = Directions.SE;
                        tileQueue.Push(Tile.Map[location.X + 1, location.Y + 1]);
                    }
                    else if (CheckInBounds(location.X - 1, location.Y - 1)
                        && Tile.Map[location.X - 1, location.Y - 1].Type == initialTile.Type
                        && canSearchNW)
                    {
                        removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X - 1, location.Y - 1]);
                        direction = Directions.NW;
                        tileQueue.Push(Tile.Map[location.X - 1, location.Y - 1]);
                    }
                    else if (CheckInBounds(location.X - 1, location.Y + 1) &&
                        Tile.Map[location.X - 1, location.Y + 1].Type == initialTile.Type
                        && canSearchSW)
                    {
                        removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X - 1, location.Y + 1]);
                        direction = Directions.SW;
                        tileQueue.Push(Tile.Map[location.X - 1, location.Y + 1]);
                    }
                    else if (CheckInBounds(location.X + 1, location.Y - 1) &&
                        Tile.Map[location.X + 1, location.Y - 1].Type == initialTile.Type
                        && canSearchNE)
                    {
                        removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X + 1, location.Y - 1]);
                        direction = Directions.NE;
                        tileQueue.Push(Tile.Map[location.X + 1, location.Y - 1]);
                    }
                    else
                    {
                        return;
                    }
                    numSearched++;
                }

                //keeps searching in that direction
                else if (direction != Directions.NONE && isTileValid(currentVertex))
                {
                    switch (direction)
                    {
                        case Directions.NORTH:
                            if (CheckInBounds(location.X, location.Y - 1) && isTileValid(Tile.Map[location.X, location.Y - 1]) &&
                                Tile.Map[location.X, location.Y - 1].Type == initialTile.Type
                                && canSearchNorth)
                            {
                                removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X, location.Y - 1]);
                                tileQueue.Push(Tile.Map[location.X, location.Y - 1]);
                            }
                            break;

                        case Directions.SOUTH:
                            if (CheckInBounds(location.X, location.Y + 1) && isTileValid(Tile.Map[location.X, location.Y + 1]) &&
                                Tile.Map[location.X, location.Y + 1].Type == initialTile.Type
                                && canSearchSouth)
                            {
                                removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X, location.Y + 1]);
                                tileQueue.Push(Tile.Map[location.X, location.Y + 1]);
                            }
                            break;


                        case Directions.WEST:
                            if (CheckInBounds(location.X - 1, location.Y) && isTileValid(Tile.Map[location.X - 1, location.Y]) &&
                                Tile.Map[location.X - 1, location.Y].Type == initialTile.Type
                                && canSearchWest)
                            {
                                removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X - 1, location.Y]);
                                tileQueue.Push(Tile.Map[location.X - 1, location.Y]);
                            }
                            break;

                        case Directions.NW:
                            if (CheckInBounds(location.X - 1, location.Y - 1) && isTileValid(Tile.Map[location.X - 1, location.Y - 1]) &&
                                Tile.Map[location.X - 1, location.Y - 1].Type == initialTile.Type
                                && canSearchNW)
                            {
                                removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X - 1, location.Y - 1]);
                                tileQueue.Push(Tile.Map[location.X - 1, location.Y - 1]);
                            }
                            break;

                        case Directions.SE:
                            if (CheckInBounds(location.X + 1, location.Y + 1) && isTileValid(Tile.Map[location.X + 1, location.Y + 1]) &&
                                Tile.Map[location.X + 1, location.Y + 1].Type == initialTile.Type
                                && canSearchSouthEast)
                            {
                                removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X + 1, location.Y + 1]);
                                tileQueue.Push(Tile.Map[location.X + 1, location.Y + 1]);
                            }
                            break;

                        case Directions.SW:
                            if (CheckInBounds(location.X - 1, location.Y + 1) && isTileValid(Tile.Map[location.X - 1, location.Y + 1]) &&
                                Tile.Map[location.X - 1, location.Y + 1].Type == initialTile.Type
                                && canSearchSW)
                            {
                                removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X - 1, location.Y + 1]);
                                tileQueue.Push(Tile.Map[location.X - 1, location.Y + 1]);
                            }
                            break;

                        case Directions.NE:
                            if (CheckInBounds(location.X + 1, location.Y - 1) && isTileValid(Tile.Map[location.X + 1, location.Y - 1]) &&
                                Tile.Map[location.X + 1, location.Y - 1].Type == initialTile.Type
                                && canSearchNE)
                            {
                                removedTiles[tileQueue.Count - 1].Add(Tile.Map[location.X + 1, location.Y - 1]);
                                tileQueue.Push(Tile.Map[location.X + 1, location.Y - 1]);
                            }
                            break;

                        default:
                            tileQueue.Pop();

                            switch (direction)
                            {
                                case Directions.NORTH:
                                    canSearchNorth = false;
                                    break;
                                case Directions.WEST:
                                    canSearchWest = false;
                                    break;
                                case Directions.NW:
                                    canSearchNW = false;
                                    break;
                                case Directions.SW:
                                    canSearchSW = false;
                                    break;
                                case Directions.SOUTH:
                                    canSearchSouth = false;
                                    break;
                                case Directions.NE:
                                    canSearchNE= false;
                                    break;
                                case Directions.SE:
                                    canSearchSouthEast = false;
                                    break;
                                default:
                                    return;
                            }

                            direction = Directions.NONE;
                            break;
                    }
                    numSearched++;
                }

                if(tileQueue.Count > 0 && tileQueue.Count < 4
                    && initialStackCapacity != tileQueue.Count)
                {
                    SearchForDirections(location, tileQueue, tileQueue.Peek());
                }

                //nothing found, return null
                else
                {
                    return;
                }
            }
        }
    }
}
