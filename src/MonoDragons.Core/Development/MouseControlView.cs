using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Development
{
    public sealed class MouseControlView : IVisualAutomaton
    {
        private readonly Point _position;
        private MouseState _m;

        public MouseControlView()
            : this (new Point(1400, 840)) { }
        
        public MouseControlView(Point position)
        {
            _position = position;
        }
        
        public void Draw(Transform2 parentTransform)
        {
            var color = DevText.Color;
            var font = DevText.Font;
            UI.DrawTextRight($"Mouse: {_m.Position.X.PadLeft(4, '0')}, {_m.Position.Y.PadLeft(4, '0')}", 
                new Rectangle(_position, new Point(200, DevText.LineHeight)), color, font);
        }

        public void Update(TimeSpan delta)
        {
            _m = Mouse.GetState();
        }
    }
}