using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace SimpleChecklist.LegacyDataRepository.Models.Collections
{
    [CollectionDataContract]
    public class DoneItemsGroup : ObservableCollection<DoneItem>
    {
        public DateTime? FinishDateTime => Items?.FirstOrDefault()?.FinishDateTime;

        public string Title => FinishDateTime?.ToString("d");

        public string ShortName => FinishDateTime?.ToString("d");
    }
}