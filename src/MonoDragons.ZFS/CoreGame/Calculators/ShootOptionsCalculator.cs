using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.Mechanics.Covors;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public class ShootOptionsCalculator
    {
        public ShootOptionsCalculator()
        {
            Event.Subscribe(EventSubscription.Create<MovementFinished>(CalculateTargets, this));
        }

        public void CalculateTargets(MovementFinished e)
        {
            var targetsAvailable = new RangedTargetsAvailable
            {
                Targets = CurrentData.LivingCharacters
                    .Where(x => x != CurrentData.CurrentCharacter && CanShoot(CurrentData.CurrentCharacter, x))
                    .Select(x => new Target
                    {
                        Character = x,
                        CoverToThem = new ShotCalculation(CurrentData.Map, CurrentData.CurrentCharacter.CurrentTile, x.CurrentTile).GetBestShot(),
                        CoverFromThem = CanShoot(x, CurrentData.CurrentCharacter)
                            ? new ShotCalculation(CurrentData.Map, x.CurrentTile, CurrentData.CurrentCharacter.CurrentTile).GetBestShot()
                            : new ShotCoverInfo(new List<CoverProvided>())
                    }).ToList()
            };
            Event.Queue(targetsAvailable);
        }

        private bool CanShoot(Character attacker, Character target)
        {
            return !AreSameTeam(attacker, target) 
                && attacker.Gear.EquippedWeapon.IsRanged
                && attacker.Gear.EquippedWeapon.AsRanged().EffectiveRanges.ContainsKey(attacker.CurrentTile.Position.TileDistance(target.CurrentTile.Position))
                && attacker.State.SeeableTiles[target.CurrentTile.Position];
        }

        private bool AreSameTeam(Character attacker, Character target) => attacker.Team == target.Team;
    }
}
