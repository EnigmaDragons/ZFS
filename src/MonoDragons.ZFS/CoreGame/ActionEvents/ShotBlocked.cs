using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.CoreGame.ActionEvents
{
    public class ShotBlocked
    {
        public GameTile Blocker { get; set; }
        public Character Attacker { get; set; }
        public Character Target { get; set; }
    }
}
