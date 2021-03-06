﻿using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.Themes;

namespace MonoDragons.ZFS.GUI
{
    static class Buttons
    {
        private const int _buttonWidth = 200;
        private const int _buttonHeight = 35;
        private const int _buttonMargin = 10;
        
        public static void PlayClickSound() => Sound.SoundEffect("SFX/button-press-1.wav", 0.6f).Play();

        public class MenuContext
        {
            public int Width { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int FirstButtonYOffset { get; set; }
        }

        public static TextButton Text(MenuContext ctx, int index, string text, Action action) 
            => Text(ctx, index, text, action, () => true);

        public static TextButton Text(MenuContext ctx, int index, string text, Action action, Func<bool> condition)
        {
            return new TextButton(
                new Rectangle(
                    ctx.X + (ctx.Width - _buttonWidth) / 2,
                    ctx.Y + ctx.FirstButtonYOffset + ((_buttonMargin + _buttonHeight) * index),
                    _buttonWidth,
                    _buttonHeight),
                () =>
                {
                    PlayClickSound();
                    action();
                },
                text,
                UiColors.Buttons_Default,
                UiColors.Buttons_Hover,
                UiColors.Buttons_Press,
                condition)
            { Font = "Fonts/12", Color = UiColors.InGame_Text };
        }
    }
}
