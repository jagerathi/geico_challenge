using Microsoft.EntityFrameworkCore;
using TaskApi.Model;

namespace TaskApi.Context
{
    public class TaskApiContext : DbContext 
    {
        public TaskApiContext(DbContextOptions<TaskApiContext> options) : base(options)
        {



        }

        public DbSet<TaskItemModel> TaskItems { get;set; } = null!;
    }
}
