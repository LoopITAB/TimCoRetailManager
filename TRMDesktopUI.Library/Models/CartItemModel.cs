
namespace TRMDesktopUI.Library.Models
{
    public class CartItemModel
    {
        public ProductModel Product { get; set; }
        public int QuantityInCart { get; set; }
        ////DisplayText is no longer needed, after we created CartItemDisplayModel in the UI project.
        ////Much cleaner now !!!!!
        //public string DisplayText
        //{
        //    get
        //    {
        //        return $"({QuantityInCart}) {Product.ProductName}";
        //    }
        //}
    }
}