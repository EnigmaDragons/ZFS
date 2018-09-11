using System;
using System.Collections.Generic;
using MonoDragons.Core.Development;
using MonoDragons.Core.Physics;

namespace MonoDragons.Core.Engine
{
    public abstract class GameObjContainer : IVisualAutomaton
    {
        private readonly List<IVisual> _visuals = new List<IVisual>();
        private readonly List<IAutomaton> _automata = new List<IAutomaton>();
        private readonly List<object> _actors = new List<object>();
        private readonly bool _useAbsolutePosition;
        private bool _isInitialized;

        protected virtual Func<Transform2> GetOffset { get; set; }

        protected void Add(IVisual visual) =>  OnlyDuringInit(() => _visuals.Add(visual));
        protected void Add(IAutomaton automaton) => OnlyDuringInit(() => _automata.Add(automaton));
        protected void Add(object actor) => OnlyDuringInit(() => _actors.Add(actor));
        
        public GameObjContainer()
            : this(false) { }

        public GameObjContainer(bool useAbsolutePosition)
        {
            _useAbsolutePosition = useAbsolutePosition;
            GetOffset = () => Transform2.Zero;
        }

        protected void Add(IVisualAutomaton obj)
        {
            OnlyDuringInit(() =>
            {
                Add((IVisual) obj);
                Add((IAutomaton) obj);
            });
        }
        
        public virtual void Draw(Transform2 parentTransform)
        {
            var t = _useAbsolutePosition ? Transform2.Zero : parentTransform + GetOffset();
            _visuals.ForEach(x =>
            {
                Perf.Time($"Drew {x.GetType().Name}", ()  => x.Draw(t), 20);
            });
        }

        public virtual void Update(TimeSpan delta)
        {
            _isInitialized = true;
            _automata.ForEach(x => x.Update(delta));
        }

        private void OnlyDuringInit(Action action)
        {
            if (_isInitialized)
                throw new InvalidOperationException("May not Add new elements to the Scene after Initialization");
            action();
        }
    }
}
