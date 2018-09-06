using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.AI
{
    public class SpotHasGoodCoverCalculation
    {
        private readonly AICharacterData _data;
        private readonly Point _position;

        public SpotHasGoodCoverCalculation(AICharacterData data, Point position)
        {
            _data = data;
            _position = position;
        }

        public bool Calculate()
        {
            return HasAMatch(GetCoverDirections(), GetLastSeenPlayerDirections());
        }

        private bool HasAMatch(List<Direction> coverDirections, List<Direction> playerDirections)
        {
            return coverDirections.Any(playerDirections.Contains);
        }

        private List<Direction> GetLastSeenPlayerDirections()
        {
            return _data.SeenEnemies.SelectMany(x => _position.PrimaryDirectionsTowards(x.Value)).Distinct().ToList();
        }

        private List<Direction> GetCoverDirections()
        {
            var directions = new List<Direction>();
            if (IsPointCover(new Point(_position.X, _position.Y - 1)))
                directions.Add(Direction.Up);
            if (IsPointCover(new Point(_position.X, _position.Y + 1)))
                directions.Add(Direction.Down);
            if (IsPointCover(new Point(_position.X + 1, _position.Y)))
                directions.Add(Direction.Right);
            if (IsPointCover(new Point(_position.X - 1, _position.Y)))
                directions.Add(Direction.Left);
            return directions;
        }

        private bool IsPointCover(Point point) => CurrentData.Map.Exists(point) && CurrentData.Map[point].Cover > Cover.None;
    }
}
