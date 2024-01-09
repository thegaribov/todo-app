﻿namespace TodoListApp.Persistance.Entities;

public class TodoList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<TodoListItem> TodoListItems { get; set; }
}