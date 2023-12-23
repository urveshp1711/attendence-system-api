using UAS.Business;
using UAS.Data;
using UAS.Database;
using UAS.Dependancies.Business;
using UAS.Dependancies.Data;
using UAS.Dependancies.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddScoped<IMySQLHelper, UAS.Database.MySQLHelper>();
builder.Services.AddScoped<IUsers, Users>();
builder.Services.AddScoped<IdUsers, dUsers>();
builder.Services.AddScoped<IMySQLHelper, MySQLHelper>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<IUser, User>(); 

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
