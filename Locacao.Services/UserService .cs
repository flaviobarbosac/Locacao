using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Locacao.Services.Interface;
using System.Security.Cryptography;


namespace Locacao.Services
{
    public class UserService : BaseServices<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private const int Iterations = 10000;
        private const int HashSize = 32; // 256 bits

        public UserService(IUserRepository userRepository)
            : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userRepository.GetUserByUserNameAsync(userName);
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null)
            {
                return false;
            }

            return VerifyPassword(password, user.Password);
        }

        private string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashBytes = new byte[HashSize + 16];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, HashSize);

            return Convert.ToBase64String(hashBytes);
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

        public async Task<User> CreateUserAsync(User user)
        {
            if (await _userRepository.GetUserByUserNameAsync(user.Username) != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            user.Password = HashPassword(user.Password);
            await _userRepository.AddAsync(user);
            return await _userRepository.GetUserByUserNameAsync(user.Username);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            if (user.Password != existingUser.Password)
            {
                user.Password = HashPassword(user.Password);
            }

            await _userRepository.UpdateAsync(user);
            return await _userRepository.GetUserByUserNameAsync(user.Username);
        }

        public async Task<User> ValidateAndGetUserAsync(string username, string password)
        {
            return await _userRepository.ValidateUserCredentialsAsync(username, password);
        }       
    }
}