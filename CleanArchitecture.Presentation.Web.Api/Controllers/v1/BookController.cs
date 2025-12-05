using Asp.Versioning;
using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.Parameters.Book;
using CleanArchitecture.Application.Features.Requests.BookRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Presentation.Web.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BooksController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        //[Authorize("read:books")]
        [Route("GetList")]
        public async Task<IActionResult> GetList([FromQuery] GetBooksListParameter filters)
        {
            // get all books with pagination
            var result = await Mediator.Send(new GetBooksListRequest() 
            { 
                BookParameters = filters 
            },
            HttpContext.RequestAborted);

            // return bad request if retrieval failed
            if (!result.Succeeded)
                return FromResponse(result);

            // return response
            return Ok(result);
        }

        // GET api/<controller>/5
        [HttpGet]
        [Authorize("read:books")]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            // get book by id
            var result = await Mediator.Send(new GetBookRequest() 
            { 
                Id = id 
            },
            HttpContext.RequestAborted);

            // return not found if book does not exist
            if (!result.Succeeded)
                return FromResponse(result);

            // return response
            return Ok(result);
        }

        // POST api/<controller>
        [HttpPost]
        //[Authorize("create:books")]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] CreateBookDTO book)
        {
            var result = await Mediator.Send(new CreateBookRequest()
            {
                Book = book
            },
            HttpContext.RequestAborted);

            // return bad request if creation failed
            if (!result.Succeeded)
                return FromResponse(result);

            // return response
            return CreatedAtAction(nameof(Get), new { id = result.Data }, result);
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Authorize("update:books")]
        [Route("Put/{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody]UpdateBookDTO book)
        {
            // validate request body
            if (book == null)
                return BadRequest("Request body is required.");

            var result = await Mediator.Send(new UpdateBookRequest()
            {
                Id = id,
                Book = book
            },
            HttpContext.RequestAborted);

            // return bad request if creation failed
            if (!result.Succeeded)
                return FromResponse(result);

            // return response
            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        [Authorize("delete:books")]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // delete book
            var result = await Mediator.Send(new DeleteBookRequest() 
            { 
                Id = id 
            },
            HttpContext.RequestAborted);

            // return bad request if deletion failed
            if (!result.Succeeded)
                return FromResponse(result);

            // return response
            return NoContent();
        }
    }
}
