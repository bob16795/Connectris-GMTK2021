using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Connect4Puzzle.Graphics;

namespace Connect4Puzzle.Tiles
{
    public enum TileType {
        RED_TILE = 0,
        GREEN_TILE,
        NO_TILE,
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
        public bool controlled;

        public Tile(Point position, TileConnection connection = TileConnection.NONE, TileType type = TileType.NO_TILE) {
            this.Position = position;
            this.Connection = connection;
            this.Type = type;
        }

        public void UpdateSprite() {
            Sprite = new Sprite(new Rectangle(8 * (int)(Type), 8 * (int)(Connection), 8, 8), -new Vector2(0, 0), Color.White);
        }

        public static bool CheckHeldHeight(Tile t) {
            if (t.Connection == TileConnection.RIGHT) {
                return (Tile.Map[t.Position.X + 1, t.Position.Y - 1].Type == TileType.NO_TILE);
            } else if (t.Connection == TileConnection.LEFT) {
                return (Tile.Map[t.Position.X - 1, t.Position.Y - 1].Type == TileType.NO_TILE);
            }
            return t.Type != TileType.NO_TILE;
        }

        public static bool CheckHeldWidth(Tile t, int direction) {
            if (t.Connection == TileConnection.UP) {
                return (Tile.Map[t.Position.X + direction, t.Position.Y - 1].Type == TileType.NO_TILE);
            } else if (t.Connection == TileConnection.DOWN) {
                return (Tile.Map[t.Position.X + direction, t.Position.Y + 1].Type == TileType.NO_TILE);
            }
            return true;
        }
    }
}