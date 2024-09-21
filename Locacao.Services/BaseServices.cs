using Locacao.Repository.Interface;
using Locacao.Services.Inteface;

namespace Locacao.Services
{
    public class BaseServices<T> : IBaseServices<T> where T : Domain.Model.Base.ModelBase
    {
        private readonly IBaseRepository<T> _repository;

        public BaseServices(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(T entity)
        {
            entity.CreationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            entity.ModificationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}