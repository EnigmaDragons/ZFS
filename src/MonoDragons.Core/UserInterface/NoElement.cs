using Microsoft.Xna.Framework;

namespace MonoDragons.Core.UserInterface
{
    public sealed class NoElement : ClickableUIElement
    {
        public NoElement() : base(new Rectangle(0, 0, 1920, 1080))
        {
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
        }
    }
}
