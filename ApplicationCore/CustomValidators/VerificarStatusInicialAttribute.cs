using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Application.CustomValidators
{
    /// <summary>
    /// Forneça a opção de invalidar a inserção do status, em função do valor informado. 
    /// </summary>
    /// <remarks>
    /// Valor inicial padrão a invalidar: Concluido.
    /// </remarks>
    public class VerificarStatusInicialAttribute : ValidationAttribute
    {
        private readonly string _isInvalidMessage;
        public VerificarStatusInicialAttribute(StatusEnum bloquearStatusEnum = StatusEnum.Concluido, string? IsInvalidMessage = null)
        {
            ErrorMessage ??= $"O valor inicial de {{0}} não deve ser {Enum.GetName(bloquearStatusEnum)}.";
            IsInvalidMessage ??= "O valor de {0} é inválido.";
            _isInvalidMessage = IsInvalidMessage;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (Enum.TryParse(value?.ToString(), out StatusEnum startEnum))
            {
                if (startEnum != StatusEnum.Concluido)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture,
                        ErrorMessageString, validationContext.DisplayName), [validationContext.MemberName!]);
            }

            return new ValidationResult(string.Format(CultureInfo.CurrentCulture,
                        _isInvalidMessage, validationContext.DisplayName), [validationContext.MemberName!]);
        }
    }
}
