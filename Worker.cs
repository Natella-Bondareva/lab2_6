using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_6
{
    [Serializable]
    public class Worker
    {
        public struct Workers : IComparable<Workers>
        {
            public string fullName;
            public string position;
            public int year;
            public Workers(string[] str)
            {
                fullName = str[0] + " " + str[1] + " " + str[2];
                position = str[3];
                year = int.Parse(str[4]);
            }
            public int CompareTo(Workers other)
            {
                return fullName.CompareTo(other.fullName);
            }
        }
    }
}
