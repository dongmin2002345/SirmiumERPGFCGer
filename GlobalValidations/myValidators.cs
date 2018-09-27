using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GlobalValidations
{
    public class myValidators
    {
        #region Test function for single property validation
        //public static void TestFunction(dtoRacun inObj)
        //{
        //    //DTO_arpu_tmp objForPropValidate = new DTO_arpu_tmp();
        //    //var context = new ValidationContext(objForPropValidate, serviceProvider: null, items: null);
        //    ////var propInfo = typeof(DTO_arpu_tmp).GetProperty("Name");
        //    //var propInfo = GetProperty<DTO_arpu_tmp>(o => o.Name);
        //    //List<ValidationAttribute> attr = propInfo.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().ToList();
        //    //attr[0].IsValid(objForPropValidate.Name);
        //    //var valResult = attr[0].GetValidationResult(objForPropValidate.Name, context);
        //    //string errMess = valResult.ErrorMessage;


        //    dtoRacun objForPropValidate = inObj; //new DTO_arpu_tmp();
        //    //objForPropValidate.Name = "fdfd";
        //    //objForPropValidate.Prezime = "";
        //    List<string> errors = new List<string>();
        //    bool isOk = myValidators.ValidateProperty<dtoRacun>(objForPropValidate, o => o.ime, out errors);
        //    if (isOk)
        //    {// sve ok
        //        MessageBox.Show("Name je OK.");
        //    }
        //    else
        //    {//Validacija nije prosla
        //        string allErrors = "";
        //        if (errors != null && errors.Count > 0)
        //        {
        //            foreach (var item in errors)
        //            {
        //                allErrors += item + Environment.NewLine;
        //            }
        //        }
        //        MessageBox.Show("Name nije ok. Errors:" + Environment.NewLine + "" + allErrors);
        //    }


        //    List<string> errors2 = new List<string>();
        //    bool isOk2 = myValidators.ValidateObject<dtoRacun>(objForPropValidate, out errors2);
        //    if (isOk2)
        //    {// sve ok
        //        MessageBox.Show("Objekat je OK.");
        //    }
        //    else
        //    {//Validacija nije prosla
        //        string allErrors = "";
        //        if (errors2 != null && errors2.Count > 0)
        //        {
        //            foreach (var item in errors2)
        //            {
        //                allErrors += item + Environment.NewLine;
        //            }
        //        }
        //        MessageBox.Show("Objekat nije ok. Errors:" + Environment.NewLine + "" + allErrors);
        //    }


        //}

        #endregion

        public static bool ValidateObject<T>(T inObject, out List<string> outErrors)
        {
            bool toRet = false;
            outErrors = new List<string>();
            try
            {
                ValidationContext context = new ValidationContext(inObject, serviceProvider: null, items: null);
                List<System.ComponentModel.DataAnnotations.ValidationResult> validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                toRet = Validator.TryValidateObject(inObject, context, validationResults, true);
                if (!toRet)
                {
                    outErrors = new List<string>();
                    foreach (var item in validationResults)
                    {
                        outErrors.Add(item.ErrorMessage);
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return toRet;
        }

        public static bool ValidateProperty<T>(T inObject, Expression<Func<T, object>> inPropertyExpression, out List<string> outErrors)
        {
            bool toRet = false;
            outErrors = new List<string>();
            try
            {
                var compiledExpresion = inPropertyExpression.Compile();
                PropertyInfo propInfo = GetProperty<T>(inPropertyExpression);
                ValidationContext context = new ValidationContext(inObject, serviceProvider: null, items: null) { MemberName = propInfo.Name };
                List<System.ComponentModel.DataAnnotations.ValidationResult> validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                toRet = Validator.TryValidateProperty(compiledExpresion(inObject), context, validationResults);
                if (!toRet)
                {
                    outErrors = ValidationResultToString(validationResults);
                }
                else
                {

                }

                //List<ValidationAttribute> attrList = propInfo.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().ToList();
                //if (attrList != null && attrList.Count > 0)
                //{

                //    foreach (ValidationAttribute item in attrList)
                //    {
                //        //bool isValid = item.IsValid(compiledExpresion(inObject));
                //        System.ComponentModel.DataAnnotations.ValidationResult vr = item.GetValidationResult(compiledExpresion(inObject), context);
                //        if (vr != null && vr != System.ComponentModel.DataAnnotations.ValidationResult.Success)
                //        {
                //            if (vr != null)
                //            {
                //                toRet.Add(vr.ErrorMessage);
                //            }
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {

            }
            return toRet;
        }

        public static PropertyInfo GetProperty<T>(Expression<Func<T, Object>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            return typeof(T).GetProperty(body.Member.Name);
        }

        public static List<string> ValidationResultToString(List<System.ComponentModel.DataAnnotations.ValidationResult> validationResults)
        {
            List<string> outErrors = new List<string>();
            if (validationResults != null && validationResults.Count > 0)
            {
                foreach (var item in validationResults)
                {
                    string tmpMess = "";
                    if (item.MemberNames != null && item.MemberNames.Count() > 0)
                    {
                        tmpMess += "Property: ";
                        var tmpList = item.MemberNames.ToList();
                        for (int i = 0; i < tmpList.Count; i++)
                        {
                            string memName = tmpList[i];
                            tmpMess += memName;
                            if (i < (tmpList.Count - 1))
                            {
                                tmpMess += ", ";
                            }
                        }
                    }
                    tmpMess += ", Message: " + item.ErrorMessage;
                    outErrors.Add(tmpMess);
                }
            }
            return outErrors;
        }

    }//[Class]
}//[NameSpace]

