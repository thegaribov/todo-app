namespace TodoListApp.Persistance.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActivated { get; set; }
        public DateTime ActivationExpireDate { get; set; }

        public ICollection<TodoList> TodoLists { get; set; }
    }
}
