using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public  class PedidoCommandHandlerTests
    {
        [Fact(DisplayName = "Adicionar Item novo pedido com sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarNovoItem_NovoPedido_DeveExecutarComSucesso()
        {
            //Arrange
            var pedidoCommand = new AdicionarItempedidoCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Produto teste", 2, 100);

            var mocker = new AutoMocker();
            var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();

            mocker.GetMock<IPedidoRepository>()
            .Setup(r => r.UnityOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            //mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None, Times.Once));
        }

        [Fact(DisplayName = "Adicionar novo Item Pedido rascunho com sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarNovoItem_NovoItemPedidoRascunho_DeveExecutarComSucesso()
        {
            //Arrange

            var clienteId = Guid.NewGuid();

            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(clienteId);

            var peditoItemexistente = new PedidoItem(Guid.NewGuid(), "produto xpto", 2, 100);
            pedido.IncrementarOuAdicionarItem(peditoItemexistente);

            var pedidoCommand = new AdicionarItempedidoCommand(clienteId, Guid.NewGuid(), "Produto teste", 2, 100);

            var mocker = new AutoMocker();
            var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();


            mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClientId(clienteId)).Returns(Task.FromResult(pedido));

            mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnityOfWork.Commit()).Returns(Task.FromResult(true));


            //Act
            var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()));
            mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()));
            mocker.GetMock<IPedidoRepository>().Verify(r => r.UnityOfWork.Commit());
        }
    
        [Fact(DisplayName = "Adicionar Item existente ao pedido Rascunho com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_ItemExistentePedidoRascunho_DeveExecutarComSucesso()
        {
           //Arrange
           var clienteId = Guid.NewGuid();
           var produtoId = Guid.NewGuid();

            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(clienteId);
            var pedidoItemExistente = new PedidoItem(produtoId, "Produto xpto", 2, 100);
            pedido.IncrementarOuAdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItempedidoCommand(clienteId, produtoId, "Produto xpto", 2, 100);

            var mocker = new AutoMocker();
            var pedidoCommandHandler = mocker.CreateInstance<PedidoCommandHandler>();

            mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClientId(clienteId))
                .Returns(Task.FromResult(pedido));

            mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnityOfWork.Commit())
                .Returns(Task.FromResult(true));

            //Act
            var result = await pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);


            //Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.UnityOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            //Arrange
            var pedidoCommand = new AdicionarItempedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            var mocker = new AutoMocker();
            var pedidoCommandHandler = mocker.CreateInstance<PedidoCommandHandler>();

            //Act
            var result = await pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);


            //Assert
            Assert.False(result);

            mocker.GetMock<IMediator>()
                .Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None),Times.Exactly(5));
        }
    }
}
