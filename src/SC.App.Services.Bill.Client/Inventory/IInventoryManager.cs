using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Inventory.Client;

namespace SC.App.Services.Bill.Client.Inventory
{
    public interface IInventoryManager
    {
        Task<ResponseOfUpdateStockResponse> UpdateStockAsync(HttpRequest request, ICollection<UpdateStockItem> items);
    }
}