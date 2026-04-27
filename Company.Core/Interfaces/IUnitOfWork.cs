using Company.Core.Interfaces;

namespace Company.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> CompleteAsync();
    }
}