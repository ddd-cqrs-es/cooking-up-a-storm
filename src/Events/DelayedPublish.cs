using System;

namespace Restaurant.Events
{
    internal class DelayedPublish : Message
    {
        public readonly Message Message;
        public readonly DateTime At;

        public DelayedPublish(Message message, DateTime at)
        {
            Message = message;
            At = at;
        }
    }
}