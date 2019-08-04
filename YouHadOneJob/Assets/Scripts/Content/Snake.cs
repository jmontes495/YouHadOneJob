using System;
using System.Collections.Generic;
using UnityEngine;

namespace YouHadOneJob
{
    public class Snake
    {
        public event Action<Tile> OnCreateFood;
        public event Action<Tile, Tile> OnCreateSnake;
        public event Action<Tile, Tile, SnakeDirection> OnMoveHead;
        public event Action<Tile, Tile, Tile> OnMoveTail;
        public event Action<Tile> OnLose;
        public event Action OnRestart;

        private int xSize;
        private int ySize;
        private SnakeDirection direction;
        private SnakeDirection nextDirection;
        private Dictionary<Tile, TileState> tiles;
        private LinkedList<Tile> snake;
        private SnakeState state;

        public SnakeState State
        {
            get { return state; }
        }

        public int XSize
        {
            get { return xSize; }
        }

        public Snake (int xSize, int ySize, int obstaclesCount)
        {
            this.xSize = xSize;
            this.ySize = ySize;
            direction = SnakeDirection.Right;
            nextDirection = direction;
            CreateTiles (xSize, ySize);
            state = SnakeState.Lost;
        }

        public void Initialize ()
        {
            CreateSnake (headX: xSize / 2, headY: ySize / 2);
            CreateFood ();
            state = SnakeState.Playing;
        }

        private void CreateTiles (int xSize, int ySize)
        {
            tiles = new Dictionary<Tile, TileState> (xSize * ySize);
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    Tile tile = new Tile (x, y);
                    tiles.Add (tile, TileState.Empty);
                }
            }
        }

        private void CreateSnake (int headX, int headY)
        {
            snake = new LinkedList<Tile> ();
            InitSnake (headX, headY);
        }

        private void InitSnake (int headX, int headY)
        {
            Tile head = new Tile (headX, headY);
            Tile tail = GetNextTile (head, direction.GetOpposite ()).Value;
            snake.AddFirst (head);
            snake.AddLast (tail);
            tiles[head] = TileState.Snake;
            tiles[tail] = TileState.Snake;

            if (OnCreateSnake != null)
                OnCreateSnake (head, tail);
        }

        private void CreateFood ()
        {
            Tile emptyTile = GetRandomEmptyTile ();
            tiles[emptyTile] = TileState.Food;

            if (OnCreateFood != null)
                OnCreateFood (emptyTile);
        }

        private Tile? GetNextTile (Tile tile, SnakeDirection direction)
        {
            switch (direction)
            {
                case SnakeDirection.Right:
                    if (tile.x >= xSize - 1)
                        return null;
                    return new Tile (tile.x + 1, tile.y);
                case SnakeDirection.Left:
                    if (tile.x <= 0)
                        return null;
                    return new Tile (tile.x - 1, tile.y);
                case SnakeDirection.Up:
                    if (tile.y >= ySize - 1)
                        return null;
                    return new Tile (tile.x, tile.y + 1);
                case SnakeDirection.Down:
                    if (tile.y <= 0)
                        return null;
                    return new Tile (tile.x, tile.y - 1);
            }
            throw new UnityException ();
        }

        private TileState GetTileState (Tile? tile)
        {
            if (tile == null)
                return TileState.OutOfGrid;
            return tiles[tile.Value];
        }

        private Tile GetRandomEmptyTile ()
        {
            while (true)
            {
                int randomX = UnityEngine.Random.Range (0, xSize);
                int randomY = UnityEngine.Random.Range (0, ySize);
                Tile tile = new Tile (randomX, randomY);
                TileState tileState = GetTileState (tile);
                if (tileState == TileState.Empty)
                    return tile;
            }
        }

        public void ChangeDirection (SnakeDirection direction)
        {
            if (direction == this.direction.GetOpposite ())
                return;
            nextDirection = direction;
        }

        public void Tick ()
        {
            if (state == SnakeState.Lost)
                return;

            SnakeDirection lastDirection = direction;
            direction = nextDirection;
            Tile? newHead = GetNextTile (snake.First.Value, direction);
            TileState tileState = GetTileState (newHead);
            switch (tileState)
            {
                case TileState.Empty:
                    if (OnMoveHead != null)
                        OnMoveHead (snake.First.Value, newHead.Value, lastDirection);
                    snake.AddFirst (newHead.Value);
                    tiles[newHead.Value] = TileState.Snake;

                    if (OnMoveTail != null)
                        OnMoveTail (snake.Last.Value, snake.Last.Previous.Value, snake.Last.Previous.Previous.Value);
                    tiles[snake.Last.Value] = TileState.Empty;
                    snake.RemoveLast ();
                    break;

                case TileState.Food:
                    if (OnMoveHead != null)
                        OnMoveHead (snake.First.Value, newHead.Value, direction);
                    snake.AddFirst (newHead.Value);
                    tiles[newHead.Value] = TileState.Snake;

                    CreateFood ();
                    break;

                case TileState.Snake:
                case TileState.Obstacle:
                case TileState.OutOfGrid:
                    state = SnakeState.Lost;
                    if (OnLose != null)
                        OnLose (snake.First.Value);
                    break;
            }
        }

        public void Restart ()
        {
            direction = SnakeDirection.Right;
            nextDirection = direction;
            ResetTiles ();
            snake.Clear ();
            if (OnRestart != null)
                OnRestart ();
            InitSnake (headX: xSize / 2, headY: ySize / 2);
            CreateFood ();
            state = SnakeState.Playing;
        }

        private void ResetTiles ()
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    Tile tile = new Tile (x, y);
                    tiles[tile] = TileState.Empty;
                }
            }
        }
    }

}