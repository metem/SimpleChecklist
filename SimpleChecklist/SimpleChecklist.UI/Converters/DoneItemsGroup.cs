using System;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.UI.Converters
{
    public class DoneItemsGroup : ObservableCollection<DoneItem>
    {
        public DateTime? FinishDateTime => Items?.FirstOrDefault()?.FinishDateTime.ToLocalTime();

        public string Title => FinishDateTime?.ToString("d");

        public string ShortName => FinishDateTime?.ToString("d");
    }
}