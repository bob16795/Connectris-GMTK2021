using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Connect4Puzzle.Graphics;

namespace Connect4Puzzle.UI
{
    public delegate void UIAction(int i);
    class UIManager
    {
        /// <summary>
        /// Singleton stuff
        /// </summary>
        private static readonly Lazy<UIManager>
            lazy =
            new Lazy<UIManager>
                (() => new UIManager());
        public static UIManager Instance { get { return lazy.Value; } }
        
        public List<UIElement> UIElements;
        public UIElement focused;

        /// <summary>
        /// creates the ui manager
        /// </summary>
        public UIManager()
        {
            UIElements = new List<UIElement>();
        }

        /// <summary>
        /// Do some random stuff before draw
        /// </summary>
        /// <param name="gameTime">a gameTime Object</param>
        public void Update(GameTime gameTime)
        {
            foreach (UIElement e in UIElements)
            {
                e.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws ALL UIElements
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (UIElement e in UIElements)
            {
                e.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// processes a click anywhere on the screen
        /// </summary>
        /// <param name="position">the position the screen was clicked</param>
        /// <returns>true if the click is used</returns>
        public bool ProcessClick(Point position)
        {
            position -= Sprite.GetOrigin();
            position.X = (int)(position.X / Sprite.GetScale());
            position.Y = (int)(position.Y / Sprite.GetScale());
            foreach (UIElement e in UIElements)
            {
                if (e.Bounds.Contains(position) && e.IsActive)
                {
                    return e.WhenClicked(position - e.Bounds.Location);
                }
            }
            return false;
        }
        
        public void MouseMove(Point position)
        {
            position -= Sprite.GetOrigin();
            position.X = (int) (position.X / Sprite.GetScale());
            position.Y = (int) (position.Y / Sprite.GetScale());
            foreach (UIElement e in UIElements)
            {
                if (e.Bounds.Contains(position) && e.IsActive)
                {
                    e.WhenMoved(position - e.Bounds.Location);
                }
            }
        }

        /// <summary>
        /// adds a uielement to the list
        /// </summary>
        /// <param name="element">the element to add</param>
        public void Add(UIElement element)
        {
            UIElements.Add(element);
        }

        /// <summary>
        /// navigates the ui
        /// </summary>
        /// <param name="reverse">weter to go backwards</param>
        public void Nav(bool reverse)
        {
            UIElement start = focused;
            do 
            {
                focused.Focused = false;
                if (reverse && focused.prevFocus != null) {
                    focused.prevFocus.Focused = true;
                    focused = focused.prevFocus;
                } else if (focused.nextFocus != null) {
                    focused.nextFocus.Focused = true;
                    focused = focused.nextFocus;
                }
            } while (focused.IsActive == false && focused != start);
        }

        /// <summary>
        /// setup the focus loop for the uielements
        /// </summary>
        /// <param name="elements">the elements in order</param>
        public static void SetupFocusLoop(List<UIElement> elements)
        {
            for (int i = 1; i < elements.Count; i++)
            {
                elements[i - 1].nextFocus = elements[i];
                elements[i].prevFocus = elements[i - 1];
            }
            elements[elements.Count - 1].nextFocus = elements[0];
            elements[0].prevFocus = elements[elements.Count - 1];
            return;
        }
    }

    /// <summary>
    /// A sprite that can be resized by tiling
    /// Example
    ///  A| B| C
    /// --+--+--
    ///  D| E| F
    /// --+--+--
    ///  G| H| I
    ///   
    /// Can be resized To
    /// 
    ///  A| B| B| C
    /// --+--+--+--
    ///  D| E| E| F
    /// --+--+--+--
    ///  D| E| E| F
    /// --+--+--+--
    ///  G| H| H| I
    /// 
    /// It Dosent have to be a square tho
    /// this prevents ugly scaling artifacts on UIElements
    /// 
    /// Example:
    /// UISprite s = new UISprite(_spriteSheetTexture, new Rectangle(0, 0, 15, 42), new Rectangle(5, 31, 5, 8), new Vector2(0, 0), Color.White);
    /// s.Draw(_spriteBatch, new Rectangle(40, 40, _graphics.PreferredBackBufferWidth - 80, _graphics.PreferredBackBufferHeight - 80), 0);
    /// </summary>
    public class UISprite : Sprite
    {
        private Rectangle[,] renderSections = new Rectangle[3, 3];

        /// <summary>
        /// Stores the Center Rectangle, really just for refrence
        /// </summary>
        private Rectangle _Center;
        public UISprite(Rectangle position, Rectangle center, Vector2 origin, Color tintColor) : base(position, origin, tintColor)
        {
            Position = position;
            Center = center;
            Origin = origin;
            TintColor = tintColor;
        }

        /// <summary>
        /// Setup renderSections if Center is changed
        /// </summary>
        public Rectangle Center { get => _Center;
            set
            {
                if (!Position.Contains(value)) { throw new InvalidOperationException(); }
                _Center = value;
                for (int i = 0; i < 3; i++)
                {
                    renderSections[0, i].X = Position.X;
                    renderSections[0, i].Width = Center.X - Position.X;
                    renderSections[1, i].X = Center.X;
                    renderSections[1, i].Width = Center.Width;
                    renderSections[2, i].X = Center.Right;
                    renderSections[2, i].Width = Position.Right - Center.Right;
                    renderSections[i, 0].Y = Position.Y;
                    renderSections[i, 0].Height = Center.Y - Position.Y;
                    renderSections[i, 1].Y = Center.Y;
                    renderSections[i, 1].Height = Center.Height;
                    renderSections[i, 2].Y = Center.Bottom;
                    renderSections[i, 2].Height = Position.Bottom - Center.Bottom;
                }
            }
        }

        /// <summary>
        /// Draws the UISprite
        /// </summary>
        /// <param name="spriteBatch">the Sprite Batch Object</param>
        /// <param name="renderRect">Where the sprite should be rendered</param>
        /// <param name="rotation">should be zero for now</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle renderRect, float rotation)
        {
            Rectangle tmp;

            renderRect.X = (int)(Sprite.GetScale() * renderRect.X);
            renderRect.Y = (int)(Sprite.GetScale() * renderRect.Y);
            renderRect.Location += Sprite.GetOrigin();
            renderRect.Width = (int)(Sprite.GetScale() * renderRect.Width);
            renderRect.Height = (int)(Sprite.GetScale() * renderRect.Height);

            renderRect.Width -= (renderRect.Width - renderSections[0, 0].Width - renderSections[2, 0].Width) % Center.Width;
            renderRect.Height -= (renderRect.Height - renderSections[0, 0].Height - renderSections[0, 2].Height) % Center.Height;
            // draw sections Excluding A C G and I (the corners)
            for (int x = renderRect.X + renderSections[0, 0].Width; x < renderRect.Right - renderSections[0, 2].Width ; x += renderSections[1, 1].Width)
            {
                // section B
                tmp = renderSections[1, 0];
                tmp.Location = new Point(x, renderRect.Top);
                spriteBatch.Draw(texture, tmp, renderSections[1, 0], TintColor, rotation, Origin, SpriteEffects.None, 0f);

                for (int y = renderRect.Y + renderSections[0, 0].Height; y < renderRect.Bottom - renderSections[0, 2].Height; y += renderSections[1, 1].Height)
                {
                    // section E
                    tmp = renderSections[1, 1];
                    tmp.Location = new Point(x, y);
                    spriteBatch.Draw(texture, tmp, renderSections[1, 1], TintColor, rotation, Origin, SpriteEffects.None, 0f);
                }
                // section H
                tmp = renderSections[1, 2];
                tmp.Location = new Point(x, renderRect.Bottom - renderSections[0, 2].Height);
                spriteBatch.Draw(texture, tmp, renderSections[1, 2], TintColor, rotation, Origin, SpriteEffects.None, 0f);
            }
            for (int y = renderRect.Y + renderSections[0, 0].Height; y < renderRect.Bottom - renderSections[0, 2].Height; y += renderSections[1, 1].Height)
            {
                //Section D
                tmp = renderSections[0, 1];
                tmp.Location = new Point(renderRect.Left, y);
                spriteBatch.Draw(texture, tmp, renderSections[0, 1], TintColor, rotation, Origin, SpriteEffects.None, 0f);

                //Section F
                tmp = renderSections[2, 1];
                tmp.Location = new Point(renderRect.Right - renderSections[2, 0].Width, y);
                spriteBatch.Draw(texture, tmp, renderSections[2, 1], TintColor, rotation, Origin, SpriteEffects.None, 0f);
            }
            //Section A
            tmp = renderSections[0, 0];
            tmp.Location = new Point(renderRect.Left, renderRect.Top);
            spriteBatch.Draw(texture, tmp, renderSections[0, 0], TintColor, rotation, Origin, SpriteEffects.None, 0f);

            //Section C
            tmp = renderSections[0, 2];
            tmp.Location = new Point(renderRect.Left, renderRect.Bottom - renderSections[0, 2].Height);
            spriteBatch.Draw(texture, tmp, renderSections[0, 2], TintColor, rotation, Origin, SpriteEffects.None, 0f);

            //Section G
            tmp = renderSections[2, 0];
            tmp.Location = new Point(renderRect.Right - renderSections[2, 0].Width, renderRect.Top);
            spriteBatch.Draw(texture, tmp, renderSections[2, 0], TintColor, rotation, Origin, SpriteEffects.None, 0f);

            //Section I
            tmp = renderSections[2, 2];
            tmp.Location = new Point(renderRect.Right - renderSections[2, 0].Width, renderRect.Bottom - renderSections[0, 2].Height);
            spriteBatch.Draw(texture, tmp, renderSections[2, 2], TintColor, rotation, Origin, SpriteEffects.None, 0f);
        }
    }
}
