namespace SourceName.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string Token { get; }
        string UserId { get; }
    }
}