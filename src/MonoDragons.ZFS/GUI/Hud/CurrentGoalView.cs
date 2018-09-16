using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.CoreGame;

namespace MonoDragons.ZFS.GUI.Hud
{
    public sealed class CurrentGoalView : IVisual
    {       
        public void Draw(Transform2 parentTransform)
        {
            UI.DrawTextRight(CurrentData.PrimaryObjective.Description, 
                new Rectangle(0.01.VW(), 0.01.VH(), 0.98.VW(), 30), GuiFonts.DefaultColor.WithAlpha(0.7), GuiFonts.Medium);
        }
    }
}
