using UnityEngine;

namespace YouHadOneJob
{
    public class Snakeing : TabContent
    {
        [SerializeField]
        private UISnake uiSnake;

        private const float tickTime = 0.3f;

        private Snake snake;
        private float elapsedTickTime;

        private void Awake ()
        {
            snake = new Snake (xSize: 18, ySize: 10, obstaclesCount: 8);
            uiSnake.Setup (snake);
        }

        protected override string GetInstructionsText ()
        {
            return "";
        }

        protected override string GetTabText ()
        {
            return "WIP";
        }

        protected override void Tick (bool isFocused)
        {
            if (isFocused && snake.State == SnakeState.Playing)
            {
                if (Input.GetKeyDown (KeyCode.A))
                    snake.ChangeDirection (SnakeDirection.Left);
                if (Input.GetKeyDown (KeyCode.S))
                    snake.ChangeDirection (SnakeDirection.Down);
                if (Input.GetKeyDown (KeyCode.D))
                    snake.ChangeDirection (SnakeDirection.Right);
                if (Input.GetKeyDown (KeyCode.W))
                    snake.ChangeDirection (SnakeDirection.Up);

                elapsedTickTime += Time.deltaTime;
                if (elapsedTickTime >= tickTime)
                {
                    elapsedTickTime = 0;
                    snake.Tick ();
                }
            }
        }
    }

}