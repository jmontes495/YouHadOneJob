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

        private void Start ()
        {
            snake = new Snake (xSize: 18, ySize: 10, obstaclesCount: 8);
            uiSnake.Setup (snake);
            snake.Initialize ();
        }

        protected override string GetInstructionsText ()
        {
            if (snake != null && snake.State == SnakeState.Lost)
                return "Press SPACE to restart";
            return "Move with A, S, D, W";
        }

        protected override string GetTabText ()
        {
            return "Fun!";
        }

        protected override void Tick (bool isFocused)
        {
            if (isFocused && snake != null)
            {
                if (snake.State == SnakeState.Playing)
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
                else
                {
                    if (Input.GetKeyDown (KeyCode.Space))
                        snake.Restart ();
                }
            }
        }
    }

}