using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.LegacyDataRepository.Models.Utils;
using SimpleChecklist.Models.Collections;

namespace SimpleChecklist.LegacyDataRepository
{
    public class LegacyFileRepository : IRepository
    {
        private readonly IRepository _inner;
        private readonly Func<string, IFile> _fileUtils;
        private bool useLegacyRepository = false;

        public LegacyFileRepository(IRepository inner, Func<string, IFile> fileUtils)
        {
            _inner = inner;
            _fileUtils = fileUtils;

            useLegacyRepository = !_fileUtils(AppSettingsLegacy.ApplicationDataFileName).Exist;
        }

        private async Task<ObservableCollection<SimpleChecklist.Models.Collections.ToDoItem>> LoadToDoItems()
        {
            try
            {
                var fileUtils = _fileUtils(AppSettingsLegacy.TaskListFileName);

                if (!fileUtils.Exist)
                {
                    return new ObservableCollection<SimpleChecklist.Models.Collections.ToDoItem>();
                }

                var data = await fileUtils.ReadBytesAsync();

                if (data != null)
                {
                    var result = XmlBinarySerializer.Deserialize<ObservableCollection<SimpleChecklist.Models.Collections.ToDoItem>>(data);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // ignored
            }

            return new ObservableCollection<SimpleChecklist.Models.Collections.ToDoItem>();
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

            if (useLegacyRepository)
            {
                var legacyToDoItems = (await LoadToDoItems()).Select(Mapper.Map<Common.Entities.ToDoItem>);

                toDoItems.AddRange(legacyToDoItems);
            }

            return toDoItems;
        }

        public async Task<IEnumerable<Common.Entities.DoneItem>> GetDoneItemsAsync()
        {
            var doneItems = (await _inner.GetDoneItemsAsync())?.ToList() ?? new List<Common.Entities.DoneItem>();

            if (useLegacyRepository)
            {
                var legacyDoneItems = from doneGroup in await LoadDoneItems()
                    from doneItem in doneGroup
                    select Mapper.Map<Common.Entities.DoneItem>(doneItem);

                doneItems.AddRange(legacyDoneItems);
            }

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

        public Task SetSettingsAsync(Settings settings)
        {
            return _inner.SetSettingsAsync(settings);
        }

        public Task<Settings> GetSettingsAsync()
        {
            return _inner.GetSettingsAsync();
        }
    }
}