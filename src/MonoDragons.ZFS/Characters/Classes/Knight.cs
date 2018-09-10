using System.Collections.Generic;
using MonoDragons.ZFS.Characters.Gear;

namespace MonoDragons.ZFS.Characters
{
    public sealed class Knight : CharacterClass
    {
        protected override List<WeaponType> Proficiencies => new List<WeaponType> { WeaponType.Shield, WeaponType.Shotgun }; 
    }
}