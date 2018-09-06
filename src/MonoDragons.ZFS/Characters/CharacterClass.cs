﻿using MonoDragons.ZFS.Characters.Gear;

namespace MonoDragons.ZFS.Characters
{
    public class CharacterClass
    {
        public WeaponSet WeaponSet { get; set; } = new WeaponSet("Default", new RsxCarbine(), new SideArm());
        public CharacterStatsMods StatMods { get; set; } = new CharacterStatsMods();
    }
}