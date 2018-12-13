using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core;
using SimpleChecklist.UI.Converters;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class SaveFilePickerViewModel : Screen
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly Func<string, IFile> _file;
        private IDirectory _currentDirectory;
        private string _fileName;

        public SaveFilePickerViewModel(IDialogUtils dialogUtils, Func<string, IFile> file)
        {
            _dialogUtils = dialogUtils;
            _file = file;
            FilesList = new ObservableCollection<KeyValuePair<FileType, string>>();
        }

        public Action<string> FileChoosen { get; set; }

        public string Extension { get; set; }

        public string FileName
        {
            get => _fileName;
            set
            {
                if (value == _fileName) return;
                _fileName = value;
                NotifyOfPropertyChange(() => FileName);
            }
        }

        public ICommand FileChoosenCommand => new Command(item =>
        {
            var data = (KeyValuePair<FileType, string>) item;
            switch (data.Key)
            {
                case FileType.Directory:
                    ChangeListedDirectory(data.Value == AppSettings.ParentDirectory
                        ? _currentDirectory.GetParent()
                        : _currentDirectory.GetChild(data.Value));
                    break;
            }
        });

        public ObservableCollection<KeyValuePair<FileType, string>> FilesList { get; }

        public async void SaveClick()
        {
            var file = Path.Combine(_currentDirectory.Path, FileName);

            file = $"{file}{Extension}";

            var fileExist = _file(file).Exist;

            var alertResult = false;

            if (fileExist)
                alertResult =
                    await
                        _dialogUtils.DisplayAlertAsync(AppTexts.Alert,
                            string.Format(AppTexts.FileAlreadyExist, FileName), AppTexts.Yes,
                            AppTexts.No);

            if (!fileExist || alertResult)
                FileChoosen?.Invoke(file);
        }

        public void ChangeListedDirectory(IDirectory directory)
        {
            if (!directory.Exist)
                return;

            try
            {
                var directories = directory.GetDirectories();

                var filesList = new List<KeyValuePair<FileType, string>>
                {
                    new KeyValuePair<FileType, string>(FileType.Directory, AppSettings.ParentDirectory)
                };

                filesList.AddRange(
                    directories.Select(dir => new KeyValuePair<FileType, string>(FileType.Directory, dir.Name)));

                _currentDirectory = directory;

                FilesList.Clear();

                foreach (var keyValuePair in filesList)
                    FilesList.Add(keyValuePair);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}