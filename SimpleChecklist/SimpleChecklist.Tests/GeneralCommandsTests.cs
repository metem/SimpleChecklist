﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Moq;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core;
using SimpleChecklist.Core.Commands.General;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.UI;
using SimpleChecklist.UI.ViewModels;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class GeneralCommandsTests
    {
        [Test]
        [TestCase(true, new[] { "todo1", "todo2", "todo3" }, new[] { "newtodo1", "newtodo2" },
            new[] { "todo1", "todo2", "todo3", "newtodo2", "newtodo1" })]
        [TestCase(true, new string[0], new[] { "newtodo1", "newtodo2" }, new[] { "newtodo2", "newtodo1" })]
        [TestCase(false, new[] { "todo1", "todo2", "todo3" }, new[] { "newtodo1", "newtodo2" },
            new[] { "todo1", "todo2", "todo3" })]
        public void AddTasksFromTestFileTest(bool loadAccepted, string[] toDoItemsDescriptionsInit,
            string[] toDoItemsDescriptionsToAdd, string[] toDoItemsDescriptionsExpected)
        {
            // given
            var fileMock = new FileMock(true,
                toDoItemsDescriptionsToAdd.Aggregate(string.Empty, (current, s) => current + s + "\r\n"));
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(loadAccepted, fileMock, fileMock);
            var applicationData = new ApplicationData(Mock.Of<IRepository>());
            foreach (var toDoItemDescription in toDoItemsDescriptionsInit)
            {
                applicationData.ToDoItems.Add(new ToDoItem { Data = toDoItemDescription });
            }
            var addTasksFromTextFileCommand = new AddTasksFromTextFileCommand(dialogUtilsMock.Object,
                applicationData);

            // when
            addTasksFromTextFileCommand.ExecuteAsync().Wait();

            // then
            var toDoItems = applicationData.ToDoItems;

            for (int i = 0; i < toDoItems.Count; i++)
            {
                Assert.AreEqual(toDoItems[i].Data,
                    toDoItemsDescriptionsExpected[i]);
            }
        }

        [Test]
        [TestCase(true, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, 3, 2)]
        [TestCase(true, new string[0], new[] { "done1" }, 0, 1)]
        [TestCase(false, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, 0, 0)]
        public void SaveAndLoadBackupCommandTest(bool loadAccepted, string[] toDoItemsDescriptions,
            string[] doneItemsDescriptions, int toDoItemsCountExpected, int doneItemsCountExpected)
        {
            //given
            var fileMock = new FileMock();
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(loadAccepted, fileMock, fileMock);
            var applicationData = new ApplicationData(Mock.Of<IRepository>())
            {
                ToDoItems =
                    new ObservableCollection<ToDoItem>(
                        toDoItemsDescriptions.Select(
                            doItemsDescription => new ToDoItem() { Data = doItemsDescription })),
                DoneItems =
                    new ObservableCollection<DoneItem>(
                        doneItemsDescriptions.Select(
                            doItemsDescription => new DoneItem() { Data = doItemsDescription }))
            };

            var createBackupCommand = new CreateBackupCommand(dialogUtilsMock.Object, applicationData);
            createBackupCommand.ExecuteAsync().Wait();
            applicationData = new ApplicationData(Mock.Of<IRepository>());
            var loadBackupCommand =
                new LoadBackupCommand(dialogUtilsMock.Object, new MessagesStream(), applicationData);

            // when
            loadBackupCommand.ExecuteAsync().Wait();

            // then
            Assert.AreEqual(toDoItemsCountExpected, applicationData.ToDoItems.Count);
            Assert.AreEqual(doneItemsCountExpected, applicationData.DoneItems.Count);

            for (int index = 0; index < applicationData.ToDoItems.Count; index++)
            {
                Assert.AreEqual(toDoItemsDescriptions[index],
                    applicationData.ToDoItems[index].Data);
            }

            for (int index = 0; index < applicationData.DoneItems.Count; index++)
            {
                Assert.AreEqual(doneItemsDescriptions[index],
                    applicationData.DoneItems[index].Data);
            }
        }

        [Test]
        [TestCase(true, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, new[] { "done2", "done1", "Done:", "todo3", "todo2", "todo1", "To do:" })]
        [TestCase(true, new string[0], new[] { "done1" }, new[] { "done1", "Done:", "To do:" })]
        [TestCase(false, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, new string[0])]
        public void SaveTasksToTextFileCommandTest(bool loadAccepted, string[] toDoItemsDescriptions, string[] doneItemsDescriptions, string[] toDoItemsDescriptionsExpected)
        {
            //given
            var fileMock = new FileMock();
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(loadAccepted, fileMock, fileMock);

            var builder = new ContainerBuilder();
            builder.RegisterModule<SimpleChecklistUIModule>();
            builder.RegisterInstance(Mock.Of<IAppUtils>()).As<IAppUtils>();
            builder.RegisterInstance(dialogUtilsMock.Object).As<IDialogUtils>();
            builder.RegisterInstance(fileMock).As<IFile>();
            var container = builder.Build();

            container.Resolve<MessagesStream>().PutToStream(new EventMessage(EventType.Started));
            var settingsViewModel = container.Resolve<SettingsViewModel>();
            var applicationData = container.Resolve<ApplicationData>();

            applicationData.DoneItems = new ObservableCollection<DoneItem>(
                        doneItemsDescriptions.Select(
                            doItemsDescription => new DoneItem() { Data = doItemsDescription }));
            applicationData.ToDoItems = new ObservableCollection<ToDoItem>(
                        toDoItemsDescriptions.Select(
                            doItemsDescription => new ToDoItem() { Data = doItemsDescription }));

            // when
            settingsViewModel.SaveTasksToTextFileClickCommand?.Execute(null);
            Utils.WaitFor(() => !string.IsNullOrEmpty(fileMock.TextData), 5000);
            applicationData.DoneItems = new ObservableCollection<DoneItem>();
            applicationData.ToDoItems = new ObservableCollection<ToDoItem>();
            settingsViewModel.AddTasksFromTextFileClickCommand?.Execute(null);
            Utils.WaitFor(() => applicationData.ToDoItems != null, 5000);   

            // then
            CollectionAssert.AreEqual(toDoItemsDescriptionsExpected, applicationData.ToDoItems.Select(item => item.Data));
            Assert.AreEqual(0, applicationData.DoneItems.Count);
        }
    }
}
