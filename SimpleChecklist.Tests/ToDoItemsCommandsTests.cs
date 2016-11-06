using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SimpleChecklist.Core.Commands.ToDoItemsCommands;
using SimpleChecklist.Core.Entities;
using SimpleChecklist.Core.Interfaces.Utils;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.Core.Repositories.v1_3;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class ToDoItemsCommandsTests
    {
        [Test]
        public void AddToDoItem()
        {
            // given
            var fileMock = Utils.CreateFileMock(true, null);
            var toDoItemToAdd = new ToDoItem();
            var applicationRepository = new XmlFileApplicationRepository(s => fileMock.Object);
            var addToDoItemCommand = new AddToDoItemCommand(toDoItemToAdd, applicationRepository);

            // when
            addToDoItemCommand.ExecuteAsync().Wait();

            // then
            Assert.IsTrue(applicationRepository.ToDoItems.Contains(toDoItemToAdd));
        }

        [Test]
        public void RemoveToDoItem()
        {
            // given
            var fileMock = Utils.CreateFileMock(true, null);
            var toDoItemToRemove = new ToDoItem();
            var applicationRepository = new XmlFileApplicationRepository(s => fileMock.Object);
            applicationRepository.AddItem(toDoItemToRemove);
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(true, new Mock<IFile>().Object);
            var removeToDoItemCommand = new RemoveToDoItemCommand(toDoItemToRemove, applicationRepository, dialogUtilsMock.Object);

            // when
            removeToDoItemCommand.ExecuteAsync().Wait();

            // then
            Assert.IsFalse(applicationRepository.ToDoItems.Contains(toDoItemToRemove));
        }

        [Test]
        public void MoveToDoneListTest()
        {
            // given
            var fileMock = Utils.CreateFileMock(true, null);
            var toDoItemToMove = new ToDoItem();
            var applicationRepository = new XmlFileApplicationRepository(s => fileMock.Object);
            applicationRepository.AddItem(toDoItemToMove);
            var messagesStream = new MessagesStream();
            var moveToDoneListCommand = new MoveToDoneListCommand(toDoItemToMove, applicationRepository, messagesStream);
            var stream = messagesStream.GetStream();
            var doneRefreshRequested = false;
            stream.Subscribe(
                message =>
                    doneRefreshRequested =
                        (message as EventMessage)?.EventType == EventType.DoneListRefreshRequested);

            // when
            moveToDoneListCommand.ExecuteAsync().Wait();

            // then
            Assert.IsFalse(applicationRepository.ToDoItems.Contains(toDoItemToMove));
            Assert.IsTrue(applicationRepository.DoneItems.Any(item => item.Description == toDoItemToMove.Description));
            Assert.IsTrue(Utils.WaitFor(() => doneRefreshRequested, 1000));
        }
    }
}