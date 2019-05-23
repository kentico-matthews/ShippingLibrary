using CMS;
using CMS.DataEngine;
using CMS.Ecommerce;

[assembly: RegisterModule(typeof(ShippingLibrary.CustomCartItemHandler))]
namespace ShippingLibrary
{
    class CustomCartItemHandler : Module
    {
        public CustomCartItemHandler() : base("CustomCartItemHandler")
        { }

        protected override void OnInit()
        {
            base.OnInit();
            ShoppingCartItemInfo.TYPEINFO.Events.Insert.Before += ShoppingCartItem_InsertBefore;
        }
        private void ShoppingCartItem_InsertBefore(object sender, ObjectEventArgs e)
        {

            ShoppingCartItemInfo cartItem = (ShoppingCartItemInfo)e.Object;
            cartItem.CartItemCustomData.SetValue("CartItemGUID", cartItem.CartItemGUID);
        }
    }
}
