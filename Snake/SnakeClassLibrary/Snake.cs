using System.Collections.Generic;
using System.Linq;

namespace SnakeClassLibrary
{
    public struct Point
    {
        public bool feature;
        public int X;
        public int Y;
        
        public Point (bool afeature, int x = 0, int y = 0)
        {

            feature = afeature;
            X = x;
            Y = y;
        }
    }

    public enum Direction
    {
        Up, Down, Left, Right, Error
    }

    public enum Item
    {
        SnakeSegment, Prize, Zero
    }

    public enum GameState
    {
        Begin, Process, End
    }

    public class Snake
    {
        public Queue<Point> segments;

        public Snake(int length)
        {
            segments = new Queue<Point>();

            for (int i = 0; i < length; i++)
            {
                if(i == 0)
                {
                    segments.Enqueue(new Point(true, i, 0));
                }
                else
                {
                    segments.Enqueue(new Point(false, i, 0));
                }
            }
                
        }

        public IEnumerable<Point> Segments { get { return segments; } }

        public void AddSegment(Direction direction)
        {
            Point newSegment = segments.Last();
            switch (direction)
            {
                case Direction.Up: newSegment.Y--; break;
                case Direction.Down: newSegment.Y++; break;
                case Direction.Left: newSegment.X--; break;
                case Direction.Right: newSegment.X++; break;
            }

            segments.Enqueue(newSegment);
        }

        public Point DeleteSegment()
        {
            Point segment = segments.Dequeue();
            return segment;
        }
    }
}
