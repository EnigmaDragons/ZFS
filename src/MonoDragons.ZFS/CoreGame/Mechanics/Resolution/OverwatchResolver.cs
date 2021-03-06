﻿using System;
using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.Calculators;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    public class OverwatchResolver
    {
        private List<Character> _overwatchingEnemies = new List<Character>();
        private Moved _moved;
        private bool _hasMoved;
        private bool _hasUpdatedSight;
        private bool _hasUpdatedPerception;

        public OverwatchResolver()
        {
            Event.Subscribe<TurnBegun>(x => UpdateOverwatchersAndReset(), this);
            Event.Subscribe<Moved>(OnMovement, this);
            Event.Subscribe<TilesSeen>(_ => OnSightUpdate(), this);
            Event.Subscribe<TilesPerceived>(_ => OnPerceptionUpdate(), this);
        }

        private void UpdateOverwatchersAndReset()
        {
            _overwatchingEnemies = CurrentData.LivingCharacters.Where(x => x.State.IsOverwatching && x.Team != CurrentData.CurrentCharacter.Team).ToList();
            _hasMoved = false;
            _hasUpdatedSight = false;
            _hasUpdatedPerception = false;
        }

        private void OnMovement(Moved moved)
        {
            _moved = moved;
            _hasMoved = true;
            OnMoveResolved();
        }

        private void OnPerceptionUpdate()
        {
            _hasUpdatedPerception = true;
            OnMoveResolved();
        }

        private void OnSightUpdate()
        {
            _hasUpdatedSight = true;
            OnMoveResolved();
        } 

        private void OnMoveResolved()
        {
            if (!_hasMoved || !_hasUpdatedPerception || !_hasUpdatedSight)
                return;
            _hasMoved = false;
            _hasUpdatedPerception = false;
            _hasUpdatedSight = false;

            var shots = _overwatchingEnemies
                .Where(x => x.State.IsOverwatching && x.State.OverwatchedTiles.ContainsKey(_moved.Position))
                .Select(x => new ShotConfirmed
                {
                    Proposed = new ProposedShotCalculation(x, CurrentData.CurrentCharacter,
                        new ShotCalculation(CurrentData.Map, CurrentData.CurrentCharacter.CurrentTile, x.CurrentTile).GetBestShot(),
                        x.State.OverwatchedTiles[_moved.Position]).CalculateShot()
                })
                .ToList();
            Action action = () => Event.Queue(new MoveResolved { Character = _moved.Character });
            shots.ForEach(x =>
            {
                x.OnFinished = action;
                action = () => Event.Queue(x);
            });
            action();
        }
    }
}
