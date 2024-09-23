using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Locacao.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly Infraestructure.LocacaoDbContext _context;
        private readonly DbSet<User> _dbSet;
        private const int Iterations = 10000;
        private const int HashSize = 32; // 256 bits

        public UserRepository(Infraestructure.LocacaoDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("userName cannot be null or empty", nameof(userName));
            }

            var user = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Username == userName);

            if (user != null)
            {
                // Substituímos a senha hash por um placeholder
                user.Password = "[PROTECTED]";
            }

            return user;
        }

        // Método adicional para validar credenciais
        public async Task<User> ValidateUserCredentialsAsync(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password cannot be null or empty");
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Username == userName);

            if (user != null && VerifyPassword(password, user.Password))
            {
                // Se as credenciais são válidas, retornamos o usuário com a senha substituída
                user.Password = "[PROTECTED]";
                return user;
            }

            return null;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }
            return true;
        }
    }
}