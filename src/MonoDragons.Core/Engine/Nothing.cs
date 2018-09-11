using System;
using MonoDragons.Core.Physics;

namespace MonoDragons.Core.Engine
{
    public sealed class Dummy : IVisualAutomaton
    {
        public void Update(TimeSpan delta)
        {
            // He does nothing
        }

        public void Draw(Transform2 parentTransform)
        {
            // He is invisible
        }
    }
}