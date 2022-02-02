using DataModel;
using Microsoft.AspNetCore.Mvc;
using MongoAccess;
using Publisher;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IDataAccess, DataAccess>();
builder.Services.Configure<DbConifg>(
    builder.Configuration.GetSection(DbConifg.Db));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/get", async (
   [FromQuery(Name = "start")] DateTime? start,
   [FromQuery(Name = "stop")] DateTime? stop,
   [FromQuery(Name = "type")] ContentType? contentType,
   [FromQuery(Name = "name")] string? name,
   IDataAccess dataAccess) =>
{
    try
    {
        var items = await dataAccess.TryGet<Item>(
          filter: new Item
          {
              ContentType = contentType,
              Name = name
          },
           from: start ?? DateTime.MinValue,
           to: stop ?? DateTime.Now);

        return Results.Ok(items);
    }
    catch (Exception)
    {
        return Results.NotFound("No data found for give query");
    }



});

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();

