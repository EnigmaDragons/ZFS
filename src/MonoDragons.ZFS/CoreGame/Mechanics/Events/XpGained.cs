using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Events
{
    public struct XpGained
    {
        public Character Character { get; set; }
        public int XpAmount { get; set; }
    }
}