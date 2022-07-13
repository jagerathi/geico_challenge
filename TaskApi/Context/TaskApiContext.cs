using Microsoft.EntityFrameworkCore;
using TaskApi.Model;

namespace TaskApi.Context
{
    public class TaskApiContext : DbContext 
    {
        public TaskApiContext(DbContextOptions<TaskApiContext> options) : base(options)
        {

        }

        public DbSet<TaskItem> TaskItems { get;set; } = null!;
    }
}
