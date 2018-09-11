using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Development
{
    public sealed class MouseControlView : IVisualAutomaton
    {
        private readonly Point _position;
        private MouseState _mRaw;
        private MouseState _mScaled;

        public MouseControlView()
            : this (new Point(1300, 800)) { }
        
        public MouseControlView(Point position)
        {
            _position = position;
        }
        
        public void Draw(Transform2 parentTransform)
        {
            var color = DevText.Color;
            var font = DevText.Font;
            UI.DrawTextRight($"Mouse-Raw: {_mRaw.Position.X.PadLeft(4, '0')}, {_mRaw.Position.Y.PadLeft(4, '0')}", 
                new Rectangle(_position, new Point(300, DevText.LineHeight)), color, font);
            UI.DrawTextRight($"Mouse-Virt: {_mScaled.Position.X.PadLeft(4, '0')}, {_mScaled.Position.Y.PadLeft(4, '0')}", 
                new Rectangle(_position + new Vector2(0, DevText.LineHeight).ToPoint(), new Point(300, DevText.LineHeight)), color, font);
        }

        public void Update(TimeSpan delta)
        {
            _mRaw = Mouse.GetState();
            _mScaled = ScaledMouse.GetState();
        }
    }
}