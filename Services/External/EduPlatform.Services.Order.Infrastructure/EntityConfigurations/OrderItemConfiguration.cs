using EduPlatform.Services.Order.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
namespace EduPlatform.Services.Order.Infrastructure.EntityConfigurations
{

	public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		private const string DEFAULT_SCHEMA = "ordering";
		void IEntityTypeConfiguration<OrderItem>.Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.ToTable("OrderItems", DEFAULT_SCHEMA);

			builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
		}
	}
}
