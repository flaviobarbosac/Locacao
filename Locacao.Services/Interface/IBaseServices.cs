using Locacao.Domain.Model;

namespace Locacao.Services.Inteface
{
    public interface IBaseServices<T>
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
