namespace Banking.Application.Models.Requests;

public abstract class BaseRequest
{
    public Guid UserId { get; set; } // TODO: should be loaded; represent app userId -> someone logged in to the mobile app, system worker, branch employee
}
