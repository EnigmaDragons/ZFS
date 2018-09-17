﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Development;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.AI;
using MonoDragons.ZFS.CoreGame.Calculators;
using MonoDragons.ZFS.CoreGame.Controls;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.GUI;
using Camera = MonoDragons.ZFS.GUI.Camera;

namespace MonoDragons.ZFS.CoreGame
{
    public class TacticsGame : GameObjContainer, IInitializable
    {
        private enum MouseAction
        {
            None,
            Move,
            Shoot,
        }

        private readonly TurnBasedCombat _combat;
        private readonly Point _startingCameraTile;
        private readonly Camera _camera = new Camera();
        
        private KeyboardControls _keyboard = new KeyboardControls();
        private MouseState _lastMouseState;
        private MouseAction _mouseAction = MouseAction.None;
        private Point Target;
        private GameDrawMaster _drawMaster = new GameDrawMaster();
        private bool _shouldIgnoreClicks;

        public TacticsGame(TurnBasedCombat combatEngine, Point startingCameraTile)
        {
            _combat = combatEngine;
            _startingCameraTile = startingCameraTile;
        }

        public void Init()
        {
            GetOffset = () => new Transform2(-_camera.Position.ToVector2());

            // TODO: Make Mouse Management a separate component
            Event.Subscribe(EventSubscription.Create<MovementOptionsAvailable>(e => _mouseAction = MouseAction.Move, this));
            Event.Subscribe(EventSubscription.Create<MovementConfirmed>(e => _mouseAction = MouseAction.None, this));
            Event.Subscribe(EventSubscription.Create<ShootSelected>(e => _mouseAction = MouseAction.Shoot, this));
            Event.Subscribe(EventSubscription.Create<MenuRequested>(e => _shouldIgnoreClicks = true, this));
            Event.Subscribe(EventSubscription.Create<MenuDismissed>(e => _shouldIgnoreClicks = false, this));

            var clickUi = new ClickUI();
            Add(clickUi);
            Add(new EnemyAI());
            Add(new ActionOptionsCalculator());
            Add(new HideUI());
            Add(new MovementOptionsCalculator());
            Add(new ShootOptionsCalculator());
            Add(new ProposedShotCalculator());
            Add(new AvailableTargetsUI());
            Add(new VisibilityCalculator());
            Add(new PerceptionCalculator());
            Add(new FriendlyPerceptionUpdater());
            Add(new FriendlyVisionCalculator());
            Add(new NewEnemySpotter());
            Add(new DialogWatcher());
            Add(Event.UseQueue());
            Add(_drawMaster);
            Add(_combat);
            Add(_camera);
#if DEBUG
            var eventsView = new RecentEventDebugLogView { Position = new Vector2(0, 150), MaxLines = 30, HideTextPart = "MonoDragons.ZFS." };
            eventsView.FilterStartsWith.Add("MonoDragons.Core");
            eventsView.FilterStartsWith.Add("MonoDragons.ZFS.CoreGame.StateEvents.TilesSeen");
            eventsView.FilterStartsWith.Add("MonoDragons.ZFS.CoreGame.StateEvents.TilesPerceived");
            eventsView.FilterStartsWith.Add("MonoDragons.ZFS.CoreGame.StateEvents.Moved");
            eventsView.FilterStartsWith.Add("MonoDragons.ZFS.CoreGame.StateEvents.MoveResolved");
            eventsView.FilterStartsWith.Add("MonoDragons.ZFS.AI.AIActionQueued");
            Add(eventsView);
#endif
            Add(new HudView(clickUi));
            CurrentData.Highlights = new Highlights();
            CurrentData.HighHighlights = new HighHighlights();
            Add((IAutomaton)CurrentData.Highlights);
            Add((IAutomaton)CurrentData.HighHighlights);
            Add(CurrentData.PrimaryObjective);

            CalculateInitVision(new VisibilityCalculator(), new PerceptionCalculator(), new FriendlyPerceptionUpdater());
            _combat.Init();
            _camera.Init(_startingCameraTile);
        }

        private static void CalculateInitVision(VisibilityCalculator visibilityCalculator,
            PerceptionCalculator perceptionCalculator, FriendlyPerceptionUpdater perceptionUpdater)
        {
            CurrentData.Characters.ForEach(x =>
            {
                visibilityCalculator.UpdateSight(x);
                perceptionCalculator.UpdatePerception(x);
            });
            perceptionUpdater.UpdatePerception();
        }

        public override void Update(TimeSpan delta)
        {
            var mouse = ScaledMouse.GetState();
            if (CurrentGame.TheGame.IsActive)
            {
                var positionOnMap = mouse.Position + _camera.Position;
                var tilePoint = CurrentData.Map.MapPositionToTile(positionOnMap.ToVector2());
                CurrentData.HoveredTile = tilePoint;

                if (_combat.Map.Exists(tilePoint.X, tilePoint.Y) && mouse.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
                    Target = tilePoint;
                else if (_combat.Map.Exists(Target.X, Target.Y) && Target == tilePoint && mouse.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed)
                    InvokeClickAction(tilePoint.X, tilePoint.Y);
                else if (mouse.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed)
                    Target = new Point(-1, -1);

                _lastMouseState = mouse;
            }
            base.Update(delta);
        }

        private void InvokeClickAction(int x, int y)
        {
            if (_shouldIgnoreClicks)
                return;
            if (_mouseAction.Equals(MouseAction.Move))
                _combat.MoveTo(x, y);
            if (_mouseAction.Equals(MouseAction.Shoot))
                _combat.Shoot(x, y);
        }
    }
}
