using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    static class PathOutput
    {      
        public static string PathOutputMapsPartial(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassDomain);
            pathOutput = Path.Combine(pathBase, "Maps", tableInfo.ClassName, string.Format("{0}Map.ext.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory(pathBase, "Maps", tableInfo.ClassName);

            return pathOutput;
        }
        
        public static string PathOutputMaps(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassDomain);
            pathOutput = Path.Combine(pathBase, "Maps", tableInfo.ClassName, string.Format("{0}MapBase.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory(pathBase, "Maps", tableInfo.ClassName);

            return pathOutput;
        }
        
        public static string PathOutputDbContext(Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassDomain);
            pathOutput = Path.Combine(pathBase, "Context", string.Format("DbContext{0}.{1}", configContext.ContextName, "cs"));
            PathOutputBase.MakeDirectory("Context", pathBase);
            return pathOutput;
        }

        public static string PathOutputDomainModelsPartial(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassDomain);

            var filename = tableInfo.ClassName;
            pathOutput = Path.Combine(pathBase, "Models", filename, string.Format("{0}.ext.{1}", filename, "cs"));
            PathOutputBase.MakeDirectory(pathBase, "Models", filename);

            return pathOutput;
        }
        
        public static string PathOutputDomainModels(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathOutputBase.PathBase(configContext.OutputClassDomain);
            pathOutput = Path.Combine(pathBase, "Models", tableInfo.ClassName, string.Format("{0}.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory(pathBase, "Models", tableInfo.ClassName);

            return pathOutput;
        }

        public static string PathOutputRepository(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathOutputBase.PathBase(configContext.OutputClassDomain);
            pathOutput = Path.Combine(pathBase, "Repository", tableInfo.ClassName, string.Format("{0}Repository.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory(pathBase, "Repository", tableInfo.ClassName);

            return pathOutput;
        }
        
        public static string PathOutputApp(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Services", string.Format("{0}ServiceBase.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory("Services", pathBase);
            return pathOutput;
        }
        
        public static string PathOutputAppPartial(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Services", string.Format("{0}Service.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory("Services", pathBase);
            return pathOutput;
        }
        
        public static string PathOutputContainer(Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathOutputBase.PathBase(configContext.OutputClassApi);

            pathOutput = Path.Combine(pathBase, "Config", string.Format("ConfigContainer{0}.{1}", configContext.Module, "cs"));
            PathOutputBase.MakeDirectory("Config", pathBase);

            return pathOutput;
        }
        
        public static string PathOutputContainerPartial(Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathOutputBase.PathBase(configContext.OutputClassApi);

            pathOutput = Path.Combine(pathBase, "Config", string.Format("ConfigContainer{0}.ext.{1}", configContext.Module, "cs"));
            PathOutputBase.MakeDirectory("Config", pathBase);

            return pathOutput;
        }
        
        public static string PathOutputAutoMapper(Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathOutputBase.PathBase(configContext.OutputClassApp);

            pathOutput = Path.Combine(pathBase, "Config", string.Format("AutoMapperConfig{0}.{1}", configContext.ContextName, "cs"));
            PathOutputBase.MakeDirectory("Config", pathBase);

            return pathOutput;
        }

        public static string PathOutputAutoMapperProfileBase(Context configContext, TableInfo tableInfo)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Config", string.Format("DomainToDto{0}Base.{1}", configContext.ContextName, "cs"));
            PathOutputBase.MakeDirectory("Config", pathBase);
            return pathOutput;
        }

        public static string PathOutputAutoMapperProfile(Context configContext, TableInfo tableInfo)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Config", string.Format("DomainToDto{0}.{1}", configContext.ContextName, "cs"));
            PathOutputBase.MakeDirectory("Config", pathBase);
            return pathOutput;
        }
        
        public static string PathOutputFilter(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassFilter);
            pathOutput = Path.Combine(pathBase, "Filters", string.Format("{0}Filter.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory("Filters", pathBase);
            return pathOutput;
        }
        
        public static string PathOutputFilterPartial(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassFilter);
            var fileName = tableInfo.ClassName;
            pathOutput = Path.Combine(pathBase, "Filters", string.Format("{0}Filter.ext.{1}", fileName, "cs"));
            PathOutputBase.MakeDirectory("Filters", pathBase);
            return pathOutput;
        }
        
        public static string PathOutputDto(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassDto);
            pathOutput = Path.Combine(pathBase, "Dto", tableInfo.ClassName, string.Format("{0}Dto.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory(pathBase, "Dto", tableInfo.ClassName);
            return pathOutput;
        }
        
        public static string PathOutputDtoSpecialized(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassDto);
            pathOutput = Path.Combine(pathBase, "Dto", tableInfo.ClassName, string.Format("{0}DtoSave.ext.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory(pathBase, "Dto", tableInfo.ClassName);
            return pathOutput;
        }
        
        public static string PathOutputDtoSpecializedResult(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassDto);
            pathOutput = Path.Combine(pathBase, "Dto", tableInfo.ClassName, string.Format("{0}DtoResult.ext.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory(pathBase, "Dto", tableInfo.ClassName);
            return pathOutput;
        }
        
        public static string PathOutputApi(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathOutputBase.PathBase(configContext.OutputClassApi);
            pathOutput = Path.Combine(pathBase, "Controllers", string.Format("{0}Controller.{1}", tableInfo.ClassName, "cs"));
            PathOutputBase.MakeDirectory("Controllers", pathBase);
            return pathOutput;
        }
        

    }
}
