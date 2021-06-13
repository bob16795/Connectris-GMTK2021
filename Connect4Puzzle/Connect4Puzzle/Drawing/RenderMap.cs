using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Connect4Puzzle.Tiles;
using Connect4Puzzle.UI;
using Connect4Puzzle.Graphics;

namespace Connect4Puzzle.Drawing
{
    //Header=======================================
    //Names: sciencedoge, prestosilver
    //Date: 6/11/2021
    //Purpose: Renders the tile map to the screen
    //=============================================
    class RenderMap
    {
        //fields
        public static UIPanel bg = new UIPanel(new Rectangle(((int)Sprite.DEF_WIDTH - 192) / 2 - 1, ((int)Sprite.DEF_HEIGHT - 480) / 2 - 1, 195, 483));

        //ctor

        /// <summary>
        /// Creates a new RenderMap object
        /// </summary>
        /// <param name="tiles">The tiles being rendered</param>
        public RenderMap() { }

        /// <summary>
        /// Draws the tiles to the screen
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            Tile[,] tileGrid = Tile.Map;
            bg.Draw(new GameTime(), sb);
            for(int i = 0; i < tileGrid.GetLength(0); i++)
            {
                for(int j = 0; j < tileGrid.GetLength(1); j++)
                {
                    if (tileGrid[i, j].Sprite == null) tileGrid[i, j].UpdateSprite();
                    if(tileGrid[i, j].Type != TileType.NO_TILE)
                    {
                        tileGrid[i, j].Sprite.Draw(sb, new Point
                        (((int)Sprite.DEF_WIDTH - 192) / 2 +
                        i * (tileGrid[i, j].Sprite.Position.Width * 3),
                        ((int)Sprite.DEF_HEIGHT - 480) / 2 +
                        j * (tileGrid[i, j].Sprite.Position.Height * 3)), 0f, 
                        new Vector2(tileGrid[i, j].Sprite.Position.Width * 3,
                        tileGrid[i, j].Sprite.Position.Height * 3));
                    }
                }
            }
        }
    }
}
