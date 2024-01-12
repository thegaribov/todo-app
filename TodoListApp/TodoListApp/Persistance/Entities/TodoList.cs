using System.Text.Json.Serialization;

namespace TodoListApp.Persistance.Entities;

public class TodoList
{
    public string Name { get; set; }
    public int Id { get; set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public ICollection<TodoListItem> TodoListItems { get; set; }
}
