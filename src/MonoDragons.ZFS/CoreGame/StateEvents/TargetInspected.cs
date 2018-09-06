using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.Mechanics.Covors;

namespace MonoDragons.ZFS.CoreGame.StateEvents
{
    public class RangedTargetInspected
    {
        public Character Attacker { get; set; }
        public Character Defender { get; set; }
        public ShotCoverInfo AttackerBlockInfo { get; set; }
        public ShotCoverInfo DefenderBlockInfo { get; set; }
    }
}
