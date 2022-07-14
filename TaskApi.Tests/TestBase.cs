using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskApi.Context;
using TaskApi.Controllers;
using TaskApi.Domain;
using TaskApi.Enums;

namespace TaskApi.Tests
{
    public class TestBase
    {
        private TaskApiContext SetupContext()
        {
            var options = new DbContextOptionsBuilder<TaskApiContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var context = new TaskApiContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        protected TaskItemsController SetupController(int recordsToGenerate, Status? status = null, Priority? priority = null)
        {
            var context = SetupContext();
            var controller = new TaskItemsController(context);

            for (int i = 1; i <= recordsToGenerate; ++i)
            {
                var item = new TaskItem
                {
                    Id = i,
                    Name = $"Default Item {i}",
                    Description = $"Description of default item {i}",
                    DueDate = DateTime.Today.AddDays(1.0),
                    EndDate = DateTime.Today.AddDays(2.0),
                    StartDate = DateTime.Today,
                    Priority = priority.GetValueOrDefault(Priority.Low),
                    Status = status.GetValueOrDefault(Status.New)
                };
                context.Add(item);
            }
            context.SaveChanges();
            return controller;
        }
    }
}
