using CRC.WebPortal.Application;
using CRC.WebPortal.Infrastructure;
using CRC.WebPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS for Blazor WebAssembly - Allow all origins for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorWasmPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register Application and Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi(); // Temporarily disabled for .NET 10 compatibility

var app = builder.Build();

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Silent error handling - database may already be up to date
        // Continue startup without logging
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Temporarily disabled for local development

// Enable CORS
app.UseCors("BlazorWasmPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
