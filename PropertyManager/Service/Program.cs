using DataAccess.database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Service.property.Manager;
using Service.property.DataStores;
using Service.propertyType.DataStores;
using Service.propertyType.Manager;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    //c.SwaggerDoc("Bob", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Bob", Version = "v1" });
}
    ); 
using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider()) 
    {
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<MicrosoftSqlServer>();
    builder.Services.AddSingleton<IDatabaseConnection>(new MicrosoftSqlServer(logger, builder.Configuration.GetConnectionString("PropertyManager")));
};

builder.Services.AddSingleton<IPropertyTypeDataStore, MicrosoftSqlServerPropertyTypeDataStore>();
builder.Services.AddSingleton<IPropertyTypeManager, PropertyTypeManager>();
builder.Services.AddSingleton<IPropertyDataStore, MicrosoftSqlServerPropertyDataStore>();
builder.Services.AddSingleton<IPropertyManager, PropertyManager>();

//builder.Services.AddMvc().AddMvcOptions(options =>
//{
//    options.AllowEmptyInputInBodyModelBinding = true;
//});
builder.Services.AddAutoMapper(typeof(Program));
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

public partial class Program { }