using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using TaskApi.Context;
using TaskApi.Controllers;
using TaskApi.Domain;
using TaskApi.Enums;
using TaskApi.Model;

namespace TaskApi.Tests
{
    [TestClass]
    public class TaskApiUpdateTests
    {
        private TaskApiContext SetupContext()
        {
            var options = new DbContextOptionsBuilder<TaskApiContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TaskApiContext(options);
            context.Database.EnsureCreated();

            context.TaskItems.Add(new TaskItem
            {
                Id = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                Status = Status.New,
                Description = "Default Item 1",
                DueDate = DateTime.Today,
                Name = "Default Item 1",
                Priority = Priority.Medium
            });
            
            return context;
        }

        private TaskItemsController SetupController()
        {
            var context = SetupContext();
            return new TaskItemsController(context);
        }


        [TestMethod]
        public async Task Should_Return_Same_Id()
        {
            var controller = SetupController();

            var response = await controller.GetTaskItem(1);

            Assert.IsNotNull(response.Value);

            var task = response.Value;

            task.Status = Status.InProgress;

            var response2 = await controller.PutTaskItem(task.Id, task);

            Assert.IsNotNull(response2.Value);
            Assert.AreEqual(response2.Value.Id, task.Id);


        }
        
    }
}