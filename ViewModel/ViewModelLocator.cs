using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Logic;

namespace BoxArrangementSystemFinal.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            SimpleIoc.Default.Register<ButtonsViewModel>();
            SimpleIoc.Default.Register<MainListViewModel>();
            SimpleIoc.Default.Register<TextBoxViewModel>();
            SimpleIoc.Default.Register<TimeCheckViewModel>();
            SimpleIoc.Default.Register<Manager>();

        }

        public ButtonsViewModel Buttons => ServiceLocator.Current.GetInstance<ButtonsViewModel>();
        public MainListViewModel MainList => ServiceLocator.Current.GetInstance<MainListViewModel>();
        public TextBoxViewModel TextBoxes => ServiceLocator.Current.GetInstance<TextBoxViewModel>();
        public TimeCheckViewModel TimeCheck => ServiceLocator.Current.GetInstance<TimeCheckViewModel>();
    }
}