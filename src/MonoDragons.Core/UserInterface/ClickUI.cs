using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Render;

namespace MonoDragons.Core.UserInterface
{
    public sealed class ClickUI : IAutomaton, IDisposable
    {
        public static readonly ClickableUIElement None = new NoElement();

        private readonly ClickUIBranch _elementLayer = new ClickUIBranch("Base", 0);
        private readonly Action<ClickUIBranch>[] subscribeAction;
        
        private List<ClickUIBranch> _branches;
        private ClickableUIElement _current = None;
        private bool _wasClicked;

        public ClickUI()
        {
            _branches = new List<ClickUIBranch> {_elementLayer};
            subscribeAction = new Action<ClickUIBranch>[] { Add, Remove };
        }

        public void Add(ClickUIBranch branch)
        {
            var branches = GetAllBranchesFrom(branch);
            foreach (var b in branches)
            {
                _branches.Add(b);
                b.Subscribe(subscribeAction);
            }
            _branches = _branches.OrderBy((b) => b.Priority).Reverse().ToList();
        }

        private List<ClickUIBranch> GetAllBranchesFrom(ClickUIBranch branch)
        {
            var branches = new List<ClickUIBranch> { branch };
            foreach (ClickUIBranch subBranch in branch.SubBranches())
                branches.AddRange(GetAllBranchesFrom(subBranch));
            return branches;
        }

        public void Add(ClickableUIElement element)
        {
            _elementLayer.Add(element);
        }

        public void Remove(ClickUIBranch branch)
        {
            var branches = GetAllBranchesFrom(branch);
            foreach (var b in branches)
            {
                _branches.Remove(b);
                b.Unsubscribe(subscribeAction);
                if (b.IsCurrentElement(_current) && _current.IsHovered)
                {
                    Event.Publish(new ActiveElementChanged(_current));
                    _current.OnExitted();
                    _current.IsHovered = false;
                }
            }
        }

        public void Remove(ClickableUIElement element)
        {
            _elementLayer.Remove(element);
        }

        public void Clear()
        {
            _branches.ToList().ForEach(Remove);
        }

        public void Update(TimeSpan delta)
        {
            var mouse = ScaledMouse.GetState();
            if (MouseIsOutOfGame(mouse) || !CurrentGame.TheGame.IsActive)
                return;
            
            var newElement = GetElement(mouse);
            if (newElement != _current)
                ChangeActiveElement(newElement);
            else if (WasMouseReleased(mouse))
                OnReleased(mouse);
            else if (mouse.LeftButton == ButtonState.Pressed)
                OnPressed();
        }

        private bool MouseIsOutOfGame(MouseState mouse)
        {
            return mouse.Position.X < 0 || mouse.Position.X > CurrentDisplay.GameWidth 
                || mouse.Position.Y < 0 || mouse.Position.Y > CurrentDisplay.GameHeight;
        }

        private void OnPressed()
        {
            if (_wasClicked)
                return;

            _current.OnPressed();
            _wasClicked = true;
        }

        private void OnReleased(MouseState mouse)
        {
            _current.OnReleased();
            _wasClicked = false;
            if(_current == GetElement(mouse))
                _current.OnEntered();
        }

        private void ChangeActiveElement(ClickableUIElement newElement)
        {
            if (_current.IsHovered)
            {
                _current.OnExitted();
                _current.IsHovered = false;
            }
            Event.Publish(new ActiveElementChanged(_current.IsHovered ? _current : None, newElement));
            _wasClicked = false;
            _current = newElement;
            _current.OnEntered();
            _current.IsHovered = true;
        }

        private bool WasMouseReleased(MouseState mouse)
        {
            return _wasClicked && WasReleased(mouse) && IsSameElement(mouse);
        }

        private bool IsSameElement(MouseState mouse)
        {
            return ReferenceEquals(_current, GetElement(mouse));
        }

        private static bool WasReleased(MouseState mouse)
        {
            return mouse.LeftButton == ButtonState.Released;
        }

        private ClickableUIElement GetElement(MouseState mouse)
        {
            var position = mouse.Position;
            var branch = _branches.Find((b) => b.GetElement(position) != None);
            return branch != null ? branch.GetElement(position) : None;
        }

        public void Dispose()
        {
            if (_current.IsHovered)
            {
                _current.OnExitted();
                Event.Publish(new ActiveElementChanged(_current));
            }
            _current = None;
        }
    }
}
