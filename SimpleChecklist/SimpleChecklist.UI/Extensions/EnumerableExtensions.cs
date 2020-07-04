using SimpleChecklist.Common.Entities;
using SimpleChecklist.UI.Converters;
using System.Collections.Generic;
using System.Linq;

namespace SimpleChecklist.UI.Extensions
{
    public static class EnumerableExtensions
    {
        public static List<DoneItemsGroup> ToDoneItemsGroups(this IEnumerable<DoneItem> data)
        {
            var result = new List<DoneItemsGroup>();

            foreach (var doneItems in data.GroupBy(item => item.FinishDateTime.ToLocalTime().Date))
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