using System;
using System.Collections.Generic;
using System.Text;
using Connect4Puzzle.Input;
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

        private Random random = new Random();
    
        public void DropTiles() {

            for (int y = 19; y > 0; y--)
            {
                Tile[] result = new Tile[7];
                for (int x = 0; x < 7; x++)
                {
                    if (Tile.Map[x, y] == null) Tile.Map[x, y] = new Tile(new Point(x, y)); 
                    result[x] = Tile.Map[x, y];
                    if (Tile.Map[x, y].Type == TileType.NO_TILE && Tile.CheckHeldHeight(Tile.Map[x, y - 1])) {
                        result[x] = Tile.Map[x, y - 1];
                        Tile.Map[x, y - 1] = new Tile(new Point(x, y - 1));
                    }
                }

                for (int x = 0; x < 7; x++)
                {
                    Tile.Map[x, y] = result[x];
                    Tile.Map[x, y].Position = new Point(x, y);
                }
            }
        }

        public bool Stop() {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 18; y > 0; y--)
                {
                    if (Tile.Map[x, y].controlled && Tile.Map[x, y + 1].Type != TileType.NO_TILE) return true;
                }
                if (Tile.Map[x, 19].controlled) return true;
            }
            return false;
        }

        public bool Control() {
            if (Stop()) {
                for (int y = 19; y > 0; y--)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        Tile.Map[x, y].controlled = false;
                    }
                }
                return true;
            }
            for (int y = 19; y > 0; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (Tile.Map[x, y].controlled) return true;
                }
            }
            return false;
        }

        public void MoveTiles() {
            List<Direction> Keys = InputManager.Instance.TrackInput();
            if (Keys.Contains(Direction.DOWN)) {
            }
            if (Keys.Contains(Direction.LEFT)) {
                Move(1);
            }
            if (Keys.Contains(Direction.RIGHT)) {
                Move(-1);
            }
        }

        public void Move(int direction) {
            for (int x2 = 0; x2 < 6; x2++)
            {
                int x = direction < 0 ? 6 - x2 : x2;
                Tile[] result = new Tile[20];
                for (int y = 0; y < 19; y++)
                {
                    if (Tile.Map[x, y] == null) Tile.Map[x, y] = new Tile(new Point(x, y));
                    if (Tile.Map[x, y].Position != new Point(x, y)) Tile.Map[x, y] = new Tile(new Point(x, y));
                    result[y] = Tile.Map[x, y];
                    if (!Tile.Map[x + direction, y].controlled) continue;
                    if (Tile.Map[x, y].Type == TileType.NO_TILE && Tile.CheckHeldHeight(Tile.Map[x + direction, y])) {
                        result[y] = Tile.Map[x + direction, y];
                        Tile.Map[x + direction, y] = new Tile(new Point(x + direction, y));
                    }
                }

                for (int y = 0; y < 19; y++)
                {
                    Tile.Map[x, y] = result[y];
                    Tile.Map[x, y].Position = new Point(x, y);
                }
            }
        }

        public void SpawnTiles() {
            int i = random.Next(1, 5);
            Tile.Map[0, 0] = new Tile(new Point(0, 0), TileConnection.RIGHT, TileType.RED_TILE, true);
            Tile.Map[1, 0] = new Tile(new Point(1, 0), TileConnection.LEFT, TileType.GREEN_TILE, true);
        }

        public void Update(GameTime gt) {   
            if (Tile.Map[0, 0] == null) {
                for (int y = 0; y < 20; y++)
                {
                    for (int x = 0; x < 7; x++)
                    {
                            Tile.Map[x, y] = new Tile(new Point(x, y));
                    }
                }
            }
            MoveTiles();
            if (!Control())
                SpawnTiles();
        }
    }
}