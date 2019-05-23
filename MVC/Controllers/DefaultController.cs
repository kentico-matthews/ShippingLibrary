using CMS.DocumentEngine;
using CMS.Ecommerce;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Core;

namespace MVC.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel();
            foreach(var doc in DocumentHelper.GetDocuments().Type("custom.product").Columns("DocumentName, NodeSKUID"))
            {
                Product prod = new Product { Document = doc, Sku = SKUInfoProvider.GetSKUInfo(doc.NodeSKUID) };
                vm.Products.Add(prod);
            }
            return View(vm);
        }
        public ActionResult AddToCart(int SKUID)
        {
            IShoppingService shoppingService = Service.Resolve<IShoppingService>();
            shoppingService.AddItemToCart(SKUID, 1);
            return RedirectToAction("Index");
        }

        public ActionResult ViewCart()
        {
            return View(ECommerceContext.CurrentShoppingCart);
        }

        public ActionResult Shipping()
        {

            return View(ECommerceContext.CurrentShoppingCart);
        }
        [HttpPost]
        public ActionResult UpdateAddress()
        {
            var cart = ShoppingCartInfoProvider.GetShoppingCartInfo(int.Parse(Request["CartID"]));
            var customer = CustomerInfoProvider.GetCustomerInfo(int.Parse(Request["CustomerID"]));
            var address = AddressInfoProvider.GetAddressInfo(int.Parse(Request["AddressID"]));
            var option = ShippingOptionInfoProvider.GetShippingOptions().WhereEquals("ShippingOptionName", "CustomShip").First();

            if (customer == null)
            {
                customer = new CustomerInfo()
                {
                    CustomerFirstName = Request["FirstName"],
                    CustomerLastName = Request["LastName"],
                    CustomerEmail = Request["Email"]
                };
                customer.Insert();
            }
            else
            {
                customer.CustomerFirstName = Request["FirstName"];
                customer.CustomerLastName = Request["LastName"];
                customer.CustomerEmail = Request["Email"];
                customer.Update();
            }

            if (address == null)
            {
                address = new AddressInfo()
                {
                    AddressName = customer.CustomerFirstName + customer.CustomerLastName,
                    AddressPersonalName = customer.CustomerFirstName + customer.CustomerLastName,
                    AddressCustomerID = customer.CustomerID,
                    AddressLine1 = Request["AddressLine1"],
                    AddressLine2 = Request["AddressLine2"],
                    AddressCity = Request["City"],
                    AddressStateID = int.Parse(Request["StateID"]),
                    AddressCountryID = 271,
                    AddressZip = Request["Zip"]
                };
                address.Insert();
            }
            else
            {
                address.AddressName = customer.CustomerFirstName + customer.CustomerLastName;
                address.AddressPersonalName = customer.CustomerFirstName + customer.CustomerLastName;
                address.AddressCustomerID = customer.CustomerID;
                address.AddressLine1 = Request["AddressLine1"];
                address.AddressLine2 = Request["AddressLine2"];
                address.AddressCity = Request["City"];
                address.AddressStateID = int.Parse(Request["StateID"]);
                address.AddressCountryID = 271;
                address.AddressZip = Request["Zip"];
                address.Update();
            }

            cart.Customer = customer;
            cart.ShoppingCartShippingAddress = address;
            cart.ShoppingCartShippingOptionID = option.ShippingOptionID;
            cart.Update();
            

            return RedirectToAction("Shipping");
        }

        public ActionResult Review()
        {
            ShoppingCartItemInfoProvider.LoadShoppingCartItems(ECommerceContext.CurrentShoppingCart);
            return View(ECommerceContext.CurrentShoppingCart);
        }
    }
}