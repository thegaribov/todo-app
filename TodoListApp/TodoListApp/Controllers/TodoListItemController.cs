using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Dtos;
using TodoListApp.Persistance;
using TodoListApp.Persistance.Entities;

namespace TodoListApp.Controllers;

[ApiController]
[Route("todo-list-items")]
public class TodoListItemController : ControllerBase
{
    private readonly TodoListAppDbContext _dbContext;
    private readonly IMapper _mapper;

    public TodoListItemController(TodoListAppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var todoListItems = await _dbContext.TodoListItems
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<TodoListItemDto>>(todoListItems));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoLisItem(TodoListItemCreateDto dto)
    {
        var todoListItem = new TodoListItem
        {
            Title = dto.Title,
            Content = dto.Content,
            IsCompleted = false,
            IsStarred = false,
            TodoListId = dto.TodoListId,
            Order = 1,
        };


        var todoListItems = await _dbContext.TodoListItems
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<TodoListItemDto>>(todoListItems));
    }
}
