using DevIO.Business.Models.Validations.Documents;
using FluentValidation;

namespace DevIO.Business.Models.Validations
{
    public class FornecedorValidation : AbstractValidator<Fornecedor>
    {

        public FornecedorValidation()
        {
            RuleFor(f => f.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido!")
                .Length(2, 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres!");

            When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, () =>
            {
                RuleFor(f => f.Documento.Length).Equal(ValidationCPF.TamanhoCPF)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyName}.");
                RuleFor(f => ValidationCPF.Validar(f.Documento)).Equal(true)
                    .WithMessage("O documento fornecido é inválido!");
            });

            When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, () =>
            {
                RuleFor(f => f.Documento.Length).Equal(ValidationCNPJ.TamanhoCNPJ)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyName}.");
                RuleFor(f => ValidationCNPJ.Validar(f.Documento)).Equal(true)
                    .WithMessage("O documento fornecido é inválido!");
            });
        }
    }
}
