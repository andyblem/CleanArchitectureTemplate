using CleanArchitecture.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace CleanArchitecture.Presentation.Web.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


        protected IActionResult FromResponse<T>(Response<T> response)
        {
            if (response == null) 
                return StatusCode(500);

            if (!response.Succeeded)
            {
                var message = response.Message ?? string.Empty;

                if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(response);

                if (message.Contains("conflict", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("constraint", StringComparison.OrdinalIgnoreCase))
                {
                    return Conflict(response);
                }

                return BadRequest(response);
            }

            if (response is PagedResponse<object> pagedObj)
            {
                Response.Headers["X-Total-Count"] = pagedObj.TotalSize.ToString();
                Response.Headers["X-Total-Pages"] = pagedObj.TotalPages.ToString();

                return Ok(response);
            }

            if (response.Data == null)
                return NoContent();

            return Ok(response);
        }
    }
}
