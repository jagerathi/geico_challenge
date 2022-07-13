using Microsoft.EntityFrameworkCore;
using TaskApi.Context;
using TaskApi.Controllers;
using TaskApi.Enums;
using TaskApi.Model;

namespace TaskApi.Tests
{
    [TestClass]
    public class TaskApiCreateTests
    {
        private TaskApiContext SetupContext()
        {
            var options = new DbContextOptionsBuilder<TaskApiContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TaskApiContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private TaskItemsController SetupController()
        {
            var context = SetupContext();
            return new TaskItemsController(context);
        }

        [TestMethod]
        public void Should_Return_Task_Item_With_Id()
        {
            var task = new TaskItemModel
            {
                Description = "Test Task",
                DueDate = DateTime.Today,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                Name = "Test Task",
                Priority = Priority.Low,
                Status = Status.New
            };

            var controller = SetupController();

            var response = controller.PostTaskItem(task);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);
            Assert.AreNotEqual(response.Result.Value?.Id, 0);
        }
        
    }
}