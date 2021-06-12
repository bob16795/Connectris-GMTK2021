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
        private bool[,] visited;
        private Directions direction;
        private int numSearched;

        //properties


        //ctor
        public WinManager()
        {
            visited = new bool[7, 7];
            direction = default;
            numSearched = 0;

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
                    switch (direction)
                    {
                        case Directions.NORTH:
                            if (isTileValid(Tile.Map[location.X, location.Y - 1]) &&
                                Tile.Map[location.X, location.Y - 1].Type == TileType.GREEN_TILE)
                            {
                                tileQueue.Enqueue(Tile.Map[location.X, location.Y - 1]);
                            }
                            break;
                    }
                }
                else
                {
                    if(tileQueue.Count != 0)
                    {
                        tileQueue.Dequeue();
                    }
                }
                
            }

            List<Tile> tiles = new List<Tile>();

            tiles.AddRange(tileQueue);

            return tiles;
        }

        /// <summary>
        /// resets the boolean array
        /// </summary>
        public void ResetBooleans()
        {
            visited = new bool[7, 7];
            visited[3, 3] = true;
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
