using NerdStore.Core.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public static int Quantidade_Max_Produto => 15;
        public static int Quantidade_Min_Produto => 1;

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public Guid ClientId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }
        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> pedidoItems => _pedidoItems;

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        private void CalcularValorTotal()
        {
            ValorTotal = _pedidoItems.Sum(a => a.Quantidade * a.ValorUnitario);
        }


        #region [ Item ]
        public PedidoItem BuscarItem(PedidoItem pedidoItem)
        {
            return _pedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
        }

        public void IncrementarOuAdicionarItem(PedidoItem pedidoItem)
        {

            var itemExistente = BuscarItem(pedidoItem);

            if (itemExistente != null)
                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);
            else
                _pedidoItems.Add(pedidoItem);


            CalcularValorTotal();
        }

        public void AtualizarItem(PedidoItem pedidoItemAtualizado)
        {
            ValidarItemInexistente(pedidoItemAtualizado);

            var pedidoItem = BuscarItem(pedidoItemAtualizado);

            pedidoItem.Atualizar(pedidoItemAtualizado.ProdutoNome, pedidoItemAtualizado.Quantidade, pedidoItemAtualizado.ValorUnitario);

            CalcularValorTotal();
        }      

        public void RemoverItem(PedidoItem pedidoItemRemovido)
        {
            ValidarItemInexistente(pedidoItemRemovido);

            _pedidoItems.Remove(pedidoItemRemovido);

            CalcularValorTotal();
        }

        public void ValidarItemInexistente(PedidoItem pedidoItem)
        {
            var item = BuscarItem(pedidoItem);

            if(item == null)
                throw new DomainException($"produto não faz parte do pedido.");
        }
        #endregion
        
        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clientId)
            {
                var pedido = new Pedido
                {
                    ClientId = clientId,
                };

                pedido.TornarRascunho();

                return pedido;
            }
        }
    }

    public enum PedidoStatus
    {
        Rascunho = 0,
        Iniciado = 1,
        Pago = 4,
        Entegue = 5,
        Cancelado = 6
    }

    public class PedidoItem
    {
        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal volorUnitario)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            ValorUnitario = volorUnitario;
            AdicionarUnidades(quantidade);
        }

        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public void Atualizar(string produtoNome, int quantidade, decimal volorUnitario)
        {
            ProdutoNome = produtoNome;
            ValorUnitario = volorUnitario;
            Quantidade = 0;
            AdicionarUnidades(quantidade);
        }

        public void AdicionarUnidades(int quantidade)
        {
            Quantidade += quantidade;

            ValidarQuantidade();
        }

        public void ValidarQuantidade()
        {
            if (Quantidade > Pedido.Quantidade_Max_Produto)
                throw new DomainException($"Quantidade máxima de {Pedido.Quantidade_Max_Produto} por produto foi estrapolada.");
            if (Quantidade < Pedido.Quantidade_Min_Produto)
                throw new DomainException($"Quantidade mínima de {Pedido.Quantidade_Min_Produto} por produto não foi atendida.");
        }
    }
}
