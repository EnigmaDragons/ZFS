using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Events
{
    public class LevelledUp
    {
        public Character Character { get; set; }
        public CharacterStats OldStats { get; set; }
    }
}