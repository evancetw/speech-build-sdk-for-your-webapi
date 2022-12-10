using Arasaka.Member.Api.Repositories;
using Arasaka.Member.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Arasaka.Member.Api.Constants;
using static Arasaka.Member.Api.Extensions;

namespace Arasaka.Member.Api.Modules;

public static class MembersModule
{
    public static IServiceCollection RegisterMembersModule(this IServiceCollection services)
    {
        return services;
    }

    public static async Task ConfigureMembersModuleAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ArasakaDbContext>();

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
    }

    public static IEndpointRouteBuilder MapMembersModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/members", async (ArasakaDbContext db) =>
        {
            return (await db.Members.ToListAsync()).Select(x => x.ConvertToViewModel());
        });

        endpoints.MapPost("/members", async (ArasakaDbContext db, RegisterMemberViewModel registerMemberViewModel) =>
        {
            var memberEntity = registerMemberViewModel.ConvertToEntity();
            memberEntity.RegisterTime = DateTimeOffset.UtcNow;
            memberEntity.State = MemberState.Unverified.ToString();
            memberEntity.LastUpdateTime = memberEntity.RegisterTime;

            await db.Members.AddAsync(memberEntity);
            await db.SaveChangesAsync();

            return Results.Created($"/members/{memberEntity.Id}", memberEntity.ConvertToViewModel());
        });

        endpoints.MapGet("/members/{id}", async (ArasakaDbContext db, long id) =>
        {
            var originMemberEntity = await db.Members.FindAsync(id);
            if (originMemberEntity is null)
                return Results.NotFound();

            return Results.Ok(originMemberEntity.ConvertToViewModel());
        });

        endpoints.MapPut("/members/{id}", async (ArasakaDbContext db, UpdateMemberViewModel updateMemberViewModel, long id) =>
        {
            var originMemberEntity = await db.Members.FindAsync(id);
            if (originMemberEntity is null)
                return Results.NotFound();

            updateMemberViewModel.AssignFieldsTo(originMemberEntity);
            originMemberEntity.LastUpdateTime = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync();

            return Results.Ok(originMemberEntity.ConvertToViewModel());
        });

        endpoints.MapDelete("/members/{id}", async (ArasakaDbContext db, long id) =>
        {
            var originMemberEntity = await db.Members.FindAsync(id);
            if (originMemberEntity is null)
                return Results.NotFound();

            db.Members.Remove(originMemberEntity);
            await db.SaveChangesAsync();

            return Results.Ok();
        });

        return endpoints;
    }
}
