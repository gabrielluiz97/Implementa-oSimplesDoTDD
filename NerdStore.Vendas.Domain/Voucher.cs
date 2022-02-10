using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain
{
    public  class Voucher
    {
        public Voucher(Guid id, string codigo, decimal? valorDesconto, decimal? percentualDeDesconto, int quantidade, 
            DateTime dataValidade, bool ativo, bool utilizado, TipoDeDescontoVoucher tipoDeDesconto)
        {
            Id = id;
            Codigo = codigo;
            ValorDesconto = valorDesconto;
            PercentualDeDesconto = percentualDeDesconto;
            Quantidade = quantidade;
            DataValidade = dataValidade;
            Ativo = ativo;
            Utilizado = utilizado;
            TipoDeDesconto = tipoDeDesconto;
        }

        public Guid Id { get; private set; }
        public TipoDeDescontoVoucher TipoDeDesconto { get; private set; }
        public string Codigo { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public decimal? PercentualDeDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }

        public ValidationResult ValidarAplicabilidade() 
        {
            return new VoucherAplicavelValidador().Validate(this);
        }

        protected static bool DataVencimentoSuperiorAtual(DateTime dataValidade)
        {
            return dataValidade >= DateTime.Now;
        }

        public class VoucherAplicavelValidador : AbstractValidator<Voucher>
        {
            public static string CodigoErroMsg => "Código do Voucher é inválido";
            public static string DataValidadeErroMsg => "Voucher está expirado";
            public static string AtivoErroMsg => "Voucher não está ativo";
            public static string UtilizadoErroMsg => "Voucher já foi utilizado";
            public static string QuantidadeErroMsg => "Esse Voucher não está mais disponível";
            public static string ValorDescontoErroMsg => "O valor do desconto deve dser maior que zero";
            public static string PercentualDescontoErroMsg => "O Percentual de desconto deve dser maior que zero";

            public VoucherAplicavelValidador()
            {
                RuleFor(c => c.Codigo)
                    .NotEmpty()
                    .WithMessage(CodigoErroMsg);

                RuleFor(d => d.DataValidade)
                    .Must(DataVencimentoSuperiorAtual)
                    .WithMessage(DataValidadeErroMsg);            
                
                
                RuleFor(a => a.Ativo)
                    .Equal(true)
                    .WithMessage(AtivoErroMsg);             
                
                RuleFor(u => u.Utilizado)
                    .Equal(false)
                    .WithMessage(UtilizadoErroMsg);              
                
                RuleFor(u => u.Quantidade)
                    .GreaterThan(0)
                    .WithMessage(QuantidadeErroMsg);


                When(f => f.TipoDeDesconto == TipoDeDescontoVoucher.valor, () =>
                 {
                     RuleFor(f => f.ValorDesconto)
                        .NotNull()
                        .WithMessage(ValorDescontoErroMsg)
                        .GreaterThan(0)
                        .WithMessage(ValorDescontoErroMsg); 
                 });

                When(f => f.TipoDeDesconto == TipoDeDescontoVoucher.porcentagem, () =>
                {
                    RuleFor(f => f.PercentualDeDesconto)
                       .NotNull()
                       .WithMessage(PercentualDescontoErroMsg)
                       .GreaterThan(0)
                       .WithMessage(PercentualDescontoErroMsg);
                });
            }
        }
    }   

    public enum TipoDeDescontoVoucher
    {
        porcentagem = 0,
        valor = 1
    }
}
