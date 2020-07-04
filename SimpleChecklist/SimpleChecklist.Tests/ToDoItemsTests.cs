using Autofac;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.UI.ViewModels;
using System.Linq;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class ToDoItemsTests
    {
        [Test]
        public void AddToDoItem()
        {
            // given
            var container = Utils.Initialize(true);
            var taskListViewModel = container.Resolve<TaskListViewModel>();

            // when
            taskListViewModel.EntryText = "test item";
            taskListViewModel.AddClickCommand.Execute(null);

            // then
            Assert.IsTrue(taskListViewModel.ToDoItems.Any(item => item.Data == "test item"));
        }

        [Test]
        public void MoveToDoneListTest()
        {
            // given
            var container = Utils.Initialize(true);
            var taskListViewModel = container.Resolve<TaskListViewModel>();
            var doneListViewModel = container.Resolve<DoneListViewModel>();

            ToDoItem item = new ToDoItem() { Data = "test item" };
            taskListViewModel.ToDoItems.Add(item);

            // when
            taskListViewModel.DoneClickCommand?.Execute(item);

            // then
            Assert.IsFalse(taskListViewModel.ToDoItems.Any());
            Assert.IsTrue(doneListViewModel.DoneItems.Any(doneItem => doneItem.Data == item.Data));
        }

        [Test]
        public void RemoveToDoItem()
        {
            // given
            var container = Utils.Initialize(true);
            var taskListViewModel = container.Resolve<TaskListViewModel>();

            ToDoItem item = new ToDoItem() { Data = "test item" };
            taskListViewModel.ToDoItems.Add(item);

            // when
            taskListViewModel.RemoveClickCommand?.Execute(item);

            // then
            Assert.IsFalse(taskListViewModel.ToDoItems.Any());
        }
    }
}