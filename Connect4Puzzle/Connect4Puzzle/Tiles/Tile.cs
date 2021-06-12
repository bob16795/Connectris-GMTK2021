using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Connect4Puzzle.Graphics;
using Connect4Puzzle.Drawing;

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
        public static ParticleSystem ps;
        public static Tile[,] Map = new Tile[8, 20];
        public Point Position;
        public TileType Type;
        public TileConnection Connection;
        public Sprite Sprite;
        public bool controlled;
        private ParticleProps props;

        public Tile(Point position, TileConnection connection = TileConnection.NONE, TileType type = TileType.NO_TILE, bool controlled = false) {
            if (ps == null)
                ps = new ParticleSystem();
            this.Position = position;
            this.Connection = connection;
            this.Type = type;
            this.controlled = controlled;
            props = new ParticleProps{
                Position = this.Position.ToVector2() * 24 + RenderMap.bg.Bounds.Location.ToVector2() + new Vector2(12, -24),
                Velocity = new Vector2(0, 0),
                VelocityVariation = new Vector2(100, 100),
                StartColor = type == TileType.RED_TILE ? Color.Red : Color.Green,
                EndColor = Color.Gray,
                SizeStart = 4,
                SizeEnd = 3,
                LifeTime = 1.5f,
                LifeTimeVariation = 1f,
            };
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

        public static void Remove(Point p, bool bad) {
            if (Tile.Map[p.X, p.Y].Connection == TileConnection.UP)
                Tile.Map[p.X, p.Y + 1].Connection = TileConnection.NONE;
            else if (Tile.Map[p.X, p.Y].Connection == TileConnection.DOWN)
                Tile.Map[p.X, p.Y - 1].Connection = TileConnection.NONE;
            else if (Tile.Map[p.X, p.Y].Connection == TileConnection.LEFT)
                Tile.Map[p.X + 1, p.Y].Connection = TileConnection.NONE;
            else if (Tile.Map[p.X, p.Y].Connection == TileConnection.RIGHT)
                Tile.Map[p.X - 1, p.Y].Connection = TileConnection.NONE;

            for (int i = 0; i < 100; i++)
            {
                ps.Emit(Tile.Map[p.X, p.Y].props);
            }
            if (Tile.Map[p.X, p.Y].Type == TileType.GREEN_TILE) {
                MapManager.Instance.Score += 100;
            } else if (bad && Tile.Map[p.X, p.Y].Type == TileType.RED_TILE) {
                MapManager.Instance.Score -= 10;
            }
            Tile.Map[p.X, p.Y] = new Tile(new Point(p.X, p.Y));
        }
    }
}