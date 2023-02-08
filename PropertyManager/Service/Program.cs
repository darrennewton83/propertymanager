using DataAccess.database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Service.property.Manager;
using Service.property.DataStores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider()) 
    {
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<MicrosoftSqlServer>();
    builder.Services.AddSingleton<IDatabaseConnection>(new MicrosoftSqlServer(logger, builder.Configuration.GetConnectionString("PropertyManager")));
};

builder.Services.AddSingleton<IPropertyDataStore, SqlPropertyDataStore>();
builder.Services.AddSingleton<IPropertyManager, PropertyManager>();
var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();