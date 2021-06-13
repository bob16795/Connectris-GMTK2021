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
        BAD_TILE,
        NO_TILE,
    }

    public enum TileConnection {
        UP = 0,
        DOWN,
        LEFT,
        RIGHT,
        NO_CONNECTION,
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

        public Tile(Tile t) : this(t.Position, t.Connection, t.Type, t.controlled) {}

        public Tile(Point position, TileConnection connection = TileConnection.NO_CONNECTION, TileType type = TileType.NO_TILE, bool controlled = false) {
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
            if (t.Position.Y == 19) return true;
            if (t.Connection == TileConnection.RIGHT) {
                return (Tile.Map[t.Position.X + 1, t.Position.Y + 1].Type == TileType.NO_TILE);
            } else if (t.Connection == TileConnection.LEFT) {
                return (Tile.Map[t.Position.X - 1, t.Position.Y + 1].Type == TileType.NO_TILE);
            }
            return t.Type != TileType.NO_TILE;
        }

        public static bool CheckHeldWidth(Tile t, int direction) {
            direction *= -1;
            if (t.Connection == TileConnection.UP) {
                if (t.Position.X + direction < 0 || t.Position.X + direction > 7) return true;
                return (Tile.Map[t.Position.X + direction, t.Position.Y - 1].Type == TileType.NO_TILE);
            } else if (t.Connection == TileConnection.DOWN) {
                if (t.Position.X + direction < 0 || t.Position.X + direction > 7) return true;
                return (Tile.Map[t.Position.X + direction, t.Position.Y + 1].Type == TileType.NO_TILE);
            }
            return t.Type != TileType.NO_TILE;
        }

        public void ResetConnection() {
            Connection = TileConnection.NO_CONNECTION;
        }

        public void MakeBad() {
            Type = TileType.BAD_TILE;
            Connection = TileConnection.NO_CONNECTION;
        }

        public static void Remove(Tile t, bool bad) {
            Point p = t.Position;
            if (t.Connection == TileConnection.UP)
                Tile.Map[p.X, p.Y - 1].ResetConnection();
            else if (t.Connection == TileConnection.DOWN)
                Tile.Map[p.X, p.Y + 1].ResetConnection();
            else if (t.Connection == TileConnection.LEFT)
                Tile.Map[p.X - 1, p.Y].ResetConnection();
            else if (t.Connection == TileConnection.RIGHT)
                Tile.Map[p.X + 1, p.Y].ResetConnection();
            t.props.Position = p.ToVector2() * 24 + RenderMap.bg.Bounds.Location.ToVector2() + new Vector2(12, -24);
            for (int i = 0; i < 25; i++)
            {
                ps.Emit(Tile.Map[p.X, p.Y].props);
            }
            if (t.Type == TileType.GREEN_TILE) {
                MapManager.Instance.Score += 100;
            } else if (bad && t.Type == TileType.RED_TILE) {
                MapManager.Instance.Score -= 125;
            }
            t.Type = TileType.NO_TILE;
            t.Connection = TileConnection.NO_CONNECTION;
        }
    }
}