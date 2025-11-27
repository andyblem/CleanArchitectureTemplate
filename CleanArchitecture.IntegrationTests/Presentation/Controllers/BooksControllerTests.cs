//using System.Net;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Xunit;

//namespace CleanArchitecture.ApiTests.Controllers
//{
//    public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
//    {
//        private readonly WebApplicationFactory<Program> _factory;

//        public BooksControllerTests(WebApplicationFactory<Program> factory)
//        {
//            _factory = factory;
//        }

//        [Fact]
//        public async Task SwaggerJson_Is_Available()
//        {
//            var client = _factory.CreateClient();
//            var response = await client.GetAsync("/swagger/v1/swagger.json");
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var json = await response.Content.ReadAsStringAsync();
//            json.Should().Contain("\"openapi\"");
//        }
//    }
//}