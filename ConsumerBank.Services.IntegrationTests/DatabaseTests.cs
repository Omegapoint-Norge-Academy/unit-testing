using ConsumerBank.Services.DbObjects;
using ConsumerBank.Services.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConsumerBank.Services.IntegrationTests
{
    // Disse testene vil feile hvis/n�r testressursene i Azure blir slettet....
    public class DatabaseTests : IDisposable
    {
        private readonly Database _database;
        private readonly List<int> _personIdsToDelete = new();

        public DatabaseTests()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<DatabaseTests>()
                .Build();
            
            var options = new DbOptions(configuration[nameof(DbOptions.Database)]!, configuration[nameof(DbOptions.DbUsername)]!, configuration[nameof(DbOptions.DbPassword)]!);
            _database = new Database(options);
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