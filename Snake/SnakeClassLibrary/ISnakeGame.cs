using System;

namespace SnakeClassLibrary
{
    interface ISnakeGame
    {
        void Initialization();
        void Start();
        void Step();
        void Stop();
        void Instruction(Direction direction);
        Action<Point, Item> Draw { get; set; }

        GameState State { get; }
    }
}
