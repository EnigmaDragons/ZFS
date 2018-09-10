using System.Collections.Generic;
using MonoDragons.ZFS.Characters.Gear;

namespace MonoDragons.ZFS.Characters
{
    public abstract class CharacterClass
    {
        public string Name => GetType().Name;
        protected abstract List<WeaponType> Proficiencies { get; }
        public WeaponSet WeaponSet { get; set; } = new WeaponSet("Default", new RsxCarbine(), new SideArm());
        public CharacterStatsMods StatMods { get; set; } = new CharacterStatsMods();
    }
}