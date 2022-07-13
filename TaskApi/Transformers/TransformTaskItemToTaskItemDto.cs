using TaskApi.Domain;
using TaskApi.Model;

namespace TaskApi.Transformers
{
    public class TransformTaskItemToTaskItemDto
    {
        public static TaskItemDto Transform(TaskItem item)
        {
            return new TaskItemDto
            {
                Id = item.Id,
                EndDate = item.EndDate,
                Status = item.Status,
                StartDate = item.StartDate,
                DueDate = item.DueDate,
                Priority = item.Priority,
                Description = item.Description,
                Name = item.Name
            };
        }
    }
}