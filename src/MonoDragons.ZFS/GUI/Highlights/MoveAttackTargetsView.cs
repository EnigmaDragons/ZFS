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
        private DictionaryWithDefault<Point, List<Character>> _availableTargetsMap;
        private DictionaryWithDefault<Point, List<IVisual>> _attackTargetAggregateVisualMap;
        private DictionaryWithDefault<Point, List<IAutomaton>> _attackTargetAggregateAutomataMap;
        private DictionaryWithDefault<Character, List<IVisual>> _attackTargetVisualMap;
        private DictionaryWithDefault<Character, List<IAutomaton>> _attackTargetAutomataMap;
        private List<IVisual> _visuals = new List<IVisual>();
        private List<IAutomaton> _automata = new List<IAutomaton>();
        private Point _hoveredPoint;

        public MoveAttackTargetsView()
        {
            ClearOptions();
            Event.Subscribe(EventSubscription.Create<MovementOptionsAvailable>(ShowOptions, this));
            Event.Subscribe(EventSubscription.Create<MovementConfirmed>(e => ClearOptions(), this));
        }

        private void ClearOptions()
        {
            _possibleMoves = new List<Point>();
            _availableTargetsMap = new DictionaryWithDefault<Point, List<Character>>(new List<Character>());
            _attackTargetAggregateVisualMap = new DictionaryWithDefault<Point, List<IVisual>>(new List<IVisual>());
            _attackTargetAggregateAutomataMap = new DictionaryWithDefault<Point, List<IAutomaton>>(new List<IAutomaton>());
            _attackTargetVisualMap = new DictionaryWithDefault<Character, List<IVisual>>(new List<IVisual>());
            _attackTargetAutomataMap = new DictionaryWithDefault<Character, List<IAutomaton>>(new List<IAutomaton>());
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
            var availableTargets = _availableTargetsMap;
            _availableTargetsMap = new DictionaryWithDefault<Point, List<Character>>(new List<Character>());
            if (!availableTargets.ContainsKey(_hoveredPoint))
            {
                availableTargets[_hoveredPoint] = CurrentData.LivingCharacters.Where(CanShoot).ToList();
                availableTargets[_hoveredPoint].Where(x => !_attackTargetVisualMap.ContainsKey(x) && CurrentData.FriendlyPerception[x.CurrentTile.Position]).ForEach(x =>
                {
                    var anim = new TileRotatingEdgesAnim(x.CurrentTile.Position, UiColors.AvailableTargetsView_TileRotatingEdgesAnim).Initialized();
                    _attackTargetAutomataMap[x] = new List<IAutomaton> { anim };
                    _attackTargetVisualMap[x] = new List<IVisual>
                    {
                        new ColoredRectangle
                        {
                            Transform = x.CurrentTile.Transform,
                            Color = UiColors.AvailableTargetsView_Rectanges
                        },
                        anim
                    };
                });
                _attackTargetAggregateVisualMap[_hoveredPoint] = availableTargets[_hoveredPoint].SelectMany(x => _attackTargetVisualMap[x]).ToList();
                _attackTargetAggregateAutomataMap[_hoveredPoint] = availableTargets[_hoveredPoint].SelectMany(x => _attackTargetAutomataMap[x]).ToList();
            }
            _visuals = _attackTargetAggregateVisualMap[_hoveredPoint];
            _automata = _attackTargetAggregateAutomataMap[_hoveredPoint];
        }

        private bool CanShoot(Character target)
        {
            return target.Team == Team.Enemy 
                && CurrentData.CurrentCharacter.Gear.EquippedWeapon.IsRanged
                && CurrentData.CurrentCharacter.Gear.EquippedWeapon.AsRanged().EffectiveRanges.ContainsKey(_hoveredPoint.TileDistance(target.CurrentTile.Position))
                && new ShotCalculation(CurrentData.Map, CurrentData.Map[_hoveredPoint], target.CurrentTile).CanShoot();
        }
    }
}
