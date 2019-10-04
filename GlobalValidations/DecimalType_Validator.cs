using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlobalValidations
{
    public class DecimalType_Validator : ValidationAttribute
    {
        public DecimalType_Validator(string errorMessage = "") : base("{0} mora da bude decimalna vrednost.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            if (!string.IsNullOrEmpty(value.ToString()) && !Decimal.TryParse(value.ToString(), out decimal tmp))
            {
                return new ValidationResult(errorMessage, new List<string>() { validationContext.DisplayName });
            }
            return ValidationResult.Success;
        }
    }
}
