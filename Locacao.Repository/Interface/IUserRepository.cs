using Locacao.Domain.Model;

namespace Locacao.Repository.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByUserNameAsync(string userName = null);
        Task<User> ValidateUserCredentialsAsync(string userName, string password);
    }
}
