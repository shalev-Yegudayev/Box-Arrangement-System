using DataStructures;
using Logic.DataMembers;
using System;
using System.Collections.Generic;

namespace Logic
{
    internal class Buy
    {
        private void AddToTimeCheckListBuy(DoubleLinkedList<TimeData> timeCheckLst, ValueData data)
        {
            timeCheckLst.DisconnectNode(data.NodeTimeRef);
            data.NodeTimeRef.Value.LastTimeSold = DateTime.Now;
            timeCheckLst.AddLast(new TimeData(data.ItemProp));
            data.NodeTimeRef = timeCheckLst.tail;
        }

        internal bool BuyBox(BST<ValueData> itemTree, List<ValueData> offeredItems, Box box, int amount)
        {
            ValueData desiredData = new ValueData(box);
            bool isFound = itemTree.Search(desiredData, out ValueData foundItem);

            //The tree contains the desired box
            if (isFound)
            {
                AddToOfferedItemsList(itemTree, offeredItems, amount, foundItem);
            }
            else
            {
                //if theres an alternative for the desired box
                bool isClosestBox = itemTree.FindClosetBiggerNotEqual(desiredData, out ValueData closestBox);
                if (isClosestBox)
                {
                    AddToOfferedItemsList(itemTree, offeredItems, amount, closestBox);
                }
                //If theres not a single option for the user to buy
                else return false;
            }
            return true;
        }

        private void AddToOfferedItemsList(BST<ValueData> itemTree, List<ValueData> offeredItems, int amount, ValueData foundItem)
        {
            ValueData tempData = foundItem;

            //The ammount of desired box is enough for the current buy
            if (foundItem.Amount >= amount)
            {
                AddToOfferCalcTempAmount(offeredItems, foundItem, amount);
            }
            //The ammount of desired box is not enough for the current buy
            else
            {
                AddToOfferCalcTempAmount(offeredItems, foundItem, foundItem.Amount);

                //Stands for the variable that holds the amount of the customer order we already covered so far
                int checkAmount = foundItem.Amount;

                //Loop for the deal offer splits (2 splits = max offered items = 3)
                for (int i = 0; i < 2; i++)
                {
                    if (itemTree.FindClosetBiggerNotEqual(tempData, out tempData))
                    {
                        if (tempData.Amount < amount - checkAmount)
                        {
                            checkAmount += tempData.Amount;
                            AddToOfferCalcTempAmount(offeredItems, tempData, tempData.Amount);
                        }
                        else
                        {
                            AddToOfferCalcTempAmount(offeredItems, tempData, amount - checkAmount);
                            break;
                        }
                    }
                    //if we dont have an appropriate alternative we stop the deal offer
                    else break;
                }
            }
        }

        private void AddToOfferCalcTempAmount(List<ValueData> offeredItems, ValueData foundItem, int amountToCalculate)
        {
            offeredItems.Add(foundItem);
            foundItem.AmountAtPurchase = amountToCalculate;
        }

        //Iterates through evey offered item and acts according to remaining amount after purchase
        internal bool DealAccepted(BST<ValueData> itemTree, ObservableDictionary<string, string> itemsHashTable, DoubleLinkedList<TimeData> timeCheckLst,
            List<ValueData> offeredItems, List<ValueData> boughtItems, int amount, int minAmount)
        {
            foreach (var item in boughtItems)
            {
                //Reduces Amount from Item and resets its LastTimeSold Property
                if (item.Amount > amount)
                {
                    item.Amount -= amount;
                    AddToTimeCheckListBuy(timeCheckLst, item);

                    if (item.Amount < minAmount) return false;
                }
                //if we tried to order more than the amount of current product
                else
                {
                    amount -= item.Amount;
                    if (itemTree.Remove(item, out _))
                    {
                        itemsHashTable.Remove(item.ItemProp.ToString());
                        timeCheckLst.DisconnectNode(item.NodeTimeRef);
                    }
                }
            }
            DeleteAllLists(offeredItems, boughtItems);
            return true;
        }

        internal void DeleteAllLists(List<ValueData> offeredItems, List<ValueData> boughtItems)
        {
            offeredItems.Clear();
            boughtItems.Clear();
        }
    }
}
