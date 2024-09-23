namespace Locacao.Dto
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
