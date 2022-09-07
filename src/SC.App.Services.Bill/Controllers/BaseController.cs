using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Filters;

namespace SC.App.Services.Bill.Controllers
{
    [CustomExceptionFilter]
    public class BaseController : ControllerBase
    {
    }
}