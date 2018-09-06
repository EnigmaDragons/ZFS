using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.StateEvents
{
    public sealed class CharacterDeceased
    {
        public Character Victim { get; set; }
        public Character Killer { get; set; }
    }
}
