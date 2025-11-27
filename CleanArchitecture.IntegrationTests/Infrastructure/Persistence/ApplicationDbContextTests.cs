using CleanArchitecture.Infrastructure.Persistence.Contexts;
using CleanArchitecture.Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace CleanArchitecture.IntegrationTests.Infrastructure.Persistence
{
    public class ApplicationDbContextTests
    {
        [Fact]
        public async Task Can_Add_And_Query_Book_Using_Sqlite_InMemory()
        {
            // arrange - create in-memory sqlite
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            // create schema
            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();
            }

            // act - insert and query
            using (var context = new ApplicationDbContext(options))
            {
                var book = new Book { Title = "Integration Book", ISBN = "INT-001", Price = 10m };
                await context.Books.AddAsync(book);
                await context.SaveChangesAsync();

                var fetched = await context.Books.FirstOrDefaultAsync(b => b.ISBN == "INT-001");
                fetched.Should().NotBeNull();
                fetched!.Title.Should().Be("Integration Book");
            }

            connection.Close();
        }
    }
}