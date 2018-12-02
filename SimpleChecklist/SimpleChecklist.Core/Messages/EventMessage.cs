namespace SimpleChecklist.Core.Messages
{
    public class EventMessage : IMessage
    {
        public EventMessage(EventType eventType)
        {
            EventType = eventType;
        }
        public EventType EventType { get; }
    }
}