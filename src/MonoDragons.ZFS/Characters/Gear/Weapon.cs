﻿using System;

namespace MonoDragons.ZFS.Characters.Gear
{
    public abstract class Weapon
    {
        public abstract string Name { get; }
        public abstract string Image { get; }
        public abstract string ShortDescription { get; }
        public abstract bool IsRanged { get; }
        public abstract bool IsMelee { get; }
        public abstract WeaponType Type { get; }

        public RangedWeapon AsRanged()
        {
            if (!IsRanged)
                throw new InvalidOperationException("Non Ranged Weapons cannot be used as Ranged Weapon.");
            return (RangedWeapon)this;
        }

        public MeleeWeapon AsMelee()
        {
            if (!IsMelee)
                throw new InvalidOperationException("Non Melee Weapons cannot be used as Melee Weapon.");
            return (MeleeWeapon)this;
        }
    }
}
