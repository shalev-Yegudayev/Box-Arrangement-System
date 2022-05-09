using DataStructures;
using Logic.DataMembers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;

namespace Logic
{
    public class Manager
    {
        readonly Supply supply;
        readonly Buy buy;

        public  BST<ValueData> ItemTree;
        public ObservableDictionary<string, string> ItemsHashTable { get; set; }
        readonly DoubleLinkedList<TimeData> timeCheckLst;

        public List<ValueData> OfferedItems { get; set; }
        public List<ValueData> BoughtItems { get; set; }

        readonly Timer timer;

        const int maxItems = 100;
        readonly TimeSpan timeSpan = new TimeSpan(0, 0, 0, 5);//Represents After how much time without purchase a box will consider expired
        public readonly int MinSupplyAmount = 5;

        public Manager()
        {
            supply = new Supply();
            buy = new Buy();

            ItemTree = new BST<ValueData>();
            ItemsHashTable = new ObservableDictionary<string, string>();
            timeCheckLst = new DoubleLinkedList<TimeData>();

            OfferedItems = new List<ValueData>();//holds every item the user will be offered
            BoughtItems = new List<ValueData>();//holds every item that the user chose to buy

            TimeSpan checkPeriod = new TimeSpan(0, 0, 30);//represents the time cycle for the expiration check
            TimeSpan expPeriod = new TimeSpan(1, 0, 0, 10);//Represents the time after which the expiration check will begin firstly

            timer = new Timer(CheckExpirationAndDelete, null, expPeriod, checkPeriod);//REMEMBER TO CHANGE ACCORDING TO CONFIG
        }
        //Checks expiration according to (timespan) variable and deletes what expired from every DS in the manager
        private void CheckExpirationAndDelete(object state)
        {
            while (timeCheckLst.head != null && timeCheckLst.head.Value.LastTimeSold + timeSpan <= DateTime.Now)
            {
                timeCheckLst.RemoveFirst(out TimeData data);
                Application.Current.Dispatcher.BeginInvoke(new Action(() => ItemsHashTable.Remove(data.ItemTimeProp.ToString())));
                ItemTree.Remove(new ValueData(data.ItemTimeProp), out _);
            }
        }

        //Checks expiration according to certain timespan and creates a stringBuilder with the details of every box that didnt sold in that time
        public StringBuilder CheckExpirationAndShow(TimeSpan checkTimeSpan)
        {
            StringBuilder sb = new StringBuilder();
            DoubleLinkedList<TimeData>.Node tmpNode = timeCheckLst.head;
            while (tmpNode != null && tmpNode.Value.LastTimeSold + checkTimeSpan <= DateTime.Now)
            {
                sb.AppendLine($"{tmpNode.Value.ItemTimeProp} Last Time Sold: {tmpNode.Value.LastTimeSold}");
                tmpNode = tmpNode.Next;
            }
            return sb;
        }



        public int AddItem(Box item, int amount, out int leftover)
        {
            //isSuccessfullSupply Will be false only if the user tried to order less than MinSupplyAmount
            bool isSuccessfullSupply = supply.AddItem(ItemTree, ItemsHashTable, timeCheckLst, maxItems, MinSupplyAmount, item, amount, out int leftoverAmount);
            leftover = leftoverAmount;
            if (!isSuccessfullSupply) return -1;

            //Returns 1 when the user tried to order more than maxItems
            if (isSuccessfullSupply && leftoverAmount > 0) return 1;

            //Return 0 when the amount is valid 
            return 0;
        }

        public bool BuyBox(Box box, int amount) => buy.BuyBox(ItemTree, OfferedItems, box, amount);

        public bool DealAccepted(int amount) => buy.DealAccepted(ItemTree, ItemsHashTable, timeCheckLst, OfferedItems, BoughtItems, amount, MinSupplyAmount);

        public void DealDenied() => buy.DeleteAllLists(OfferedItems, BoughtItems);

    }
}

