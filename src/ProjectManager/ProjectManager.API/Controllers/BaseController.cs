using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectManager.Application.Common;

namespace ProjectManager.API.Controllers
{
    [Route("api/{controller}")]
    public abstract class BaseController : Controller
    {
        protected string ConvertToJson(object o) => JsonConvert.SerializeObject(o);
        protected IActionResult OkJson(object o) => Ok(ConvertToJson(o));
        protected IActionResult FromCommandResult(CommandResult result) 
            => result.IsSuccess
            ? StatusCode(result.StatusCode, result.ReturnValue)
            : StatusCode(result.StatusCode, result.Message);
    }
}
