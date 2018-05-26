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
    public abstract class HelperSysObjectsBaseBack : HelperSysObjectsBase
    {
        public abstract void DefineTemplateByTableInfoFieldsFront(Context config, TableInfo tableInfo, UniqueListInfo infos);
        public abstract void DefineTemplateByTableInfoFront(Context config, TableInfo tableInfo);
        public abstract void DefineTemplateByTableInfoFieldsBack(Context config, TableInfo tableInfo, UniqueListInfo infos);
        public abstract void DefineTemplateByTableInfoBack(Context config, TableInfo tableInfo);

    }
}
