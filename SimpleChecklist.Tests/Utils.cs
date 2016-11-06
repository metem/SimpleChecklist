using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SimpleChecklist.Core.Interfaces.Utils;

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

        public static Mock<IDialogUtils> CreateDialogUtilsMock(bool dialogResult, IFile openFileDialogResult)
        {
            var dialogUtilsMock = new Mock<IDialogUtils>();
            dialogUtilsMock.Setup(
                    utils =>
                        utils.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                            It.IsAny<string>()))
                .Returns(Task.Run(() => dialogResult));
            dialogUtilsMock.Setup(utils => utils.OpenFileDialogAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.Run(() => openFileDialogResult));
            return dialogUtilsMock;
        }

        public static Mock<IFile> CreateFileMock(bool fileExists, string readTextResult)
        {
            var fileMock = new Mock<IFile>();
            fileMock.Setup(file => file.Exist).Returns(fileExists);
            fileMock.Setup(file => file.ReadTextAsync()).Returns(Task.Run(() => readTextResult));
            return fileMock;
        }

        public static string GenerateBackupFile(string[] toDoItemsDescriptions, string[] doneItemsDescriptions)
        {
            return $@"{toDoItemsDescriptions.Aggregate(
                $@"{doneItemsDescriptions.Aggregate(
                    "<ApplicationData xmlns=\"http://schemas.datacontract.org/2004/07/SimpleChecklist.Core.Repositories.v1_3\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                    "<DoneItems xmlns:a=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">",
                    (current, doneItemsDescription) => current +
                                                       "<a:anyType i:type=\"b:DoneItem\" xmlns:b=\"http://schemas.datacontract.org/2004/07/SimpleChecklist.Core.Entities\">\r\n" +
                                                       "<b:CreationDateTime>2016-10-30T14:06:27.9890555+01:00</b:CreationDateTime>\r\n" +
                                                       $"<b:Description>{doneItemsDescription}</b:Description>\r\n" +
                                                       "<b:ItemColor><b:A>255</b:A><b:B>90</b:B><b:G>90</b:G><b:R>255</b:R></b:ItemColor>\r\n" +
                                                       "<b:FinishDateTime>2016-10-30T14:12:18.6929615+01:00</b:FinishDateTime>\r\n" +
                                                       "</a:anyType>")}</DoneItems><ToDoItems xmlns:a=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">",
                (current, toDoItemsDescription) => current +
                                                   "<a:anyType i:type=\"b:ToDoItem\" xmlns:b=\"http://schemas.datacontract.org/2004/07/SimpleChecklist.Core.Entities\">\r\n" +
                                                   "<b:CreationDateTime>2016-10-30T22:49:02.31016+01:00</b:CreationDateTime>\r\n" +
                                                   $"<b:Description>{toDoItemsDescription}</b:Description>\r\n" +
                                                   "<b:ItemColor><b:A>255</b:A><b:B>255</b:B><b:G>255</b:G><b:R>255</b:R></b:ItemColor>\r\n" +
                                                   "</a:anyType>")}</ToDoItems></ApplicationData>";
        }
    }
}