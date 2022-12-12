using Arasaka.Member.Api.Modules.Members.ViewModels;
using Arasaka.Member.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Arasaka.Member.Api.Modules.Members.Constants;

namespace Arasaka.Member.Api.Modules.Members;

public static class MembersModule
{
    /// <summary>
    /// key: current state & command
    /// value: next state
    /// </summary>
    public static Dictionary<(MemberState, MemberCommand), MemberState> StateDiagram = new Dictionary<(MemberState, MemberCommand), MemberState>()
    {
        [(MemberState.FSM_BEGIN, MemberCommand.SignUp)] = MemberState.Unverified,

        [(MemberState.Unverified, MemberCommand.Verify)] = MemberState.Verified,
        [(MemberState.Unverified, MemberCommand.Ban)] = MemberState.Banned,
        [(MemberState.Unverified, MemberCommand.Remove)] = MemberState.FSM_END,

        [(MemberState.Verified, MemberCommand.Restrict)] = MemberState.Restricted,
        [(MemberState.Verified, MemberCommand.Remove)] = MemberState.FSM_END,

        [(MemberState.Restricted, MemberCommand.Ban)] = MemberState.Banned,
        [(MemberState.Restricted, MemberCommand.Allow)] = MemberState.Verified,
        [(MemberState.Restricted, MemberCommand.Remove)] = MemberState.FSM_END,

        [(MemberState.Banned, MemberCommand.Permit)] = MemberState.Restricted,
        [(MemberState.Banned, MemberCommand.Remove)] = MemberState.FSM_END,
    };

    public static (bool CanTransfer, MemberState? NextState) TryGetTransferResult(MemberState currentState, MemberCommand command)
    {
        if (StateDiagram.TryGetValue((currentState, command), out var nextState))
        {
            return (true, nextState);
        }

        return (false, null);
    }

    public static IServiceCollection RegisterMembersModule(this IServiceCollection services)
    {
        return services;
    }

    public static Task ConfigureMembersModuleAsync(this WebApplication app)
    {
        return Task.CompletedTask;
    }

    private static async Task<IResult> HandleMemberCommadAsync(ArasakaDbContext db, long id, MemberCommand command)
    {
        var memberEntity = await db.Members.FindAsync(id);
        if (memberEntity is null)
            return Results.NotFound();

        var currentState = Enum.Parse<MemberState>(memberEntity.State);
        var transferStateResult = TryGetTransferResult(currentState, command);

        if (transferStateResult.CanTransfer)
        {
            memberEntity.State = transferStateResult.NextState.Value.ToString();
            memberEntity.LastUpdateTime = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync();

            return Results.Ok(memberEntity.ConvertToViewModel());
        }

        return Results.BadRequest();
    }

    public static IEndpointRouteBuilder MapMembersModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/members:signup", async (ArasakaDbContext db, SignUpMemberViewModel signupMemberViewModel) =>
        {
            var memberEntity = signupMemberViewModel.ConvertToEntity();
            memberEntity.RegisterTime = DateTimeOffset.UtcNow;
            memberEntity.State = MemberState.Unverified.ToString();
            memberEntity.LastUpdateTime = memberEntity.RegisterTime;

            await db.Members.AddAsync(memberEntity);
            await db.SaveChangesAsync();

            return Results.Created($"/members/{memberEntity.Id}", memberEntity.ConvertToViewModel());
        });

        endpoints.MapPost("/members/{id}:verify", async (ArasakaDbContext db, long id) =>
        {
            return await HandleMemberCommadAsync(db, id, MemberCommand.Verify);
        });

        endpoints.MapPost("/members/{id}:restrict", async (ArasakaDbContext db, long id) =>
        {
            return await HandleMemberCommadAsync(db, id, MemberCommand.Restrict);
        });

        endpoints.MapPost("/members/{id}:allow", async (ArasakaDbContext db, long id) =>
        {
            return await HandleMemberCommadAsync(db, id, MemberCommand.Allow);
        });

        endpoints.MapPost("/members/{id}:ban", async (ArasakaDbContext db, long id) =>
        {
            return await HandleMemberCommadAsync(db, id, MemberCommand.Ban);
        });

        endpoints.MapPost("/members/{id}:permit", async (ArasakaDbContext db, long id) =>
        {
            return await HandleMemberCommadAsync(db, id, MemberCommand.Permit);
        });

        endpoints.MapDelete("/members/{id}:remove", async (ArasakaDbContext db, long id) =>
        {
            var originMemberEntity = await db.Members.FindAsync(id);
            if (originMemberEntity is null)
                return Results.NoContent();

            db.Members.Remove(originMemberEntity);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        //endpoints.MapGet("/members:list", async ([FromQuery] PaginationParameters pagination, ArasakaDbContext db, HttpContext http, CancellationToken token) =>
        //{
        //    var queryResult = await db.Members.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();

        //    //foreach (var item in queryResult)
        //    //{
        //    //    yield return item.ConvertToViewModel();
        //    //}

        //    return queryResult.Select(x => x.ConvertToViewModel());



        //    //var originMemberEntity = await db.Members.FindAsync(id);
        //    //if (originMemberEntity is null)
        //    //    return Results.NoContent();

        //    //db.Members.Remove(originMemberEntity);
        //    //await db.SaveChangesAsync();

        //    //return Results.NoContent();
        //});

        endpoints.MapGet("/members:list", async ([FromQuery] int pageSize, [FromQuery] int pageNumber, ArasakaDbContext db, HttpContext http, CancellationToken token) =>
        {
            var queryResult = await db.Members.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            //foreach (var item in queryResult)
            //{
            //    yield return item.ConvertToViewModel();
            //}

            return queryResult.Select(x => x.ConvertToViewModel());



            //var originMemberEntity = await db.Members.FindAsync(id);
            //if (originMemberEntity is null)
            //    return Results.NoContent();

            //db.Members.Remove(originMemberEntity);
            //await db.SaveChangesAsync();

            //return Results.NoContent();
        });


        return endpoints;
    }
}
