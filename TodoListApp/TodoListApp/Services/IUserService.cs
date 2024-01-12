using TodoListApp.Persistance.Entities;

namespace TodoListApp.Services
{
    public interface IUserService
    {
        User CurrentUser { get; }
        bool IsAuthenticateed { get; }
        bool IsCurrentUserInRole(params string[] roles);
    }
}
