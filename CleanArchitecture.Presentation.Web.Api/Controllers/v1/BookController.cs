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
        public async Task<IActionResult> GetList([FromQuery] GetAllBooksParameter filters)
        {
            // get all books with pagination
            var getBooksListResult = await Mediator.Send(new GetBooksListRequest() { BookParameters = filters });

            // return bad request if retrieval failed
            if (!getBooksListResult.Succeeded)
                return BadRequest();

            // return response
            return Ok(getBooksListResult);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery]int id)
        {
            // get book by id
            var getBookResult = await Mediator.Send(new GetBookRequest() { Id = id });

            // return not found if book does not exist
            if (!getBookResult.Succeeded)
                return NotFound(getBookResult);

            // return response
            return Ok(getBookResult);
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
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            // delete book
            var deleteBookResult = await Mediator.Send(new DeleteBookRequest() { Id = id });

            // return bad request if deletion failed
            if (!deleteBookResult.Succeeded)
                return BadRequest(deleteBookResult);

            // return response
            return NoContent();
        }
    }
}
