using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Core;
using SimpleChecklist.Core.Interfaces.Utils;
using SimpleChecklist.UI.Converters;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class OpenFilePickerViewModel : Screen
    {
        private IDirectory _currentDirectory;

        public OpenFilePickerViewModel()
        {
            FilesList = new ObservableCollection<KeyValuePair<FileType, string>>();
        }

        public Action<string> FileChoosen { get; set; }

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
                case FileType.File:
                    FileChoosen?.Invoke(Path.Combine(_currentDirectory.Path, data.Value));
                    break;
            }
        });

        public ObservableCollection<KeyValuePair<FileType, string>> FilesList { get; }

        public IEnumerable<string> AllowedFileTypes { get; set; }

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

                var files =
                    directory.GetFiles()
                        .Where(file => AllowedFileTypes.Any(allowedFileType => file.Name.EndsWith(allowedFileType)));

                _currentDirectory = directory;

                filesList.AddRange(files.Select(file => new KeyValuePair<FileType, string>(FileType.File, file.Name)));

                FilesList.Clear();

                foreach (var keyValuePair in filesList)
                    FilesList.Add(keyValuePair);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}