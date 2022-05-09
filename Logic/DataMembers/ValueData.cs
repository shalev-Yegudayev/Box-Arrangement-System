using DataStructures;
using System;

namespace Logic.DataMembers
{
    public class ValueData : IComparable<ValueData>
    {
        public Box ItemProp { get; set; }
        public int Amount { get; set; }
        public int AmountAtPurchase { get; set; } //Will hold the amount that customer asked to but from a certain box  

        public DoubleLinkedList<TimeData>.Node NodeTimeRef { get; set; }

        public ValueData(Box box) => ItemProp = box;

        public int CompareTo(ValueData other) => ItemProp.CompareTo(other.ItemProp);

        public override string ToString() => $"Measurements: {ItemProp}\nAmount: {Amount}\nLast Time Sold: {NodeTimeRef.Value.LastTimeSold}";
    }
}
