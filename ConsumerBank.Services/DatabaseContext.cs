using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ConsumerBank.Services.DbObjects;
using Microsoft.Data.SqlClient;
using ConsumerBank.Services.Options;

namespace ConsumerBank.Services;

public interface IDatabase
{
    Task<int> SavePerson(PersonEntity person);
    Task SaveAddress(AddressEntity address);
    Task UpdateLoan(int personId, decimal amount);
}

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options), IDatabase
{
    public DbSet<PersonEntity> Persons { get; set; } = null!;
    public DbSet<AddressEntity> Address { get; set; } = null!;
    public DbSet<LoanEntity> Loans { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PersonEntity>(entity =>
        {
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<LoanEntity>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Amount).HasPrecision(15, 2);
        });

        modelBuilder.Entity<AddressEntity>(entity =>
        {
            entity.HasKey(x => x.Id);
        });
    }

    public async Task<int> SavePerson(PersonEntity person)
    {
        Persons.Add(person);
        await SaveChangesAsync();
        return person.Id;
    }

    public async Task SaveAddress(AddressEntity address)
    {
        Address.Add(address);
        await SaveChangesAsync();
    }

    public async Task UpdateLoan(int personId, decimal amount)
    {
        var loan = await Loans.FirstOrDefaultAsync(loan => loan.PersonId == personId);
        if (loan == null)
        {
            loan = new()
            {
                PersonId = personId,
                Amount = amount
            };
            await Loans.AddAsync(loan);
        }
        else
        {
            loan.Amount += amount;
        }
        await SaveChangesAsync();
    }
}