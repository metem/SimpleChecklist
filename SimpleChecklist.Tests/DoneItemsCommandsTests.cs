using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.Commands.DoneItemsCommands;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class DoneItemsCommandsTests
    {
        [Test]
        public void RemoveDoneItem()
        {
            // given
            var doneItemToRemove = new DoneItem();
            var applicationData = new ApplicationData(Mock.Of<IRepository>());
            applicationData.DoneItems.Add(doneItemToRemove);
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(true, new Mock<IFile>().Object);
            var messagesStream = new MessagesStream();
            var removeToDoItemCommand = new RemoveDoneItemCommand(doneItemToRemove, applicationData, dialogUtilsMock.Object, messagesStream);
            var stream = messagesStream.GetStream();
            var doneRefreshRequested = false;
            stream.Subscribe(
                message =>
                    doneRefreshRequested =
                        (message as EventMessage)?.EventType == EventType.DoneListRefreshRequested);

            // when
            removeToDoItemCommand.ExecuteAsync().Wait();

            // then
            Assert.IsFalse(applicationData.DoneItems.Contains(doneItemToRemove));
            Assert.IsTrue(Utils.WaitFor(() => doneRefreshRequested, 1000));
        }

        [Test]
        public void UndoneDoneItemTest()
        {
            // given
            var doneItemToUndone = new DoneItem();
            var applicationData = new ApplicationData(Mock.Of<IRepository>());
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(true, new Mock<IFile>().Object);
            applicationData.DoneItems.Add(doneItemToUndone);
            var messagesStream = new MessagesStream();
            var undoneDoneItemCommand = new UndoneDoneItemCommand(doneItemToUndone, applicationData,
                dialogUtilsMock.Object, messagesStream);
            var stream = messagesStream.GetStream();
            var doneRefreshRequested = false;
            stream.Subscribe(
                message =>
                    doneRefreshRequested =
                        (message as EventMessage)?.EventType == EventType.DoneListRefreshRequested);

            // when
            undoneDoneItemCommand.ExecuteAsync().Wait();

            // then
            Assert.IsFalse(applicationData.DoneItems.Contains(doneItemToUndone));
            Assert.IsTrue(applicationData.ToDoItems.Any(item => item.Description == doneItemToUndone.Description));
            Assert.IsTrue(Utils.WaitFor(() => doneRefreshRequested, 1000));
        }
    }
}