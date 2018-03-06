using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class GlobalizationModelBinderDecimal : IModelBinder
{
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        var isNull = value.AttemptedValue == "null" || string.IsNullOrEmpty(value.AttemptedValue) || value.AttemptedValue == " ";

        if (isNull)
            return null;

        var newValue = !isNull ?
            ((value.AttemptedValue.IndexOf('.') != -1 && value.AttemptedValue.IndexOf(',') == -1) ?
                value.AttemptedValue.Replace(".", ",") : value.AttemptedValue.ToString()) : null;


        try
        {
            return Convert.ToDecimal(newValue, CultureInfo.CurrentCulture);
        }
        catch (Exception ex)
        {
            throw new Exception(String.Format("Ocorreu um erro no mvc model binder com a conversão do valor {0} para decimal ", newValue), ex);
        }
        

    }
}
