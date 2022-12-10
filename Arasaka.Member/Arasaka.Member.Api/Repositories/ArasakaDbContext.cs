using Microsoft.EntityFrameworkCore;
using Arasaka.Member.Api.Modules.Members.Models;

namespace Arasaka.Member.Api.Repositories;

class ArasakaDbContext : DbContext
{
    public ArasakaDbContext(DbContextOptions options) : base(options) { }
    public DbSet<MemberEntity> Members { get; set; } = null!;
}
