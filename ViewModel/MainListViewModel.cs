using DataStructures;
using Logic;
using Logic.DataMembers;
using System;
using System.Windows;

namespace BoxArrangementSystemFinal.ViewModel
{
    public class MainListViewModel
    {
        public ObservableDictionary<string, string> WholeStorage { get; set; }

        public string CurrentItem { get; set; }
        static public Action ItemSelected { get; set; }
        readonly Manager manager;
        public MainListViewModel(Manager manager)
        {
            this.manager = manager;
            WholeStorage = manager.ItemsHashTable;
            ItemSelected = new Action(ListView_SelectionChanged);
        }

        void ListView_SelectionChanged()
        {
            string x = CurrentItem.Substring(CurrentItem.IndexOf("X:") + 2, CurrentItem.IndexOf('/') - CurrentItem.IndexOf("X:") - 2);
            double.TryParse(x, out double width);
            string y = CurrentItem.Substring(CurrentItem.IndexOf(' ') + 1, CurrentItem.IndexOf(',') - CurrentItem.IndexOf(" ") - 1);
            double.TryParse(y, out double height);
            manager.ItemTree.Search(new ValueData(new Box(width, height)), out ValueData data);
            MessageBox.Show(data.ToString(),"Box Details");
        }
    }
}
