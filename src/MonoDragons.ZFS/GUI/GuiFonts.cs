﻿using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Memory;

namespace MonoDragons.ZFS.GUI
{
    static class GuiFonts
    {
        public const string Header = "Fonts/24";
        public const string Large = "Fonts/18";
        public const string Medium = "Fonts/14";
        public const string Body = "Fonts/12";
        public static readonly SpriteFont BodySpriteFont = Resources.Load<SpriteFont>(Body);
    }
}
