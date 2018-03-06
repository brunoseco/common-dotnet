using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class GlobalizationModelBinderDate : IModelBinder
{
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (value != null)
            return value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);

        return value;
    }
}
