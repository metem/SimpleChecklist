using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core;
using SimpleChecklist.UI.Converters;
using SimpleChecklist.UI.Extensions;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class DoneListViewModel : BaseViewModel
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly Workspace _workspace;

        private List<DoneItem> _doneItems = new List<DoneItem>();

        public DoneListViewModel(IDialogUtils dialogUtils, Workspace workspace)
        {
            _dialogUtils = dialogUtils;
            _workspace = workspace;
            _workspace.AddDoneItem += AddDoneItem;
        }

        public List<DoneItem> DoneItems
        {
            get => _doneItems;
            set
            {
                if (_doneItems != value)
                {
                    _doneItems = value;
                    OnPropertyChanged(nameof(DoneItemsGroup));
                }
            }
        }

        public List<DoneItemsGroup> DoneItemsGroup => DoneItems.ToDoneItemsGroups();

        public ICommand RemoveClickCommand => new Command(async item =>
        {
            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.RemoveTaskConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                RemoveDoneItem((DoneItem)item);
            }
        });

        public ICommand UndoneClickCommand => new Command(async item =>
        {
            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.UndoneTaskConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                var doneItem = (DoneItem)item;
                RemoveDoneItem(doneItem);
                _workspace.AddToDoItem(doneItem);
            }
        });

        private void AddDoneItem(DoneItem item)
        {
            _doneItems.Add(item);
            OnPropertyChanged(nameof(DoneItemsGroup));
        }

        private void RemoveDoneItem(DoneItem item)
        {
            _doneItems.Remove(item);
            OnPropertyChanged(nameof(DoneItemsGroup));
        }
    }
}