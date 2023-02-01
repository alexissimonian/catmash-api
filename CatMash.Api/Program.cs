using System.Diagnostics;
using CatMash.Domain.Interfaces;
using CatMash.Domain.Services;
using CatMash.Persistence.EFCatMashRepository.Models;
using CatMash.Persistence.EFCatMashRepository.Repositories;
using Microsoft.EntityFrameworkCore;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<CatContext>(opt =>
    opt.UseInMemoryDatabase("CatScoreList"));
builder.Services.AddScoped<ICatMashService, CatMashService>();
builder.Services.AddScoped<ICatMashRepository, CatMashRepository>();
var app = builder.Build();

bool isUnderTest = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.ToLower().StartsWith("xunit"));
if (!isUnderTest)
{
    // Init data
    await using (var scope = app.Services.CreateAsyncScope())
    {
        var repository = scope.ServiceProvider.GetRequiredService<ICatMashRepository>();
        await repository.InitDataContext();
    }
}

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }