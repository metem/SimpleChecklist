using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.Commands.ToDoItemsCommands;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class ToDoItemsCommandsTests
    {
        [Test]
        public void AddToDoItem()
        {
            // given
            var toDoItemToAdd = new ToDoItem();
            var applicationData = new ApplicationData(Mock.Of<IRepository>());
            var addToDoItemCommand = new AddToDoItemCommand(toDoItemToAdd, applicationData);

            // when
            addToDoItemCommand.ExecuteAsync().Wait();

            // then
            Assert.IsTrue(applicationData.ToDoItems.Contains(toDoItemToAdd));
        }

        [Test]
        public void RemoveToDoItem()
        {
            // given
            var toDoItemToRemove = new ToDoItem();
            var applicationData = new ApplicationData(Mock.Of<IRepository>());
            applicationData.ToDoItems.Add(toDoItemToRemove);
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(true, new Mock<IFile>().Object, new Mock<IFile>().Object);
            var removeToDoItemCommand = new RemoveToDoItemCommand(toDoItemToRemove, applicationData, dialogUtilsMock.Object);

            // when
            removeToDoItemCommand.ExecuteAsync().Wait();

            // then
            Assert.IsFalse(applicationData.ToDoItems.Contains(toDoItemToRemove));
        }

        [Test]
        public void MoveToDoneListTest()
        {
            // given
            var toDoItemToMove = new ToDoItem();
            var applicationData = new ApplicationData(Mock.Of<IRepository>());
            applicationData.ToDoItems.Add(toDoItemToMove);
            var messagesStream = new MessagesStream();
            var moveToDoneListCommand = new MoveToDoneListCommand(toDoItemToMove, applicationData, messagesStream);
            var stream = messagesStream.GetStream();
            var doneRefreshRequested = false;
            stream.Subscribe(
                message =>
                    doneRefreshRequested =
                        (message as EventMessage)?.EventType == EventType.DoneListRefreshRequested);

            // when
            moveToDoneListCommand.ExecuteAsync().Wait();

            // then
            Assert.IsFalse(applicationData.ToDoItems.Contains(toDoItemToMove));
            Assert.IsTrue(applicationData.DoneItems.Any(item => item.Description == toDoItemToMove.Description));
            Assert.IsTrue(Utils.WaitFor(() => doneRefreshRequested, 1000));
        }
    }
}