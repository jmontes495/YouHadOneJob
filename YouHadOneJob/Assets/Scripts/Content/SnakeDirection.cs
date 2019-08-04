using UnityEngine;

namespace YouHadOneJob
{
    public enum SnakeDirection
    {
        Right = 0,
        Left = 1,
        Up = 2,
        Down = 3,
    }

    public static class SnakeDirectionExtensions
    {
        public static SnakeDirection GetOpposite (this SnakeDirection snakeDirection)
        {
            switch (snakeDirection)
            {
                case SnakeDirection.Right:
                    return SnakeDirection.Left;
                case SnakeDirection.Left:
                    return SnakeDirection.Right;
                case SnakeDirection.Up:
                    return SnakeDirection.Down;
                case SnakeDirection.Down:
                    return SnakeDirection.Up;
            }
            throw new UnityException ();
        }
    }
}