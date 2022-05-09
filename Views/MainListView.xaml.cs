using BoxArrangementSystemFinal.ViewModel;
using System.Windows.Controls;

namespace BoxArrangementSystemFinal.Views
{
    public partial class MainListView : UserControl
    {
        public MainListView() => InitializeComponent();

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => MainListViewModel.ItemSelected();
    }

}
