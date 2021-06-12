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
        public static Tile[,] Map = new Tile[8, 20];
        public Point Position;
        public TileType Type;
        public TileConnection Connection;
        public Sprite Sprite;
        public bool controlled;

        public Tile(Point position, TileConnection connection = TileConnection.NONE, TileType type = TileType.NO_TILE, bool controlled = false) {
            this.Position = position;
            this.Connection = connection;
            this.Type = type;
            this.controlled = controlled;
        }

        public void UpdateSprite() {
            Sprite = new Sprite(new Rectangle(8 * (int)(Type), 8 * (int)(Connection), 8, 8), -new Vector2(0, 0), Color.White);
        }

        public static bool CheckHeldHeight(Tile t) {
            if (t.Connection == TileConnection.RIGHT) {
                return (Tile.Map[t.Position.X + 1, t.Position.Y + 1].Type == TileType.NO_TILE);
            } else if (t.Connection == TileConnection.LEFT) {
                return (Tile.Map[t.Position.X - 1, t.Position.Y + 1].Type == TileType.NO_TILE);
            }
            return t.Type != TileType.NO_TILE;
        }

        public static bool CheckHeldWidth(Tile t, int direction) {
            if (t.Connection == TileConnection.UP) {
                return (Tile.Map[t.Position.X + direction, t.Position.Y - 1].Type == TileType.NO_TILE);
            } else if (t.Connection == TileConnection.DOWN) {
                return (Tile.Map[t.Position.X + direction, t.Position.Y + 1].Type == TileType.NO_TILE);
            }
            return t.Type != TileType.NO_TILE;
        }

        public static void Remove(Point p) {
            if (Tile.Map[p.X, p.Y].Connection == TileConnection.UP)
                Tile.Map[p.X, p.Y + 1].Connection = TileConnection.NONE;
            else if (Tile.Map[p.X, p.Y].Connection == TileConnection.DOWN)
                Tile.Map[p.X, p.Y - 1].Connection = TileConnection.NONE;
            else if (Tile.Map[p.X, p.Y].Connection == TileConnection.LEFT)
                Tile.Map[p.X + 1, p.Y].Connection = TileConnection.NONE;
            else if (Tile.Map[p.X, p.Y].Connection == TileConnection.RIGHT)
                Tile.Map[p.X - 1, p.Y].Connection = TileConnection.NONE;

            Tile.Map[p.X, p.Y] = new Tile(new Point(p.X, p.Y));
        }
    }
}