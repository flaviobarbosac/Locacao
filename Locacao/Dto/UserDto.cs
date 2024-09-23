namespace Locacao.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; }    
        public int Profile { get; set; }
    }
}