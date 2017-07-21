using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.UI.Converters;

namespace SimpleChecklist.UI.Extensions
{
    public static class EnumerableExtensions
    {
        public static ObservableCollection<DoneItemsGroup> ToDoneItemsGroups(this IEnumerable<IDoneItem> data)
        {
            var result = new ObservableCollection<DoneItemsGroup>();

            foreach (var doneItems in data.GroupBy(item => item.FinishDateTime.Date))
            {
                var doneItemsGroup = new DoneItemsGroup();
                foreach (var doneItem in doneItems)
                {
                    doneItemsGroup.Add(doneItem);
                }
                result.Add(doneItemsGroup);
            }

            return result;
        }
    }
}