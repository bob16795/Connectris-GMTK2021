using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Connect4Puzzle.Tiles;

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
        private Tile[,] tileGrid;
        
        //ctor

        /// <summary>
        /// Creates a new RenderMap object
        /// </summary>
        /// <param name="tiles">The tiles being rendered</param>
        public RenderMap(Tile[,] tiles)
        {
            this.tileGrid = tiles;
        }

        /// <summary>
        /// Draws the tiles to the screen
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < tileGrid.GetLength(0); i++)
            {
                for(int j = 0; j < tileGrid.GetLength(1); j++)
                {
                    if (tileGrid[i, j].Sprite == null) tileGrid[i, j].UpdateSprite();
                    if(tileGrid[i, j].Type != TileType.NO_TILE)
                    {
                        tileGrid[i, j].Sprite.Draw(sb, new Point
                        (i * (tileGrid[i, j].Sprite.Position.Width * 2), j *
                        (tileGrid[i, j].Sprite.Position.Height * 2)), 0f, 
                        new Vector2(tileGrid[i, j].Sprite.Position.Width * 2,
                        tileGrid[i, j].Sprite.Position.Height * 2));
                    }
                }
            }
        }
    }
}
