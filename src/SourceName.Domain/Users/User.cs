using SourceName.Domain.Common.Entities;

namespace SourceName.Domain.Users
{
    [CollectionName("Users")]
    public class User : BaseEntity<string>
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}