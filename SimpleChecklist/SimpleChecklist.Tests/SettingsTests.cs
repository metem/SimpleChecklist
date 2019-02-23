using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Moq;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.UI;
using SimpleChecklist.UI.ViewModels;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class SettingsTests
    {
        [Test]
        [TestCase(true, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, 3, 2)]
        [TestCase(true, new string[0], new[] { "done1" }, 0, 1)]
        [TestCase(false, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, 0, 0)]
        public void SaveAndLoadBackup(bool loadAccepted, string[] toDoItemsDescriptions,
            string[] doneItemsDescriptions, int toDoItemsCountExpected, int doneItemsCountExpected)
        {
            //given
            var container = Initialize(loadAccepted);
            var settingsViewModel = container.Resolve<SettingsViewModel>();
            var applicationData = container.Resolve<ApplicationData>();

            applicationData.ToDoItems =
                new ObservableCollection<ToDoItem>(
                    toDoItemsDescriptions.Select(
                        doItemsDescription => new ToDoItem() { Data = doItemsDescription }));
            applicationData.DoneItems =
                new ObservableCollection<DoneItem>(
                    doneItemsDescriptions.Select(
                        doItemsDescription => new DoneItem() { Data = doItemsDescription }));

            // when
            settingsViewModel.CreateBackupClickCommand?.Execute(null);
            Utils.WaitFor(() => !string.IsNullOrEmpty(container.Resolve<IFile>().ReadTextAsync().Result), 5000);
            applicationData.DoneItems = new ObservableCollection<DoneItem>();
            applicationData.ToDoItems = new ObservableCollection<ToDoItem>();
            settingsViewModel.LoadBackupClickCommand?.Execute(null);
            Utils.WaitFor(() => applicationData.ToDoItems != null, 5000);

            // then
            Assert.AreEqual(toDoItemsCountExpected, applicationData.ToDoItems.Count);
            Assert.AreEqual(doneItemsCountExpected, applicationData.DoneItems.Count);

            if (loadAccepted)
            {
                CollectionAssert.AreEqual(toDoItemsDescriptions, applicationData.ToDoItems.Select(item => item.Data));
                CollectionAssert.AreEqual(doneItemsDescriptions, applicationData.DoneItems.Select(item => item.Data));
            }
        }

        [Test]
        [TestCase(true, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, new[] { "done2", "done1", "Done:", "todo3", "todo2", "todo1", "To do:" })]
        [TestCase(true, new string[0], new[] { "done1" }, new[] { "done1", "Done:", "To do:" })]
        [TestCase(false, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, new string[0])]
        public void SaveAndLoadTextFile(bool loadAccepted, string[] toDoItemsDescriptions, string[] doneItemsDescriptions, string[] toDoItemsDescriptionsExpected)
        {
            //given
            var container = Initialize(loadAccepted);
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
            Utils.WaitFor(() => !string.IsNullOrEmpty(container.Resolve<IFile>().ReadTextAsync().Result), 5000);
            applicationData.DoneItems = new ObservableCollection<DoneItem>();
            applicationData.ToDoItems = new ObservableCollection<ToDoItem>();
            settingsViewModel.AddTasksFromTextFileClickCommand?.Execute(null);
            Utils.WaitFor(() => applicationData.ToDoItems != null, 5000);

            // then
            CollectionAssert.AreEqual(toDoItemsDescriptionsExpected, applicationData.ToDoItems.Select(item => item.Data));
            Assert.AreEqual(0, applicationData.DoneItems.Count);
        }

        private static IContainer Initialize(bool loadAccepted)
        {
            var fileMock = new FileMock();
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(loadAccepted, fileMock, fileMock);

            var builder = new ContainerBuilder();
            builder.RegisterModule<SimpleChecklistUIModule>();
            builder.RegisterInstance(Mock.Of<IAppUtils>()).As<IAppUtils>();
            builder.RegisterInstance(dialogUtilsMock.Object).As<IDialogUtils>();
            builder.RegisterInstance(fileMock).As<IFile>();
            var container = builder.Build();
            container.Resolve<MessagesStream>().PutToStream(new EventMessage(EventType.Started));
            return container;
        }
    }
}
