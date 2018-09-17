using System;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.Mechanics.Covors;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public class ProposedShotCalculation
    {
        private readonly Character _attacker;
        private readonly Character _defender;
        private readonly ShotCoverInfo _attackerBlockInfo;
        private readonly ShotCoverInfo _defenderBlockInfo;

        public ProposedShotCalculation(Character attacker, Character defender, ShotCoverInfo attackerBlockInfo, ShotCoverInfo defenderBlockInfo)
        {
            _attacker = attacker;
            _defender = defender;
            _attackerBlockInfo = attackerBlockInfo;
            _defenderBlockInfo = defenderBlockInfo;
        }

        public ShotProposed CalculateShot()
        {
            var distance = _attacker.CurrentTile.Position.TileDistance(_defender.CurrentTile.Position);
            var attackerWeapon = _attacker.Gear.EquippedWeapon.AsRanged();
            var proposed = new ShotProposed();
            var attackerShotPart = new ShotPart
            {
                Character = _attacker,
                HitChance = new HitChanceCalculation(_attacker.Accuracy, _defenderBlockInfo.BlockChance,
                    _defender.Stats.Agility, _defender.State.IsHiding).Get(),
                BulletDamage =
                    (int)Math.Ceiling((attackerWeapon.DamagePerHit * attackerWeapon.EffectiveRanges[distance])),
                BlockInfo = _attackerBlockInfo,
                IsHiding = _attacker.State.IsHiding,
                NumBullets = attackerWeapon.NumShotsPerAttack,
            };

            var defenderShotPart = new ShotPart
            {
                Character = _defender,
                BlockInfo = _defenderBlockInfo,
                IsHiding = _defender.State.IsHiding

            };

            if (_defender.Gear.EquippedWeapon.IsRanged)
            {
                var defenderWeapon = _defender.Gear.EquippedWeapon.AsRanged();
                defenderShotPart.HitChance = new HitChanceCalculation(_defender.Accuracy, _attackerBlockInfo.BlockChance, _attacker.Stats.Agility, _attacker.State.IsHiding).Get();
                defenderShotPart.NumBullets = defenderWeapon.NumShotsPerAttack;
                if (defenderWeapon.EffectiveRanges.ContainsKey(distance))
                    defenderShotPart.BulletDamage = (int) Math.Ceiling(defenderWeapon.DamagePerHit * defenderWeapon.EffectiveRanges[distance]);
            }

            proposed.ShotContext[AttackRole.Defender] = defenderShotPart;
            proposed.ShotContext[AttackRole.Attacker] = attackerShotPart;
            
            proposed.AttackerDamage = (int)((defenderShotPart.HitChance / 100f) * (defenderShotPart.NumBullets * defenderShotPart.BulletDamage) - _attacker.Stats.Guts);
            proposed.DefenderDamage = (int)((attackerShotPart.HitChance / 100f) * (attackerShotPart.NumBullets * attackerShotPart.BulletDamage) - _defender.Stats.Guts);
            return proposed;
        }
    }
}
