using System;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleChecklist.Common.Interfaces;

namespace SimpleChecklist.UI.Converters
{
    public class DoneItemsGroup : ObservableCollection<IDoneItem>
    {
        public DateTime? FinishDateTime => Items?.FirstOrDefault()?.FinishDateTime;

        public string Title => FinishDateTime?.ToString("d");

        public string ShortName => FinishDateTime?.ToString("d");
    }
}