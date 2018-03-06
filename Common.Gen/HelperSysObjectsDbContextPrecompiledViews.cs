using Common.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public class HelperSysObjectsDbContextPrecompiledViews : HelperSysObjectsBase
    {
        public HelperSysObjectsDbContextPrecompiledViews(IEnumerable<Context> contexts)
        {
            this.Contexts = contexts;
            PathOutputBase.UsePathProjects = true;
            this._defineTemplateFolder = new DefineTemplateFolder();
        }
        public override void DefineTemplateByTableInfoFields(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            throw new NotImplementedException();
        }

        public override void DefineTemplateByTableInfo(Context config, TableInfo tableInfo)
        {
            throw new NotImplementedException();
        }

        #region Execute Templates

        private void ExecuteTemplateDbContextGenerateViewsInCode(Context configContext)
        {
            var pathOutput = PathOutput.PathOutputPreCompiledView(configContext);
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["GerarPreCompilacaoDoContexto"]) == false)
                return;

            if (configContext.OutputClassInfra.IsNullOrEmpty())
                return;

            var ctx = LoadDbContext(configContext);
            if (ctx.IsNull())
                return;

            var objectContext = ((IObjectContextAdapter)ctx).ObjectContext;
            var mappingCollection = (StorageMappingItemCollection)objectContext
                .MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);

            var errors = new List<EdmSchemaError>();
            var generateViews = mappingCollection.GenerateViews(errors);
            var computeMappingHashValue = mappingCollection.ComputeMappingHashValue();

            var pathMain = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(), DefineTemplateName.PrecompiledViewMain());
            var pathConditional = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(), DefineTemplateName.PrecompiledViewConditional());
            var pathViews = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(), DefineTemplateName.PrecompiledView());

            var templateMain = Read.AllText(pathMain);
            var templateConditional = Read.AllText(pathConditional);
            var templateView = Read.AllText(pathViews);

            var makeClassCondidional = string.Empty;
            var makeClassviews = string.Empty;

            foreach (var item in generateViews)
            {

                var viewNameConditional = string.Format("{0}.{1}", item.Key.EntityContainer.Name, item.Key.Name);
                var viewNameMethod = string.Format("{0}_{1}", item.Key.EntityContainer.Name, item.Key.Name);

                makeClassCondidional += Tabs.TabItemMethod() + templateConditional
                    .Replace("<#viewName#>", viewNameConditional)
                    .Replace("<#viewNameMethod#>", viewNameMethod) + System.Environment.NewLine;


                makeClassviews += templateView
                    .Replace("<#viewNameMethod#>", viewNameMethod)
                    .Replace("<#viewSql#>", item.Value.EntitySql) + System.Environment.NewLine;

            }

            templateMain = templateMain.Replace("<#ComputeMappingHashValue#>", computeMappingHashValue);
            templateMain = templateMain.Replace("<#conditional#>", makeClassCondidional);
            templateMain = templateMain.Replace("<#viewsList#>", makeClassviews).Replace("<#module#>", configContext.Module);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(templateMain);
            }

        }
        private void ExecuteTemplateDbContextGenerateViews(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputPreCompiledView(configContext);
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["GerarPreCompilacaoDoContexto"]) == false)
                return;


            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(), DefineTemplateName.PrecompiledViewBasic());
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }


        #endregion

        #region Helpers
        protected DbContext LoadDbContext(Context configContext)
        {
            try
            {
                var dll = configContext.OutputClassInfra.Split('\\').LastOrDefault();
                var log = FactoryLog.GetInstace();
                var pathOutputDbContext = string.Format(@"{0}\bin\Debug\{1}.dll", configContext.OutputClassInfra, dll);
                var assembly = Assembly.LoadFrom(pathOutputDbContext);
                var className = string.Format("DbContext{0}", configContext.Module);
                var type = assembly.GetTypes().Where(_ => _.Name == className).SingleOrDefault();
                var instanceCtx = Activator.CreateInstance(type, log);
                var ctx = instanceCtx as DbContext;
                return ctx;
            }
            catch
            {
                return null;
            }
        }

        public override string TransformFieldString(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldDateTime(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate, bool onlyDate = false)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldBool(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldPropertyInstance(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldHtml(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }



        public override string TransformFieldUpload(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldTextStyle(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldTextEditor(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldTextTag(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
