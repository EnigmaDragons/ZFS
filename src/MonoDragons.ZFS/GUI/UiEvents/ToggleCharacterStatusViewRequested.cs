using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.GUI
{
    public sealed class ToggleCharacterStatusViewRequested
    {
        public Character Character { get; }

        public ToggleCharacterStatusViewRequested(Character character) => Character = character;
    }
}
