using System;
using System.Collections.Generic;
using System.Text;
using Connect4Puzzle.Input;
using Connect4Puzzle.Music;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Connect4Puzzle.Tiles
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
        
        public bool lost;

        public int Score;

        private Random random = new Random();

        private int counter;

        public int Stop = 0;
        
        private int startLevel = 0;

        public int level => Score / 1000 + startLevel;

        public int Speed => 40 / (level + 1) + 3;
    
        private int combo;

        public void DropTiles() {
            for (int y = 19; y > 0; y--)
            {
                Tile[] result = new Tile[8];
                for (int x = 0; x < 8; x++)
                {
                    if (Tile.Map[x, y] == null) Tile.Map[x, y] = new Tile(new Point(x, y)); 
                    result[x] = new Tile(Tile.Map[x, y]);
                    if (Tile.Map[x, y].Type == TileType.NO_TILE && Tile.CheckHeldHeight(new Point(x, y - 1))) {
                        result[x] = new Tile(Tile.Map[x, y - 1]);
                        Tile.Map[x, y - 1] = new Tile(new Point(x, y - 1));
                    } else {
                        result[x] = new Tile(Tile.Map[x, y]);
                    }
                }

                for (int x = 0; x < 8; x++)
                {
                    Tile.Map[x, y] = result[x];
                    Tile.Map[x, y].Position = new Point(x, y);
                }
            }
            int removed = 0;
            List<Tile> remove = new List<Tile>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (Tile.Map[x, y].Type == TileType.NO_TILE || Tile.Map[x, y].controlled) continue;
                    List<Tile> rem = WinManager.Instance.BFSearch(new Point(x, y), Point.Zero, Tile.Map[x, y].Type);
                    if (rem.Count == 0) continue;
                    if (Tile.Map[x, y].Type == TileType.GREEN_TILE)
                        removed += 1;
                    remove.AddRange(rem);
                }
            }
            HashSet<Tile> removes = new HashSet<Tile>(remove);
            List<Point> reds = new List<Point>();
            List<Point> all = new List<Point>();
            if (removes.Count != 0) {
                foreach (Tile s in Tile.Map) {
                    if (s.controlled || s.Type == TileType.NO_TILE || s.Type == TileType.BAD_TILE || remove.Contains(s)) continue;
                    all.Add(s.Position);
                    if (s.Type == TileType.RED_TILE) {
                        reds.Add(s.Position);
                    }
                }
            }
            foreach (Tile t in removes)
            {
                if (t.Type == TileType.RED_TILE && all.Count != 0) {
                    int r = random.Next(0, all.Count);
                    Tile.Map[all[r].X, all[r].Y].MakeBad();
                    all.RemoveAt(r);
                } else if (t.Type == TileType.GREEN_TILE && reds.Count != 0) {
                    int r = random.Next(0, reds.Count);
                    Tile.Remove(Tile.Map[all[r].X, all[r].Y], false);
                    reds.RemoveAt(r);
                }
                Tile.Remove(t, true);
            }
            if (removed != 0) {
                combo += removed;
                SoundManager.Instance.PlayCombo(combo);
            } else {
                combo -= 1;
            }

            for (int x = 0; x < 8 && Stop != 0; x++ )
            {
                for (int y = 18; y >= 0 && Stop != 0; y--)
                {
                    if (Tile.Map[x, y].controlled && !Tile.Map[x, y + 1].controlled && Tile.Map[x, y + 1].Type != TileType.NO_TILE) {Stop --; return;}
                }
                if (Tile.Map[x, 19].controlled) {Stop --; return;}
            }
        }

        public bool Control() {
            if (Stop <= 0) {
                for (int y = 19; y >= 0; y--)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        Tile.Map[x, y].controlled = false;
                    }
                }
                SoundManager.Instance.PlaySFX("snap");
                return false;
            }
            for (int y = 19; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (Tile.Map[x, y].controlled == true) return true;
                }
            }
            return false;
        }

        public void MoveTiles() {
            List<Direction> keys = InputManager.Instance.TrackInput();
            if (keys.Contains(Direction.DOWN)) {
                if (counter ++ % 5 == 0)
                    DropTiles();
            }
            else if (keys.Contains(Direction.LEFT)) {
                if (counter ++ % 5 == 0)
                    Move(1);
            }
            else if (keys.Contains(Direction.RIGHT)) {
                if (counter ++ % 5 == 0)
                    Move(-1);
            } else {counter = 0;}
        }

        public void Move(int direction) {
            for (int x2 = 0; x2 < 7; x2++)
            {
                int x = direction < 0 ? 7 - x2 : x2;
                Tile[] result = new Tile[20];
                for (int y = 0; y < 20; y++)
                {
                    if (Tile.Map[x, y] == null) Tile.Map[x, y] = new Tile(new Point(x, y));
                    if (Tile.Map[x, y].Position != new Point(x, y)) Tile.Map[x, y] = new Tile(new Point(x, y));
                    result[y] = Tile.Map[x, y];
                    if (!Tile.Map[x + direction, y].controlled) continue;
                    if (Tile.Map[x, y].Type == TileType.NO_TILE && Tile.CheckHeldWidth(Tile.Map[x + direction, y], direction)) {
                        result[y] = Tile.Map[x + direction, y];
                        Tile.Map[x + direction, y] = new Tile(new Point(x + direction, y));
                    }
                }

                for (int y = 0; y < 20; y++)
                {
                    Tile.Map[x, y] = result[y];
                    Tile.Map[x, y].Position = new Point(x, y);
                }
            }
        }

        public void SpawnTiles() {
            Stop = 2;

            lost = Tile.Map[3, 0].Type != TileType.NO_TILE && !Tile.Map[3, 0].controlled;
            int i = (int)(random.NextDouble() * 4);
            switch (i)
            {
                case 0:
                Tile.Map[3, 0] = new Tile(new Point(3, 0), TileConnection.RIGHT, TileType.RED_TILE, true);
                Tile.Map[4, 0] = new Tile(new Point(4, 0), TileConnection.LEFT, TileType.GREEN_TILE, true);
                break;
                case 1:
                Tile.Map[3, 0] = new Tile(new Point(3, 0), TileConnection.RIGHT, TileType.GREEN_TILE, true);
                Tile.Map[4, 0] = new Tile(new Point(4, 0), TileConnection.LEFT, TileType.RED_TILE, true);
                break;
                case 2:
                Tile.Map[3, 0] = new Tile(new Point(3, 0), TileConnection.DOWN, TileType.GREEN_TILE, true);
                Tile.Map[3, 1] = new Tile(new Point(3, 1), TileConnection.UP, TileType.RED_TILE, true);
                break;
                case 3:
                Tile.Map[3, 0] = new Tile(new Point(3, 0), TileConnection.DOWN, TileType.RED_TILE, true);
                Tile.Map[3, 1] = new Tile(new Point(3, 1), TileConnection.UP, TileType.GREEN_TILE, true);
                break;
            }
            DropTiles();
        }

        public void Update(GameTime gt) {   
            if (Tile.Map[0, 0] == null) {
                for (int y = 0; y < 20; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                            Tile.Map[x, y] = new Tile(new Point(x, y));
                    }
                }
            }
            List<TileType> non = new List<TileType>{TileType.NO_TILE, TileType.BAD_TILE};
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    switch (Tile.Map[x, y].Connection) {
                        case TileConnection.UP:
                            if (non.Contains(Tile.Map[x, y - 1].Type)) Tile.Map[x, y].ResetConnection();
                            break;
                        case TileConnection.DOWN:
                            if (non.Contains(Tile.Map[x, y + 1].Type)) Tile.Map[x, y].ResetConnection();
                            break;
                        case TileConnection.LEFT:
                            if (non.Contains(Tile.Map[x - 1, y].Type)) Tile.Map[x, y].ResetConnection();
                            break;
                        case TileConnection.RIGHT:
                            if (non.Contains(Tile.Map[x + 1, y].Type)) Tile.Map[x, y].ResetConnection();
                            break;
                        default:
                            Tile.Map[x, y].ResetConnection();
                            break;
                    }
                }
            }
            if (Score < 0) lost = true;
            if (!Control())
                SpawnTiles();
            MoveTiles();
        }
    }
}