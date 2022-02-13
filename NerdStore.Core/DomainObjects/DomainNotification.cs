using MediatR;
using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Core.DomainObjects
{
    public class DomainNotification : Message, INotification
    {
        public DomainNotification(string key, string value)
        {
            Key = key;
            Value = value;
            Version = 1;
            Timestamp = DateTime.Now;
            DomainNotificationId = Guid.NewGuid();
        }

        public DateTime Timestamp { get; private set; }
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }
    }
}
