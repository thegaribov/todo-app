using AutoMapper;
using TodoListApp.Dtos;
using TodoListApp.Persistance.Entities;

namespace TodoListApp.Mappers;

public class TodoListItemMapper : Profile
{
	public TodoListItemMapper()
	{
		CreateMap<TodoListItem, TodoListItemDto>();
	}
}
