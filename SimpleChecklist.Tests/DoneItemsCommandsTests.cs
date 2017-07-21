using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.Commands.DoneItemsCommands;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.Core.Repositories.v1_3;

namespace SimpleChecklist.Tests
{
    [TestFixture]
    public class DoneItemsCommandsTests
    {
        [Test]
        public void RemoveDoneItem()
        {
            // given
            var fileMock = Utils.CreateFileMock(true, null);
            var doneItemToRemove = new DoneItem();
            var applicationRepository = new XmlFileApplicationRepository(s => fileMock.Object);
            applicationRepository.AddItem(doneItemToRemove);
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(true, new Mock<IFile>().Object);
            var messagesStream = new MessagesStream();
            var removeToDoItemCommand = new RemoveDoneItemCommand(doneItemToRemove, applicationRepository, dialogUtilsMock.Object, messagesStream);
            var stream = messagesStream.GetStream();
            var doneRefreshRequested = false;
            stream.Subscribe(
                message =>
                    doneRefreshRequested =
                        (message as EventMessage)?.EventType == EventType.DoneListRefreshRequested);

            // when
            removeToDoItemCommand.ExecuteAsync().Wait();

            // then
            Assert.IsFalse(applicationRepository.DoneItems.Contains(doneItemToRemove));
            Assert.IsTrue(Utils.WaitFor(() => doneRefreshRequested, 1000));
        }

        [Test]
        public void UndoneDoneItemTest()
        {
            // given
            var fileMock = Utils.CreateFileMock(true, null);
            var doneItemToUndone = new DoneItem();
            var applicationRepository = new XmlFileApplicationRepository(s => fileMock.Object);
            var dialogUtilsMock = Utils.CreateDialogUtilsMock(true, new Mock<IFile>().Object);
            applicationRepository.AddItem(doneItemToUndone);
            var messagesStream = new MessagesStream();
            var undoneDoneItemCommand = new UndoneDoneItemCommand(doneItemToUndone, applicationRepository,
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
            Assert.IsFalse(applicationRepository.DoneItems.Contains(doneItemToUndone));
            Assert.IsTrue(applicationRepository.ToDoItems.Any(item => item.Description == doneItemToUndone.Description));
            Assert.IsTrue(Utils.WaitFor(() => doneRefreshRequested, 1000));
        }
    }
}