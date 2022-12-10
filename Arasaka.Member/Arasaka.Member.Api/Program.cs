using Arasaka.Member.Api;
using Arasaka.Member.Api.Repositories;
using Arasaka.Member.Api.Utities.JsonConverters;
using Arasaka.Member.Api.ViewModels;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using static Arasaka.Member.Api.Constants;
using static Arasaka.Member.Api.Extensions;

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

// Auto apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ArasakaDbContext>();
    await db.Database.MigrateAsync();

    #region 幫新 DB 初始化一些會員

    var memberCount = await db.Members.CountAsync();
    if (memberCount == 0)
    {
        int seed = 9527;
        var rnd = new Random(seed);
        var addresses = new string[] { "台北市", "台中市", "台南市" };
        var registerFrom = new string[] { "官網", "FB", "Email" };

        for (int i = 0; i < 100; i++)
        {
            string userName = $"user{i + seed:000}";
            DateTimeOffset birthTime = new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMinutes(rnd.Next(-525_600, 525_600));  // +/- 10 years
            DateTimeOffset registerTime = new DateTimeOffset(2022, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMinutes(rnd.Next(43_200, 129_600));  // + 1 ~ 3 months
            MemberState memberState = RandomEnum<MemberState>(rnd);

            var member = new MemberEntity()
            {
                Name = userName,
                Birthday = DateOnly.FromDateTime(birthTime.DateTime),
                Address = addresses[rnd.Next(addresses.Length)],
                Email = $"{userName}@arasaka.corp",
                PhoneNumber = ("09" + rnd.Next().ToString()).Substring(0, 8),
                Gender = RandomEnum<Gender>(rnd).ToString(),
                State = memberState.ToString(),
                RegisterFrom = registerFrom[rnd.Next(registerFrom.Length)],
                RegisterTime = registerTime,
                LastUpdateTime = memberState == MemberState.Unverified ? registerTime : registerTime.AddMinutes(rnd.Next(1_800, 129_600))  // + 30 mins ~ 3 months
            };

            await db.Members.AddAsync(member);
        }

        await db.SaveChangesAsync();
    }

    #endregion
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Member API V1");
    });
}

app.MapGet("/members", async (ArasakaDbContext db) =>
{
    return (await db.Members.ToListAsync()).Select(x => x.ConvertToViewModel());
});

app.MapPost("/members", async (ArasakaDbContext db, RegisterMemberViewModel registerMemberViewModel) =>
{
    var memberEntity = registerMemberViewModel.ConvertToEntity();
    memberEntity.RegisterTime = DateTimeOffset.UtcNow;
    memberEntity.State = MemberState.Unverified.ToString();
    memberEntity.LastUpdateTime = memberEntity.RegisterTime;

    await db.Members.AddAsync(memberEntity);
    await db.SaveChangesAsync();

    return Results.Created($"/members/{memberEntity.Id}", memberEntity.ConvertToViewModel());
});

app.MapGet("/members/{id}", async (ArasakaDbContext db, long id) =>
{
    var originMemberEntity = await db.Members.FindAsync(id);
    if (originMemberEntity is null)
        return Results.NotFound();

    return Results.Ok(originMemberEntity.ConvertToViewModel());
});

app.MapPut("/members/{id}", async (ArasakaDbContext db, UpdateMemberViewModel updateMemberViewModel, long id) =>
{
    var originMemberEntity = await db.Members.FindAsync(id);
    if (originMemberEntity is null)
        return Results.NotFound();

    updateMemberViewModel.AssignFieldsTo(originMemberEntity);
    originMemberEntity.LastUpdateTime = DateTimeOffset.UtcNow;
    await db.SaveChangesAsync();

    return Results.Ok(originMemberEntity.ConvertToViewModel());
});

app.MapDelete("/members/{id}", async (ArasakaDbContext db, long id) =>
{
    var originMemberEntity = await db.Members.FindAsync(id);
    if (originMemberEntity is null)
        return Results.NotFound();

    db.Members.Remove(originMemberEntity);
    await db.SaveChangesAsync();

    return Results.Ok();
});


app.Run();
