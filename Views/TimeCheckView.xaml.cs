using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace BoxArrangementSystemFinal.Views
{
    public partial class TimeCheckView : UserControl
    {
        public TimeCheckView() => InitializeComponent();

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

    }
}
