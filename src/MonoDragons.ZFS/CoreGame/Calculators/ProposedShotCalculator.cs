using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public class ProposedShotCalculator
    {
        public ProposedShotCalculator()
        {
            Event.Subscribe<RangedTargetInspected>(PreviewShot, this);
        }

        private void PreviewShot(RangedTargetInspected e)
        {
            var proposed = new ProposedShotCalculation(e.Attacker, e.Defender, e.AttackerBlockInfo, e.DefenderBlockInfo).CalculateShot();
            EventQueue.Instance.Add(new ActionReadied(() => EventQueue.Instance.Add(new ShotConfirmed { Proposed = proposed, OnFinished = () => EventQueue.Instance.Add(new ActionResolved()) })));
            EventQueue.Instance.Add(proposed);
        }
    }
}
