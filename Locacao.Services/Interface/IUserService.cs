using Locacao.Domain.Model;
using Locacao.Services.Inteface;

namespace Locacao.Services.Interface
{
    public interface IUserService : IBaseServices<User>
    {        
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<User> ValidateAndGetUserAsync(string username, string password);
    }
}