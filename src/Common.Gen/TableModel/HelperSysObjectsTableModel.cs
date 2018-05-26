using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace Common.Gen
{

    public class HelperSysObjectsTableModel : HelperSysObjectsBaseBack
    {
        public HelperSysObjectsTableModel(Context context, string templatePathBase)
        {
            var _contexts = new List<Context> {
                context
            };

            this.Contexts = _contexts;
            PathOutputBase.UsePathProjects = true;
            this._defineTemplateFolder = new DefineTemplateFolder();
            this._defineTemplateFolder.SetTemplatePathBase(templatePathBase);
            base.ArquitetureType = ArquitetureType.TableModel;

        }
        public HelperSysObjectsTableModel(Context context) : this(context, "Templates\\Back") { }
        public HelperSysObjectsTableModel(IEnumerable<Context> contexts) : this(contexts, "Templates\\Back") { }
        public HelperSysObjectsTableModel(IEnumerable<Context> contexts, string template)
        {
            this.Contexts = contexts;
            PathOutputBase.UsePathProjects = true;
            this._defineTemplateFolder = new DefineTemplateFolder();
            this._defineTemplateFolder.SetTemplatePathBase(template);
        }

        public override void DefineTemplateByTableInfo(Context config, TableInfo tableInfo)
        {
            this.DefineTemplateByTableInfoBack(config, tableInfo);
            this.DefineTemplateByTableInfoFront(config, tableInfo);
        }

        public override void DefineTemplateByTableInfoFields(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            DefineTemplateByTableInfoFieldsBack(config, tableInfo, infos);
            DefineTemplateByTableInfoFieldsFront(config, tableInfo, infos);
        }

        public override void DefineTemplateByTableInfoFieldsFront(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            new HelperSysObjectsVue(config).DefineTemplateByTableInfoFields(config, tableInfo, infos);
        }

        public override void DefineTemplateByTableInfoFront(Context config, TableInfo tableInfo)
        {
            new HelperSysObjectsVue(config).DefineTemplateByTableInfo(config, tableInfo);
        }

        public override void DefineTemplateByTableInfoFieldsBack(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            this.ExecuteTemplateModelsBase(tableInfo, config, infos);
            this.ExecuteTemplateModels(tableInfo, config, infos);

            this.ExecuteTemplateRepositoryBase(tableInfo, config, infos);
            this.ExecuteTemplateRepository(tableInfo, config, infos);

            this.ExecuteTemplateFilterBase(tableInfo, config, infos);
            this.ExecuteTemplateFilter(tableInfo, config, infos);

            this.ExecuteTemplateMapsBase(tableInfo, config, infos);
            this.ExecuteTemplateMaps(tableInfo, config, infos);

            this.ExecuteTemplateAppBase(tableInfo, config, infos);
            this.ExecuteTemplateApp(tableInfo, config, infos);

            this.ExecuteTemplateDtoBase(tableInfo, config, infos);
            this.ExecuteTemplateDtoSave(tableInfo, config, infos);
            this.ExecuteTemplateDtoGet(tableInfo, config, infos);

            this.ExecuteTemplateApi(tableInfo, config, infos);
            this.ExecuteTemplateApiPartial(tableInfo, config, infos);
        }

        public override void DefineTemplateByTableInfoBack(Context config, TableInfo tableInfo)
        {
            this.ExecuteTemplateDbContext(tableInfo, config);

            this.ExecuteTemplateAutoMapperProfileBase(tableInfo, config);
            this.ExecuteTemplateAutoMapperProfile(tableInfo, config);

            this.ExecuteTemplateContainer(tableInfo, config);
            this.ExecuteTemplateContainerPartial(tableInfo, config);

            this.ExecuteTemplateAutoMapper(tableInfo, config);
        }

        #region Execute Templates

        private void ExecuteTemplateFilterBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputFilter(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "filter");
            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "models.property");

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplatePropertys = Read.AllText(tableInfo, pathTemplatePropertys, this._defineTemplateFolder);
            if (!File.Exists(pathTemplateClass))
                return;

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    classBuilderPropertys = MakeFilterDateRange(TextTemplatePropertys, classBuilderPropertys, item);

                    if (item.Type == "bool")
                        classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, item.PropertyName, "bool?");
                    else
                        classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, item.PropertyName, item.Type);
                }
            }

            classBuilder = classBuilder.Replace("<#property#>", classBuilderPropertys);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateFilter(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputFilterPartial(tableInfo, configContext);

            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "filter.partial");
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {

                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateAppBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputApp(tableInfo, configContext);
            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "app");
            if (!File.Exists(pathTemplateClass))
                return;

            var TextTemplateAppClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, TextTemplateAppClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateApp(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputAppPartial(tableInfo, configContext);

            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "app.partial");
            if (!File.Exists(pathTemplateClass))
                return;

            var TextTemplateAppClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, TextTemplateAppClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateApi(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputApi(tableInfo, configContext);

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "api");
            var pathTemplateApiGet = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "api.get");

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateApiGet = Read.AllText(tableInfo, pathTemplateApiGet, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderApiGet = string.Empty;

            if (!tableInfo.IsCompositeKey && tableInfo.Keys.IsNotNull())
            {
                classBuilderApiGet = TextTemplateApiGet;
                classBuilderApiGet = classBuilderApiGet.Replace("<#className#>", tableInfo.ClassName);
                classBuilderApiGet = classBuilderApiGet.Replace("<#KeyName#>", tableInfo.Keys.FirstOrDefault());
                classBuilderApiGet = classBuilderApiGet.Replace("<#KeyType#>", tableInfo.KeysTypes.FirstOrDefault());
            }

            classBuilder = classBuilder.Replace("<#ApiGet#>", classBuilderApiGet);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateApiPartial(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputApiPartial(tableInfo, configContext);

            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "api.partial");

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateDtoBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDto(tableInfo, configContext);
            if ((File.Exists(pathOutput) && tableInfo.CodeCustomImplemented))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "dto");
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "models.property");
            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplatePropertys = Read.AllText(tableInfo, pathTemplatePropertys, this._defineTemplateFolder);


            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            if (infos.IsAny())
            {

                foreach (var item in infos)
                {
                    if (Audit.IsAuditField(item.PropertyName))
                        continue;

                    if (item.IsKey == 1)
                    {
                        classBuilder = classBuilder.Replace("<#KeyName#>", item.PropertyName);
                        var cast = item.Type == "string" ? ".ToString()" : string.Empty;
                        classBuilder = classBuilder.Replace("<#toString()#>", cast);
                        var expressionInclusion = item.Type == "string" ? string.Format("string.IsNullOrEmpty(this.{0})", item.PropertyName) : string.Format("this.{0} == 0", item.PropertyName);
                    }

                    var itempropert = TextTemplatePropertys.
                            Replace("<#type#>", item.Type).
                            Replace("<#propertyName#>", item.PropertyName);

                    classBuilderPropertys += string.Format("{0}{1}{2}", Tabs.TabModels(), itempropert, System.Environment.NewLine);

                }
            }

            classBuilder = classBuilder.Replace("<#property#>", classBuilderPropertys);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateDtoGet(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDtoSpecializedResult(tableInfo, configContext);

            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "dto.get");
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateDtoSave(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDtoSpecialized(tableInfo, configContext);

            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "dto.save");
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateAutoMapper(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = PathOutput.PathOutputAutoMapper(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "automapper");
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateAutoMapperProfileBase(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = PathOutput.PathOutputAutoMapperProfileBase(configContext, tableInfo);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "profile");
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateMappers = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "profile.registers");
            var pathTemplateMappersSave = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "profile.registers.save");
            var pathTemplateMappersGet = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "profile.registers.get");

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var textTemplateMappers = Read.AllText(tableInfo, pathTemplateMappers, this._defineTemplateFolder);
            var textTemplateMappersSave = Read.AllText(tableInfo, pathTemplateMappersSave, this._defineTemplateFolder);
            var textTemplateMappersGet = Read.AllText(tableInfo, pathTemplateMappersGet, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderMappers = string.Empty;

            foreach (var item in configContext.TableInfo)
            {
                var className = item.ClassName;

                if (!string.IsNullOrEmpty(className))
                {
                    var itemMapper = textTemplateMappers.
                            Replace("<#className#>", className);

                    var itemMapperSave = textTemplateMappersSave.Replace("<#className#>", className);
                    var itemMapperGet = textTemplateMappersGet.Replace("<#className#>", className);

                    classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabSets(), itemMapper, System.Environment.NewLine);
                    classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabSets(), itemMapperSave, System.Environment.NewLine);
                    classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabSets(), itemMapperGet, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder.Replace("<#registers#>", classBuilderMappers);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateAutoMapperProfile(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = PathOutput.PathOutputAutoMapperProfile(configContext, tableInfo);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "profile.partial");
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateDbContext(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = PathOutput.PathOutputDbContext(configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "context");
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateRegister = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "context.mappers");

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateMappers = Read.AllText(tableInfo, pathTemplateRegister, this._defineTemplateFolder);
            if (configContext.Module.IsNullOrEmpty())
                textTemplateClass = textTemplateClass.Replace("<#module#>", configContext.ProjectName);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderMappers = string.Empty;

            foreach (var item in configContext.TableInfo)
            {
                var itemMappaer = TextTemplateMappers.Replace("<#className#>", item.ClassName);
                classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabMaps(), itemMappaer, System.Environment.NewLine);
            }

            classBuilder = classBuilder.Replace("<#mappers#>", classBuilderMappers);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateContainer(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = PathOutput.PathOutputContainer(configContext);

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "container");
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateInjections = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "container.injections");

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateInjections = Read.AllText(tableInfo, pathTemplateInjections, this._defineTemplateFolder);

            if (configContext.Module.IsNullOrEmpty())
                textTemplateClass = textTemplateClass.Replace("<#domainSource#>", configContext.ProjectName);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderMappers = string.Empty;

            foreach (var item in configContext.TableInfo)
            {
                if (!string.IsNullOrEmpty(item.ClassName))
                {
                    var itemInjections = TextTemplateInjections.
                            Replace("<#namespace#>", configContext.Namespace).
                            Replace("<#module#>", configContext.Module.IsNullOrEmpty() ? configContext.ProjectName : configContext.Module).
                            Replace("<#className#>", item.ClassName).
                            Replace("<#domainSource#>", configContext.DomainSource.IsNullOrEmpty() ? configContext.ProjectName : configContext.DomainSource).
                            Replace("<#contextName#>", configContext.ContextName).
                            Replace("<#namespaceDomainSource#>", configContext.NamespaceDomainSource);

                    classBuilderMappers += string.Format("{0}{1}{2}", "            ", itemInjections, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder.Replace("<#injections#>", classBuilderMappers);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateContainerPartial(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = PathOutput.PathOutputContainerPartial(configContext);

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "container.partial");
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateModelsBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            var pathOutput = PathOutput.PathOutputDomainModels(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "models");
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "models.property");

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var textTemplatePropertys = Read.AllText(tableInfo, pathTemplatePropertys, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;
            var classBuilderFilters = string.Empty;
            
            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    if (item.IsKey == 1)
                    {
                        classBuilder = classBuilder.
                            Replace("<#KeyName#>", item.PropertyName).
                            Replace("<#KeyNameType#>", item.Type);

                        var cast = item.Type == "string" ? ".ToString()" : string.Empty;
                        classBuilder = classBuilder.Replace("<#toString()#>", cast);
                    }

                    var itempropert = textTemplatePropertys.
                            Replace("<#type#>", item.Type).
                            Replace("<#propertyName#>", item.PropertyName);

                    classBuilderPropertys += string.Format("{0}{1}{2}", Tabs.TabModels(), itempropert, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder.Replace("<#property#>", classBuilderPropertys);
            classBuilder = classBuilder.Replace("<#filtersExpressions#>", classBuilderFilters);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateModels(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDomainModelsPartial(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "models.partial");
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            classBuilder = classBuilder.Replace("<#WhereSingle#>", MakeKeysFromGet(tableInfo));

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateRepositoryBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            var pathOutput = PathOutput.PathOutputRepository(tableInfo, configContext);

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "repository");
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateFilters = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "repository.filters.expression");

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var textTemplateFilters = Read.AllText(tableInfo, pathTemplateFilters, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderFilters = string.Empty;
           

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    if (item.IsKey == 1)
                    {
                        classBuilder = classBuilder.
                            Replace("<#KeyName#>", item.PropertyName).
                            Replace("<#KeyNameType#>", item.Type);

                        var cast = item.Type == "string" ? ".ToString()" : string.Empty;
                        classBuilder = classBuilder.Replace("<#toString()#>", cast);
                    }

                    var itemFilters = string.Empty;

                    if (item.Type == "string")
                    {
                        itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                        itemFilters = itemFilters.Replace("<#condition#>", string.Format("_ => _.{0}.Contains(filters.{0})", item.PropertyName));
                        itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                    }
                    else if (item.Type == "DateTime")
                    {
                        var itemFiltersStart = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}Start", item.PropertyName));
                        itemFiltersStart = itemFiltersStart.Replace("<#condition#>", string.Format("_ => _.{0} >= filters.{0}Start ", item.PropertyName));
                        itemFiltersStart = itemFiltersStart.Replace("<#filtersRange#>", string.Empty);

                        var itemFiltersEnd = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}End", item.PropertyName));
                        itemFiltersEnd = itemFiltersEnd.Replace("<#condition#>", string.Format("_ => _.{0} <= filters.{0}End", item.PropertyName));
                        itemFiltersEnd = itemFiltersEnd.Replace("<#filtersRange#>", string.Format("filters.{0}End = filters.{0}End.AddDays(1).AddMilliseconds(-1);", item.PropertyName));

                        itemFilters = String.Format("{0}{1}{2}{3}{4}", itemFiltersStart, System.Environment.NewLine, Tabs.TabSets(), itemFiltersEnd, System.Environment.NewLine);

                    }
                    else if (item.Type == "DateTime?")
                    {
                        var itemFiltersStart = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}Start", item.PropertyName));
                        itemFiltersStart = itemFiltersStart.Replace("<#condition#>", string.Format("_ => _.{0} != null && _.{0}.Value >= filters.{0}Start.Value", item.PropertyName));
                        itemFiltersStart = itemFiltersStart.Replace("<#filtersRange#>", string.Empty);

                        var itemFiltersEnd = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}End", item.PropertyName));
                        itemFiltersEnd = itemFiltersEnd.Replace("<#condition#>", string.Format("_ => _.{0} != null &&  _.{0}.Value <= filters.{0}End", item.PropertyName));
                        itemFiltersEnd = itemFiltersEnd.Replace("<#filtersRange#>", string.Format("filters.{0}End = filters.{0}End.Value.AddDays(1).AddMilliseconds(-1);", item.PropertyName));

                        itemFilters = String.Format("{0}{1}{2}{3}{4}", itemFiltersStart, System.Environment.NewLine, Tabs.TabSets(), itemFiltersEnd, System.Environment.NewLine);

                    }
                    else if (item.Type == "bool?")
                    {
                        itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                        itemFilters = itemFilters.Replace("<#condition#>", string.Format("_ => _.{0} != null && _.{0}.Value == filters.{0}", item.PropertyName));
                        itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                    }
                    else if (item.Type == "int?" || item.Type == "Int64?" || item.Type == "Int16?" || item.Type == "decimal?" || item.Type == "float?")
                    {
                        itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                        itemFilters = itemFilters.Replace("<#condition#>", string.Format("_ => _.{0} != null && _.{0}.Value == filters.{0}", item.PropertyName));
                        itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                    }
                    else
                    {
                        itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                        itemFilters = itemFilters.Replace("<#condition#>", string.Format("_ => _.{0} == filters.{0}", item.PropertyName));
                        itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                    }


                    classBuilderFilters += string.Format("{0}{1}{2}", Tabs.TabSets(), itemFilters, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder.Replace("<#filtersExpressions#>", classBuilderFilters);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateRepository(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {

            var pathOutput = PathOutput.PathOutputRepositoryPartial(tableInfo, configContext);

            if (File.Exists(pathOutput))
                return;


            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "repository.partial");
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateFilters = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "repository.filters.expression");

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var textTemplateFilters = Read.AllText(tableInfo, pathTemplateFilters, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateMaps(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputMapsPartial(tableInfo, configContext);

            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "maps.partial");
            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }
        }

        private void ExecuteTemplateMapsBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputMaps(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "maps");
            var pathTemplateLength = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "maps.length");
            var pathTemplateRequired = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "maps.required");
            var pathTemplateMapper = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "maps.mapper");
            var pathTemplateManyToMany = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "maps.manytomany");
            var pathTemplateCompositeKey = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), "maps.compositekey");

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var textTemplateLength = Read.AllText(tableInfo, pathTemplateLength, this._defineTemplateFolder);
            var textTemplateRequired = Read.AllText(tableInfo, pathTemplateRequired, this._defineTemplateFolder);
            var textTemplateMapper = Read.AllText(tableInfo, pathTemplateMapper, this._defineTemplateFolder);
            var textTemplateManyToMany = Read.AllText(tableInfo, pathTemplateManyToMany, this._defineTemplateFolder);
            var textTemplateCompositeKey = Read.AllText(tableInfo, pathTemplateCompositeKey, this._defineTemplateFolder);

            var classBuilderitemTemplateLength = string.Empty;
            var classBuilderitemTemplateRequired = string.Empty;
            var classBuilderitemplateMapper = string.Empty;
            var classBuilderitemplateMapperKey = string.Empty;
            var classBuilderitemplateManyToMany = string.Empty;
            var classBuilderitemplateCompositeKey = string.Empty;

            string classBuilder = MakeClassBuilderMapORM(tableInfo, configContext, infos, textTemplateClass, textTemplateLength, textTemplateRequired, textTemplateMapper, textTemplateManyToMany, textTemplateCompositeKey, ref classBuilderitemTemplateLength, ref classBuilderitemTemplateRequired, ref classBuilderitemplateMapper, ref classBuilderitemplateMapperKey, ref classBuilderitemplateManyToMany, ref classBuilderitemplateCompositeKey);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }


        }

        #endregion

        #region helpers

        private string MakePropertyNameKeyAlias(string column, string className)
        {

            if (column.ToLower() == "id")
                return string.Format("{0}Id", className);


            return column;
        }
        private void MigatePathFileDomain(Context config, TableInfo tableInfo)
        {
            var fileBaseName = tableInfo.ClassName;
            var pathBase = Path.GetDirectoryName(PathOutput.PathOutputDomainModels(tableInfo, config)).Replace(fileBaseName, "");

            var FileVariants = new string[] {
                    string.Format("{0}.cs", fileBaseName),
                    string.Format("{0}.ext.cs", fileBaseName),
                    string.Format("{0}.Validation.cs", fileBaseName),
                    string.Format("{0}.Validation.ext.cs", fileBaseName),
                    string.Format("{0}Custom.ext.cs", fileBaseName),
                };

            MigatePathFile(pathBase, fileBaseName, FileVariants);

        }
        private void MigatePathFileDto(Context config, TableInfo tableInfo)
        {
            var fileBaseName = tableInfo.ClassName;
            var pathBase = Path.GetDirectoryName(PathOutput.PathOutputDto(tableInfo, config)).Replace(fileBaseName, "");

            var FileVariants = new string[] {
                    string.Format("{0}Dto.cs", fileBaseName),
                    string.Format("{0}DtoSpecialized.ext.cs", fileBaseName),
                    string.Format("{0}DtoSpecializedResult.ext.cs", fileBaseName),
                    string.Format("{0}DtoSpecializedReport.ext.cs", fileBaseName),
                    string.Format("{0}DtoSpecializedDetails.ext.cs", fileBaseName),
                };

            MigatePathFile(pathBase, fileBaseName, FileVariants);

        }
        private void MigatePathFile(string pathBase, string fileBaseName, string[] FileVariants)
        {

            var folder = Path.Combine(pathBase, fileBaseName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var file in FileVariants)
            {
                var fileFound = new DirectoryInfo(pathBase).GetFiles(file).SingleOrDefault();
                if (fileFound != null)
                {
                    if (File.Exists(fileFound.FullName))
                    {
                        fileFound.CopyTo(Path.Combine(folder, file));
                        fileFound.Delete();
                    }
                }

            }
        }
        private bool IsPrimaryKey(SqlDataReader reader)
        {
            return Convert.ToInt32(reader["Chave"]) == 1;
        }
        private bool IsOneToOneRelation(SqlDataReader reader)
        {
            return reader["FKCOLUMN_NAME"].ToString() == reader["PKCOLUMN_NAME"].ToString();
        }
        private void ClearContextCodeFiles(Context config, string path, string subfolder, Func<TableInfo, bool> predicate, string noDeleteFilesPartner = null)
        {
            if (!config.ClearAllFiles)
                return;

            if (String.IsNullOrEmpty(path))
                return;

            var directory = new DirectoryInfo(Path.Combine(path, subfolder));
            if (!directory.Exists)
                return;

            var files = directory.GetFiles().Where(_ => _.Extension == ".cs");
            foreach (var item in files)
            {

                if (!String.IsNullOrEmpty(noDeleteFilesPartner))
                    if (item.Name.Contains(noDeleteFilesPartner))
                        continue;


                if (item.Name.Contains(".ext"))
                    continue;

                var deleteDenied = this.Contexts
                    .Where(_ => _.TableInfo
                        .Where(__ => item.Name.Contains(__.TableName))
                        .Where(predicate).Any()).Any();


                if (deleteDenied)
                    continue;


                item.Delete();
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
