using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.GUI
{
    public sealed class DisplayCharacterStatusRequested
    {
        public Character Character { get; }

        public DisplayCharacterStatusRequested(Character character) => Character = character;
    }
}