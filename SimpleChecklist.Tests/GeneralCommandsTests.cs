using System.Linq;
using Moq;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Core.Commands.General;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class GeneralCommandsTests
    {
        [Test]
        [TestCase(true, new[] {"todo1", "todo2", "todo3"}, new[] {"newtodo1", "newtodo2"},
            new[] {"todo1", "todo2", "todo3", "newtodo2", "newtodo1"})]
        [TestCase(true, new string[0], new[] {"newtodo1", "newtodo2"}, new[] {"newtodo2", "newtodo1"})]
        [TestCase(false, new[] {"todo1", "todo2", "todo3"}, new[] {"newtodo1", "newtodo2"},
            new[] {"todo1", "todo2", "todo3"})]
        public void AddTasksFromTestFileTest(bool loadAccepted, string[] toDoItemsDescriptionsInit,
            string[] toDoItemsDescriptionsToAdd, string[] toDoItemsDescriptionsExpected)
        {
            // given
            var fileMock = Utils.CreateFileMock(true,
                toDoItemsDescriptionsToAdd.Aggregate(string.Empty, (current, s) => current + s + "\r\n"));
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(loadAccepted, fileMock.Object);
            var applicationData = new ApplicationData(Mock.Of<IRepository>());
            foreach (var toDoItemDescription in toDoItemsDescriptionsInit)
            {
                applicationData.ToDoItems.Add(new ToDoItem {Description = toDoItemDescription});
            }
            var addTasksFromTextFileCommand = new AddTasksFromTextFileCommand(dialogUtilsMock.Object,
                applicationData);

            // when
            addTasksFromTextFileCommand.ExecuteAsync().Wait();

            // then
            var toDoItems = applicationData.ToDoItems;

            for (int i = 0; i < toDoItems.Count(); i++)
            {
                Assert.AreEqual(toDoItems[i].Description,
                    toDoItemsDescriptionsExpected[i]);
            }
        }

        [Test]
        [TestCase(true, new[] {"todo1", "todo2", "todo3"}, new[] {"done1", "done2"}, 3, 2)]
        [TestCase(true, new string[0], new[] {"done1"}, 0, 1)]
        [TestCase(false, new[] {"todo1", "todo2", "todo3"}, new[] {"done1", "done2"}, 0, 0)]
        public void LoadBackupCommandTest(bool loadAccepted, string[] toDoItemsDescriptions,
            string[] doneItemsDescriptions, int toDoItemsCountExpected, int doneItemsCountExpected)
        {
            // given
            string backupFileText = Utils.GenerateBackupFile(toDoItemsDescriptions, doneItemsDescriptions);
            var fileMock = Utils.CreateFileMock(true, backupFileText);
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(loadAccepted, fileMock.Object);
            var applicationData = new ApplicationData(Mock.Of<IRepository>());
            var loadBackupCommand = new LoadBackupCommand(dialogUtilsMock.Object, applicationData);

            // when
            loadBackupCommand.ExecuteAsync().Wait();

            // then
            Assert.AreEqual(toDoItemsCountExpected, applicationData.ToDoItems.Count());
            Assert.AreEqual(doneItemsCountExpected, applicationData.DoneItems.Count());

            for (int index = 0; index < applicationData.ToDoItems.Count; index++)
            {
                Assert.AreEqual(toDoItemsDescriptions[index],
                    applicationData.ToDoItems[index].Description);
            }

            for (int index = 0; index < applicationData.DoneItems.Count(); index++)
            {
                Assert.AreEqual(doneItemsDescriptions[index],
                    applicationData.DoneItems[index].Description);
            }
        }
    }
}
