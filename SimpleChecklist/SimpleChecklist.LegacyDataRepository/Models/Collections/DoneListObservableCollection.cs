using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleChecklist.LegacyDataRepository.Models.Collections
{
    public class DoneListObservableCollection
    {
        public DoneListObservableCollection()
        {
            DoneItemsGroups = new ObservableCollection<DoneItemsGroup>();
        }

        public void RemoveDoneItem(DoneItem doneItem)
        {
            var doneItemsGroup = DoneItemsGroups.FirstOrDefault(group => group.Contains(doneItem));
            if (doneItemsGroup == null) return;

            doneItemsGroup.Remove(doneItem);
            if (!doneItemsGroup.Any())
            {
                DoneItemsGroups.Remove(doneItemsGroup);
            }
        }

        public ObservableCollection<DoneItemsGroup> DoneItemsGroups { get; private set; }

        public void Load(ObservableCollection<DoneItemsGroup> data)
        {
            DoneItemsGroups = data;
        }

        public void Add(string text)
        {
            var doneItem = new DoneItem { Description = text };
            Add(doneItem);
        }

        public void Add(ToDoItem item)
        {
            var doneItem = new DoneItem(item);
            Add(doneItem);
        }

        public void Add(DoneItem item)
        {
            var doneItemsGroup =
                DoneItemsGroups.FirstOrDefault(
                    group => group.FinishDateTime != null && group.FinishDateTime.Value.Date == item.FinishDateTime.Date);

            if (doneItemsGroup == null || !doneItemsGroup.Any())
            {
                DoneItemsGroups.Insert(0, new DoneItemsGroup {item});
                return;
            }

            var doneItems = doneItemsGroup.FirstOrDefault(doneItem => doneItem.FinishDateTime > item.FinishDateTime);
            var index = doneItems == null ? 0 : doneItemsGroup.IndexOf(doneItems);
            doneItemsGroup.Insert(index, item);
        }
    }
}