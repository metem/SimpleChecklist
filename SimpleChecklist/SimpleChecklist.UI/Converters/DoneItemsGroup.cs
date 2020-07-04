using SimpleChecklist.Common.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleChecklist.UI.Converters
{
    public class DoneItemsGroup : ObservableCollection<DoneItem>
    {
        public DateTime? FinishDateTime => Items?.FirstOrDefault()?.FinishDateTime.ToLocalTime();

        public string ShortName => FinishDateTime?.ToString("d");
        public string Title => FinishDateTime?.ToString("d");
    }
}