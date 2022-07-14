using TaskApi.Enums;
using TaskApi.Exceptions;
using TaskApi.Model;

namespace TaskApi.Tests
{
    [TestClass]
    public class TaskApiCreateTests : TestBase
    {
        [TestMethod]
        public async Task Should_Return_Task_Item_With_Id()
        {
            var task = new TaskItemDto
            {
                Description = "Test Task",
                DueDate = DateTime.Today.AddDays(1.0),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2.0),
                Name = "Test Task",
                Priority = Priority.Low,
                Status = Status.New
            };

            var controller = SetupController(0);

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

            var controller = SetupController(0);

            async Task act() => await controller.PostTaskItem(task);

            await Assert.ThrowsExceptionAsync<InvalidDueDateException>(act);

        }

        [TestMethod]
        public async Task Should_Throw_Exception_Of_Invalid_End_Date_If_End_Date_Prior_To_Start_Date()
        {
            var task = new TaskItemDto
            {
                Description = "Test Task",
                DueDate = DateTime.Today.AddDays(1.0),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(-1.0),
                Name = "Test Task",
                Priority = Priority.Low,
                Status = Status.New
            };

            var controller = SetupController(0);

            async Task act() => await controller.PostTaskItem(task);

            await Assert.ThrowsExceptionAsync<InvalidEndDateException>(act);
        }

        [TestMethod]
        public async Task Should_Throw_Exception_Of_Too_Many_High_Priority_Tasks_For_Due_Date_Exception_When_Open_Tasks_Surpass_Limit_For_Due_Date()
        {
            var task = new TaskItemDto
            {
                Description = "Test Task",
                DueDate = DateTime.Today.AddDays(1.0),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2.0),
                Name = "Test Task",
                Priority = Priority.High,
                Status = Status.New
            };

            var controller = SetupController(100, Status.New, Priority.High);

            async Task act() => await controller.PostTaskItem(task);

            await Assert.ThrowsExceptionAsync<TooManyHighPriorityTasksForDueDateException>(act);
        }
        
    }
}