using System.Collections.Generic;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.Mechanics.Covors;

namespace MonoDragons.ZFS.CoreGame
{
    public class ShotProposed
    {
        public Dictionary<AttackRole, ShotPart> ShotContext { get; } = new Dictionary<AttackRole, ShotPart>();
        public int AttackerDamage { get; set; }
        public int DefenderDamage { get; set; }
    }

    public enum AttackRole
    {
        Attacker,
        Defender
    }
    
    public sealed class ShotPart
    {
        public Character Character { get; set; }
        public AttackRole Role { get; set; }
        public int HitChance { get; set; }
        public int NumBullets { get; set; }
        public int BulletDamage { get; set; }
        public ShotCoverInfo BlockInfo { get; set; }
        public bool IsHiding { get; set; }
    }
}
