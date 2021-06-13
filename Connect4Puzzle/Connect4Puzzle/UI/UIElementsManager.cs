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
        public static UIButton creditsButton;
        public static UIButton backButton;

        public static UIPanel nextTile;

        public static UIText ScoreText;
        public static UIText attributionsText;

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
                FiniteStateMachineManager.Instance.Reset();
                FiniteStateMachineManager.Instance.CurrentState = GameState.GAME;               
                ResetButtons();
                UIElementsManager.ScoreText.IsActive = true;
                UIElementsManager.nextTile.IsActive = true;
                UIElementsManager.menuButton.IsActive = true;
            });

            UIManager.Instance.Add(nextButton);

            okButton = new UIButton(font,
                new Rectangle((Sprite.graphics.PreferredBackBufferWidth / 2) - 110,
                (2 * Sprite.graphics.PreferredBackBufferHeight / 3) - 100,
                200, 100));

            okButton.IsActive = false;

            okButton.Text.Text = "Return to game";

            okButton.onClick = new UIAction((i) =>
            {
                FiniteStateMachineManager.Instance.CurrentState = GameState.GAME;
                ResetButtons();
                UIElementsManager.ScoreText.IsActive = true;
                UIElementsManager.nextTile.IsActive = true;
                UIElementsManager.menuButton.IsActive = true;
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

            ScoreText = new UIText(font, new Rectangle(0, 0, 0, 0), 2, Color.White);
            ScoreText.update = new UITextUpdate(() => {
                return MapManager.Instance.Score.ToString("D8") + "\n\n" + MapManager.Instance.level.ToString("D2");
            });
            ScoreText.IsActive = false;
            UIManager.Instance.Add(ScoreText);

            attributionsText = new UIText(font, new Rectangle(0, 0, 0, 0), 2, Color.White);
            attributionsText.update = new UITextUpdate(() => {
                return MapManager.Instance.Score.ToString("         Main Programmers\n---------------------------------------------------"
                    + "\n              prestosilver\n              sciencedoge\n\n" +
                    "                   ssets\n-------------------------------------------------------------------------\n" +
                    "     remaxim - OpenGameArt\n" +
                    "CC-BY-SA 3.0, GPL 2.0, GPL 3.0");
            });
            attributionsText.IsActive = false;
            UIManager.Instance.Add(attributionsText);

            creditsButton = new UIButton(font,
                new Rectangle((Sprite.graphics.PreferredBackBufferWidth / 2) - 110,
                (2 * Sprite.graphics.PreferredBackBufferHeight / 3) + 50,
                200, 100));
            creditsButton.IsActive = true;
            creditsButton.Text.Text = "Credits";
            UIManager.Instance.Add(creditsButton);

            creditsButton.onClick = new UIAction((i) =>
            {
                FiniteStateMachineManager.Instance.CurrentState = GameState.CREDITS;
                attributionsText.IsActive = true;
                ResetButtons();
                UIElementsManager.backButton.IsActive = true;
            });

            backButton = new UIButton(font,
                new Rectangle((Sprite.graphics.PreferredBackBufferWidth / 2) - 110,
                (2 * Sprite.graphics.PreferredBackBufferHeight / 3) + 50,
                200, 100));
            backButton.IsActive = false;
            
            backButton.Text.Text = "Back";
            UIManager.Instance.Add(backButton);

            backButton.onClick = new UIAction((i) =>
            {
                FiniteStateMachineManager.Instance.CurrentState = GameState.MAIN_MENU;
                ResetButtons();
                attributionsText.IsActive = false;
                UIElementsManager.creditsButton.IsActive = true;
            });

        }
        
        /// <summary>
        /// Resets all buttons to false
        /// </summary>
        public void ResetButtons()
        {
            UIElementsManager.ScoreText.IsActive = false;
            UIElementsManager.playButton.IsActive = false;
            UIElementsManager.okButton.IsActive = false;
            UIElementsManager.nextTile.IsActive = false;
            UIElementsManager.nextButton.IsActive = false;
            UIElementsManager.menuButton.IsActive = false;
            UIElementsManager.creditsButton.IsActive = false;
            UIElementsManager.backButton.IsActive = false;
        }
    }
}
