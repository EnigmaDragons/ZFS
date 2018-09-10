using System.Collections.Generic;
using MonoDragons.ZFS.Characters.Gear;

namespace MonoDragons.ZFS.Characters
{
    public class Decker : CharacterClass
    {
        protected override List<WeaponType> Proficiencies => new List<WeaponType> { WeaponType.Pad, WeaponType.Pistol }; 
    }
}