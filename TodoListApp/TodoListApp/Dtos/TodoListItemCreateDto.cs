namespace TodoListApp.Dtos
{
    public class TodoListItemCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int TodoListId { get; set; }
        public int Order { get; set; }
    }
}
