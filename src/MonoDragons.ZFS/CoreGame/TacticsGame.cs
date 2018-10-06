using Microsoft.Xna.Framework;
using MonoDragons.Core.Development;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.AI;
using MonoDragons.ZFS.CoreGame.Calculators;
using MonoDragons.ZFS.CoreGame.Controls;
using MonoDragons.ZFS.CoreGame.Mechanics.Resolution;
using MonoDragons.ZFS.GUI;
using Camera = MonoDragons.ZFS.GUI.Camera;

namespace MonoDragons.ZFS.CoreGame
{
    public sealed class TacticsGame : GameObjContainer, IInitializable
    {
        private readonly TurnBasedCombat _combat;
        private readonly Point _startingCameraTile;
        private readonly Camera _camera = new Camera();
        private readonly GameDrawMaster _drawMaster = new GameDrawMaster();

        public TacticsGame(TurnBasedCombat combatEngine, Point startingCameraTile)
        {
            _combat = combatEngine;
            _startingCameraTile = startingCameraTile;
        }

        public void Init()
        {
            GetOffset = () => new Transform2(-_camera.Position.ToVector2());

            var clickUi = new ClickUI();
            Add(new MouseControls(clickUi, _camera, _combat));
            Add(new KeyboardControls());
            Add(clickUi);
            Add(new EnemyAI());
            Add(new ActionOptionsCalculator());
            Add(new HideUI());
            Add(new MovementOptionsCalculator());
            Add(new ShootOptionsCalculator());
            Add(new ProposedShotCalculator());
            Add(new AvailableTargetsUI());
            Add(new FriendlyVisionCalculator());
            Add(new NewEnemySpotter());
            Add(new DialogWatcher());
            Add(new FinishActionIfCurrentCharacterDies());
            Add(new LevelEndConditions());
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

            Add(new LevelInitialVisibilityCalculator(
                new VisibilityCalculator(),
                new PerceptionCalculator(), 
                new FriendlyPerceptionUpdater()).Initialized());
            _combat.Init();
            _camera.Init(_startingCameraTile);
        }
    }
}
