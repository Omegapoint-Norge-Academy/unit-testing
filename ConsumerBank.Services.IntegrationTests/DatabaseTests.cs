using ConsumerBank.Services.Contracts;
using ConsumerBank.Services.DbObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsumerBank.Services.IntegrationTests
{
    // Disse testene vil feile hvis/når testressursene i Azure blir slettet....
    public class DatabaseTests : IDisposable
    {
        private readonly DatabaseContext _database;
        private readonly List<int> _personIdsToDelete = new();
        private readonly ILoanerService _service;

        public DatabaseTests()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<DatabaseTests>()
                .Build();

            var serviceProvider = new ServiceCollection().AddDbContext<DatabaseContext>(options =>
                    {
                        options.UseSqlServer(configuration.GetConnectionString("ConsumerBankDb"));
                    }
                )
                .AddTransient<ILoanerService, LoanerService>()
                .AddTransient<IDatabase, DatabaseContext>()
                .BuildServiceProvider();

            _database = serviceProvider.GetRequiredService<DatabaseContext>();
            _service = serviceProvider.GetRequiredService<ILoanerService>();
        }

        private async Task<int> InsertPersonInDb()
        {
            var person = new PersonEntity
            {
                FirstName = "abcd",
                LastName = "abcd",
                SocialSecurityNumber = "1234"
            };
            await _database.Persons.AddAsync(person);
            await _database.SaveChangesAsync();
            _personIdsToDelete.Add(person.Id);
            return person.Id;
        }

        [Fact]
        public async Task Apply_ExistingPerson_ShouldSucceed()
        {
            // Arrange
            var personId = await InsertPersonInDb();

            // Act
            var result = await _service.Apply(new LoanRequest { Amount = 100, Person = new Person { Id = personId } });

            // Assert
            Assert.True(result);
            var actualAmount = await AssertHasLoan(personId);
            Assert.True(actualAmount >= 100);
        }

        [Theory]
        [InlineData(100)]
        public async Task Apply_NewPerson_ShouldSucceed(int expectedAmount)
        {
            // Arrange
            var person = new Person
            {
                CustomerAddress = new Address
                {
                    Street = "Nedre Slottsgate 15",
                    City = "Oslo",
                    Zip = "0157"
                },
                FirstName = "Abcdef",
                LastName = "abcdef",
                SocialSecurityNumber = "01010112345"
            };
            var request = new LoanRequest { Amount = expectedAmount, Person = person };

            // Act
            var result = await _service.Apply(request);

            // Assert
            Assert.True(result);
            var actualPersonId = await AssertPersonExists(person.SocialSecurityNumber);
            var actualAmount = await AssertHasLoan(actualPersonId);
            Assert.Equal(expectedAmount, actualAmount);
        }

        private async Task<decimal> AssertHasLoan(int actualPersonId)
        {
            var loan = await _database.Loans.Where(l => l.PersonId == actualPersonId).FirstOrDefaultAsync();
            Assert.NotNull(loan);
            return loan.Amount;
        }

        private async Task<int> AssertPersonExists(string SocialSecurityNumber)
        {
            var actualPerson = await _database.Persons.Where(p => p.SocialSecurityNumber == SocialSecurityNumber).FirstOrDefaultAsync();
            Assert.NotNull(actualPerson);
            return actualPerson.Id;
        }

        public void Dispose()
        {
            // Dispose is always called after a completed test, regardless of the result. So this is a good place to clean up data after tests.
            foreach (var personId in _personIdsToDelete)
            {
                try
                {
                    var personToDelete = _database.Persons.Find(personId);
                    if (personToDelete != null)
                    {
                        _database.Persons.Remove(personToDelete);
                    }

                    var loansToDelete = _database.Loans.Where(l => l.PersonId == personId).ToList();
                    if (loansToDelete.Count > 0)
                    {
                        _database.Loans.RemoveRange(loansToDelete);
                    }

                    _database.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Log and Mute this exception - we don't want unhandled exceptions in Dispose.
                }
            }
        }
    }
}