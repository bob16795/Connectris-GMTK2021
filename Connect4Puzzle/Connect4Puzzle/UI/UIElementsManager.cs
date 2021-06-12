using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Connect4Puzzle.UI;
using Connect4Puzzle.Graphics;
using Connect4Puzzle.Tiles;
using Connect4Puzzle.Drawing;
using Connect4Puzzle.FSM;

namespace Connect4Puzzle.UI
{
    //HEADER==================================================
    //Names: sciencedoge, prestosilver
    //Date: 6/11/2021
    //Purpose: Manages the creation of all of the UI elements
    //========================================================
    class UIElementsManager
    {
        //singleton
        public static readonly Lazy<UIElementsManager> elements
            = new Lazy<UIElementsManager>(() => new UIElementsManager());

        //singleton property
        public UIElementsManager Instance { get { return elements.Value; } }

        //Fields
        public static UIButton playButton;
        public static UIButton nextButton;
        public static UIButton okButton;
        public static UIButton menuButton;
        public static UIButton nextTileButton;

        public static SpriteFont font;


        //ctor
        public UIElementsManager()
        {
            playButton = new UIButton(font,
                new Rectangle((Sprite.graphics.PreferredBackBufferWidth / 2) - 110,
                (2 * Sprite.graphics.PreferredBackBufferHeight / 3) - 100,
                200, 100));

            playButton.Text.Text = "Play Game";

            playButton.onClick = new UIAction((i) =>
            {
                System.Diagnostics.Debug.WriteLine("test");
                FiniteStateMachineManager.Instance.CurrentState = GameState.INSTRUCTIONS;
            });
            UIManager.Instance.Add(playButton);

            nextButton = new UIButton(font,
                new Rectangle((Sprite.graphics.PreferredBackBufferWidth / 2) - 110,
                (2 * Sprite.graphics.PreferredBackBufferHeight / 3) - 100,
                200, 100));

            nextButton.Text.Text = "Next";
            nextButton.IsActive = false;

            nextButton.onClick = new UIAction((i) =>
            {
                System.Diagnostics.Debug.WriteLine("test");
                FiniteStateMachineManager.Instance.CurrentState = GameState.GAME;
            });

            UIManager.Instance.Add(nextButton);

            okButton = new UIButton(font,
                new Rectangle((Sprite.graphics.PreferredBackBufferWidth / 2) - 110,
                (2 * Sprite.graphics.PreferredBackBufferHeight / 3) - 100,
                200, 100));

            okButton.IsActive = false;

            okButton.Text.Text = "OK";

            okButton.onClick = new UIAction((i) =>
            {
                System.Diagnostics.Debug.WriteLine("test");
                FiniteStateMachineManager.Instance.CurrentState = GameState.GAME;
            });
            UIManager.Instance.Add(okButton);

            menuButton = new UIButton(font,
                new Rectangle(Sprite.graphics.PreferredBackBufferWidth - 100,
                50,
                75, 50));

            menuButton.IsActive = false;

            menuButton.Text.Text = "Pause";

            menuButton.onClick = new UIAction((i) =>
            {
                System.Diagnostics.Debug.WriteLine("test");
                FiniteStateMachineManager.Instance.CurrentState = GameState.MENU;
            });

            UIManager.Instance.Add(menuButton);

            nextTileButton = new UIButton(font,
                new Rectangle((Sprite.graphics.PreferredBackBufferWidth - 100),
                100,
                50, 80));

            nextTileButton.IsActive = false;
            UIManager.Instance.Add(nextTileButton);
        }
    }
}
