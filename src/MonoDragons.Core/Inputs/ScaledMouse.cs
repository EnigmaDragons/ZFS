using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Render;

namespace MonoDragons.Core.Inputs
{
    public static class ScaledMouse
    {
        public static MouseState GetState()
        {
            var raw = Mouse.GetState();
            return new MouseState((int)(raw.X / CurrentDisplay.XScale), 
                (int)(raw.Y / CurrentDisplay.YScale), 
                    raw.ScrollWheelValue, 
                    raw.LeftButton,
                    raw.MiddleButton,
                    raw.RightButton,
                    raw.XButton1,
                    raw.XButton2);
        }
    }
}