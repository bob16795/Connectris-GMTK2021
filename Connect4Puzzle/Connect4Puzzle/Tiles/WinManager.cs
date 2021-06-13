
using System;
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

    class WinManager
    {
        //fields
        private int numSearched;
        private Random random;

        private bool canSearchNorth;
        private bool canSearchWest;
        private bool canSearchNW;
        private bool canSearchSW;
        private bool canSearchSouth;
        private bool canSearchSouthEast;
        private bool canSearchNE;

        private Point original;


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
            numSearched = 0;

            random = new Random();

            original = default;

        }

        //methods
        /// <summary>
        /// performs breadth first search to find 
        /// potential four in a rows
        /// </summary>
        public List<Tile> BFSearch(Point pos, Point dir, TileType type = TileType.NO_TILE)
        {
            List<Tile> result = new List<Tile>();
            if (!new Rectangle(0, 0, 8, 20).Contains(pos)) return result;
            if (dir == Point.Zero) {
                for (int x = -1; x < 2; x ++)
                {
                    for (int y = -1; y < 2; y ++)
                    {
                        if (x == 0 && y == 0) continue;
                        Point d = new Point(x, y);
                        List<Tile> tiles = BFSearch(pos + d, d, type);
                        if (tiles.Count >= 3) {
                            if (!Tile.Map[pos.X, pos.Y].controlled)
                                result.Add(Tile.Map[pos.X, pos.Y]);
                            result.AddRange(tiles);
                        }
                    }
                }
            } else {
                List<Tile> tiles = BFSearch(pos + dir, dir, type);
                if (Tile.Map[pos.X, pos.Y].Type == type && !Tile.Map[pos.X, pos.Y].controlled)
                    result.Add(Tile.Map[pos.X, pos.Y]);
                else
                    return result;
                if (tiles.Count != 0)
                    result.AddRange(tiles);
            }
            return result;
        }
    }
}