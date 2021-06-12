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

        public WinManager Instance { get { return win.Value; } }

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
            if(t  == default)
            {
                return null;
            }

            ResetBooleans();
            Queue<Tile> tileQueue = new Queue<Tile>();

            Point location = t.Position;

            tileQueue.Enqueue(t);

            while(numSearched < 4)
            {
                Tile currentVertex = null;

                if(tileQueue.Count > 0)
                {
                    currentVertex = tileQueue.Peek();
                    location = currentVertex.Position;
                }
                               
                if(direction == default && isTileValid(currentVertex))
                {
                    try
                    {
                        if (Tile.Map[location.X + 1, location.Y].Type == TileType.GREEN_TILE)
                        {
                            tileQueue.Enqueue(Tile.Map[location.X + 1, location.Y]);
                            direction = Directions.EAST;
                        }
                        else if (Tile.Map[location.X - 1, location.Y].Type == TileType.GREEN_TILE)
                        {
                            tileQueue.Enqueue(Tile.Map[location.X - 1, location.Y]);
                            direction = Directions.WEST;
                        }
                        else if (Tile.Map[location.X, location.Y - 1].Type == TileType.GREEN_TILE)
                        {
                            tileQueue.Enqueue(Tile.Map[location.X, location.Y - 1]);
                            direction = Directions.NORTH;
                        }
                        else if (Tile.Map[location.X, location.Y + 1].Type == TileType.GREEN_TILE)
                        {
                            tileQueue.Enqueue(Tile.Map[location.X, location.Y + 1]);
                            direction = Directions.SOUTH;
                        }
                        else if (Tile.Map[location.X - 1, location.Y - 1].Type == TileType.GREEN_TILE)
                        {
                            tileQueue.Enqueue(Tile.Map[location.X - 1, location.Y - 1]);
                            direction = Directions.NW;
                        }
                        else if (Tile.Map[location.X - 1, location.Y + 1].Type == TileType.GREEN_TILE)
                        {
                            tileQueue.Enqueue(Tile.Map[location.X - 1, location.Y + 1]);
                            direction = Directions.SW;
                        }
                        else if (Tile.Map[location.X + 1, location.Y - 1].Type == TileType.GREEN_TILE)
                        {
                            tileQueue.Enqueue(Tile.Map[location.X + 1, location.Y - 1]);
                            direction = Directions.NE;
                        }
                        else if (Tile.Map[location.X + 1, location.Y + 1].Type == TileType.GREEN_TILE)
                        {
                            tileQueue.Enqueue(Tile.Map[location.X + 1, location.Y + 1]);
                            direction = Directions.SE;
                        }
                        else
                        {
                            return null;
                        }
                        numSearched++;
                    }
                    catch
                    {
                        return null;
                    }                   
                }
                else if(direction != default && isTileValid(currentVertex))
                {
                    try
                    {
                        numSearched++;

                        switch (direction)
                        {
                            case Directions.NORTH:
                                if (isTileValid(Tile.Map[location.X, location.Y - 1]) &&
                                    Tile.Map[location.X, location.Y - 1].Type == TileType.GREEN_TILE)
                                {
                                    tileQueue.Enqueue(Tile.Map[location.X, location.Y - 1]);
                                }
                                break;

                            case Directions.SOUTH:
                                if (isTileValid(Tile.Map[location.X, location.Y + 1]) &&
                                    Tile.Map[location.X, location.Y + 1].Type == TileType.GREEN_TILE)
                                {
                                    tileQueue.Enqueue(Tile.Map[location.X, location.Y + 1]);
                                }
                                break;

                            case Directions.WEST:
                                if (isTileValid(Tile.Map[location.X - 1, location.Y]) &&
                                    Tile.Map[location.X - 1, location.Y].Type == TileType.GREEN_TILE)
                                {
                                    tileQueue.Enqueue(Tile.Map[location.X - 1, location.Y]);
                                }
                                break;

                            case Directions.EAST:
                                if (isTileValid(Tile.Map[location.X + 1, location.Y]) &&
                                    Tile.Map[location.X + 1, location.Y].Type == TileType.GREEN_TILE)
                                {
                                    tileQueue.Enqueue(Tile.Map[location.X + 1, location.Y]);
                                }
                                break;

                            case Directions.NW:
                                if (isTileValid(Tile.Map[location.X - 1, location.Y - 1]) &&
                                    Tile.Map[location.X - 1, location.Y - 1].Type == TileType.GREEN_TILE)
                                {
                                    tileQueue.Enqueue(Tile.Map[location.X - 1, location.Y - 1]);
                                }
                                break;

                            case Directions.SW:
                                if (isTileValid(Tile.Map[location.X - 1, location.Y + 1]) &&
                                    Tile.Map[location.X - 1, location.Y + 1].Type == TileType.GREEN_TILE)
                                {
                                    tileQueue.Enqueue(Tile.Map[location.X - 1, location.Y + 1]);
                                }
                                break;

                            case Directions.NE:
                                if (isTileValid(Tile.Map[location.X + 1, location.Y - 1]) &&
                                    Tile.Map[location.X + 1, location.Y - 1].Type == TileType.GREEN_TILE)
                                {
                                    tileQueue.Enqueue(Tile.Map[location.X + 1, location.Y - 1]);
                                }
                                break;

                            case Directions.SE:
                                if (isTileValid(Tile.Map[location.X + 1, location.Y + 1]) &&
                                    Tile.Map[location.X + 1, location.Y + 1].Type == TileType.GREEN_TILE)
                                {
                                    tileQueue.Enqueue(Tile.Map[location.X + 1, location.Y + 1]);
                                }
                                break;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                    
                }
                else
                {
                    return null;
                }
            }


            List<Tile> tiles = new List<Tile>();

            tiles.AddRange(tileQueue);

            if(tiles.Count == 4)
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
    }
}
