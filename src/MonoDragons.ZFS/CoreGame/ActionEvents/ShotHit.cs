using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.ActionEvents
{
    public class ShotHit
    {
        public Character Target { get; set; }
        public Character Attacker { get; set; }
        public int DamageAmount { get; set; }
    }
}
