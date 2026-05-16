namespace EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories
{
	public interface IUnitOfWork
	{
		void Commit();
		Task CommitAsync();
	}
}
