using System;
using System.Collections.Generic;
using CMS;
using CMS.Base;
using CMS.Ecommerce;

[assembly: RegisterImplementation(typeof(IDeliveryBuilder), typeof(ShippingLibrary.CustomDeliveryBuilder))]
namespace ShippingLibrary
{
    class CustomDeliveryBuilder : DefaultDeliveryBuilder
    {
        protected override void GetItemCustomDataContainers(List<IDataContainer> containers, CalculationRequestItem requestItem)
        {
            containers.Add(requestItem.ItemCustomData);
        }
        protected override void AddItems(CalculationRequest request, Func<CalculationRequestItem, bool> itemSelector = null)
        {
            base.AddItems(request, itemSelector);
        }
    }
}
