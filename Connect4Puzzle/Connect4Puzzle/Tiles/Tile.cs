using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using theNamespace.Graphics;

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
        public static Tile[,] Map = new Tile[7, 28];
        public Point Position;
        public TileType Type;
        public TileConnection Connection;
        public Sprite Sprite;

        public void UpdateSprite() {
            Sprite = new Sprite(new Rectangle(8 * (int)(Type), 8 * (int)(Connection), 8, 8), -new Vector2(0, 0), Color.White);
        }

        public static bool CheckHeldWeird(Tile t) {
            if (t.Connection == TileConnection.RIGHT) {
                return (Tile.Map[t.Position.X + 1, t.Position.Y - 1].Type == TileType.NO_TILE);
            } else if (t.Connection == TileConnection.LEFT) {
                return (Tile.Map[t.Position.X - 1, t.Position.Y - 1].Type == TileType.NO_TILE);
            }
            return false;
        }
    }
}