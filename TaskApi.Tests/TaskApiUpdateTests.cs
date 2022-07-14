using System.Threading.Tasks;
using TaskApi.Enums;
using TaskApi.Exceptions;
using TaskApi.Model;

namespace TaskApi.Tests
{
    [TestClass]
    public class TaskApiUpdateTests : TestBase
    {
        
        [TestMethod]
        public async Task Should_Return_Same_Id()
        {
            var controller = SetupController(1);

            var response = await controller.GetTaskItem(1);

            Assert.IsNotNull(response.Value);

            var task = response.Value;

            task.Status = Status.InProgress;

            var response2 = await controller.PutTaskItem(task.Id, task);

            Assert.IsNotNull(response2.Value);
            Assert.AreEqual(response2.Value.Id, task.Id);
        }

        [TestMethod]
        public async Task Should_Throw_Exception_Of_Task_Item_Not_Found_When_Invalid_Id_Used()
        {
            var controller = SetupController(1);

            var task = new TaskItemDto
            {
                Id = Int32.MaxValue,
                Name = "Bad",
                Description = "Bad",
                DueDate = DateTime.Today,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                Status = Status.New,
                Priority = Priority.Low
            };

            async Task act() => await controller.PutTaskItem(task.Id, task);

            await Assert.ThrowsExceptionAsync<TaskItemNotFoundException>(act);
        }

        [TestMethod]
        public async Task Should_Throw_Exception_Of_Invalid_End_Date_If_End_Date_Prior_To_Start_Date()
        {
            var controller = SetupController(1);

            var response  = await controller.GetTaskItem(1);

            Assert.IsNotNull(response.Value);

            var task = response.Value;

            task.EndDate = task.StartDate.AddDays(-1.0);

            async Task act() => await controller.PutTaskItem(task.Id, task);

            await Assert.ThrowsExceptionAsync<InvalidEndDateException>(act);
        }

        [TestMethod]
        public async Task Should_Throw_Exception_Of_Too_Many_High_Priority_Tasks_For_Due_Date_When_Changing_Task_To_High_Priority_And_Open_Tasks_Surpass_Limit_For_Due_Date()
        {
            var controller = SetupController(100, Status.New, Priority.High);

            var newTask = new TaskItemDto
            {
                Name = "New Task",
                Description = "Changing to HIgh Priority next",
                Status = Status.New,
                Priority = Priority.Low,
                StartDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(1.0),
                EndDate = DateTime.Today.AddDays(2.0),
            };

            var response = await controller.PostTaskItem(newTask);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(response.Value?.Id, 0);

            var updateTask = response.Value;

            if(updateTask != null)
            { 
                updateTask.Priority = Priority.High;
                async Task act() => await controller.PutTaskItem(updateTask.Id, updateTask);
                await Assert.ThrowsExceptionAsync<TooManyHighPriorityTasksForDueDateException>(act);
            }
        }
    }
}