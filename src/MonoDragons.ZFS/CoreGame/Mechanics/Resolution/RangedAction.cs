using System;
using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.ActionEvents;
using MonoDragons.ZFS.GUI;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    public class RangedAction
    {
        private readonly Queue<Action> _eventQueue = new Queue<Action>();

        public RangedAction()
        {
            Event.Subscribe(EventSubscription.Create<ShotConfirmed>(ResolveShot, this));
            Event.Subscribe(EventSubscription.Create<AttackAnimationsFinished>(_ => AdvanceQueue(), this));
        }

        private void AdvanceQueue()
        {
            if (_eventQueue.Count > 0)
                _eventQueue.Dequeue().Invoke();
        }

        private void ResolveShot(ShotConfirmed e)
        {
            ResolveRangedAttack(e.Proposed, AttackRole.Attacker, AttackRole.Defender);
            _eventQueue.Enqueue(() =>
            {
                if (e.Proposed.ShotContext[AttackRole.Defender].Character.State.RemainingHealth > 0)
                    ResolveRangedAttack(e.Proposed, AttackRole.Defender, AttackRole.Attacker);
                AdvanceQueue();
            });
            _eventQueue.Enqueue(() =>
            {
                _eventQueue.Enqueue(() => e.OnFinished());
                AdvanceQueue();
            });
            AdvanceQueue();
        }

        private void ResolveRangedAttack(ShotProposed e, AttackRole shooterRole, AttackRole currentTarget)
        {
            var target = e.ShotContext[currentTarget];
            var shooter = e.ShotContext[shooterRole];
            for (var i = 0; i < shooter.NumBullets; i++)
            {
                var blockRoll = Rng.Int(0, target.IsHiding ? 50 : 100);
                if(blockRoll < target.BlockInfo.BlockChance)
                {
                    target.BlockInfo.Covers.Shuffle();
                    foreach (var cover in target.BlockInfo.Covers)
                        if (blockRoll < (int)cover.Cover)
                        {
                            ShotBlocked(shooter.Character, target.Character, cover.Providers.Random());
                            break;
                        }
                        else
                            blockRoll -= (int)cover.Cover;
                }
                else
                {
                    if (Rng.Int(0, 100) < shooter.HitChance)
                        ShotHit(shooter.Character, target.Character, shooter.BulletDamage);
                    else
                        ShotMissed(shooter.Character, target.Character);
                }  
            }
        }

        private void ShotHit(Character attacker, Character defender, int damage)
        {
            _eventQueue.Enqueue(() =>
            {
                Event.Queue(new ShotFired {Attacker = attacker, Target = defender});
                Event.Queue(new ShotHit {Attacker = attacker, Target = defender, DamageAmount = damage});
            });
        }

        private void ShotMissed(Character attacker, Character defender)
        {
            _eventQueue.Enqueue(() =>
            {
                Event.Queue(new ShotFired {Attacker = attacker, Target = defender});
                Event.Queue(new ShotMissed {Attacker = attacker, Target = defender});
            });
        }

        private void ShotBlocked(Character attacker, Character defender, GameTile blocker)
        {
            _eventQueue.Enqueue(() =>
            {
                Event.Queue(new ShotFired {Attacker = attacker, Target = defender});
                Event.Queue(new ShotBlocked {Attacker = attacker, Target = defender, Blocker = blocker});
            });
        }
    }
}
