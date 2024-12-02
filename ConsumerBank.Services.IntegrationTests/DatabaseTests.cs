using ConsumerBank.Services.DbObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsumerBank.Services.IntegrationTests
{
    public class DatabaseTests //: IDisposable
    {

        #region spoiler setup
        //private readonly DatabaseContext _database;
        //private readonly List<int> _personIdsToDelete = new();
        //private readonly ILoanerService _service;

        //public DatabaseTests()
        //{
        //    IConfiguration configuration = new ConfigurationBuilder()
        //        .AddJsonFile("appsettings.json")
        //        .AddUserSecrets<DatabaseTests>()
        //        .Build();

        //    var serviceProvider = new ServiceCollection().AddDbContext<DatabaseContext>(options =>
        //            {
        //                options.UseSqlServer(configuration.GetConnectionString("ConsumerBankDb"));
        //            }
        //        )
        //        .AddTransient<ILoanerService, LoanerService>()
        //        .AddTransient<IDatabase, DatabaseContext>()
        //        .BuildServiceProvider();

        //    _database = serviceProvider.GetRequiredService<DatabaseContext>();
        //    _service = serviceProvider.GetRequiredService<ILoanerService>();
        //}

        //private async Task<int> InsertPersonInDb()
        //{
        //    var person = new PersonEntity
        //    {
        //        FirstName = "abcd",
        //        LastName = "abcd",
        //        SocialSecurityNumber = "1234"
        //    };
        //    await _database.Persons.AddAsync(person);
        //    await _database.SaveChangesAsync();
        //    _personIdsToDelete.Add(person.Id);
        //    return person.Id;
        //}

        //[Fact]
        //public async Task Apply_ExistingPerson_ShouldSucceed()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact]
        //public async Task Apply_NewPerson_ShouldSucceed()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //public void Dispose()
        //{
        //    // Dispose is always called after a completed test, regardless of the result. So this is a good place to clean up data after tests.
        //    foreach (var personId in _personIdsToDelete)
        //    {
        //        try
        //        {
        //            var personToDelete = _database.Persons.Find(personId);
        //            if (personToDelete != null)
        //            {
        //                _database.Persons.Remove(personToDelete);
        //            }

        //            var loansToDelete = _database.Loans.Where(l => l.PersonId == personId).ToList();
        //            if (loansToDelete.Count > 0)
        //            {
        //                _database.Loans.RemoveRange(loansToDelete);
        //            }

        //            _database.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            // Log and Mute this exception - we don't want unhandled exceptions in Dispose.
        //        }
        //    }
        //}
        #endregion
    }
}