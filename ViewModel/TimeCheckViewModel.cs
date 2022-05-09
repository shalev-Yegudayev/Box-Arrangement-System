using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Logic;
using System;
using System.Text;
using System.Windows;

namespace BoxArrangementSystemFinal.ViewModel
{
    public class TimeCheckViewModel : ViewModelBase
    {
        string timeCheck;
        public string TimeCheck { get => timeCheck; set => Set(ref timeCheck, value); }

        Manager manager;
        public RelayCommand TimeCheckClick { get; set; }

        public TimeCheckViewModel(Manager manager)
        {
            this.manager = manager;
            TimeCheckClick = new RelayCommand(CheckExpirationShow);
        }

        public void CheckExpirationShow()
        {
            int.TryParse(timeCheck, out int input);
            StringBuilder stringBuilder = manager.CheckExpirationAndShow(new TimeSpan(0, 0, 0, input));
            MessageBox.Show(stringBuilder.ToString());
        }
    }
}
