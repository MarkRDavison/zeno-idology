namespace Idology.Space.Core.Commands.FindPath;

public sealed class FindPathCommandHandler : ISpaceCommandHandler<FindPathCommand, Vector2[]>
{

    public interface IWeightedGraph<Vector2> where Vector2 : IEquatable<Vector2>
    {
        float Cost(Vector2 from, Vector2 to);

        IEnumerable<Vector2> Neighbors(Vector2 node);

        float Heuristic(Vector2 from, Vector2 to);
    }

    public class YourGraph : IWeightedGraph<Vector2>
    {
        private readonly LevelData _levelData;

        public YourGraph(LevelData levelData)
        {
            _levelData = levelData;
        }

        public float Cost(Vector2 from, Vector2 to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }

        private static bool IsValid(Vector2 coord, LevelData level)
        {
            if (coord.X >= 0 && coord.X < level.Width &&
                coord.Y >= 0 && coord.Y < level.Height)
            {
                var tile = level.Tiles[(int)coord.Y][(int)coord.X];
                return tile.IsEmpty;
            }

            return false;
        }

        public IEnumerable<Vector2> Neighbors(Vector2 node)
        {
            if (IsValid(new(node.X + 1, node.Y), _levelData))
            {
                yield return new(node.X + 1, node.Y);
            }

            if (IsValid(new(node.X - 1, node.Y), _levelData))
            {
                yield return new(node.X - 1, node.Y);
            }

            if (IsValid(new(node.X, node.Y + 1), _levelData))
            {
                yield return new(node.X, node.Y + 1);
            }

            if (IsValid(new(node.X, node.Y - 1), _levelData))
            {
                yield return new(node.X, node.Y - 1);
            }
        }

        public float Heuristic(Vector2 from, Vector2 to)
        {
            //Consider fast and problem-aware heuristic
            return Cost(from, to);
        }
    }

    public class AStarSearch<TGraph> where TGraph : IWeightedGraph<Vector2>
    {
        private TGraph _graph;
        private readonly Dictionary<Vector2, Vector2> _cameFrom = new Dictionary<Vector2, Vector2>();
        private readonly Dictionary<Vector2, float> _costSoFar = new Dictionary<Vector2, float>();

        public AStarSearch(TGraph graph)
        {
            _graph = graph;
        }

        public List<Vector2> CreatePath(Vector2 start, Vector2 goal, bool exhaustive)
        {
            _cameFrom.Clear();
            _costSoFar.Clear();

            var frontier = new PriorityQueue<Vector2, float>();
            frontier.Enqueue(start, 0);

            _cameFrom[start] = start;
            _costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (exhaustive && current.Equals(goal))
                {
                    break;
                }

                foreach (var next in _graph.Neighbors(current))
                {
                    var newCost = _costSoFar[current] + _graph.Cost(current, next);
                    if (!_costSoFar.TryGetValue(next, out var storedNextCost) || newCost < storedNextCost)
                    {
                        _costSoFar[next] = newCost;
                        var priority = newCost + _graph.Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        _cameFrom[next] = current;
                    }
                }
            }

            var result = new List<Vector2>();
            Vector2 prev;
            if (_cameFrom.ContainsKey(goal))
            {
                prev = goal;
                result.Add(prev);
            }
            else
            {
                return [];
            }

            do
            {
                prev = _cameFrom[prev];
                result.Add(prev);
            } while (!prev.Equals(start));

            result.Reverse();

            return result;
        }
    }

    public Vector2[] HandleCommand(FindPathCommand command)
    {
        int startX = (int)command.StartTile.X;
        int startY = (int)command.StartTile.Y;
        int endX = (int)command.EndTile.X;
        int endY = (int)command.EndTile.Y;

        if (startX < 0 || startX >= command.LevelData.Width ||
            startY < 0 || startY >= command.LevelData.Height ||
            endX < 0 || endX >= command.LevelData.Width ||
            endY < 0 || endY >= command.LevelData.Height)
        {
            return [];
        }

        var graph = new YourGraph(command.LevelData);

        var search = new AStarSearch<YourGraph>(graph);

        var path = search.CreatePath(
            new Vector2(startX, startY),
            new Vector2(endX, endY),
            false);

        return [.. path];
    }
}
