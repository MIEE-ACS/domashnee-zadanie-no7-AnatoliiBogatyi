using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeClassLibrary
{
    public class SnakeGame : ISnakeGame
    {
        Random rnd = new Random();
        const int SizeX = 50;
        const int SizeY = 22;
        const int SnakeLength = 3;
        public int Score = 0;


        private Snake mySnake;
        public int OneMoreRound { get; set; }

        Point currentPrize;
        Direction currentDirection;
        GameState gameState;

        public Action<Point, Item> Draw { get; set; }

        public GameState State
        {
            get { return gameState; }
        }

        public void Initialization()
        {
            OneMoreRound = 0;
            mySnake = new Snake(SnakeLength);
            currentPrize = PrizeFactory();
            currentDirection = Direction.Right;

            Draw(currentPrize, Item.Prize);
            SnakeDraw();

            gameState = GameState.Begin;
        }

        public void Instruction(Direction direction)
        {
            if (direction == Direction.Error)
            {
                Stop();
                return;
            }
            if(Math.Abs(currentDirection - direction) != 1//Как-то так себе получилось
                || currentDirection == Direction.Down && direction == Direction.Left || direction == Direction.Down && currentDirection == Direction.Left)
                currentDirection = direction;
        }

        public void Start()
        {
            gameState = GameState.Process;
        }

        public void Step()
        {
            mySnake.AddSegment(currentDirection);


            Queue<Point> trashTemp = new Queue<Point>();
            for (int i = 0; i < mySnake.segments.Count; ++i)
            {
                Point tempP = mySnake.segments.ElementAt(i);
                if (i == 0)
                    tempP.feature = true;
                else
                    tempP.feature = false;
                trashTemp.Enqueue(tempP);
                Draw(tempP, Item.SnakeSegment);
            }
            mySnake.segments = trashTemp;

            Point temp = mySnake.Segments.Last();
            temp.feature = true;
            Draw(temp, Item.SnakeSegment);

            if (mySnake.Segments.Last().X == currentPrize.X && mySnake.Segments.Last().Y == currentPrize.Y)
            {
                if (currentPrize.feature == true)
                    OneMoreRound += 5;
                currentPrize = PrizeFactory();
                Draw(currentPrize, Item.Prize);
                return;
            }
            else if (IsSegment())
            {
                Stop();
            }
            else if (IsRegion(mySnake.segments.Last()) != Direction.Error)
            {
                if (OneMoreRound > 0)
                {
                    --OneMoreRound;
                    TeleportSnake(IsRegion(mySnake.segments.Last()));
                }
                else
                    Stop();
            }
            Draw(mySnake.DeleteSegment(), Item.Zero);
        }

        public void Stop()
        {
            gameState = GameState.End;
        }


        private void TeleportSnake(Direction dir)
        {
            TelDraw(dir);
        }

        private void TelDraw(Direction dir)
        {
            Queue<Point> trashTemp = new Queue<Point>();
            for (int i = 0; i < mySnake.segments.Count; ++i)
            {
                Point tempP = mySnake.segments.ElementAt(i);
                if(i == mySnake.segments.Count - 1)
                {
                    switch (dir)
                    {
                        case Direction.Right:
                            tempP.X = SizeX + 1;
                            break;
                        case Direction.Left:
                            tempP.X = - 1;//Тк следующий шаг изменит на 1
                            break;
                        case Direction.Down:
                            tempP.Y = SizeY + 1;
                            break;
                        case Direction.Up:
                            tempP.Y = - 1;
                            break;
                        default:
                            break;
                    }
                }
                trashTemp.Enqueue(tempP);
                Draw(tempP, Item.SnakeSegment);
            }
            mySnake.segments = trashTemp;
            
        }

        private void SnakeDraw()
        {
            foreach (var point in mySnake.Segments)
            {
                Draw(point, Item.SnakeSegment);
            }
        }

        private Point PrizeFactory()
        {
            Point prize = mySnake.Segments.ElementAt(0);
            while (mySnake.Segments.Contains(prize))
            {
                if (rnd.Next(1, 100) > 50/*94*/)//Выкрутил побольше, чтобы проще было найти
                {
                    prize = new Point { feature = true, X = rnd.Next(0, SizeX), Y = rnd.Next(0, SizeY) };
                    return prize;
                }
                prize = new Point { feature = false, X = rnd.Next(0, SizeX), Y = rnd.Next(0, SizeY) };

            }
            return prize;
        }

        private bool IsSegment()
        {
            int c = mySnake.Segments.Count();
            return mySnake.Segments.Take(c - 1).Contains(mySnake.Segments.Last());
        }

        private Direction IsRegion(Point segment)
        {
            if(segment.X < 0)
            {
                return Direction.Right;
            }

            if(segment.X > SizeX)
            {
                return Direction.Left;
            }

            if(segment.Y < 0)
            {
                return Direction.Down;
            }

            if(segment.Y > SizeY)
            {
                return Direction.Up;
            }

            return Direction.Error;//Не ошибка на деле)
        }
    }
}
