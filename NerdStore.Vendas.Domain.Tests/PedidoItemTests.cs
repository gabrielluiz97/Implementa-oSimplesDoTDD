using NerdStore.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Adicionar Item Pedido abaixo do permitido")]
        [Trait("Categoria", "Pedido item Tests")]

        public void AdicionarItemPedido_ItemAbaixoDoPermitido_DeveRetornarException()
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.Quantidade_Min_Produto  - 1, 100));
        }

        [Fact(DisplayName = "Adicionar Item Pedido Acima do permitido")]
        [Trait("Categoria", "Pedido item Tests")]

        public void AdicionarItemPedido_ItemAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.Quantidade_Max_Produto + 1, 100));
        }

        [Fact(DisplayName = "Atualizar ItemPedido Acima do Permitido")]
        [Trait("Categoria", "Pedido item Tests")]

        public void AtualizarItemPedido_AtualizarPedidoItemAcimaDoPermitido_DeveRetornarUmaException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(productId, "Produto", 1, 100);
            var pedidoItemASerAtualizado = new PedidoItem(productId, "Produto Atualizado", 2, 100);


            //Act & Assert
            Assert.Throws<DomainException>(() => pedidoItem.Atualizar(pedidoItemASerAtualizado.ProdutoNome, Pedido.Quantidade_Max_Produto + 1, pedidoItemASerAtualizado.ValorUnitario));
        }

        [Fact(DisplayName = "Atualizar ItemPedido Abaixo do Permitido")]
        [Trait("Categoria", "Pedido item Tests")]

        public void AtualizarItemPedido_AtualizarPedidoItemAbaixoDoPermitido_DeveRetornarUmaException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(productId, "Produto", 1, 100);
            var pedidoItemASerAtualizado = new PedidoItem(productId, "Produto Atualizado", 2, 100);


            //Act & Assert
            Assert.Throws<DomainException>(() => pedidoItem.Atualizar(pedidoItemASerAtualizado.ProdutoNome, Pedido.Quantidade_Min_Produto - 1, pedidoItemASerAtualizado.ValorUnitario));
        }
    }
}
