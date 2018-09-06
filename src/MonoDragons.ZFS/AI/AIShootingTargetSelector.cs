using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.Calculators;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.AI
{
    class AIShootingTargetSelector : AIActorBase
    {
        public AIShootingTargetSelector()
        {
            Event.Subscribe<ShootSelected>(SelectTarget, this);
        }

        private void SelectTarget(ShootSelected e)
        {
            IfAITurn(() => Shoot(e.AvailableTargets
                .OrderBy(x => new ShotCalculation(Char.CurrentTile, x.Character.CurrentTile).GetBestShot().BlockChance)
                .ThenByDescending(x => x.Character.Stats.Guts).First()));
        }

        private void Shoot(Target target)
        {
            EventQueue.Instance.Add(new RangedTargetInspected
            {
                Attacker = CurrentData.CurrentCharacter,
                Defender = target.Character,
                AttackerBlockInfo = target.CoverFromThem,
                DefenderBlockInfo = target.CoverToThem
            });
        }
    }
}
