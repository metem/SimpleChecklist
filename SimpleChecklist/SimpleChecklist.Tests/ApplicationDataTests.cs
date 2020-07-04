using Autofac;
using NUnit.Framework;
using SimpleChecklist.Common;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core;
using SimpleChecklist.UI.Commands;
using SimpleChecklist.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class ApplicationDataTests
    {
        [Test]
        public async Task SaveAndLoadBackupsTest()
        {
            // given
            const int NUMBER_OF_BACKUPS = 25;
            var container = Utils.Initialize(true);

            var taskListViewModel = container.Resolve<TaskListViewModel>();
            var doneListViewModel = container.Resolve<DoneListViewModel>();

            var saveApplicationDataCommand = container.Resolve<SaveApplicationDataCommand>();
            var loadApplicationDataCommand = container.Resolve<LoadApplicationDataCommand>();

            var fileFunc = container.Resolve<Func<string, IFile>>();
            var directoryFunc = container.Resolve<Func<string, IDirectoryFilesReader>>();
            var dateTimeProvider = container.Resolve<IDateTimeProvider>() as MockedDateTimeProvider;

            List<ToDoItem> toDoItems = new List<ToDoItem>();
            List<DoneItem> doneItems = new List<DoneItem>();

            var backupsDir = directoryFunc(AppSettings.BackupsDir);

            // when
            for (int i = 1; i <= NUMBER_OF_BACKUPS + 1; i++) // + 1 because last save will create app data
            {
                ToDoItem toDoItem = new ToDoItem() { Data = $"backup_{i}" };
                DoneItem doneItem = new DoneItem() { Data = $"backup_{i}" };

                toDoItems.Add(toDoItem);
                doneItems.Add(doneItem);
                taskListViewModel.ToDoItems.Add(toDoItem);
                doneListViewModel.DoneItems.Add(doneItem);

                await saveApplicationDataCommand.ExecuteAsync();
                dateTimeProvider.AddSeconds(1);

                taskListViewModel.ToDoItems.Clear();
                doneListViewModel.DoneItems.Clear();
            }

            // then
            for (int i = NUMBER_OF_BACKUPS; i > (NUMBER_OF_BACKUPS - int.Parse(AppSettings.BackupsLimit)); i--)
            {
                await fileFunc("appdata.json").DeleteAsync();
                await loadApplicationDataCommand.ExecuteAsync();

                CollectionAssert.AreEquivalent(new[] { toDoItems[i - 1] }, taskListViewModel.ToDoItems);
                CollectionAssert.AreEquivalent(new[] { doneItems[i - 1] }, doneListViewModel.DoneItems);

                var lastBackup = (await backupsDir.GetFilesAsync())
                    .Where(file => file.NameWithExtension.EndsWith(AppSettings.ApplicationDataFileName))
                    .OrderByDescending(file => file.NameWithExtension)
                    .First();

                await lastBackup.DeleteAsync();

                taskListViewModel.ToDoItems.Clear();
                doneListViewModel.DoneItems.Clear();
            }
        }

        [Test]
        public async Task SaveAndLoadDataTest()
        {
            // given
            var container = Utils.Initialize(true);

            var taskListViewModel = container.Resolve<TaskListViewModel>();
            var doneListViewModel = container.Resolve<DoneListViewModel>();

            ToDoItem toDoItem = new ToDoItem() { Data = "test" };
            DoneItem doneItem = new DoneItem() { Data = "test" };

            taskListViewModel.ToDoItems.Add(toDoItem);
            doneListViewModel.DoneItems.Add(doneItem);

            var saveApplicationDataCommand = container.Resolve<SaveApplicationDataCommand>();
            var loadApplicationDataCommand = container.Resolve<LoadApplicationDataCommand>();

            // when
            await saveApplicationDataCommand.ExecuteAsync();
            taskListViewModel.ToDoItems.Clear();
            doneListViewModel.DoneItems.Clear();
            await loadApplicationDataCommand.ExecuteAsync();

            // then
            CollectionAssert.AreEquivalent(new[] { toDoItem }, taskListViewModel.ToDoItems);
            CollectionAssert.AreEquivalent(new[] { doneItem }, doneListViewModel.DoneItems);
        }
    }
}