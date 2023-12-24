using api_attendance_system.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

{
    IConfiguration Configuration;
    var builder = WebApplication.CreateBuilder(args);

    //Add Custom Dependancies
    builder.Services.AddDependanciesSingletone();
    builder.Services.AddDependanciesScoped();
    builder.Services.AddDependanciesTransient();

    builder.Services.AddControllers();

    if (builder.Environment.IsDevelopment())
    {
        var builder1 = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables();
        Configuration = builder1.Build();
    }
    else
    {
        var builder1 = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables();
        Configuration = builder1.Build();
    }

    builder.Services.AddJWT(Configuration);
    builder.Services.AddAuthorization();


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

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}