using System.Linq;
using Moq;
using NUnit.Framework;
using SimpleChecklist.Core.Commands.General;
using SimpleChecklist.Core.Entities;
using SimpleChecklist.Core.Interfaces.Utils;
using SimpleChecklist.Core.Repositories.v1_3;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class GeneralCommandsTests
    {
        [Test]
        [TestCase(true, new[] {"todo1", "todo2", "todo3"}, new[] {"newtodo1", "newtodo2"}, new[] {"todo1", "todo2", "todo3", "newtodo2", "newtodo1"})]
        [TestCase(true, new string[0], new[] {"newtodo1", "newtodo2"}, new[] {"newtodo2", "newtodo1"})]
        [TestCase(false, new[] {"todo1", "todo2", "todo3"}, new[] {"newtodo1", "newtodo2"}, new[] {"todo1", "todo2", "todo3"})]
        public void AddTasksFromTestFileTest(bool loadAccepted, string[] toDoItemsDescriptionsInit,
            string[] toDoItemsDescriptionsToAdd, string[] toDoItemsDescriptionsExpected)
        {
            // given
            var fileMock = Utils.CreateFileMock(true,
                toDoItemsDescriptionsToAdd.Aggregate(string.Empty, (current, s) => current + s + "\r\n"));
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(loadAccepted, fileMock.Object);
            var applicationRepository = new XmlFileApplicationRepository(s => new Mock<IFile>().Object);
            foreach (var toDoItemDescription in toDoItemsDescriptionsInit)
            {
                applicationRepository.AddItem(new ToDoItem {Description = toDoItemDescription});
            }
            var addTasksFromTextFileCommand = new AddTasksFromTextFileCommand(dialogUtilsMock.Object,
                applicationRepository);

            // when
            addTasksFromTextFileCommand.ExecuteAsync().Wait();

            // then
            for (int i = 0; i < applicationRepository.ToDoItems.Count(); i++)
            {
                Assert.AreEqual(applicationRepository.ToDoItems.ElementAt(i).Description,
                    toDoItemsDescriptionsExpected[i]);
            }
        }

        [Test]
        [TestCase(true, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, 3, 2)]
        [TestCase(true, new string[0], new[] { "done1" }, 0, 1)]
        [TestCase(false, new[] { "todo1", "todo2", "todo3" }, new[] { "done1", "done2" }, 0, 0)]
        public void LoadBackupCommandTest(bool loadAccepted, string[] toDoItemsDescriptions,
            string[] doneItemsDescriptions, int toDoItemsCountExpected, int doneItemsCountExpected)
        {
            // given
            string backupFileText = Utils.GenerateBackupFile(toDoItemsDescriptions, doneItemsDescriptions);
            var fileMock = Utils.CreateFileMock(true, backupFileText);
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(loadAccepted, fileMock.Object);
            var applicationRepository = new XmlFileApplicationRepository(s => new Mock<IFile>().Object);
            var loadBackupCommand = new LoadBackupCommand(dialogUtilsMock.Object, applicationRepository);

            // when
            loadBackupCommand.ExecuteAsync().Wait();

            // then
            Assert.AreEqual(toDoItemsCountExpected, applicationRepository.ToDoItems.Count());
            Assert.AreEqual(doneItemsCountExpected, applicationRepository.DoneItems.Count());

            for (int index = 0; index < applicationRepository.ToDoItems.Count(); index++)
            {
                Assert.AreEqual(toDoItemsDescriptions[index],
                    applicationRepository.ToDoItems.ElementAt(index).Description);
            }

            for (int index = 0; index < applicationRepository.DoneItems.Count(); index++)
            {
                Assert.AreEqual(doneItemsDescriptions[index],
                    applicationRepository.DoneItems.ElementAt(index).Description);
            }
        }
    }
}
