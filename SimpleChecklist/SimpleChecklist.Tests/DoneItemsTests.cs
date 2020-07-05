using Autofac;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.UI.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class DoneItemsTests
    {
        [Test]
        public async Task RemoveDoneItem()
        {
            // given
            var container = Utils.Initialize(true);
            var doneListViewModel = container.Resolve<DoneListViewModel>();

            DoneItem item = new DoneItem() { Data = "test item" };
            doneListViewModel.DoneItems.Add(item);

            // when
            await doneListViewModel.RemoveDoneItemAsync(item);

            // then
            Assert.IsFalse(doneListViewModel.DoneItems.Any());
        }

        [Test]
        public async Task UndoneDoneItemTest()
        {
            // given
            var container = Utils.Initialize(true);
            var doneListViewModel = container.Resolve<DoneListViewModel>();
            var taskListViewModel = container.Resolve<TaskListViewModel>();

            DoneItem item = new DoneItem() { Data = "test item" };
            doneListViewModel.DoneItems.Add(item);

            // when
            await doneListViewModel.UndoneItemAsync(item);

            // then
            Assert.IsFalse(doneListViewModel.DoneItems.Any());
            Assert.IsTrue(taskListViewModel.ToDoItems.Any(todoItem => todoItem.Data == item.Data));
        }
    }
}