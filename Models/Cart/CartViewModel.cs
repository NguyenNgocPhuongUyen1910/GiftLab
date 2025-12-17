using System.Collections.Generic;
using System.Linq;

namespace GiftLab.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        public decimal Subtotal => Items.Sum(x => x.LineTotal);

        public decimal ShippingFee => Items.Any() ? 32400 : 0;

        public decimal GrandTotal => Subtotal + ShippingFee;
    }

}
