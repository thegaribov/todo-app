using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Helpers.Paging;
using TodoListApp.Persistance;
using TodoListApp.Persistance.Entities;
using TodoListApp.Services;

namespace TodoListApp.Controllers;

[Route("api/todo-lists")]
[ApiController]
[Authorize]
public class TodoListController : ControllerBase
{
    private readonly TodoListAppDbContext _dbContext;
    private readonly IUserService _userService;

    public TodoListController(TodoListAppDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] QueryParams queryParams)
    {
        var todoListsQuery = _dbContext.TodoLists
            .AsNoTracking()
            .AsQueryable();

        var paginator = new Paginator<TodoList>(todoListsQuery, queryParams.Page, queryParams.PageSize);

        HttpContext.Response.Headers.Add("X-Pagination", paginator.ToString());

        return Ok(await paginator.QuerySet.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var todoList = await _dbContext.TodoLists.FirstOrDefaultAsync(tl => tl.Id == id);
        if (todoList == null)
        {
            return NotFound();
        }

        return Ok(todoList);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] string name)
    {
        var todoList = new TodoList
        {
            Name = name,
            UserId = _userService.CurrentUser.Id,
        };

        await _dbContext.TodoLists.AddAsync(todoList);
        await _dbContext.SaveChangesAsync();

        return Created(string.Empty, todoList);
    }

    [HttpPut("{id}", Name = "todolist-update")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] string name)
    {
        var todoList = await _dbContext.TodoLists.FirstOrDefaultAsync(tl => tl.Id == id);
        if (todoList == null)
        {
            return NotFound();
        }

        todoList.Name = name;

        await _dbContext.SaveChangesAsync();

        return Ok(todoList);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAsync([FromRoute] int id)
    {
        var todoList = await _dbContext.TodoLists.FirstOrDefaultAsync(tl => tl.Id == id);
        if (todoList == null)
        {
            return NotFound();
        }

        _dbContext.TodoLists.Remove(todoList);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
