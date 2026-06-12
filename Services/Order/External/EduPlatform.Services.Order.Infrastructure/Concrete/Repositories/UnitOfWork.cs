using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Services.Order.Infrastructure.Data;

namespace EduPlatform.Services.Order.Infrastructure.Concrete.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _context;
		public UnitOfWork(AppDbContext context)
		{
			_context = context;
		}
		void IUnitOfWork.Commit()
		{
			_context.SaveChanges();
		}
		async Task IUnitOfWork.CommitAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
