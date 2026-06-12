using Microsoft.EntityFrameworkCore;
namespace EduPlatform.Services.Order.Infrastructure.Data
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
	: base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureAssembly).Assembly);
			base.OnModelCreating(modelBuilder);
		}
	}
}
