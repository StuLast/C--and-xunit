using Microsoft.Data.Sqlite;
using RoomBookingApp.Persistence;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.Processors;

var builder = WebApplication.CreateBuilder(args);


//  Add Database Context
var connString = "Datasource=:memory:";
var conn = new SqliteConnection(connString);
conn.Open();
builder.Services.AddDbContext<RoomBookingAppDbContext>(opt => opt.UseSqlite(conn));

//  Dependency Injection
builder.Services.AddScoped<IRoomBookingRequestProcessor, RoomBookingRequestProcessor>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

