using MediatR;
using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoItemAdicionadoEvent : Event
    {
        public PedidoItemAdicionadoEvent(Guid clienteId, Guid pedidoId, Guid produto, string nome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            Produto = produto;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public Guid ClienteId { get; set; }
        public Guid PedidoId { get; set; }
        public Guid Produto { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
