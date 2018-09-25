using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.ZFS.GUI.Hud
{
    public sealed class GameWorldClickable : ClickableUIElement
    {
        private readonly Action _onClick;

        public GameWorldClickable(Action onClick)
            : base(new Rectangle(0, 0, 5000, 5000))
        {
            _onClick = onClick;
        }

        public override void OnEntered()
        {
        }

        public override void OnExitted()
        {
        }

        public override void OnPressed()
        {
        }

        public override void OnReleased()
        {
            _onClick();
        }
    }
}