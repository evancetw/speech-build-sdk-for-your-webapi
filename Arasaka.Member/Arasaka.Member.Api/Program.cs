using Arasaka.Member.Api.Modules.Members;
using Arasaka.Member.Api.Repositories;
using Arasaka.Member.Api.Utities.JsonConverters;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure JSON options
// https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio#use-jsonoptions
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

var connectionString = builder.Configuration.GetConnectionString("Arasaka") ?? "Data Source=arasaka.db";  // Data Source 的值會是 SQLite 的檔案名稱
builder.Services.AddSqlite<ArasakaDbContext>(connectionString);
//builder.Services.AddDbContext<ArasakaDB>(options => options.UseInMemoryDatabase("members"));

builder.Services.RegisterMembersModule();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Member API",
        Description = "Demo with minimal API and EntityFramework Core",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Auto apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ArasakaDbContext>();
        await db.Database.MigrateAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Member API V1");
    });
}

// Configure modules
await app.ConfigureMembersCRUDModuleAsync();
app.MapMembersCRUDModuleEndpoints();  // Map endpoints

app.MapMembersModuleEndpoints();


app.Run();
