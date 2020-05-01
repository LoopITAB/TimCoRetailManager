
using System.ComponentModel;

namespace TRMDesktopUI.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        private int _quantityInCart;

        public ProductDisplayModel Product { get; set; }
        public int QuantityInCart
        {
            get { return _quantityInCart; }
            set
            {
                _quantityInCart = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuantityInCart)));
                CallPropertyChanged(nameof(QuantityInCart));
                CallPropertyChanged(nameof(DisplayText));
            }
        }

        public string DisplayText
        {
            get
            {
                return $"({QuantityInCart}) {Product.ProductName}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}