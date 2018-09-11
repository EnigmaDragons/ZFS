using System;
using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core.Physics;

namespace MonoDragons.Core.Engine
{
    public sealed class GameRoot : IInitializableVisualAutomaton
    {
        private readonly List<IInitializable> _initializables;
        private readonly List<IVisual> _visuals;
        private readonly List<IAutomaton> _automata;
        
        public GameRoot(params object[] objs)
        {
            _initializables = objs.Where(x => x is IInitializable).Cast<IInitializable>().ToList();
            _visuals = objs.Where(x => x is IVisual).Cast<IVisual>().ToList();
            _automata = objs.Where(x => x is IAutomaton).Cast<IAutomaton>().ToList();
        }

        public void Init() => _initializables.ForEach(x => x.Init());
        public void Update(TimeSpan delta) => _automata.ForEach(x => x.Update(delta));
        public void Draw(Transform2 parentTransform) => _visuals.ForEach(x => x.Draw(parentTransform));
    }
}