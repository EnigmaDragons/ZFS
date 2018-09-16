using MonoDragons.Core.Engine;

namespace MonoDragons.ZFS.CoreGame
{
    public interface PrimaryObjective : IAutomaton
    {
        string Description { get; }
    }
}
