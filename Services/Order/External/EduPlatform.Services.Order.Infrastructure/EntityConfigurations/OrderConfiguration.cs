using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EduPlatform.Services.Order.Infrastructure.EntityConfigurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order.Domain.OrderAggregate.Order>
	{
		private const string DEFAULT_SCHEMA = "ordering";
		public void Configure(EntityTypeBuilder<Domain.OrderAggregate.Order> builder)
		{
			builder.ToTable("Orders", DEFAULT_SCHEMA);
			builder.OwnsOne(o => o.Address).WithOwner();
		}
	}
}
