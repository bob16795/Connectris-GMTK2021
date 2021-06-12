using System;
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

        public static readonly Lazy<WinManager>
            win = new Lazy<WinManager>(() => new WinManager());

        public static WinManager Instance { get { return win.Value; } }

        //properties


        //ctor
        public WinManager()
        {
            direction = default;
            numSearched = 0;

            direction = Directions.NONE;

        }

        //methods
        /// <summary>
        /// performs breadth first search to find 
        /// potential four in a rows
        /// </summary>
        public List<Tile> BFSearch(Tile t)
        {
            Tile initialTile = t;

            if(t  == default)
            {
                return null;
            }

            ResetBooleans();
            Stack<Tile> tileQueue = new Stack<Tile>();
            Point location = t.Position;



            tileQueue.Push(t);

            while(numSearched < 3)
            {
                Tile currentVertex = null;

                if (tileQueue.Count > 0)
                {
                    currentVertex = tileQueue.Peek();
                    location = currentVertex.Position;
                }

                if (direction == Directions.NONE && isTileValid(currentVertex))
                {
                    if (CheckInBounds(location.X + 1, location.Y) &&
                        Tile.Map[location.X + 1, location.Y].Type == initialTile.Type)
                    {
                        tileQueue.Push(Tile.Map[location.X + 1, location.Y]);
                        direction = Directions.EAST;
                    }
                    else if (CheckInBounds(location.X - 1, location.Y)
                        && Tile.Map[location.X - 1, location.Y].Type == initialTile.Type)
                    {
                        tileQueue.Push(Tile.Map[location.X - 1, location.Y]);
                        direction = Directions.WEST;
                    }
                    else if (CheckInBounds(location.X, location.Y - 1)
                        && Tile.Map[location.X, location.Y - 1].Type == initialTile.Type)
                    {
                        tileQueue.Push(Tile.Map[location.X, location.Y - 1]);
                        direction = Directions.NORTH;
                    }
                    else if (CheckInBounds(location.X, location.Y + 1)
                        && Tile.Map[location.X, location.Y + 1].Type == initialTile.Type)
                    {
                        tileQueue.Push(Tile.Map[location.X, location.Y + 1]);
                        direction = Directions.SOUTH;
                    }
                    else if (CheckInBounds(location.X - 1, location.Y - 1)
                        && Tile.Map[location.X - 1, location.Y - 1].Type == initialTile.Type)
                    {
                        tileQueue.Push(Tile.Map[location.X - 1, location.Y - 1]);
                        direction = Directions.NW;
                    }
                    else if (CheckInBounds(location.X - 1, location.Y + 1) &&
                        Tile.Map[location.X - 1, location.Y + 1].Type == initialTile.Type)
                    {
                        tileQueue.Push(Tile.Map[location.X - 1, location.Y + 1]);
                        direction = Directions.SW;
                    }
                    else if (CheckInBounds(location.X + 1, location.Y - 1) &&
                        Tile.Map[location.X + 1, location.Y - 1].Type == initialTile.Type)
                    {
                        tileQueue.Push(Tile.Map[location.X + 1, location.Y - 1]);
                        direction = Directions.NE;
                    }
                    else if (CheckInBounds(location.X + 1, location.Y + 1) &&
                        Tile.Map[location.X + 1, location.Y + 1].Type == initialTile.Type)
                    {
                        tileQueue.Push(Tile.Map[location.X + 1, location.Y + 1]);
                        direction = Directions.SE;
                    }
                    else
                    {
                        return null;
                    }
                    numSearched++;
                }
                else if (direction != Directions.NONE && isTileValid(currentVertex))
                {
                    numSearched++;

                    switch (direction)
                    {
                        case Directions.NORTH:
                            if (CheckInBounds(location.X, location.Y - 1) && isTileValid(Tile.Map[location.X, location.Y - 1]) &&
                                Tile.Map[location.X, location.Y - 1].Type == initialTile.Type)
                            {
                                tileQueue.Push(Tile.Map[location.X, location.Y - 1]);
                            }
                            else
                            {
                                return null;
                            }
                            break;

                        case Directions.SOUTH:
                            if (CheckInBounds(location.X, location.Y + 1) && isTileValid(Tile.Map[location.X, location.Y + 1]) &&
                                Tile.Map[location.X, location.Y + 1].Type == initialTile.Type)
                            {
                                tileQueue.Push(Tile.Map[location.X, location.Y + 1]);
                            }
                            else
                            {
                                return null;
                            }
                            break;

                        case Directions.WEST:
                            if (CheckInBounds(location.X - 1, location.Y) && isTileValid(Tile.Map[location.X - 1, location.Y]) &&
                                Tile.Map[location.X - 1, location.Y].Type == initialTile.Type)
                            {
                                tileQueue.Push(Tile.Map[location.X - 1, location.Y]);
                            }
                            else
                            {
                                return null;
                            }
                            break;

                        case Directions.EAST:
                            if (CheckInBounds(location.X + 1, location.Y) && isTileValid(Tile.Map[location.X + 1, location.Y]) &&
                                Tile.Map[location.X + 1, location.Y].Type == initialTile.Type)
                            {
                                tileQueue.Push(Tile.Map[location.X + 1, location.Y]);
                            }
                            else
                            {
                                return null;
                            }
                            break;

                        case Directions.NW:
                            if (CheckInBounds(location.X - 1, location.Y - 1) && isTileValid(Tile.Map[location.X - 1, location.Y - 1]) &&
                                Tile.Map[location.X - 1, location.Y - 1].Type == initialTile.Type)
                            {
                                tileQueue.Push(Tile.Map[location.X - 1, location.Y - 1]);
                            }
                            else
                            {
                                return null;
                            }
                            break;

                        case Directions.SW:
                            if (CheckInBounds(location.X - 1, location.Y + 1) && isTileValid(Tile.Map[location.X - 1, location.Y + 1]) &&
                                Tile.Map[location.X - 1, location.Y + 1].Type == initialTile.Type)
                            {
                                tileQueue.Push(Tile.Map[location.X - 1, location.Y + 1]);
                            }
                            else
                            {
                                return null;
                            }
                            break;

                        case Directions.NE:
                            if (CheckInBounds(location.X + 1, location.Y - 1) && 
                                isTileValid(Tile.Map[location.X + 1, location.Y - 1]) &&
                                Tile.Map[location.X + 1, location.Y - 1].Type == initialTile.Type)
                            {
                                tileQueue.Push(Tile.Map[location.X + 1, location.Y - 1]);
                            }
                            else
                            {
                                return null;
                            }
                            break;

                        case Directions.SE:
                            if (CheckInBounds(location.X + 1, location.Y + 1) &&
                                isTileValid(Tile.Map[location.X + 1, location.Y + 1]) &&
                                Tile.Map[location.X + 1, location.Y + 1].Type == initialTile.Type)
                            {
                                tileQueue.Push(Tile.Map[location.X + 1, location.Y + 1]);
                            }
                            else
                            {
                                return null;
                            }
                            break;
                    }
                }
                else
                {
                    return null;
                }
            }


            List<Tile> tiles = new List<Tile>();
            tiles.AddRange(tileQueue);

            if (tiles.Count == 4)
            {
                return tiles;
            }
            else
            {
                return null;
            }
            
            
        }

        /// <summary>
        /// resets the boolean array
        /// </summary>
        public void ResetBooleans()
        {
            direction = Directions.NONE;
            numSearched = 0;
        }

        public bool isTileValid(Tile t)
        {
            if(t != null)
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
    }
}
