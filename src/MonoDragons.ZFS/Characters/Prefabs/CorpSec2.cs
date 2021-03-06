﻿using Microsoft.Xna.Framework;
using MonoDragons.ZFS.Characters.Gear;
using MonoDragons.ZFS.Themes;

namespace MonoDragons.ZFS.Characters.Prefabs
{
    public class CorpSec2 : Character
    {
        public CorpSec2(bool mustKill) : base(
            new CharacterBody("CorporationSecurity2", new Vector2(-13, -42), TeamColors.Enemy.Characters_GlowColor),
            new CharacterStats
            {
                Name = "CorpSec Trooper",
                HP = 40,
                Movement = 7,
                Accuracy = 5,
                Guts = 6,
                Agility = 6,
                Perception = 8
            },
            new CharacterGear(WeaponLists.RandomPrimary(), WeaponLists.RandomSecondary()),
            Team.Enemy,
            "Characters/CorpSec-face.png",
            "Characters/CorpSec-bust.png")
        {
            State.MustKill = mustKill;
            State.NextScene = "FinalFloor";
        }
    }
}
