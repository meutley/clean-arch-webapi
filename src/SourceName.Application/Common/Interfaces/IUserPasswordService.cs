namespace SourceName.Application.Common.Interfaces
{
    public interface IUserPasswordService
    {
        void CreateHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool ValidateHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}