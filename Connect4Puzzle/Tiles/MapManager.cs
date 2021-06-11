using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using theNamespace.FSM;

namespace theNamespace.Tiles
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
    }
}