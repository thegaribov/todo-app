namespace TodoListApp.Persistance.Entities;

public class TodoListItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsStarred { get; set; }
    public bool IsCompleted { get; set; }
    public int Order { get; set; }

    public int TodoListId { get; set; }
    public TodoList TodoList { get; set; }
}
