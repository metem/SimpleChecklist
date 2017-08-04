using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Tests
{
    public static class Utils
    {
        public static bool WaitFor(Func<bool> condition, int milisecondsTimeout)
        {
            var task = Task.Run(() =>
            {
                while (!condition())
                {
                    Thread.Sleep(500);
                }
            });
            return task.Wait(milisecondsTimeout);
        }

        public static Mock<IDialogUtils> CreateDialogUtilsMock(bool dialogResult, IFile openFileDialogResult,
            IFile saveFileDialogResult)
        {
            var dialogUtilsMock = new Mock<IDialogUtils>();
            dialogUtilsMock.Setup(
                    utils =>
                        utils.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                            It.IsAny<string>()))
                .Returns(Task.Run(() => dialogResult));
            dialogUtilsMock.Setup(utils => utils.OpenFileDialogAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.Run(() => openFileDialogResult));
            dialogUtilsMock.Setup(
                    utils => utils.SaveFileDialogAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.Run(() => saveFileDialogResult));
            return dialogUtilsMock;
        }
    }
}