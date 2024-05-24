using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Model;
using Service.TodoService;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[action]/[controller]")]
       public class ToDoController : ControllerBase
    {
        private readonly ToDoContext _dbContext;

        public ToDoController(ToDoContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("[Controller]/[Action]")]
        [HttpGet] 
        
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetToDos()
        {
            if (_dbContext.ToDos==null)
            {
                return NotFound();
            }

            return await _dbContext.ToDos.ToListAsync();
        }
        [Route("[Controller]/[Action]")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetToDo(int id)
        {
            if (_dbContext.ToDos == null)
            {
                return NotFound();
            }

            var todo = await _dbContext.ToDos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        
        public async Task<ActionResult<TodoDto>> PostToDo(TodoDto todo)
        {
            _dbContext.ToDos.Add(todo);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetToDo), new {id = todo.ID}, todo);
        }

        [Route("[Controller]/[Action]")]
        [HttpPut]

        public async Task<IActionResult> PutToDo(int id, TodoDto todo)
        {
            if (id != todo.ID)
            {
                return BadRequest();
            }

            _dbContext.Entry(todo).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (ToDoAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        private bool ToDoAvailable(int id)
        {
            return (_dbContext.ToDos?.Any(x => x.ID == id)).GetValueOrDefault();
        }

        [Route("[Controller]/[Action]")]
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteToDo(int id)
        {
            if (_dbContext.ToDos == null)
            {
                return NotFound();
            }

            var todo = await _dbContext.ToDos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _dbContext.ToDos.Remove(todo);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        
    }

} 