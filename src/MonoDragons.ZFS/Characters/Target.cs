using MonoDragons.ZFS.CoreGame.Mechanics.Covors;

namespace MonoDragons.ZFS.Characters
{
    public class Target
    {
        public Character Character { get; set; }
        public ShotCoverInfo CoverToThem { get; set; }
        public ShotCoverInfo CoverFromThem { get; set; }
    }
}
