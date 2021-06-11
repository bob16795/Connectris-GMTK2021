using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace theNamespace.Tiles
{
    public enum TileType {
        NO_TILE = 0,
        RED_TILE,
        GREEN_TILE,
    }

    public enum TileConnection {
        UP = 0,
        DOWN,
        LEFT,
        RIGHT,
        NONE,
    }

    public class Tile
    {
        static Tile[,] Map;
        
        TileType Type;
        
        TileConnection Connection;
    }
}