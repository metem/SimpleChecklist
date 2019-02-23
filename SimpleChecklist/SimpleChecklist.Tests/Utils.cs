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
        public static void WaitFor(Func<bool> condition, int milisecondsTimeout)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            Task.Run(async () =>
            {
                while (!condition())
                {
                    await Task.Delay(1000);
                }

                autoResetEvent.Set();
            });

            if (!autoResetEvent.WaitOne(milisecondsTimeout))
            {
                throw new Exception("Timeout");
            }
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