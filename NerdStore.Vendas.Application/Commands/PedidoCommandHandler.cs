using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItempedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediator _mediator;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediator mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }


        public async Task<bool> Handle(AdicionarItempedidoCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) return false;
            
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClientId(command.ClienteId);

            var pedidoItem = new PedidoItem(command.ProdutoId, command.Nome, command.Quantidade, command.ValorUnitario);

            if (pedido == null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(command.ClienteId);

                pedido.IncrementarOuAdicionarItem(pedidoItem);

                _pedidoRepository.Adicionar(pedido);
            }
            else
            {
                var pedidoItemExistente = pedido.BuscarItem(pedidoItem);
                pedido.IncrementarOuAdicionarItem(pedidoItem);

                if(pedidoItemExistente != null)
                    _pedidoRepository.AtualizarItem(pedido.BuscarItem(pedidoItem));
                else
                    _pedidoRepository.AdicionarItem(pedido.BuscarItem(pedidoItem));

                pedido.IncrementarOuAdicionarItem(pedidoItem);

                _pedidoRepository.AdicionarItem(pedidoItem);

                _pedidoRepository.Atualizar(pedido);


            }
            

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, 
                command.ProdutoId, command.Nome, command.Quantidade, command.ValorUnitario));

            return await _pedidoRepository.UnityOfWork.Commit();
        }

        public bool ValidarComando(Command command)
        {
            if (command.EhValido()) return true;

            foreach (var error in command.ValidationResult.Errors)
                 _mediator.Publish(new DomainNotification(command.MessageType, error.ErrorMessage));

            return false;
        }
    }
}
