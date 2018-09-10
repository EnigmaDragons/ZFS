using System.Collections.Generic;
using MonoDragons.ZFS.Characters.Gear;

namespace MonoDragons.ZFS.Characters
{
    public sealed class Ninja : CharacterClass
    {
        protected override List<WeaponType> Proficiencies => new List<WeaponType> { WeaponType.Blade, WeaponType.Grenade }; 
    }
}