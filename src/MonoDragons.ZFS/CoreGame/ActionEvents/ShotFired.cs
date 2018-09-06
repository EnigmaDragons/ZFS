using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.ActionEvents
{
    public class ShotFired
    {
        public Character Target { get; set; }
        public Character Attacker { get; set; }
    }
}
