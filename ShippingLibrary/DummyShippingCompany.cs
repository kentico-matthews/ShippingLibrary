using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using CMS.Ecommerce;
using System.Linq;

namespace ShippingLibrary
{
    public class DummyShippingCompany
    {
        public static JToken GetShippingResponse(Delivery delivery, string currencyCode)
        {
            var rand = new Random();
            DummyShipment shipment = new DummyShipment()
            {
                ShipmentID = Guid.NewGuid(),
                ShipmentCost = rand.Next(5, 10),
                ShipmentRecipient = delivery.DeliveryAddress.AddressName,
                ShipmentStreet1 = delivery.DeliveryAddress.AddressLine1,
                ShipmentStreet2 = delivery.DeliveryAddress.AddressLine2,
                ShipmentCity = delivery.DeliveryAddress.AddressCity,
                ShipmentPostalCode = delivery.DeliveryAddress.AddressZip
            };
            foreach(var item in delivery.Items)
            {
                
                DummyShippingItem shippingItem = new DummyShippingItem()
                {
                    ItemID = Guid.NewGuid(),
                    ItemTrackingNumber = "Kentico-" + rand.Next(1000, 9999),
                    ItemSku = item.Product.SKUID.ToString(),
                    ItemExpectedArrival = DateTime.Today.AddDays(rand.Next(2, 15)),
                    ItemWeight = (item.Product.SKUWeight * (double)item.Amount)
                };
                if (shipment.ShipmentItems.Where(i => i.ItemSku == shippingItem.ItemSku).Count() == 0)
                {
                    shipment.ShipmentItems.Add(shippingItem);
                }
            }
            return JToken.FromObject(shipment);
        }
        public static bool CanDeliver(Delivery delivery)
        {
            return true;
        }
    }
    public class DummyShipment
    {
        public Guid ShipmentID;
        public double ShipmentCost;
        public string ShipmentRecipient;
        public string ShipmentStreet1;
        public string ShipmentStreet2;
        public string ShipmentCity;
        public string ShipmentPostalCode;
        public List<DummyShippingItem> ShipmentItems;
        public DummyShipment()
        {
            ShipmentItems = new List<DummyShippingItem>();
        }
    }
    public class DummyShippingItem
    {
        public Guid ItemID;
        public string ItemTrackingNumber;
        public double ItemWeight;
        public DateTime ItemExpectedArrival;
        public string ItemSku;
    }
    
}
