using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Connect4Puzzle.Music
{
    //HEADER============================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/27/2021
    //Purpose: Manages all of the sound in this project
    //==================================================
    public class SoundManager
    {
        private static readonly Lazy<SoundManager>
            lazy =
            new Lazy<SoundManager>
                (() => new SoundManager());
        public static SoundManager Instance { get { return lazy.Value; } }
        
        public ContentManager content;

        public bool Muted {get {return MediaPlayer.IsMuted; } set {MediaPlayer.IsMuted = value;}}
        private SoundEffect buttonClick;
        private SoundEffect Snap;
        private SoundEffect combo;

        private SoundEffect red;
        private SoundEffect gameOver;

        private Song menu;
        private Song title;

        private String playing;

        /// <summary>
        /// Loads SFX and music
        /// </summary>
        public void LoadContent()
        {
            Snap = content.Load<SoundEffect>("Sounds/snap");
            combo = content.Load<SoundEffect>("Sounds/combo");
            menu = content.Load<Song>("Sounds/katyusha");
            title = content.Load<Song>("Sounds/title");
            red = content.Load<SoundEffect>("Sounds/reds");
            gameOver = content.Load<SoundEffect>("Sounds/lose");
        }

        public void PlayCombo(int level) {
            combo.Play(0.5f, 1 / (float)level , 0);
        }

        /// <summary>
        /// Plays Red combo
        /// </summary>
        /// <param name="level"></param>
        public void PlayRedCombo(int level)
        {
            red.Play(0.5f, 1 / (float)level, 0);
        }

        /// <summary>
        /// plays a sound effect with a given action
        /// </summary>
        /// <param name="action">action of the player</param>
        public void PlaySFX(string action)
        {
            if (Muted) return;

            switch (action.ToLower().Trim())
            {
                case "snap":
                    Snap.Play(1f, 0, 0);
                    break;
                case "button":
                    buttonClick.Play(1f, 0, 0);
                    break;
                case "gameover":
                    gameOver.Play(1f, 0, 0);
                    break;
            }
        }

        /// <summary>
        /// plays music in the game
        /// </summary>
        /// <param name="section"></param>
        public void PlayMusic(string section)
        {
            if (section == playing) return;
            playing = section;
            switch(section.ToLower().Trim())
            {
                case "continue":
                    MediaPlayer.Resume();
                    break;
                case "pause":
                    MediaPlayer.Pause();
                    break;
                case "game":
                    MediaPlayer.Stop();                  
                    MediaPlayer.Play(menu);
                    MediaPlayer.Volume = 0.25f;
                    MediaPlayer.IsRepeating = true;
                    break;
                case "menu":
                    MediaPlayer.Stop();                  
                    MediaPlayer.Play(title);
                    MediaPlayer.Volume = 0.25f;
                    MediaPlayer.IsRepeating = true;
                    break;
            }
        }

        public void StopMusic()
        {
            MediaPlayer.Pause();
        }

        public void ResumeMusic()
        {
            MediaPlayer.Resume();
        }
    }
}
