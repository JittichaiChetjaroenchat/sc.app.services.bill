using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Tag;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Tag
{
    public class UpdateTagHandler : BaseHandler, IRequestHandler<UpdateTagCommand, Response<UpdateTagResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly ITagManager _tagManager;

        public UpdateTagHandler(
            IConfiguration configuration,
            ITagManager tagManager) : base(configuration)
        {
            _configuration = configuration;
            _tagManager = tagManager;
        }

        public async Task<Response<UpdateTagResponse>> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
        {
            return await _tagManager.UpdateAsync(_configuration, command);
        }
    }
}