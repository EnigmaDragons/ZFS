using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.ActionEvents
{
    class ShotMissed
    {
        public Character Target { get; set; }
        public Character Attacker { get; set; }
    }
}
