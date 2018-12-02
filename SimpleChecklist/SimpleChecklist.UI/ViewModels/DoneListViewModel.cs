using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.UI.Converters;
using SimpleChecklist.UI.Extensions;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class DoneListViewModel : Screen
    {
        private readonly ApplicationData _appData;
        private readonly MessagesStream _messagesStream;
        private ObservableCollection<DoneItemsGroup> _doneItemsGroup;

        public DoneListViewModel(ApplicationData appData, MessagesStream messagesStream)
        {
            _appData = appData;
            _messagesStream = messagesStream;
            var stream = _messagesStream.GetStream();
            stream.Subscribe(OnNext);
        }

        public ObservableCollection<DoneItemsGroup> DoneItemsGroup
        {
            get => _doneItemsGroup;
            private set
            {
                if (_doneItemsGroup != value)
                {
                    _doneItemsGroup = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public ICommand RemoveClickCommand
            =>
                new Command(
                    item =>
                    {
                        _messagesStream.PutToStream(new DoneItemActionMessage((DoneItem) item, DoneItemAction.Remove));
                    })
        ;

        public ICommand UndoneClickCommand
            =>
                new Command(
                    item =>
                    {
                        _messagesStream.PutToStream(new DoneItemActionMessage((DoneItem) item, DoneItemAction.Undone));
                    })
        ;

        private void OnNext(IMessage message)
        {
            var eventMessage = message as EventMessage;
            if (eventMessage?.EventType == EventType.DoneListRefreshRequested)
            {
                DoneItemsGroup = _appData.DoneItems.ToDoneItemsGroups();
            }
        }
    }
}