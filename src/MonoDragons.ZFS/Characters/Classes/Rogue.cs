using System.Collections.Generic;
using MonoDragons.ZFS.Characters.Gear;

namespace MonoDragons.ZFS.Characters
{
    public sealed class Rogue : CharacterClass
    {
        protected override List<WeaponType> Proficiencies => new List<WeaponType> { WeaponType.Pad, WeaponType.Blade }; 
    }
}