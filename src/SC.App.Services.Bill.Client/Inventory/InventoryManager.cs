using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Inventory.Client;

namespace SC.App.Services.Bill.Client.Inventory
{
    public class InventoryManager : IInventoryManager
    {
        private readonly IInventoryClient _inventoryClient;

        public InventoryManager(
            IInventoryClient inventoryClient)
        {
            _inventoryClient = inventoryClient;
        }

        public async Task<ResponseOfUpdateStockResponse> UpdateStockAsync(HttpRequest request, ICollection<UpdateStockItem> items)
        {
            _inventoryClient.SetAuthorization(request.GetAuthorization());
            _inventoryClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new UpdateStock
            {
                Products = items
            };

            return await _inventoryClient.Products_UpdateStockAsync(payload);
        }
    }
}