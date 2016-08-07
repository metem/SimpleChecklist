using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Models;
using SimpleChecklist.Models.Utils;
using Xamarin.Forms;

namespace SimpleChecklist.ViewModels
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
                    ChangeListedDirectory(data.Value == "..."
                        ? _currentDirectory.GetParent()
                        : _currentDirectory.GetChild(data.Value));
                    break;
                case FileType.File:
                    FileChoosen?.Invoke(Path.Combine(_currentDirectory.Path, data.Value));
                    break;
            }
        });

        public ObservableCollection<KeyValuePair<FileType, string>> FilesList { get; }

        public void ChangeListedDirectory(IDirectory directory)
        {
            if (!directory.Exist)
                return;

            FilesList.Clear();

            _currentDirectory = directory;

            var directories = directory.GetDirectories();

            FilesList.Add(new KeyValuePair<FileType, string>(FileType.Directory, "..."));

            foreach (var dir in directories)
            {
                FilesList.Add(new KeyValuePair<FileType, string>(FileType.Directory, dir.Name));
            }

            var files = directory.GetFiles();

            foreach (var file in files)
            {
                FilesList.Add(new KeyValuePair<FileType, string>(FileType.File, file.Name));
            }
        }
    }
}