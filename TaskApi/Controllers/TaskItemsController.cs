using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Context;
using TaskApi.Domain;
using TaskApi.Exceptions;
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

            ValidateExistingItem(taskItem);

            var task = await _context.TaskItems.FindAsync(id);

            if (task == null) throw new TaskItemNotFoundException();

            if(taskItem.Priority == Enums.Priority.High && task.Priority != Enums.Priority.High)
                CheckHighPriorityLimit(taskItem);

            task.Status = taskItem.Status;
            task.EndDate = taskItem.EndDate;
            task.Description = taskItem.Description;
            task.DueDate = taskItem.DueDate;
            task.Name = taskItem.Name;
            task.Priority = taskItem.Priority;
            task.StartDate = taskItem.StartDate;
            task.Status = taskItem.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TaskItemExists(id))
                {
                    throw new TaskItemNotFoundException();
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
            ValidateNewItem(taskItem);

            var item = Transformers.TransformTaskItemDtoToTaskItem.Transform(taskItem);

            _context.Add(item);

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

            _context.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskItemExists(long id)
        {
            return (_context.TaskItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void ValidateNewItem(TaskItemDto item)
        {
            if(item.DueDate.ToUniversalTime().Date < DateTime.UtcNow.Date || item.DueDate.ToUniversalTime().Date < item.StartDate.ToUniversalTime().Date) throw new InvalidDueDateException();

            if(item.EndDate.ToUniversalTime().Date < item.StartDate.ToUniversalTime().Date) throw new InvalidEndDateException();

            if(item.Priority == Enums.Priority.High)
                CheckHighPriorityLimit(item);
        }

        private void ValidateExistingItem(TaskItemDto item)
        {
            if (item.EndDate.ToUniversalTime().Date < item.StartDate.ToUniversalTime().Date) throw new InvalidEndDateException();

        }

        private void CheckHighPriorityLimit(TaskItemDto item)
        {
            var count = _context.TaskItems.Count(x => x.DueDate.Date == item.DueDate.ToUniversalTime().Date && x.Priority == Enums.Priority.High && x.Status != Enums.Status.Finished && x.Id != item.Id);
            if(count >= 100)
                throw new TooManyHighPriorityTasksForDueDateException();
        }
    }
}
