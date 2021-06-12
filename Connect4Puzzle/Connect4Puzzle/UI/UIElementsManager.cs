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
#if DEBUG
        double frameRate = 0.0;
        double secondTimer = 0.0f;
        int frameCounter;
#endif
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

        public static UIPanel nextTile;

        public static UIText ScoreText;

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
                FiniteStateMachineManager.Instance.CurrentState = GameState.INSTRUCTIONS;
                ResetButtons();
            });
            UIManager.Instance.Add(playButton);

            nextButton = new UIButton(font,
                new Rectangle((Sprite.graphics.PreferredBackBufferWidth / 2) - 110,
                (2 * Sprite.graphics.PreferredBackBufferHeight / 3),
                200, 100));

            nextButton.Text.Text = "Next";
            nextButton.IsActive = false;

            nextButton.onClick = new UIAction((i) =>
            {
                FiniteStateMachineManager.Instance.CurrentState = GameState.GAME;
                ResetButtons();
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
                FiniteStateMachineManager.Instance.CurrentState = GameState.GAME;
                ResetButtons();
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
                FiniteStateMachineManager.Instance.CurrentState = GameState.MENU;
                ResetButtons();
            });

            UIManager.Instance.Add(menuButton);

            nextTile = new UIPanel(new Rectangle(RenderMap.bg.Bounds.Right,
                RenderMap.bg.Bounds.Top,
                50, 80));

            nextTile.IsActive = false;
            UIManager.Instance.Add(nextTile);

            //TODO: Move
            ScoreText = new UIText(font, new Rectangle(0, 0, 0, 0), 2, Color.White);
            ScoreText.update = new UITextUpdate(() => {
#if DEBUG
                return MapManager.Instance.Score.ToString("D8") + "\n" + frameRate;
#else
                return MapManager.Instance.Score.ToString("D8");
#endif
            });
            ScoreText.IsActive = false;
            UIManager.Instance.Add(ScoreText);
        }
#if DEBUG
        public void Update(GameTime gt) {
            secondTimer += gt.ElapsedGameTime.TotalSeconds;
            if (secondTimer > 1.0)
            {
                frameRate = (double)frameCounter / secondTimer;
                secondTimer = 0;
                frameCounter = 0;
            }
            frameCounter++;
        }
#endif
        
        /// <summary>
        /// Resets all buttons to false
        /// </summary>
        public void ResetButtons()
        {
            ScoreText.IsActive = false;
            playButton.IsActive = false;
            okButton.IsActive = false;
            nextTile.IsActive = false;
            nextButton.IsActive = false;
            menuButton.IsActive = false;
        }
    }
}
