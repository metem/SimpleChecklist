using System;
using System.Reactive.Subjects;

namespace SimpleChecklist.Core.Messages
{
    public class MessagesStream
    {
        private readonly Subject<IMessage> _messagesStream;

        public MessagesStream()
        {
            _messagesStream = new Subject<IMessage>();
        }

        public IObservable<IMessage> GetStream()
        {
            return _messagesStream;
        }

        public void PutToStream(IMessage message)
        {
            _messagesStream.OnNext(message);
        }
    }
}