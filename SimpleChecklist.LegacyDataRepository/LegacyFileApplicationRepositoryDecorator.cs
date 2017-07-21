using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.LegacyDataRepository.Models.Collections;
using SimpleChecklist.LegacyDataRepository.Models.Utils;
using DoneItem = SimpleChecklist.Common.Entities.DoneItem;
using ToDoItem = SimpleChecklist.Common.Entities.ToDoItem;

namespace SimpleChecklist.LegacyDataRepository
{
    public class LegacyFileApplicationRepositoryDecorator : IFileApplicationRepository
    {
        private readonly IFileApplicationRepository _inner;
        private readonly Func<string, IFile> _fileUtils;

        public LegacyFileApplicationRepositoryDecorator(IFileApplicationRepository inner, Func<string, IFile> fileUtils)
        {
            this._inner = inner;
            _fileUtils = fileUtils;
        }

        public IEnumerable<IToDoItem> ToDoItems => _inner.ToDoItems;

        public IEnumerable<IDoneItem> DoneItems => _inner.DoneItems;

        public int LoadPriority => _inner.LoadPriority;
        public void AddItem(IToDoItem item)
        {
            _inner.AddItem(item);
        }

        public bool RemoveItem(IToDoItem item)
        {
            return _inner.RemoveItem(item);
        }

        public void AddItem(IDoneItem item)
        {
            _inner.AddItem(item);
        }

        public bool RemoveItem(IDoneItem item)
        {
            return _inner.RemoveItem(item);
        }

        public async Task<ObservableCollection<ToDoItem>> LoadToDoItems()
        {
            try
            {
                var fileUtils = _fileUtils(AppSettingsLegacy.TaskListFileName);
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

        public async Task<bool> LoadFromFileAsync(string fileName)
        {
            var result = await _inner.LoadFromFileAsync(fileName);

            var todoItems =
                new ObservableCollection<IToDoItem>(
                   (await LoadToDoItems()).Select(Mapper.Map<ToDoItem>));

            var doneItems = (from doneGroup in await LoadDoneItems()
                from doneItem in doneGroup
                select Mapper.Map<DoneItem>(doneItem)).Cast<IDoneItem>();

            foreach (var item in todoItems)
            {
                ApplicationData.ToDoItems.Add(item);
            }

            foreach (var item in doneItems)
            {
                ApplicationData.DoneItems.Add(item);
            }

            return result;
        }

        public Task<bool> SaveToFileAsync(string fileName)
        {
            return _inner.SaveToFileAsync(fileName);
        }

        public Task<bool> Load(ApplicationData applicationData)
        {
            return _inner.Load(applicationData);
        }

        public ApplicationData ApplicationData => _inner.ApplicationData;
    }
}