﻿using System;
using System.Collections.Generic;
using MonoDragons.Core.Development;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Physics;
using MonoDragons.ZFS.CoreGame;

namespace MonoDragons.ZFS.GUI
{
    public class Highlights : IVisualAutomaton
    {
        private readonly List<IVisual> _visuals = new List<IVisual>();
        private readonly List<IAutomaton> _automata = new List<IAutomaton>();

        public Highlights()
        {
            Add(new AvailableMovesView(CurrentData.Map));
            Add(new AvailableTargetsView());
            Add(new OverwatchedTiles());
            Add(new MovementPathHighlights());
            Add(new MovementPathDirectionsPreview());
            Add(new OverwatchesEnemyTiles());
            Add(new MoveAttackTargetsView());
            Add(new CharacterGroundFootprints());
        }

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ForEach(x => Perf.Time($"Drew {x.GetType().Name}", () => x.Draw(parentTransform)));
        }

        public void Update(TimeSpan delta)
        {
            _automata.ForEach(x => x.Update(delta));
        }

        private void Add(IVisual visual)
        {
            _visuals.Add(visual);
        }

        private void Add(IVisualAutomaton visualAutomaton)
        {
            _visuals.Add(visualAutomaton);
            _automata.Add(visualAutomaton);
        }
    }
}
