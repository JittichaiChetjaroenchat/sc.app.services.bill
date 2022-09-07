using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Queries.Bill;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IBillManager
    {
        Task<Response<GetBillResponse>> GetAsync(IConfiguration configuration, GetBillByIdQuery query);

        Task<Response<GetBillResponse>> GetAsync(IConfiguration configuration, GetLatestBillByFilterQuery query);

        Task<Response<List<GetBillManifestResponse>>> GetAsync(IConfiguration configuration, GetLatestBillManifestByFilterQuery query);

        Task<Response<SearchBillResponse>> GetAsync(IConfiguration configuration, SearchBillByFilterQuery query);

        Task<Response<GetBillSummaryResponse>> GetAsync(IConfiguration configuration, GetBillSummaryByFilterQuery query);

        Task<Response<DepositBillResponse>> UpdateAsync(IConfiguration configuration, DepositBillCommand command);

        Task<Response<ConfirmBillResponse>> UpdateAsync(IConfiguration configuration, ConfirmBillCommand command);

        Task<Response<CancelBillResponse>> UpdateAsync(IConfiguration configuration, CancelBillCommand command);

        Task<Response<CancelBillsResponse>> UpdateAsync(IConfiguration configuration, CancelBillsCommand command);

        Task<Response<RenewBillResponse>> UpdateAsync(IConfiguration configuration, RenewBillCommand command);

        Task<Response<RenewBillsResponse>> UpdateAsync(IConfiguration configuration, RenewBillsCommand command);

        Task<Response<DoneBillResponse>> UpdateAsync(IConfiguration configuration, DoneBillCommand command);

        Task<Response<ArchiveBillResponse>> UpdateAsync(IConfiguration configuration, ArchiveBillCommand command);

        Task<Response<ArchiveBillsResponse>> UpdateAsync(IConfiguration configuration, ArchiveBillsCommand command);
    }
}