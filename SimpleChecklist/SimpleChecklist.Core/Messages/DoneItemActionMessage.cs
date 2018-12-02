using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Messages
{
    public class DoneItemActionMessage : IMessage
    {
        public DoneItem DoneItem { get; }

        public DoneItemAction Action { get; }

        public DoneItemActionMessage(DoneItem doneItem, DoneItemAction action)
        {
            DoneItem = doneItem;
            Action = action;
        }
    }
}