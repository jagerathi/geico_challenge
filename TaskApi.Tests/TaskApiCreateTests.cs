using Microsoft.EntityFrameworkCore;
using TaskApi.Context;
using TaskApi.Controllers;
using TaskApi.Enums;
using TaskApi.Exceptions;
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
        public async Task Should_Return_Task_Item_With_Id()
        {
            var task = new TaskItemDto
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

            var response = await controller.PostTaskItem(task);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(response.Value?.Id, 0);
        }

        [TestMethod]
        public async Task Should_Throw_Exception_Of_Invalid_Due_Date_If_Due_Date_Prior_To_Today()
        {
            var task = new TaskItemDto
            {
                Description = "Test Task",
                DueDate = DateTime.Today.AddDays(-1.0),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                Name = "Test Task",
                Priority = Priority.Low,
                Status = Status.New
            };

            var controller = SetupController();

            async Task act() => await controller.PostTaskItem(task);

            await Assert.ThrowsExceptionAsync<InvalidDueDateException>(act);

        }
        
    }
}