using GalaSoft.MvvmLight;

namespace BoxArrangementSystemFinal.ViewModel
{
    public class TextBoxViewModel : ViewModelBase
    {
        string buyerX;
        public string BuyerX { get => buyerX; set => Set(ref buyerX, value); }

        string buyerY;
        public string BuyerY { get => buyerY; set => Set(ref buyerY, value); }

        string buyerAmount;
        public string BuyerAmount { get => buyerAmount; set => Set(ref buyerAmount, value); }

        string supplierX;
        public string SupplierX { get => supplierX; set => Set(ref supplierX, value); }

        string supplierY;
        public string SupplierY { get => supplierY; set => Set(ref supplierY, value); }

        string supplierAmount;
        public string SupplierAmount { get => supplierAmount; set => Set(ref supplierAmount, value); }

    }
}
