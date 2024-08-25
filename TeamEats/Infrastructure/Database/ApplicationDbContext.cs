using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TeamEats.Infrastructure.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
	public ApplicationDbContext(DbContextOptions options) : base(options)
	{
	}
}

public class ApplicationUser : IdentityUser<Guid>
{
}

public class ApplicationRole : IdentityRole<Guid>
{
}