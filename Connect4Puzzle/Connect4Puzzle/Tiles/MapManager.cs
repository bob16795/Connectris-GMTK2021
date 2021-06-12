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
        
        public bool lost => Tile.Map[3, 0] == null || Tile.Map[3, 0].controlled == false && Tile.Map[3, 0].Type != TileType.NO_TILE;

        public int Score;

        private Random random = new Random();

        private int counter;

        public int Stop = 0;
        
        private int startLevel = 0;

        public int level => Score / 750 + startLevel;

        public int Speed => 40 / (level + 1) + 1;
    
        private int combo;

        public void DropTiles() {

            for (int y = 19; y > 0; y--)
            {
                Tile[] result = new Tile[8];
                for (int x = 0; x < 8; x++)
                {
                    if (Tile.Map[x, y] == null) Tile.Map[x, y] = new Tile(new Point(x, y)); 
                    result[x] = Tile.Map[x, y];
                    if (Tile.Map[x, y].Type == TileType.NO_TILE && Tile.CheckHeldHeight(Tile.Map[x, y - 1])) {
                        result[x] = new Tile(Tile.Map[x, y - 1]);
                        Tile.Map[x, y - 1] = new Tile(new Point(x, y - 1));
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
                    if (Tile.Map[x, y].Type != TileType.NO_TILE) {
                        List<Tile> rem = WinManager.Instance.BFSearch(Tile.Map[x, y]);
                        if (rem == null) continue;
                        if (Tile.Map[x, y].Type == TileType.GREEN_TILE)
                            removed += 1;
                        remove.AddRange(rem);
                    }
                }
            }
            HashSet<Tile> removes = new HashSet<Tile>(remove);
            foreach (Tile t in removes)
            {
                t.Remove(true);
            }    
            if (removed != 0) {
                combo += removed;
                SoundManager.Instance.PlayCombo(combo);
            } else {
                combo -= 1;
            }

            for (int x = 0; x < 8 && Stop != 0; x++ )
            {
                for (int y = 18; y > 0 && Stop != 0; y--)
                {
                    if (Tile.Map[x, y].controlled && !Tile.Map[x, y + 1].controlled && Tile.Map[x, y + 1].Type != TileType.NO_TILE) {Stop --; return;}
                }
                if (Tile.Map[x, 19].controlled) {Stop --; return;}
            }
        }

        public bool Control() {
            if (Stop <= 0) {
                for (int y = 19; y > 0; y--)
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