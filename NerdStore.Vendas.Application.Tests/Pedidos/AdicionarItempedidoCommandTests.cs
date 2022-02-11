using NerdStore.Vendas.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AdicionarItempedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar Item command Válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AdicionarItempedidoCommand_CommandEstaValido_DevePassarNaValidacao()
        {
            //Arrange
            var pedidoCommand = new AdicionarItempedidoCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Produto teste", 2, 100);

            //Act
            var result = pedidoCommand.EhValido();

            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Adicionar Item command inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AdicionarItempedidoCommand_CommandEstarInvalido_NaoDevePassarNaValidacao()
        {
            //Arrange
            var pedidoCommand = new AdicionarItempedidoCommand(Guid.Empty,
                Guid.Empty, string.Empty, 0, 0);

            //Act
            var result = pedidoCommand.EhValido();

            //Assert
            Assert.False(result);
            Assert.Contains(AdicionarItemPedidoValidation.IdClienteErrroMsg, pedidoCommand.ValidationResult.Errors.Select(a => a.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidation.ValorErroMsg, pedidoCommand.ValidationResult.Errors.Select(a => a.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidation.NomeErroMsg, pedidoCommand.ValidationResult.Errors.Select(a => a.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidation.QtdMinErroMsg, pedidoCommand.ValidationResult.Errors.Select(a => a.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidation.IdProdutoErroMsg, pedidoCommand.ValidationResult.Errors.Select(a => a.ErrorMessage));
        }
    }
}
