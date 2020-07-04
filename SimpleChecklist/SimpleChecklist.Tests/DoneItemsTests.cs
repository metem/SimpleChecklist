using Autofac;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.UI.ViewModels;
using System.Linq;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class DoneItemsTests
    {
        [Test]
        public void RemoveDoneItem()
        {
            // given
            var container = Utils.Initialize(true);
            var doneListViewModel = container.Resolve<DoneListViewModel>();

            DoneItem item = new DoneItem() { Data = "test item" };
            doneListViewModel.DoneItems.Add(item);

            // when
            doneListViewModel.RemoveClickCommand?.Execute(item);

            // then
            Assert.IsFalse(doneListViewModel.DoneItems.Any());
        }

        [Test]
        public void UndoneDoneItemTest()
        {
            // given
            var container = Utils.Initialize(true);
            var doneListViewModel = container.Resolve<DoneListViewModel>();
            var taskListViewModel = container.Resolve<TaskListViewModel>();

            DoneItem item = new DoneItem() { Data = "test item" };
            doneListViewModel.DoneItems.Add(item);

            // when
            doneListViewModel.UndoneClickCommand?.Execute(item);

            // then
            Assert.IsFalse(doneListViewModel.DoneItems.Any());
            Assert.IsTrue(taskListViewModel.ToDoItems.Any(todoItem => todoItem.Data == item.Data));
        }
    }
}