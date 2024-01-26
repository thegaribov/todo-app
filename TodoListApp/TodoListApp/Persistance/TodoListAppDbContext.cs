using Microsoft.EntityFrameworkCore;
using TodoListApp.Persistance.Entities;

namespace TodoListApp.Persistance;

public class TodoListAppDbContext : DbContext
{
    public TodoListAppDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<TodoListItem> TodoListItems { get; set; }
}
