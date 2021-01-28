using System;

namespace SnakeClassLibrary
{
    [Serializable]
    public class Record 
    {
        public string Name;
        public int Score;

        public Record(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public Record()
        {
            Name = string.Empty;
            Score = 0;
        }

    }
}
