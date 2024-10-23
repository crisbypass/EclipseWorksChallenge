using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace Application.CustomValidators
{
    /// <summary>
    /// Fornece a opção de invalidar um valor fornecido de data, caso seja menor que a data atual.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DataMinHojeAttribute : ValidationAttribute
    {
        private readonly string _isInvalidMessage;
        public DataMinHojeAttribute(string? IsInvalidMessage = null)
        {
            ErrorMessage ??= "O valor de {0} não pode ser menor que a data atual.";
            IsInvalidMessage ??= "O valor de {0} corresponde à uma data inválida.";
            _isInvalidMessage = IsInvalidMessage;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (DateTime.TryParse(value?.ToString(), out var date))
            {
                if (date.Date >= DateTime.Now.Date)
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
