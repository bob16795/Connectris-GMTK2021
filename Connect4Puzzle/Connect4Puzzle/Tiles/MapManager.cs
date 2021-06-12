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

        public int Score;

        private Random random = new Random();

        public bool Stop = true;
    
        public void DropTiles() {

            for (int y = 19; y > 0; y--)
            {
                Tile[] result = new Tile[8];
                for (int x = 0; x < 8; x++)
                {
                    if (Tile.Map[x, y] == null) Tile.Map[x, y] = new Tile(new Point(x, y)); 
                    result[x] = Tile.Map[x, y];
                    if (Tile.Map[x, y].Type == TileType.NO_TILE && Tile.CheckHeldHeight(Tile.Map[x, y - 1])) {
                        result[x] = Tile.Map[x, y - 1];
                        Tile.Map[x, y - 1] = new Tile(new Point(x, y - 1));
                    }
                }

                for (int x = 0; x < 8; x++)
                {
                    Tile.Map[x, y] = result[x];
                    Tile.Map[x, y].Position = new Point(x, y);
                }
            }
            for (int x = 0; x < 8 && Stop == false; x++ )
            {
                for (int y = 18; y > 0 && Stop == false; y--)
                {
                    if (Tile.Map[x, y].controlled && !Tile.Map[x, y + 1].controlled && Tile.Map[x, y + 1].Type != TileType.NO_TILE) Stop = true;
                }
                if (Tile.Map[x, 19].controlled) Stop = true;
            }
        }

        public bool Control() {
            if (Stop) {
                for (int y = 19; y > 0; y--)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        Tile.Map[x, y].controlled = false;
                    }
                }
                return false;
            }
            return true;
        }

        public void MoveTiles() {
            List<Direction> Keys = InputManager.Instance.TrackInput();
            if (Keys.Contains(Direction.DOWN)) {
            }
            else if (Keys.Contains(Direction.LEFT)) {
                Move(1);
            }
            else if (Keys.Contains(Direction.RIGHT)) {
                Move(-1);
            }
        }

        public void Move(int direction) {
            for (int x2 = 0; x2 < 7; x2++)
            {
                int x = direction < 0 ? 7 - x2 : x2;
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
            switch (i)
            {
                case 1:
                Tile.Map[3, 0] = new Tile(new Point(3, 0), TileConnection.RIGHT, TileType.RED_TILE, true);
                Tile.Map[4, 0] = new Tile(new Point(4, 0), TileConnection.LEFT, TileType.GREEN_TILE, true);
                break;
                case 2:
                Tile.Map[3, 0] = new Tile(new Point(3, 0), TileConnection.RIGHT, TileType.GREEN_TILE, true);
                Tile.Map[4, 0] = new Tile(new Point(4, 0), TileConnection.LEFT, TileType.RED_TILE, true);
                break;
                case 3:
                Tile.Map[3, 0] = new Tile(new Point(3, 0), TileConnection.DOWN, TileType.GREEN_TILE, true);
                Tile.Map[3, 1] = new Tile(new Point(3, 1), TileConnection.UP, TileType.RED_TILE, true);
                break;
                case 4:
                Tile.Map[3, 0] = new Tile(new Point(3, 0), TileConnection.DOWN, TileType.RED_TILE, true);
                Tile.Map[3, 1] = new Tile(new Point(3, 1), TileConnection.UP, TileType.GREEN_TILE, true);
                break;
            }
            Stop = false;
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
            if (!Control())
                SpawnTiles();
            MoveTiles();
        }
    }
}