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

        private void Awake ()
        {
            gridImages = gridLayout.GetComponentsInChildren<Image> (true);
        }

        public void Setup (Snake snake)
        {
            snake.OnCreateFood += OnCreateFood;
            snake.OnCreateSnake += OnCreateSnake;
            snake.OnMoveHead += OnMoveHead;
            snake.OnMoveTail += OnMoveTail;
            snake.OnLose += OnLose;
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

        private void OnMoveHead (Tile prevHead, Tile head, SnakeDirection direction)
        {
            SetImage (prevHead, bodyLinearSprite, GetDirection (prevHead, head));
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

        private void SetImage (Tile tile, Sprite sprite, SnakeDirection rotation)
        {
            int index = tile.x + (tile.y * tile.x);
            Image image = gridImages[index];
            image.sprite = sprite;
            image.color = sprite ? Color.white : Color.clear;
            image.rectTransform.localRotation = Quaternion.Euler (GetEulerRotation (rotation));
        }

        private Vector3 GetEulerRotation (SnakeDirection rotation)
        {
            switch (rotation)
            {
                case SnakeDirection.Right:
                    return Vector3.right;
                case SnakeDirection.Left:
                    return Vector3.left;
                case SnakeDirection.Up:
                    return Vector3.up;
                case SnakeDirection.Down:
                    return Vector3.down;
            }
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