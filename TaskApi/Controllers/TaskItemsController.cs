using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Context;
using TaskApi.Domain;
using TaskApi.Model;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly TaskApiContext _context;

        public TaskItemsController(TaskApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTaskItems()
        {
            return await _context.TaskItems.Select(x => Transformers.TransformTaskItemToTaskItemDto.Transform(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDto>> GetTaskItem(long id)
        {
            if (_context.TaskItems == null)
            {
                return NotFound();
            }
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return Transformers.TransformTaskItemToTaskItemDto.Transform(taskItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItemDto>> PutTaskItem(long id, TaskItemDto taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }

            var task = await _context.TaskItems.FindAsync(id);

            if (task == null) return NotFound();

            task.Status = taskItem.Status;
            task.EndDate = taskItem.EndDate;
            task.Description = taskItem.Description;
            task.DueDate = taskItem.DueDate;
            task.Name = taskItem.Name;
            task.Priority = taskItem.Priority;
            task.StartDate = taskItem.StartDate;
            task.Status = taskItem.Status;

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Transformers.TransformTaskItemToTaskItemDto.Transform(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> PostTaskItem(TaskItemDto taskItem)
        {
            var item = Transformers.TransformTaskItemDtoToTaskItem.Transform(taskItem);

            _context.TaskItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskItem), new { id = item.Id }, taskItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(long id)
        {
            if (_context.TaskItems == null)
            {
                return NotFound();
            }
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskItemExists(long id)
        {
            return (_context.TaskItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
