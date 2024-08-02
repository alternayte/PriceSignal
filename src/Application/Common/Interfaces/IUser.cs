namespace Application.Common.Interfaces;

public interface IUser
{
    string UserIdentifier { get; }
    string? Name { get; }
    string Email { get; }
    string Role { get; }
    bool IsAuthenticated { get;}
}