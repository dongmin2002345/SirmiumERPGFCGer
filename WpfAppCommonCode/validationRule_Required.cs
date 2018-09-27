using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfAppCommonCode
{
    public class validationRule_Required : ValidationRule
    {
        public validationRule_Required()
        {
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {

                ////bool isValid = item.IsValid(compiledExpresion(inObject));
                //System.ComponentModel.DataAnnotations.ValidationContext context = new System.ComponentModel.DataAnnotations.ValidationContext(value, serviceProvider: null, items: null);
                //requiredField_Validator v = new requiredField_Validator();
                //System.ComponentModel.DataAnnotations.ValidationResult vr = v.GetValidationResult(value, context);
                //if (vr != null && vr != System.ComponentModel.DataAnnotations.ValidationResult.Success)
                //{
                //    if (vr != null)
                //    {
                //        return new ValidationResult(false, vr.ErrorMessage);
                //    }
                //}
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }


            return new ValidationResult(true, null);

        }
    }
}
