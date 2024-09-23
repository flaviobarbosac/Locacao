using Locacao.Domain.Enum;

namespace Locacao.Domain.Model
{
    public class User : Base.ModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserProfile Profile { get; set; }
    }
}