using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Dtos;
using TodoListApp.Persistance;

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
}
