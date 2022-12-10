using Microsoft.EntityFrameworkCore;

namespace Arasaka.Member.Api.Repositories;

class ArasakaDbContext : DbContext
{
    public ArasakaDbContext(DbContextOptions options) : base(options) { }
    public DbSet<MemberEntity> Members { get; set; } = null!;
}
