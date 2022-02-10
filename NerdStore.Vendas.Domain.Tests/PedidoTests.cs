using NerdStore.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static NerdStore.Vendas.Domain.Pedido;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Item Pedido Vazio")]
        [Trait("Categoria", "Pedido - Vendas")]

        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor() 
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

            // Act
            pedido.IncrementarOuAdicionarItem(pedidoItem);

            // Assert
            Assert.Equal(200, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Pedido - Vendas")]

        public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
        {
            // Arrange
            var pedido =  Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(productId, "Produto Teste", 2, 100);

            var pedidoItem2 = new PedidoItem(productId, "Produto Teste", 1, 100);
           
            // Act
            pedido.IncrementarOuAdicionarItem(pedidoItem);

            pedido.IncrementarOuAdicionarItem(pedidoItem2);

            // Assert
            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.pedidoItems.Count);
            Assert.Equal(3, pedido.pedidoItems.FirstOrDefault(p=> p.ProdutoId == productId)?.Quantidade);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente Acima do permitido")]
        [Trait("Categoria", "Pedido - Vendas")]

        public void AdicionarItemPedido_ItemExistenteSomarUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var productId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(productId, "Produto Teste", 1, 100);
            var pedidoItem2 = new PedidoItem(productId, "Produto Teste", Pedido.Quantidade_Max_Produto, 100);

            //Act
            pedido.IncrementarOuAdicionarItem(pedidoItem);

            //Assert
            Assert.Throws<DomainException>(() => pedido.IncrementarOuAdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Atualizar ItemPedido inexistente no pedido")]
        [Trait("Categoria", "Pedido - Vendas")]

        public void AtualizarItemPedido_AtualizarPedidoItemInexistenteNoPedido_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var productId = Guid.NewGuid();
            var pedidoItemASerAtualizado = new PedidoItem(productId, "Produto Atualizado", 1, 100);


            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemASerAtualizado));
        }

        [Fact(DisplayName = "Atualizar ItemPedido")]
        [Trait("Categoria", "Pedido - Vendas")]

        public void AtualizarItemPedido_AtualizarPedidoItemExistenteNoPedido_DeveAtualizarValor()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var productId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(productId, "Produto", 1, 100);
            var pedidoItemASerAtualizado = new PedidoItem(productId, "Produto Atualizado", 5, 100);

            //Act 
            pedido.IncrementarOuAdicionarItem(pedidoItem);
            pedido.AtualizarItem(pedidoItemASerAtualizado);

            //Assert
            Assert.Equal(500, pedido.ValorTotal);
        }

        //[Fact(DisplayName = "Atualizar ItemPedido Acima do Permitido")]
        //[Trait("Categoria", "Pedido - Vendas")]

        //public void AtualizarItemPedido_AtualizarPedidoItemAcimaDoPermitido_DeveRetornarUmaException()
        //{
        //    // Arrange
        //    var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

        //    var productId = Guid.NewGuid();
        //    var pedidoItem = new PedidoItem(productId, "Produto", 1, 100);
        //    var pedidoItemASerAtualizado = new PedidoItem(productId, "Produto Atualizado", Pedido.Quantidade_Max_Produto + 1, 100);


        //    //Act & Assert
        //    pedido.IncrementarOuAdicionarItem(pedidoItem);           
        //    Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemASerAtualizado));
        //}


        [Fact(DisplayName = "Remover ItemPedido inexistente no pedido")]
        [Trait("Categoria", "Pedido - Vendas")]

        public void RemoverItemPedido_ItemNaoExistenteNoPedido_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var productId = Guid.NewGuid();
            var pedidoItemASerRemovido = new PedidoItem(productId, "Produto Removido", 1, 100);


            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemASerRemovido));
        }

        [Fact(DisplayName = "Remover Item Pedido existente no pedido")]
        [Trait("Categoria", "Pedido - Vendas")]

        public void RemoverItemPedido_ItemExistenteNoPedido_DeveAtualizarValor()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 1, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 100);

            //Act
            pedido.IncrementarOuAdicionarItem(pedidoItem);
            pedido.IncrementarOuAdicionarItem(pedidoItem2);
            pedido.RemoverItem(pedidoItem2);

            //Assert
            Assert.Equal(100, pedido.ValorTotal);
        }
    }
}
