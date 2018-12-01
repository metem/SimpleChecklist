using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Core.Repositories
{
    public class FileRepository : IRepository
    {
        private readonly IRepository _inner;
        private readonly Func<string, IFile> _fileFunc;
        private FileData _fileData;

        public FileRepository(IRepository inner, Func<string, IFile> fileFunc)
        {
            _inner = inner;
            _fileFunc = fileFunc;
        }

        private async Task<FileData> LoadFileAsync(string applicationDataFileName)
        {
            var file = _fileFunc(applicationDataFileName);

            try
            {
                var serializedData = await file.ReadTextAsync();

                var deserializedUser = JsonConvert.DeserializeObject<FileData>(serializedData);
                return deserializedUser;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ToDoItem>> GetToDoItemsAsync()
        {
            var applicationDataFileName = AppSettings.ApplicationDataFileName;

            if (_fileData == null)
                _fileData = await LoadFileAsync(applicationDataFileName);

            return _fileData?.ToDoItems;
        }

        public async Task<IEnumerable<DoneItem>> GetDoneItemsAsync()
        {
            var applicationDataFileName = AppSettings.ApplicationDataFileName;

            if (_fileData == null)
                _fileData = await LoadFileAsync(applicationDataFileName);

            return _fileData?.DoneItems;
        }

        public Task AddItemAsync(ToDoItem item)
        {
            return _inner.AddItemAsync(item);
        }

        public Task<bool> RemoveItemAsync(ToDoItem item)
        {
            return _inner.RemoveItemAsync(item);
        }

        public Task AddItemAsync(DoneItem item)
        {
            return _inner.AddItemAsync(item);
        }

        public Task<bool> RemoveItemAsync(DoneItem item)
        {
            return _inner.RemoveItemAsync(item);
        }

        public Task<bool> SaveChangesAsync()
        {
            return _inner.SaveChangesAsync();
        }

        public Task SetDoneItemsAsync(IEnumerable<DoneItem> doneItems)
        {
            return _inner.SetDoneItemsAsync(doneItems);
        }

        public Task SetToDoItemsAsync(IEnumerable<ToDoItem> toDoItems)
        {
            return _inner.SetToDoItemsAsync(toDoItems);
        }
    }
}