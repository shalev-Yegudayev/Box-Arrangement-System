using GalaSoft.MvvmLight.Command;
using Logic;
using Logic.DataMembers;
using System;
using System.Text;
using System.Windows;

namespace BoxArrangementSystemFinal.ViewModel
{
    public class ButtonsViewModel
    {
        Manager manager;
        public RelayCommand SupplyBtn { get; set; }
        public RelayCommand GetOfferBtn { get; set; }
        TextBoxViewModel textBoxViewModel;
        public ButtonsViewModel(Manager manager, TextBoxViewModel textBoxViewModel)
        {
            this.textBoxViewModel = textBoxViewModel;
            this.manager = manager;
            SupplyBtn = new RelayCommand(SupplyCLK);
            GetOfferBtn = new RelayCommand(GetOffer);
        }
        private void SupplyCLK()
        {
            ConvertInputToNumber(textBoxViewModel.SupplierX, textBoxViewModel.SupplierY, textBoxViewModel.SupplierAmount,
                out double width, out double height, out int amount);

            int isSupplySuccseded = manager.AddItem(new Box(width, height), amount, out int leftoverAmount);

            if (isSupplySuccseded == -1) MessageBox.Show($"You have to order at least {manager.MinSupplyAmount} boxes","Not Enough Boxes");

            if (isSupplySuccseded == 1) MessageBox.Show($"You tried to order {leftoverAmount} boxes over the limit\nBox amount has been reset to max amount", "Too Many Boxes!");

            ResetAllTextbox();
        }


        private void GetOffer()
        {
            ConvertInputToNumber(textBoxViewModel.BuyerX, textBoxViewModel.BuyerY, textBoxViewModel.BuyerAmount,
                out double width, out double height, out int amount);

            StringBuilder sb = new StringBuilder();
            if (manager.BuyBox(new Box(width, height), amount))
            {

                foreach (var item in manager.OfferedItems)
                {
                    MessageBoxResult offer = MessageBox.Show($"{item.ItemProp}   Amount: {item.Amount}\n availabe, Would you like to add to shopping cart?", "Deal Offer", MessageBoxButton.YesNo);

                    if (offer == MessageBoxResult.Yes)
                    {
                        sb.AppendLine($"{item.ItemProp}\nAmount: {item.AmountAtPurchase}\n");
                        manager.BoughtItems.Add(item);
                    }
                    else break;
                }
                ShowShoppingCart(sb);
            }
            else MessageBox.Show("Were Sorry,\nNo Available item found in our stock", "Purchase Failed");

            ResetAllTextbox();
        }

        private void ResetAllTextbox()
        {
            textBoxViewModel.SupplierX = "";
            textBoxViewModel.SupplierY = "";
            textBoxViewModel.SupplierAmount = "";

            textBoxViewModel.BuyerX = "";
            textBoxViewModel.BuyerY = "";
            textBoxViewModel.BuyerAmount = "";
        }

        private void ShowShoppingCart(StringBuilder sb)
        {
            double.TryParse(textBoxViewModel.BuyerAmount, out double amount1);
            int amount = Convert.ToInt32(Math.Round(amount1));
            MessageBoxResult shoppingCart = MessageBox.Show($"Your Shopping Cart:\n{sb}\nWould You like to accept the purchase?", "Shopping cart", MessageBoxButton.YesNo);

            if (shoppingCart == MessageBoxResult.Yes)
            {
                if (!manager.DealAccepted(amount)) MessageBox.Show($"Only {manager.BoughtItems[manager.BoughtItems.Count - 1].Amount} units left in {manager.BoughtItems[manager.BoughtItems.Count - 1].ItemProp} box,\nReorder needed!!", "Supply Low");
            }

            else if (shoppingCart == MessageBoxResult.No) manager.DealDenied();
        }

        private void ConvertInputToNumber(string x, string y, string amnt, out double width, out double height, out int amount)
        {
            double.TryParse(x, out width);
            double.TryParse(y, out height);
            double.TryParse(amnt, out double amount1);
            amount =Convert.ToInt32(Math.Round(amount1));
        }

    }
}
