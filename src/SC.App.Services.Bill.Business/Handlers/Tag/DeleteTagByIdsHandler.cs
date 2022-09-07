using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Tag;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Tag
{
    public class DeleteTagByIdsHandler : BaseHandler, IRequestHandler<DeleteTagByIdsCommand, Response<DeleteTagByIdsResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly ITagManager _tagManager;

        public DeleteTagByIdsHandler(
            IConfiguration configuration,
            ITagManager tagManager) : base(configuration)
        {
            _configuration = configuration;
            _tagManager = tagManager;
        }

        public async Task<Response<DeleteTagByIdsResponse>> Handle(DeleteTagByIdsCommand command, CancellationToken cancellationToken)
        {
            return await _tagManager.DeleteAsync(_configuration, command);
        }
    }
}