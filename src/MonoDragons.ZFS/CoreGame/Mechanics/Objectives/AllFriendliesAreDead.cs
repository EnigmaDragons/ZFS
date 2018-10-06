using System.Linq;

namespace MonoDragons.ZFS.CoreGame
{
    public sealed class AllFriendliesAreDead : GameOverCondition
    {
        public string Description => "Your whole team died!";
        public bool IsGameOver() => CurrentData.Friendlies.All(x => x.State.IsDeceased);
    }
}