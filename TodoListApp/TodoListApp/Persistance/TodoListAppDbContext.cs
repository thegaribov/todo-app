using Microsoft.EntityFrameworkCore;
using TodoListApp.Persistance.Entities;

namespace TodoListApp.Persistance;

public class TodoListAppDbContext : DbContext
{
    public TodoListAppDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    


    public DbSet<User> Users { get; set; }
    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<TodoListItem> TodoListItems { get; set; }
}
