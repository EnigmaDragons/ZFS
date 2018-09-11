using MonoDragons.Core.Engine;

namespace MonoDragons.Core.Development
{
    public sealed class DebugToolsLayout : GameObjContainer
    {
        public DebugToolsLayout()
        {
            Add(new Metrics());
            Add(new DisplayDetailsView());
            Add(new KeyboardControlView());
            Add(new MouseControlView());
            Add(new HoveredElementView());
        }
    }
}