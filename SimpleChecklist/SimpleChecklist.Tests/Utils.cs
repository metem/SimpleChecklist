using Autofac;
using Moq;
using SimpleChecklist.Common;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.UI;
using SimpleChecklist.UI.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChecklist.Tests
{
    public static class Utils
    {
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

        public static IContainer Initialize(bool loadAccepted)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<SimpleChecklistUIModule>();
            builder.RegisterInstance(Mock.Of<IAppUtils>()).As<IAppUtils>();
            builder.Register(cc => CreateDialogUtilsMock(loadAccepted, cc.Resolve<IFile>(), cc.Resolve<IFile>()).Object).As<IDialogUtils>();
            builder.RegisterType<FileMock>().As<IFile>();
            builder.RegisterType<FilesContainer>().SingleInstance();
            builder.RegisterType<DirectoryMock>().As<IDirectoryFilesReader>();
            builder.RegisterType<MockedDateTimeProvider>().As<IDateTimeProvider>().SingleInstance();
            var container = builder.Build();
            container.Resolve<LoadApplicationDataCommand>().ExecuteAsync().Wait();
            return container;
        }
    }
}