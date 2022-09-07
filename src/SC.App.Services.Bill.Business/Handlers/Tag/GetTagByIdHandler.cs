using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.Tag;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Tag
{
    public class GetTagByIdHandler : BaseHandler, IRequestHandler<GetTagByIdQuery, Response<GetTagResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly ITagManager _tagManager;

        public GetTagByIdHandler(
            IConfiguration configuration,
            ITagManager tagManager) : base(configuration)
        {
            _configuration = configuration;
            _tagManager = tagManager;
        }

        public async Task<Response<GetTagResponse>> Handle(GetTagByIdQuery query, CancellationToken cancellationToken)
        {
            return await _tagManager.GetAsync(_configuration, query);
        }
    }
}