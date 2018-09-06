using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Events
{
    class WeaponSwitched
    {
        public Character Character { get; set; }

        public WeaponSwitched(Character character)
        {
            Character = character;
        }
    }
}
