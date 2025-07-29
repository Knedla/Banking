namespace Banking.Application.Interfaces.Services;

public interface IUserContextService
{
    string? GetUserId();
    string? GetUserName();
    string[] GetRoles();
    bool IsInRole(string role);
}
