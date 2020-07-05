using Autofac;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.UI.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class SettingsTests
    {
        [Test]
        [TestCase(true, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, 3, 2)]
        [TestCase(true, new string[0], new[] { "done1" }, 0, 1)]
        [TestCase(false, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, 0, 0)]
        public async Task SaveAndLoadBackup(bool loadAccepted, string[] toDoItemsDescriptions,
            string[] doneItemsDescriptions, int toDoItemsCountExpected, int doneItemsCountExpected)
        {
            //given
            var container = Utils.Initialize(loadAccepted);
            var settingsViewModel = container.Resolve<SettingsViewModel>();
            var doneListViewModel = container.Resolve<DoneListViewModel>();
            var taskListViewModel = container.Resolve<TaskListViewModel>();

            taskListViewModel.ToDoItems =
                new ObservableCollection<ToDoItem>(
                    toDoItemsDescriptions.Select(
                        doItemsDescription => new ToDoItem() { Data = doItemsDescription }));
            doneListViewModel.DoneItems = doneItemsDescriptions.Select(doItemsDescription => new DoneItem() { Data = doItemsDescription }).ToList();

            // when
            await settingsViewModel.CreateBackupAsync();
            doneListViewModel.DoneItems = new List<DoneItem>();
            taskListViewModel.ToDoItems = new ObservableCollection<ToDoItem>();
            await settingsViewModel.LoadBackupAsync();

            // then
            Assert.AreEqual(toDoItemsCountExpected, taskListViewModel.ToDoItems.Count);
            Assert.AreEqual(doneItemsCountExpected, doneListViewModel.DoneItems.Count);

            if (loadAccepted)
            {
                CollectionAssert.AreEqual(toDoItemsDescriptions, taskListViewModel.ToDoItems.Select(item => item.Data));
                CollectionAssert.AreEqual(doneItemsDescriptions, doneListViewModel.DoneItems.Select(item => item.Data));
            }
        }

        [Test]
        [TestCase(true, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, new[] { "done2", "done1", "Done:", "todo3", "todo2", "todo1", "To do:" })]
        [TestCase(true, new string[0], new[] { "done1" }, new[] { "done1", "Done:", "To do:" })]
        [TestCase(false, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, new string[0])]
        public async Task SaveAndLoadTextFile(bool loadAccepted, string[] toDoItemsDescriptions, string[] doneItemsDescriptions, string[] toDoItemsDescriptionsExpected)
        {
            //given
            var container = Utils.Initialize(loadAccepted);
            var settingsViewModel = container.Resolve<SettingsViewModel>();
            var doneListViewModel = container.Resolve<DoneListViewModel>();
            var taskListViewModel = container.Resolve<TaskListViewModel>();

            doneListViewModel.DoneItems = doneItemsDescriptions.Select(doItemsDescription => new DoneItem() { Data = doItemsDescription }).ToList();
            taskListViewModel.ToDoItems = new ObservableCollection<ToDoItem>(
                        toDoItemsDescriptions.Select(
                            doItemsDescription => new ToDoItem() { Data = doItemsDescription }));

            // when
            await settingsViewModel.SaveTasksToTextFileAsync();
            doneListViewModel.DoneItems = new List<DoneItem>();
            taskListViewModel.ToDoItems = new ObservableCollection<ToDoItem>();
            await settingsViewModel.AddTasksFromTextFileAsync();

            // then
            CollectionAssert.AreEqual(toDoItemsDescriptionsExpected, taskListViewModel.ToDoItems.Select(item => item.Data));
            Assert.AreEqual(0, doneListViewModel.DoneItems.Count);
        }
    }
}