using TodoListApp.Persistance.Entities.Enums;
using TodoListApp.Persistance.Entities;
using TodoListApp.Persistance;
using Microsoft.EntityFrameworkCore;

namespace TodoListApp.Services;

public class UserService : IUserService
{
    private readonly TodoListAppDbContext _pustokDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private User _currentUser = null;


    public UserService(
        TodoListAppDbContext pustokDbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _pustokDbContext = pustokDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public User CurrentUser
    {
        get
        {

            return _currentUser ??= GetCurrentLoggedUser();
        }
    }

    public bool IsAuthenticateed
    {
        get
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }


    public bool IsCurrentUserInRole(params string[] roles)
    {
        return roles.Any(r => _httpContextAccessor.HttpContext.User.IsInRole(r));
    }

    private User GetCurrentLoggedUser()
    {
        var currentUserId = _httpContextAccessor.HttpContext.User
            .FindFirst(c => c.Type == "Id").Value;

        return _pustokDbContext.Users
            .Single(u => u.Id == Convert.ToInt32(currentUserId));
    }
}
