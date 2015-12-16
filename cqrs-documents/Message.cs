using System;

namespace cqrs_documents
{
    public class Message
    {
        private Message(Guid messageId)
        {
            MessageId = messageId;
        }

        public Message() : this(Guid.NewGuid())
        {
        }

        public Guid MessageId { get; private set; }
        public Guid CorrelationId { get; set; }
        public Guid CausationId { get; set; }
    }
}