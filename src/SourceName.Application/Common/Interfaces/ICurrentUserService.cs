namespace SourceName.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string Token { get; }
#if (UseRaven)
        string UserId { get; }
#else
        int? UserId { get; }
#endif
    }
}