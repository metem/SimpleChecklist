using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SimpleChecklist.Core.Interfaces;
using SimpleChecklist.Core.Interfaces.Utils;
using SimpleChecklist.Core.Utils.Serializers;

namespace SimpleChecklist.Core.Repositories.v1_3
{
    public class XmlFileApplicationRepository : IFileApplicationRepository, INotifyPropertyChanged
    {
        private readonly Func<string, IFile> _fileFunc;

        public ApplicationData ApplicationData { get; private set; }

        public XmlFileApplicationRepository(Func<string, IFile> fileFunc)
        {
            _fileFunc = fileFunc;
            ApplicationData = new ApplicationData();
        }

        public IEnumerable<IToDoItem> ToDoItems => ApplicationData.ToDoItems;

        public IEnumerable<IDoneItem> DoneItems => ApplicationData.DoneItems;

        public int LoadPriority => 1;

        public void AddItem(IToDoItem item)
        {
            ApplicationData.ToDoItems.Add(item);
        }

        public bool RemoveItem(IToDoItem item)
        {
            return ApplicationData.ToDoItems.Remove(item);
        }

        public void AddItem(IDoneItem item)
        {
            ApplicationData.DoneItems.Add(item);
        }

        public bool RemoveItem(IDoneItem item)
        {
            return ApplicationData.DoneItems.Remove(item);
        }

        public async Task<bool> LoadFromFileAsync(string fileName)
        {
            var file = _fileFunc(fileName);

            if (!file.Exist)
                return false;

            byte[] fileData = await file.ReadBytesAsync();

            var applicationData = XmlBinarySerializer.Deserialize<ApplicationData>(fileData);

            return await Load(applicationData);
        }

        public async Task<bool> SaveToFileAsync(string fileName)
        {
            byte[] fileData = XmlBinarySerializer.Serialize(ApplicationData);

            var file = _fileFunc(fileName);

            await file.CreateAsync();

            await file.SaveBytesAsync(fileData);

            return true;
        }

        public Task<bool> Load(ApplicationData applicationData)
        {
            return Task.Run(() =>
            {
                ApplicationData = applicationData;

                OnPropertyChanged(nameof(ToDoItems));
                OnPropertyChanged(nameof(DoneItems));

                return ApplicationData != null;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}