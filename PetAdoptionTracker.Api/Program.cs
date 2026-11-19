
using Microsoft.EntityFrameworkCore;
using PetAdoptionTracker.Data;
using PetAdoptionTracker.Api.Business;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<AdoptionDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AdoptionConnection")));

builder.Services.AddScoped<DataProvider>();
builder.Services.AddScoped<BusinessManager>();

builder.Services.AddHttpClient("AzureFunctions", client =>
{
    string baseUrl =
        builder.Configuration["AzureFunctions:BaseUrl"]
        ?? throw new InvalidOperationException(
            "Azure Functions BaseUrl is missing.");

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});

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
