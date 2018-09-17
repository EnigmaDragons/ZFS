using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.GUI
{
    public class AttackPreview : IVisual
    {
        private readonly CombatantSummary _attackerSummary = new CombatantSummary(false);
        private readonly CombatantSummary _defenderSummary = new CombatantSummary(true);

        private bool _hidden = true;

        public AttackPreview()
        {
            Event.Subscribe(EventSubscription.Create<ShotProposed>(DisplayPreview, this));
            Event.Subscribe(EventSubscription.Create<ActionCancelled>(e => Hide(), this));
            Event.Subscribe(EventSubscription.Create<ActionConfirmed>(e => Hide(), this));
        }

        private void DisplayPreview(ShotProposed e)
        {
            if (!CurrentData.CurrentCharacter.IsFriendly)
                return;
            
            _hidden = false;
            var a = e.ShotContext[AttackRole.Attacker];
            _attackerSummary.Update(
                a.Character.FaceImage, 
                a.Character.Stats.Name, 
                a.Character.Gear.EquippedWeapon.Image, 
                a.Character.Gear.EquippedWeapon.Name, 
                a.HitChance.ToString(), 
                a.NumBullets.ToString(), 
                a.BulletDamage.ToString(), 
                a.Character.Stats.HP, 
                a.Character.State.RemainingHealth, 
                e.AttackerDamage,
                a.Character.Team);
            var d = e.ShotContext[AttackRole.Defender];
            _defenderSummary.Update(
                d.Character.FaceImage,
                d.Character.Stats.Name,
                d.Character.Gear.EquippedWeapon.Image,
                d.Character.Gear.EquippedWeapon.Name,
                d.HitChance.ToString(),
                d.NumBullets.ToString(),
                d.BulletDamage.ToString(),
                d.Character.Stats.HP,
                d.Character.State.RemainingHealth,
                e.DefenderDamage,
                d.Character.Team);
        }

        private void Hide()
        {
            _hidden = true;
        }

        public void Draw(Transform2 parentTransform)
        {
            if (_hidden)
                return;

            const int height = 260;
            _attackerSummary.Draw(parentTransform + new Vector2(.04.VW(), height));
            _defenderSummary.Draw(parentTransform + new Vector2(.74.VW(), height));
        }
    }
}
