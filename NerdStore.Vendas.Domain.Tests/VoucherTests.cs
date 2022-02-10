using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar Voucher Tipo Valor Válido")]
        [Trait("Categoria", "Vendas - Voucher")]

        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            // Arrange
            var voucher = new Voucher(Guid.NewGuid(), "PROMO-15-REAIS", 15,
                null, 1, DateTime.Now.AddDays(15), true, false, TipoDeDescontoVoucher.valor);

            // Act
            var result = voucher.ValidarAplicabilidade();

            // Assert 
            Assert.True(result.IsValid);

        }


        [Fact(DisplayName = "Validar Voucher Tipo Valor Inválido")]
        [Trait("Categoria", "Vendas - Voucher")]

        public void Voucher_ValidarVoucherTipoValor_DeveEstarInvalido()
        {
            // Arrange
            var voucher = new Voucher(Guid.NewGuid(), "", null,
                null, 0 , DateTime.Now.AddDays(-1), false, true, TipoDeDescontoVoucher.valor);

            // Act
            var result = voucher.ValidarAplicabilidade();

            // Assert 
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);

            Assert.Contains(Voucher.VoucherAplicavelValidador.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(Voucher.VoucherAplicavelValidador.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(Voucher.VoucherAplicavelValidador.DataValidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(Voucher.VoucherAplicavelValidador.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(Voucher.VoucherAplicavelValidador.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(Voucher.VoucherAplicavelValidador.ValorDescontoErroMsg, result.Errors.Select(c => c.ErrorMessage));

        }
    }
}
