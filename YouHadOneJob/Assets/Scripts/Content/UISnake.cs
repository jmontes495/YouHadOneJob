using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    public class UISnake : MonoBehaviour
    {
        [SerializeField]
        private Sprite headSprite;
        [SerializeField]
        private Sprite tailSprite;
        [SerializeField]
        private Sprite bodyLinearSprite;
        [SerializeField]
        private Sprite bodyCurvedSprite;
        [SerializeField]
        private Sprite explosionSprite;
        [SerializeField]
        private Sprite foodSprite;
        [SerializeField]
        private Sprite obstacleSprite;
        [SerializeField]
        private GameObject gridLayout;

        private Image[] gridImages;
        private int xSize;

        private void Awake ()
        {
            gridImages = gridLayout.GetComponentsInChildren<Image> (true);
        }

        public void Setup (Snake snake)
        {
            xSize = snake.XSize;
            snake.OnCreateFood += OnCreateFood;
            snake.OnCreateSnake += OnCreateSnake;
            snake.OnMoveHead += OnMoveHead;
            snake.OnMoveTail += OnMoveTail;
            snake.OnLose += OnLose;
            snake.OnRestart += OnRestart;
        }

        private void OnCreateFood (Tile food)
        {
            SetImage (food, foodSprite, SnakeDirection.Up);
        }

        private void OnCreateSnake (Tile head, Tile tail)
        {
            SetImage (head, headSprite, GetDirection (tail, head));
            SetImage (tail, tailSprite, GetDirection (tail, head));
        }

        private void OnMoveHead (Tile prevHead, Tile head, SnakeDirection lastDirection)
        {
            SnakeDirection direction = GetDirection (prevHead, head);
            bool hasChangedDirection = lastDirection != direction;
            if (hasChangedDirection)
                SetImage (prevHead, bodyCurvedSprite, GetEulerRotationCurved (lastDirection, direction));
            else
                SetImage (prevHead, bodyLinearSprite, lastDirection);
            SetImage (head, headSprite, direction);
        }

        private void OnMoveTail (Tile prevTail, Tile tail, Tile postTail)
        {
            SetImage (prevTail, null, SnakeDirection.Up);
            SetImage (tail, tailSprite, GetDirection (tail, postTail));
        }

        private void OnLose (Tile head)
        {
            SetImage (head, explosionSprite, SnakeDirection.Up);
        }

        private void OnRestart ()
        {
            foreach (Image image in gridImages)
            {
                image.sprite = null;
                image.color = Color.clear;
                image.rectTransform.localRotation = Quaternion.Euler (GetEulerRotation (SnakeDirection.Up));
            }
        }

        private void SetImage (Tile tile, Sprite sprite, SnakeDirection rotation)
        {
            SetImage (tile, sprite, GetEulerRotation (rotation));
        }

        private void SetImage (Tile tile, Sprite sprite, Vector3 eulerRotation)
        {
            int index = tile.x + (tile.y * xSize);
            Image image = gridImages[index];
            image.sprite = sprite;
            image.color = sprite ? Color.white : Color.clear;
            image.rectTransform.localRotation = Quaternion.Euler (eulerRotation);
        }

        private Vector3 GetEulerRotation (SnakeDirection rotation)
        {
            switch (rotation)
            {
                case SnakeDirection.Right:
                    return new Vector3 (0, 0, 0);
                case SnakeDirection.Left:
                    return new Vector3 (0, 180, 0);
                case SnakeDirection.Up:
                    return new Vector3 (0, 0, 90);
                case SnakeDirection.Down:
                    return new Vector3 (0, 0, -90);
            }
            throw new UnityException ();
        }

        private Vector3 GetEulerRotationCurved (SnakeDirection lastRotation, SnakeDirection rotation)
        {
            if (lastRotation == SnakeDirection.Up && rotation == SnakeDirection.Right || lastRotation == SnakeDirection.Left && rotation == SnakeDirection.Down)
                return new Vector3 (0, 0, -90);
            if (lastRotation == SnakeDirection.Up && rotation == SnakeDirection.Left || lastRotation == SnakeDirection.Right && rotation == SnakeDirection.Down)
                return new Vector3 (0, 0, 180);
            if (lastRotation == SnakeDirection.Down && rotation == SnakeDirection.Right || lastRotation == SnakeDirection.Left && rotation == SnakeDirection.Up)
                return new Vector3 (0, 0, 0);
            if (lastRotation == SnakeDirection.Down && rotation == SnakeDirection.Left || lastRotation == SnakeDirection.Right && rotation == SnakeDirection.Up)
                return new Vector3 (0, 0, 90);
            throw new UnityException ();
        }

        private SnakeDirection GetDirection (Tile tail, Tile postTail)
        {
            if (tail.x + 1 == postTail.x)
                return SnakeDirection.Right;
            else if (tail.x - 1 == postTail.x)
                return SnakeDirection.Left;
            else if (tail.y + 1 == postTail.y)
                return SnakeDirection.Up;
            else if (tail.y - 1 == postTail.y)
                return SnakeDirection.Down;
            throw new UnityException ();
        }
    }
}