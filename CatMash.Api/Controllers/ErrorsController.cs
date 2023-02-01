using CatMash.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CatMash.Api.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    [Route("error")]
    public Exception Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context!.Error;
        switch (exception)
        {
            case CatScoreNotFoundException:
                Response.StatusCode = 400;
                return new CatScoreNotFoundException(exception.Message);
            default:
                Response.StatusCode = 500;
                return new Exception();
        }
    }
}