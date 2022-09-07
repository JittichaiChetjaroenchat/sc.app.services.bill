using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class ShippingHelper
    {
        public static EnumShippingCostType GetCostType(BillShipping billShipping)
        {
            if (billShipping.BillShippingRangeRule.Enabled)
            {
                return EnumShippingCostType.Range;
            }
            else if (billShipping.BillShippingTotalRule.Enabled)
            {
                return EnumShippingCostType.Total;
            }
            else if (billShipping.BillShippingFreeRule.Enabled)
            {
                return EnumShippingCostType.Free;
            }

            return EnumShippingCostType.Unknown;
        }

        public static decimal CalculateCost(ICollection<Order.Client.GetOrderResponse> orders, BillShipping billShipping)
        {
            var shippingCostType = GetCostType(billShipping);
            switch (shippingCostType)
            {
                case EnumShippingCostType.Range:
                    return CalculateCostRange(orders, billShipping);
                case EnumShippingCostType.Total:
                    return CalculateCostTotal(orders, billShipping);
                case EnumShippingCostType.Free:
                    return CalculateCostFree();
            }

            return 0.0m;
        }

        private static decimal CalculateCostRange(ICollection<Order.Client.GetOrderResponse> orders, BillShipping billShipping)
        {
            var amount = OrderHelper.GetAmount(orders);
            var price = OrderHelper.GetPrice(orders);
            var isHitFreeRule = IsHitFreeRule(amount, price, billShipping);

            return isHitFreeRule ? 0.0m : CalculateCostRange(amount, price, billShipping);
        }

        private static decimal CalculateCostRange(int amount, decimal price, BillShipping billShipping)
        {
            if (amount == 0)
            {
                return 0.0m;
            }

            var leftAmount = amount;
            var totalCost = 0.0m;
            var ranges = billShipping.BillShippingRangeRule.BillShippingRanges
                .OrderBy(x => x.Begin);
            foreach (var range in ranges)
            {
                totalCost += leftAmount > range.End ?
                    range.Cost * range.End :
                    range.Cost * leftAmount;

                leftAmount = leftAmount - range.End;
            }

            return totalCost;
        }

        private static decimal CalculateCostTotal(ICollection<Order.Client.GetOrderResponse> orders, BillShipping billShipping)
        {
            var amount = OrderHelper.GetAmount(orders);
            var price = OrderHelper.GetPrice(orders);
            var isHitFreeRule = IsHitFreeRule(amount, price, billShipping);

            return isHitFreeRule ? 0.0m : CalculateCostTotal(amount, billShipping);
        }

        public static decimal CalculateCostTotal(int amount, BillShipping billShipping)
        {
            if (amount == 0)
            {
                return 0.0m;
            }

            return billShipping.BillShippingTotalRule.Cost;
        }

        private static decimal CalculateCostFree()
        {
            return 0.0m;
        }

        private static bool IsHitFreeRule(int amount, decimal price, BillShipping billShipping)
        {
            bool freeRuleEnabled = billShipping.BillShippingFreeRule.Enabled;
            int conditionAmount = billShipping.BillShippingFreeRule.Amount;
            decimal conditionPrice = billShipping.BillShippingFreeRule.Price;

            if (freeRuleEnabled && (amount >= conditionAmount || price >= conditionPrice))
            {
                return true;
            }

            return false;
        }
    }
}