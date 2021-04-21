using System;
using System.Collections.Generic;
using System.Text;

namespace DinoGame
{
    static class Global
    {
        public const int TileSize = 64;
        public static readonly Camera Camera = new Camera();
        public static bool Debug = true;
    }
}
