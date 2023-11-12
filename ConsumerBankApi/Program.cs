using ConsumerBank.Services;
using ConsumerBank.Services.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<ILoanerService, LoanerService>();
builder.Services.AddTransient<IDatabase, Database>();
builder.Services.AddSingleton(_ =>
{
    var database = builder.Configuration.GetValue<string>(nameof(DbOptions.Database))!;
    var userName = builder.Configuration.GetValue<string>(nameof(DbOptions.DbUsername))!;
    var password = builder.Configuration.GetValue<string>(nameof(DbOptions.DbPassword))!;

    if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        throw new ArgumentException("Missing database settings");
    return new DbOptions(database, userName, password);
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
