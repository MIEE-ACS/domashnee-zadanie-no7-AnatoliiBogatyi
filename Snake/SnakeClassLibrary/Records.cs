using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeClassLibrary
{
    [Serializable]
    public class Records
    {
        public List<Record> listRecords = new List<Record>();

        public Records()
        {
        }

        public void AddToRecords(Record item)
        {
            listRecords.Add(item);
            listRecords = listRecords.OrderByDescending(x => x.Score).ThenBy(x => x.Name).ToList();
        }

        public string RecordsToString()
        {
            string answer = string.Empty;
            foreach (var value in listRecords)
            {
                answer += value.Name + " " + value.Score.ToString() + "\n\r";// Так не нужно делать, по хорошему, пишется в xaml
            }
            return answer;
        }
    }
}
