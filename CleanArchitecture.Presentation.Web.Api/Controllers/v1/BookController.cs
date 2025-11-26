using Asp.Versioning;
using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.Books.Commands;
using CleanArchitecture.Application.Features.Books.Queries.GetAllBooks;
using CleanArchitecture.Application.Features.Books.Queries.GetBookById;
using CleanArchitecture.Application.Filters;
using CleanArchitecture.Application.Requests.BookRequests;
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
        public async Task<IActionResult> Get([FromQuery] GetAllBooksParameter filter)
        {
          
            return Ok(await Mediator.Send(new GetAllBooksQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber  }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetBookByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize("create:books")]
        public async Task<IActionResult> Post([FromBody] CreateBookDTO book)
        {
            var createBookResult = await Mediator.Send(new CreateBookRequest()
            {
                Book = book
            });

            // return bad request if creation failed
            if (!createBookResult.Succeeded)
                return BadRequest(createBookResult);

            // return response
            return CreatedAtAction(nameof(Get), new { id = createBookResult.Data }, createBookResult);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize("update:books")]
        public async Task<IActionResult> Put([FromQuery]int id, [FromBody]UpdateBookDTO book)
        {
            var updateBookResult = await Mediator.Send(new UpdateBookRequest()
            {
                Book = book
            });

            // return bad request if creation failed
            if (!updateBookResult.Succeeded)
                return BadRequest(updateBookResult);

            // return response
            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize("delete:books")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteBookCommand { Id = id }));
        }
    }
}
