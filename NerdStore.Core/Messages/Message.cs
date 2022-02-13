using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Core.Messages
{
    public class Message
    {
        public string MessageType { get; set; }
        public Guid AggregateId { get; set; }

        public Message()
        {
            MessageType = GetType().Name;
        }
    }

    public abstract class Event: Message, INotification
    {
        protected Event()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; private set; }
    }
}
