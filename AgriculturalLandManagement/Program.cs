using Microsoft.EntityFrameworkCore;
using AgriculturalLandManagement.Data;
using AgriculturalLandManagement.Repositories;
using System.Reflection;
using AgriculturalLandManagement.Helper;
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILandRepository, LandRepository>();
builder.Services.AddScoped<ILandService, LandService>();

var app = builder.Build();

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
app.UseRequestTiming();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
