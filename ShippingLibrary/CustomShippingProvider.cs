using CMS.Base;
using CMS.Ecommerce;
using CMS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: CMS.AssemblyDiscoverable]
namespace ShippingLibrary
{
    public sealed class CustomShippingProvider : ICarrierProvider
    {
        private const string NAME = "Custom carrier provider";
        private List<KeyValuePair<string, string>> services;

        public string CarrierProviderName
        {
            get
            {
                return NAME;
            }
        }

        /// <summary>
        /// Returns a list of service code names and display names.
        /// </summary>
        public List<KeyValuePair<string, string>> GetServices()
        {
            return services ?? (services = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CustomProvider", "Custom Shipping Costs and Info")
            });
        }

        public decimal GetPrice(Delivery delivery, string currencyCode)
        {
            if (CanDeliver(delivery) && delivery.DeliveryAddress != null)
            {
                JToken response = DummyShippingCompany.GetShippingResponse(delivery, currencyCode);
                
                foreach (JToken item in response["ShipmentItems"])
                {
                    /* Not all shipping APIs allow you to send some kind of external ID to identify 
                        * your items, so you may need to do something like this to figure out which
                        * items are being referred to in the api's response.*/
                    DeliveryItem di = delivery.Items.Where(i => i.Product.SKUID == item["ItemSku"].ToInteger(0)).First();

                    //retreive the shopping cart and write it's expected delivery date to the custom data
                    ShoppingCartItemInfo sci = ShoppingCartItemInfoProvider.GetShoppingCartItemInfo(di.CustomData.GetValue("CartItemGUID").ToGuid(Guid.Empty));
                    sci.CartItemCustomData.SetValue("DeliveryDate", item["ItemExpectedArrival"].ToString());
                    sci.Update();  
                }
                return Decimal.Parse(response["ShipmentCost"].ToString());
            }

            return 0;
        }
        //can the delivery be made?
        public bool CanDeliver(Delivery delivery)
        {
            return DummyShippingCompany.CanDeliver(delivery);
        }

        /// <summary>
        /// Returns Guid.Empty. This carrier does not have a configuration interface.
        /// </summary>
        public Guid GetConfigurationUIElementGUID()
        {
            return Guid.Empty;
        }

        /// <summary>
        /// Returns Guid.Empty. This carrier does not have a carrier service configuration interface.
        /// </summary>
        public Guid GetServiceConfigurationUIElementGUID(string serviceName)
        {
            return Guid.Empty;
        }

    }
}
