using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Development
{
    public sealed class KeyboardControlView : IVisual
    {
        private readonly Point _position;

        private readonly List<Control> _active = new List<Control>();

        public KeyboardControlView()
            : this(new Point(1400, 0)) { }

        public KeyboardControlView(Point position)
        {
            _position = position;
            Event.SubscribeForever<ControlStateChanged>(c =>
            {
                if (c.State.Equals(ControlState.Active))
                    _active.Add(c.Control);
                else
                    _active.Remove(c.Control);
            }, this);
        }

        public void Draw(Transform2 parentTransform)
        {
            var color = DevText.Color;
            var font = DevText.Font;
            UI.DrawTextRight($"Keys", new Rectangle(_position, new Point(200, DevText.LineHeight)), color, font);
            _active.ForEachIndex((x, i) => 
                UI.DrawTextRight($"{x}", 
                    new Rectangle(new Point(_position.X, _position.Y + DevText.LineHeight * (i + 1)), new Point(200, DevText.LineHeight)), color, font));
        }
    }
}