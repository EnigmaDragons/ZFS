using MonoDragons.Core.Engine;

namespace MonoDragons.ZFS.CoreGame
{
    public interface GameOverCondition
    {
        string Description { get; }
        bool IsGameOver();
    }
}