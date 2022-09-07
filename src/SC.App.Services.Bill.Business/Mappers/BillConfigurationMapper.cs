using SC.App.Services.Bill.Business.Queries.BillConfiguration;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class BillConfigurationMapper
    {
        public static GetBillConfigurationResponse Map(BillConfiguration configuration)
        {
            if (configuration == null)
            {
                return null;
            }

            return new GetBillConfigurationResponse
            {
                Id = configuration.Id,
                ChannelId = configuration.ChannelId,
                CurrentNo = configuration.CurrentNo
            };
        }
    }
}