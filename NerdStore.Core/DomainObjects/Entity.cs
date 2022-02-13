using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Core.DomainObjects
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        private List<Event> _notifications;

        public void AdicionarEvento(Event evento)
        {
            _notifications = _notifications ?? new List<Event>();

            _notifications.Add(evento);
        }

        public void RemoverEvento(Event evento) 
        { 
            _notifications.Remove(evento);
        }

        public void LimparEventos()
        {
            _notifications.Clear();
        }

    }
}
