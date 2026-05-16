using System.Linq.Expressions;
namespace EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories
{
	public interface IGenericRepository<T> where T :class
	{
		IQueryable<T> GetListAll();
		Task<T> GetByFilter(Expression<Func<T, bool>> filter);
		Task<IEnumerable<T>> GetListByFilter(Expression<Func<T, bool>> filter);
		Task CreateAsync(T t);
		void Update(T t);
		void Delete(T t);
		Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
	}
}
