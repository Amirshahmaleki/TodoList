using Microsoft.EntityFrameworkCore;
using Service.Model;

namespace Service.TodoService;

public class ToDoContext : DbContext
{
    public DbSet<TodoDto> ToDos { get; set; }
    public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
    {

    }

}