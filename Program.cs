using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskManagerAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

builder.Services.AddControllers();

// ✅ Add Swagger support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Manager API", Version = "v1" });
});

var app = builder.Build();

// Auto-migrate database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();       // ✅ Enable Swagger
    app.UseSwaggerUI(c =>   // ✅ Enable Swagger UI
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at root
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();
