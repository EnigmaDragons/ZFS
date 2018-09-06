using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.Calculators;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.Themes;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.GUI
{
    public class MoveAttackTargetsView : IVisualAutomaton
    {
        private readonly List<IVisual> _emptyVisuals = new List<IVisual>();
        private readonly List<IAutomaton> _emptyAutomata = new List<IAutomaton>();

        private List<Point> _possibleMoves = new List<Point>();
        private Dictionary<Point, List<Character>> _availableTargetsMap = new Dictionary<Point, List<Character>>();
        private Dictionary<Point, List<IVisual>> _attackTargetAggregateVisualMap = new Dictionary<Point, List<IVisual>>();
        private Dictionary<Point, List<IAutomaton>> _attackTargetAggregateAutomataMap = new Dictionary<Point, List<IAutomaton>>();
        private Dictionary<Character, List<IVisual>> _attackTargetVisualMap = new Dictionary<Character, List<IVisual>>();
        private Dictionary<Character, List<IAutomaton>> _attackTargetAutomataMap = new Dictionary<Character, List<IAutomaton>>();
        private List<IVisual> _visuals = new List<IVisual>();
        private List<IAutomaton> _automata = new List<IAutomaton>();
        private Point _hoveredPoint;

        public MoveAttackTargetsView()
        {
            Event.Subscribe(EventSubscription.Create<MovementOptionsAvailable>(ShowOptions, this));
            Event.Subscribe(EventSubscription.Create<MovementConfirmed>(e => ClearOptions(), this));
        }

        private void ClearOptions()
        {
            _possibleMoves = new List<Point>();
            _availableTargetsMap = new Dictionary<Point, List<Character>>();
            _attackTargetAggregateVisualMap = new Dictionary<Point, List<IVisual>>();
            _attackTargetAggregateAutomataMap = new Dictionary<Point, List<IAutomaton>>();
            _attackTargetVisualMap = new Dictionary<Character, List<IVisual>>();
            _attackTargetAutomataMap = new Dictionary<Character, List<IAutomaton>>();
            _visuals = new List<IVisual>();
            _automata = new List<IAutomaton>();
        }

        private void ShowOptions(MovementOptionsAvailable e)
        {
            _possibleMoves = e.AvailableMoves.Select(x => x.Last()).ToList();
        }

        public void Update(TimeSpan delta)
        {
            if (CurrentData.CurrentCharacter.Team != Team.Friendly)
                return;
            _automata.ToList().ForEach(x => x.Update(delta));
            if (_hoveredPoint == CurrentData.HoveredTile)
                return;
            _hoveredPoint = CurrentData.HoveredTile;
            if (_possibleMoves.Contains(_hoveredPoint))
            {
                UpdateTargets();
            }
            else
            {
                _visuals = _emptyVisuals;
                _automata = _emptyAutomata;
            }
        }

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ToList().ForEach(x => x.Draw(parentTransform));
        }

        private void UpdateTargets()
        {
            if (!_availableTargetsMap.ContainsKey(_hoveredPoint))
            {
                _availableTargetsMap[_hoveredPoint] = CurrentData.LivingCharacters.Where(CanShoot).ToList();
                _availableTargetsMap[_hoveredPoint].Where(x => !_attackTargetVisualMap.ContainsKey(x) && CurrentData.FriendlyPerception[x.CurrentTile.Position]).ForEach(x =>
                {
                    _attackTargetVisualMap[x] = new List<IVisual>();
                    _attackTargetAutomataMap[x] = new List<IAutomaton>();
                    _attackTargetVisualMap[x].Add(new ColoredRectangle
                    {
                        Transform = x.CurrentTile.Transform,
                        Color = UiColors.AvailableTargetsView_Rectanges
                    });
                    var anim = new TileRotatingEdgesAnim(x.CurrentTile.Position, UiColors.AvailableTargetsView_TileRotatingEdgesAnim);
                    anim.Init();
                    _attackTargetVisualMap[x].Add(anim);
                    _attackTargetAutomataMap[x].Add(anim);
                });
                _attackTargetAggregateVisualMap[_hoveredPoint] = _availableTargetsMap[_hoveredPoint].SelectMany(x => _attackTargetVisualMap[x]).ToList();
                _attackTargetAggregateAutomataMap[_hoveredPoint] = _availableTargetsMap[_hoveredPoint].SelectMany(x => _attackTargetAutomataMap[x]).ToList();
            }
            _visuals = _attackTargetAggregateVisualMap[_hoveredPoint];
            _automata = _attackTargetAggregateAutomataMap[_hoveredPoint];
        }

        private bool CanShoot(Character target)
        {
            return target.Team == Team.Enemy 
                && CurrentData.CurrentCharacter.Gear.EquippedWeapon.IsRanged
                && CurrentData.CurrentCharacter.Gear.EquippedWeapon.AsRanged().EffectiveRanges.ContainsKey(_hoveredPoint.TileDistance(target.CurrentTile.Position))
                && new ShotCalculation(CurrentData.Map[_hoveredPoint], target.CurrentTile).CanShoot();
        }
    }
}
