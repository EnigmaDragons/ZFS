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
            Event.Queue(new ActionReadied(() => Event.Queue(new ShotConfirmed { Proposed = proposed, OnFinished = () => Event.Queue(new ActionResolved()) })));
            Event.Queue(proposed);
        }
    }
}
