using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Core.Repositories
{
    public class FileRepositoryCache : IRepository
    {
        private readonly IRepository _inner;
        private readonly Func<string, IFile> _fileFunc;

        private List<ToDoItem> _toDoItems;
        private List<DoneItem> _doneItems;

        public FileRepositoryCache(IRepository inner, Func<string,IFile> fileFunc)
        {
            _inner = inner;
            _fileFunc = fileFunc;
        }

        public async Task<IEnumerable<ToDoItem>> GetToDoItemsAsync()
        {
            return _toDoItems ?? (_toDoItems = (await _inner.GetToDoItemsAsync())?.ToList());
        }

        public async Task<IEnumerable<DoneItem>> GetDoneItemsAsync()
        {
            return _doneItems ?? (_doneItems = (await _inner.GetDoneItemsAsync())?.ToList());
        }

        public async Task AddItemAsync(ToDoItem item)
        {
            if (_toDoItems != null)
                _toDoItems.Add(item);
            else
                await _inner.AddItemAsync(item);
        }

        public async Task<bool> RemoveItemAsync(ToDoItem item)
        {
            if (_toDoItems != null)
                return _toDoItems.Remove(item);
            return await _inner.RemoveItemAsync(item);
        }

        public async Task AddItemAsync(DoneItem item)
        {
            if (_doneItems != null)
                _doneItems.Add(item);
            else
                await _inner.AddItemAsync(item);
        }

        public async Task<bool> RemoveItemAsync(DoneItem item)
        {
            if (_doneItems != null)
                return _doneItems.Remove(item);
            return await _inner.RemoveItemAsync(item);
        }

        public Task SetDoneItemsAsync(IEnumerable<DoneItem> doneItems)
        {
            _doneItems = new List<DoneItem>(doneItems);

            return Task.FromResult(0);
        }

        public Task SetToDoItemsAsync(IEnumerable<ToDoItem> toDoItems)
        {
            _toDoItems = new List<ToDoItem>(toDoItems);

            return Task.FromResult(0);
        }

        public async Task<bool> SaveChangesAsync()
        {
            var fileData = new FileData() {ToDoItems = _toDoItems, DoneItems = _doneItems};
            var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(FileData));

            string serializedData;
            using (var ms = new MemoryStream())
            {
                dataContractJsonSerializer.WriteObject(ms, fileData);
                serializedData = Encoding.UTF8.GetString(ms.ToArray(), 0, (int) ms.Length);
            }

            var file = _fileFunc(AppSettings.ApplicationDataFileName);

            if (!file.Exist) await file.CreateAsync();
            await file.SaveTextAsync(serializedData);
            return true;
        }
    }
}