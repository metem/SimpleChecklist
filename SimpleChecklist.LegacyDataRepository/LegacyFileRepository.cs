using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.LegacyDataRepository.Models.Collections;
using SimpleChecklist.LegacyDataRepository.Models.Utils;

namespace SimpleChecklist.LegacyDataRepository
{
    public class LegacyFileRepository : IRepository
    {
        private readonly IRepository _inner;
        private readonly Func<string, IFile> _fileUtils;

        public LegacyFileRepository(IRepository inner, Func<string, IFile> fileUtils)
        {
            _inner = inner;
            _fileUtils = fileUtils;
        }

        private async Task<ObservableCollection<ToDoItem>> LoadToDoItems()
        {
            try
            {
                var fileUtils = _fileUtils(AppSettingsLegacy.TaskListFileName);

                if (!fileUtils.Exist)
                {
                    return new ObservableCollection<ToDoItem>();
                }

                var data = await fileUtils.ReadBytesAsync();

                if (data != null)
                {
                    var result = XmlBinarySerializer.Deserialize<ObservableCollection<ToDoItem>>(data);

                    if (result.Any())
                    {
                        await fileUtils.SaveBytesAsync(new byte[0]);
                    }

                    return result;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return new ObservableCollection<ToDoItem>();
        }

        private async Task<ObservableCollection<DoneItemsGroup>> LoadDoneItems()
        {
            try
            {
                var fileUtils = _fileUtils(AppSettingsLegacy.DoneListFileName);

                if (!fileUtils.Exist)
                {
                    return new ObservableCollection<DoneItemsGroup>();
                }

                var data = await fileUtils.ReadBytesAsync();

                if (data != null)
                {
                    var result = XmlBinarySerializer.Deserialize<ObservableCollection<DoneItemsGroup>>(data);

                    if (result.Any())
                    {
                        await fileUtils.SaveBytesAsync(new byte[0]);
                    }

                    return result;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return new ObservableCollection<DoneItemsGroup>();
        }

        public async Task<IEnumerable<Common.Entities.ToDoItem>> GetToDoItemsAsync()
        {
            var toDoItems = (await _inner.GetToDoItemsAsync())?.ToList() ?? new List<Common.Entities.ToDoItem>();

            var legacyToDoItems = (await LoadToDoItems()).Select(Mapper.Map<Common.Entities.ToDoItem>);

            toDoItems.AddRange(legacyToDoItems);

            return toDoItems;
        }

        public async Task<IEnumerable<Common.Entities.DoneItem>> GetDoneItemsAsync()
        {
            var doneItems = (await _inner.GetDoneItemsAsync())?.ToList() ?? new List<Common.Entities.DoneItem>();

            var legacyDoneItems = from doneGroup in await LoadDoneItems()
                from doneItem in doneGroup
                select Mapper.Map<Common.Entities.DoneItem>(doneItem);

            doneItems.AddRange(legacyDoneItems);

            return doneItems;
        }

        public Task AddItemAsync(Common.Entities.ToDoItem item)
        {
            return _inner.AddItemAsync(item);
        }

        public Task<bool> RemoveItemAsync(Common.Entities.ToDoItem item)
        {
            return _inner.RemoveItemAsync(item);
        }

        public Task SetDoneItemsAsync(IEnumerable<Common.Entities.DoneItem> doneItems)
        {
            return _inner.SetDoneItemsAsync(doneItems);
        }

        public Task SetToDoItemsAsync(IEnumerable<Common.Entities.ToDoItem> toDoItems)
        {
            return _inner.SetToDoItemsAsync(toDoItems);
        }

        public Task AddItemAsync(Common.Entities.DoneItem item)
        {
            return _inner.AddItemAsync(item);
        }

        public Task<bool> RemoveItemAsync(Common.Entities.DoneItem item)
        {
            return _inner.RemoveItemAsync(item);
        }

        public Task<bool> SaveChangesAsync()
        {
            return _inner.SaveChangesAsync();
        }
    }
}