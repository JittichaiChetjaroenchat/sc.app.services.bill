using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Queries.Bill;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class BillHelper
    {
        private const string DEFAULT_LANGUAGE = "th";

        public static string GetNextBillNo(int currentNo)
        {
            return $"{string.Format("{0:D10}", currentNo + 1)}";
        }

        public static string GetNextRunningNo(int currentNo)
        {
            return $"{string.Format("{0:D10}", currentNo + 1)}";
        }

        public static string GetKey(string billNo)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                sha256.ComputeHash(ASCIIEncoding.UTF8.GetBytes(billNo));
                byte[] result = sha256.Hash;

                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    strBuilder.Append(result[i].ToString("x2"));
                }

                return strBuilder.ToString();
            }
        }

        public static string GetLink(string baseUrl, string key, string language)
        {
            var path = language.IsEmpty() || language.Equals(DEFAULT_LANGUAGE, StringComparison.OrdinalIgnoreCase) ? string.Empty : $"{language}/";
            var link = $"{baseUrl}/{path}bill/{key}";

            return link;
        }

        public static List<SearchBillItem> Search(List<SearchBillItem> items, string keyword)
        {
            if (keyword.IsEmpty())
            {
                return items;
            }

            return items
                .Where(x =>
                    x.Bill != null && !x.Bill.RunningNo.IsEmpty() && x.Bill.RunningNo.ToLower().Contains(keyword.ToLower()) ||
                    x.Recipient != null && !x.Recipient.Name.IsEmpty() && x.Recipient.Name.ToLower().Contains(keyword.ToLower()) ||
                    x.Recipient != null && x.Recipient.Customer != null && x.Recipient.Customer.Tags.Any(x => x.Name.ToLower().Contains(keyword.ToLower())) || 
                    x.Shipping != null && x.Shipping.Parcels.Any(a => !a.Number.IsEmpty() && a.Number.ToLower().Contains(keyword.ToLower())) || 
                    x.Tags.Any(x => x.Name.ToLower().Contains(keyword.ToLower())))
                .ToList();
        }

        public static List<SearchBillItem> Sort(List<SearchBillItem> items, string sortBy, bool sortDesc)
        {
            if (sortBy.IsEmpty())
            {
                return items;
            }

            switch (sortBy)
            {
                case "order.amount":
                    return sortDesc ? items.OrderByDescending(x => x.Order.Amount).ToList() : items.OrderBy(x => x.Order.Amount).ToList();
                default:
                    return items;
            }
        }

        public static bool IsEndState(Database.Models.Bill bill)
        {
            var isDone = IsDone(bill);
            var isCancelled = IsCancelled(bill);
            var isArchieved = IsArchieved(bill);
            var isDeleted = IsDeleted(bill);

            return isDone || isCancelled || isArchieved || isDeleted;
        }

        private static bool IsDone(Database.Models.Bill bill)
        {
            var status = EnumBillStatus.Done.GetDescription();

            return bill.BillStatus.Code == status;
        }

        private static bool IsCancelled(Database.Models.Bill bill)
        {
            var status = EnumBillStatus.Cancelled.GetDescription();

            return bill.BillStatus.Code == status;
        }

        private static bool IsArchieved(Database.Models.Bill bill)
        {
            var status = EnumBillStatus.Archived.GetDescription();

            return bill.BillStatus.Code == status;
        }

        private static bool IsDeleted(Database.Models.Bill bill)
        {
            var status = EnumBillStatus.Deleted.GetDescription();

            return bill.BillStatus.Code == status;
        }
    }
}