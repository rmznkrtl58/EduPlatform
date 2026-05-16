using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Services.Order.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace EduPlatform.Services.Order.Infrastructure.Concrete.Repositories
{
	internal class GenericRepository<T> : IGenericRepository<T>
		where T :class
	{
		private readonly AppDbContext _context;
		protected readonly DbSet<T> _dbSet;
		public GenericRepository(AppDbContext context)
		{
			_context = context;
			_dbSet = _context!.Set<T>();
			
		}
		public  async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
		{
			return await _dbSet.AnyAsync(filter);
		}

		public async Task CreateAsync(T t)
		{
			await _dbSet.AddAsync(t);
		}

		public void Delete(T t)
		{
			_dbSet.Remove(t);
		}

		public IQueryable<T> GetListAll()
		{
			return _dbSet.AsNoTracking().AsQueryable();
		}

		public async Task<IEnumerable<T>> GetListByFilter(Expression<Func<T, bool>> filter)
		{
			return await _dbSet.Where(filter).AsNoTracking().ToListAsync();
		}

		public void Update(T t)
		{
			_dbSet.Update(t);
		}

		public async Task<T> GetByFilter(Expression<Func<T, bool>> filter)
		{
			return await _dbSet.Where(filter).AsNoTracking().FirstOrDefaultAsync();
		}
	}
}
