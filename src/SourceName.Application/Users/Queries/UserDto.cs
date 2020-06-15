using SourceName.Application.Common.Mappings;
using SourceName.Domain.Users;

namespace SourceName.Application.Users
{
    public class UserDto : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
    }
}