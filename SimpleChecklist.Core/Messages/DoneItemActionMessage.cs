using SimpleChecklist.Common.Interfaces;

namespace SimpleChecklist.Core.Messages
{
    public class DoneItemActionMessage : IMessage
    {
        public IDoneItem DoneItem { get; }

        public DoneItemAction Action { get; }

        public DoneItemActionMessage(IDoneItem doneItem, DoneItemAction action)
        {
            DoneItem = doneItem;
            Action = action;
        }
    }
}