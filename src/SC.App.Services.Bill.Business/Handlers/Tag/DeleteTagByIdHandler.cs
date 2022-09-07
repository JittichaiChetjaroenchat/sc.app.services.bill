using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Tag;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Tag
{
    public class DeleteTagByIdHandler : BaseHandler, IRequestHandler<DeleteTagByIdCommand, Response<DeleteTagByIdResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly ITagManager _tagManager;

        public DeleteTagByIdHandler(
            IConfiguration configuration,
            ITagManager tagManager) : base(configuration)
        {
            _configuration = configuration;
            _tagManager = tagManager;
        }

        public async Task<Response<DeleteTagByIdResponse>> Handle(DeleteTagByIdCommand command, CancellationToken cancellationToken)
        {
            return await _tagManager.DeleteAsync(_configuration, command);
        }
    }
}