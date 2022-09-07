using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.Tag;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Tag
{
    public class GetTagByFilterHandler : BaseHandler, IRequestHandler<GetTagByFilterQuery, Response<List<GetTagResponse>>>
    {
        private readonly IConfiguration _configuration;
        private readonly ITagManager _tagManager;

        public GetTagByFilterHandler(
            IConfiguration configuration,
            ITagManager tagManager) : base(configuration)
        {
            _configuration = configuration;
            _tagManager = tagManager;
        }

        public async Task<Response<List<GetTagResponse>>> Handle(GetTagByFilterQuery query, CancellationToken cancellationToken)
        {
            return await _tagManager.GetAsync(_configuration, query);
        }
    }
}