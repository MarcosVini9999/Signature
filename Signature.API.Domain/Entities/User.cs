using Signature.API.Domain.Common;

namespace Signature.API.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }

        private User() { }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
