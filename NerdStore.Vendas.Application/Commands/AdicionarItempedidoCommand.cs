using FluentValidation;
using FluentValidation.Results;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class AdicionarItempedidoCommand : Command
    {
        public AdicionarItempedidoCommand(Guid clienteId, Guid produtoId,
            string nome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }


        public override bool EhValido()
        {
            ValidationResult = new AdicionarItemPedidoValidation().Validate(this);

            return ValidationResult.IsValid;
        }

    }

    public class AdicionarItemPedidoValidation : AbstractValidator<AdicionarItempedidoCommand>
    {
        public static string IdClienteErrroMsg => "Id do cliente inválido";
        public static string IdProdutoErroMsg => "Id do produto inválido";
        public static string NomeErroMsg => "O nome do produto não foi informado";
        public static string QtdMaxErroMsg => $"A quantidade máxima de um item é {Pedido.Quantidade_Max_Produto}";
        public static string QtdMinErroMsg => $"a quantidade mínima de um item é {Pedido.Quantidade_Min_Produto}";
        public static string ValorErroMsg => "O valor do desconto deve dser maior que zero";

        public AdicionarItemPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdClienteErrroMsg);

            RuleFor(c => c.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdProdutoErroMsg);

            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage(NomeErroMsg);

            RuleFor(c => c.ValorUnitario)
                .GreaterThan(0)
                .WithMessage(ValorErroMsg);

            RuleFor(c => c.Quantidade)
                .GreaterThan(Pedido.Quantidade_Min_Produto)
                .WithMessage(QtdMinErroMsg)
                .LessThan(Pedido.Quantidade_Max_Produto)
                .WithMessage(QtdMaxErroMsg);

        }
    }
}

