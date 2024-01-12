using TodoListApp.Persistance.Entities;

namespace TodoListApp.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
