using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalValidations
{
    public class requiredField_Validator : ValidationAttribute
    {
        public requiredField_Validator() : base("{0} ne sme da bude prazno polje.")
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            if (value == null || value.ToString().Length == 0)
            {
                return new ValidationResult(errorMessage, new List<string>() { validationContext.DisplayName });
            }
            return ValidationResult.Success;
        }//[ValidationResult]
    }//[Class]
}
