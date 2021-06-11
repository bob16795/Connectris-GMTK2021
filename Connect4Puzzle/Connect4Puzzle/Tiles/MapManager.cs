using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace theNamespace.Tiles
{
    class MapManager
    {
        /// <summary>
        /// Singleton Stuff
        /// </summary>
        private static readonly Lazy<MapManager>
            lazy =
            new Lazy<MapManager>
                (() => new MapManager());
        public static MapManager Instance { get { return lazy.Value; } }
    
        public void DropTiles() {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 27; y++)
                {
                    if (Tile.Map[x, y].Type == TileType.NO_TILE && Tile.CheckHeldWeird(Tile.Map[x, y + 1])) {
                        Tile.Map[x, y + 1] = Tile.Map[x, y];
                        Tile.Map[x, y + 1] = new Tile(new Point(x, y));
                    }
                }
            }
        }

        public void Update(GameTime gt) {
            DropTiles();
        }
    }
}