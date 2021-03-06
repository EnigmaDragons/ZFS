﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Render;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.Mechanics.Resolution;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.CoreGame
{
    public class TurnBasedCombat : IAutomaton
    {
        private readonly List<object> _actionResolvers = ActionResolvers.CreateAll();
        private readonly List<object> _effectResolvers = GameEffectResolvers.CreateAll();
        private readonly List<object> _characterResolvers = new List<object> { new CharacterEventProcessing() };
        
        public GameMap Map { get; }
        public IReadOnlyList<IReadOnlyList<Point>> AvailableMoves { get; private set; }
        public List<Target> Targets { get; private set; }

        public TurnBasedCombat(GameMap map)
        {
            Map = map;

            Event.Subscribe(EventSubscription.Create<ActionResolved>(OnActionResolved, this));
            Event.Subscribe(EventSubscription.Create<MovementOptionsAvailable>(x => AvailableMoves = x.AvailableMoves, this));
            Event.Subscribe(EventSubscription.Create<RangedTargetsAvailable>(x => Targets = x.Targets, this));
        }

        public void Init()
        {
            CurrentData.InitLevel();
        }

        private void OnActionResolved(ActionResolved obj)
        {
            Event.Queue(new TurnEnded());
        }

        public void MoveTo(int x, int y)
        {
            if (AvailableMoves.Any(move => move.Last().X == x && move.Last().Y == y))
                Event.Publish(new MovementConfirmed(AvailableMoves.First(move => move.Last().X == x && move.Last().Y == y)));
        }

        public void Shoot(int x, int y)
        {
            if (!Targets.Any(target => target.Character.CurrentTile.Position.X == x && target.Character.CurrentTile.Position.Y == y))
                return;

            var attackTarget = Targets.First(target => target.Character.CurrentTile.Position.X == x && target.Character.CurrentTile.Position.Y == y);
            Event.Publish(new RangedTargetInspected
            {
                Attacker = CurrentData.CurrentCharacter,
                Defender = attackTarget.Character,
                AttackerBlockInfo = attackTarget.CoverFromThem,
                DefenderBlockInfo = attackTarget.CoverToThem
            });
        } 

        public void Update(TimeSpan delta)
        {
            CurrentData.Characters.ToList().ForEach(x => x.Update(delta));
        }
    }
}