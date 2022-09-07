using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Tag;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Tag
{
    public class CreateTagHandler : BaseHandler, IRequestHandler<CreateTagCommand, Response<CreateTagResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly ITagManager _tagManager;

        public CreateTagHandler(
            IConfiguration configuration,
            ITagManager tagManager) : base(configuration)
        {
            _configuration = configuration;
            _tagManager = tagManager;
        }

        public async Task<Response<CreateTagResponse>> Handle(CreateTagCommand command, CancellationToken cancellationToken)
        {
            return await _tagManager.CreateAsync(_configuration, command);
        }
    }
}