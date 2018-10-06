using System;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.GUI;
using MonoDragons.ZFS.GUI.Hud;

namespace MonoDragons.ZFS.CoreGame.Controls
{
    sealed class MouseControls : IAutomaton
    {
        private readonly Camera _camera;
        private readonly TurnBasedCombat _combat;

        private enum MouseAction
        {
            None,
            Move,
            Shoot,
        }
        
        private MouseAction _mouseAction = MouseAction.None;
        private bool _shouldIgnoreClicks;
        
        public MouseControls(ClickUI clickUi, Camera camera, TurnBasedCombat combat)
        {
            clickUi.Add(new GameWorldClickable(OnGameWorldClick));
            _camera = camera;
            _combat = combat;
            Event.Subscribe(EventSubscription.Create<MovementOptionsAvailable>(e => _mouseAction = MouseAction.Move, this));
            Event.Subscribe(EventSubscription.Create<MovementConfirmed>(e => _mouseAction = MouseAction.None, this));
            Event.Subscribe(EventSubscription.Create<ShootSelected>(e => _mouseAction = MouseAction.Shoot, this));
            Event.Subscribe(EventSubscription.Create<MenuRequested>(e => _shouldIgnoreClicks = true, this));
            Event.Subscribe(EventSubscription.Create<MenuDismissed>(e => _shouldIgnoreClicks = false, this));
        }

        private void OnGameWorldClick()
        {
            var mouse = ScaledMouse.GetState();
            var positionOnMap = mouse.Position + _camera.Position;
            var tilePoint = CurrentData.Map.MapPositionToTile(positionOnMap.ToVector2());
            var x = tilePoint.X;
            var y = tilePoint.Y;
            if (_mouseAction.Equals(MouseAction.Move))
                _combat.MoveTo(x, y);
            if (_mouseAction.Equals(MouseAction.Shoot))
                _combat.Shoot(x, y);
        }

        public void Update(TimeSpan delta)
        {
            if (!CurrentGame.TheGame.IsActive) return;
            
            var positionOnMap = ScaledMouse.GetState().Position + _camera.Position;
            var tilePoint = CurrentData.Map.MapPositionToTile(positionOnMap.ToVector2());
            CurrentData.HoveredTile = tilePoint;
        }
    }
}