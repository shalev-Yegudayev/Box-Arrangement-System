using System;

namespace Logic.DataMembers
{
    public class TimeData : IComparable<TimeData>
    {
        public Box ItemTimeProp { get; set; }
        public DateTime LastTimeSold { get; set; }

        public TimeData(Box item)
        {
            ItemTimeProp = item;
            LastTimeSold = DateTime.Now;
        }

        public int CompareTo(TimeData other) => ItemTimeProp.CompareTo(other.ItemTimeProp);

    }
}
