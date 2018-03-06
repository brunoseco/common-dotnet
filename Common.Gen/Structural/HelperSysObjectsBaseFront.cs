using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Mapping;
using Common.Infrastructure.Log;
using System.Reflection;
using System.Data.Entity;

namespace Common.Gen
{
    public abstract class HelperSysObjectsBaseFront : HelperSysObjectsBase
    {
        protected string FormFieldReplace(Context configContext, TableInfo tableInfo, Info info, string textTemplateForm, string str = "")
        {
            var cols = "col-md-12";

            if (tableInfo.TwoCols || configContext.TwoCols)
                cols = "col-md-6";

            if (IsStringLengthBig(info, configContext))
                cols = "col-md-12";

            var colSize = HelperFieldConfig.GetColSizeField(tableInfo, info.PropertyName);
            if (colSize.IsSent())
                cols = string.Format("col-md-{0}", colSize);

            return textTemplateForm
                .Replace("<#formField#>", str)
                .Replace("<#colformField#>", cols.ToString());
        }


        public override void DefineTemplateByTableInfo(Context config, TableInfo tableInfo) { }

        public override void DefineTemplateByTableInfoFields(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            foreach (var item in infos)
            {
                var order = TypeConvertCSharp.OrderByType(item.Type);

                if (tableInfo.FieldsConfig.IsAny())
                {
                    var orderField = tableInfo.FieldsConfig
                        .Where(_ => _.Name.ToLower() == item.ColumnName.ToString().ToLower())
                        .SingleOrDefault();

                    if (orderField.IsNotNull() && orderField.Order.IsSent())
                        order = orderField.Order;
                }

                item.Order = order;
            }

            base.CastOrdenabledToUniqueListInfo(infos);
        }

    }
}
