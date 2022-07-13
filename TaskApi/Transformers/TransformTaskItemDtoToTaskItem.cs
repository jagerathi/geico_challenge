using TaskApi.Domain;
using TaskApi.Model;

namespace TaskApi.Transformers
{
    public class TransformTaskItemDtoToTaskItem
    {
        public static TaskItem Transform(TaskItemDto item)
        {
            return new TaskItem
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