using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;


namespace Common.Gen
{

    public class HelperSysObjectsAngular20 : HelperSysObjectsBaseFront
    {
        public HelperSysObjectsAngular20(Context context) : this(context, "Templates\\Front")
        {

        }
        public HelperSysObjectsAngular20(Context context, string template)
        {
            var _contexts = new List<Context> {
                context
            };

            this.Contexts = _contexts;
            PathOutputBase.UsePathProjects = true;
            this._defineTemplateFolder = new DefineTemplateFolder();
            this._defineTemplateFolder.SetTemplatePathBase(template);

        }
        public HelperSysObjectsAngular20(IEnumerable<Context> contexts)
        {
            this.Contexts = contexts;
            PathOutputBase.UsePathProjects = true;
        }

        public override void DefineTemplateByTableInfo(Context configContext, TableInfo tableInfo)
        {
            var overrideFile = configContext.OverrideFiles;
            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Template = DefineTemplateNameAngular20.Angular20ComponentServiceTs(tableInfo),
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentServiceTs(tableInfo, configContext),
                Operation = EOperation.Front_angular_Service,
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentHtml(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20ComponentHTml(tableInfo),
                Operation = EOperation.Front_angular_Component,
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                ConfigContext = configContext,
                TableInfo = tableInfo,
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentAppRouting(configContext),
                Template = DefineTemplateNameAngular20.Angular20AppRoute(),
                TemplateClassItem = new List<TemplateClass>{

                    new TemplateClass {

                        TemplateName = DefineTemplateNameAngular20.Angular20AppRouteItem(),
                        TagTemplate = "<#classItems#>"
                    },
                    new TemplateClass {

                        TemplateName = DefineTemplateNameAngular20.Angular20AppRouteItemPrint(),
                        TagTemplate = "<#classItemsPrint#>"
                    }
                },
                ExecuteProcess = base.GenericTagsTransformerRoutes,
                Flow = EFlowTemplate.Class,
                Layer = ELayer.Front,
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                ConfigContext = configContext,
                TableInfo = tableInfo,
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentAppCustomRouting(configContext),
                Template = DefineTemplateNameAngular20.Angular20AppCustomRoute(),
                Flow = EFlowTemplate.Class,
                Layer = ELayer.Front,
                OverrideFile = overrideFile,
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20ComponentTs(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentCss(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20ComponentCsss(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentModuleTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20ComponentModuleTs(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentRoutingModuleTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20ComponentRoutingModuleTs(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentDetailsTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentDetailsTs(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentEditTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentEditTs(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentEditHtml(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentEditHtml(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentEditCss(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentEditCss(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentCreate(tableInfo, configContext, "ts"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentCreate(tableInfo, "ts"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentCreate(tableInfo, configContext, "html"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentCreate(tableInfo, "html"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentCreateContainer(tableInfo, configContext, "ts"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentCreateContainer(tableInfo, "ts"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentCreateContainer(tableInfo, configContext, "html"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentCreateContainer(tableInfo, "html"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentCreateContainer(tableInfo, configContext, "css"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentCreateContainer(tableInfo, "css"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentEditContainer(tableInfo, configContext, "ts"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentEditContainer(tableInfo, "ts"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentEditContainer(tableInfo, configContext, "html"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentEditContainer(tableInfo, "html"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentEditContainer(tableInfo, configContext, "css"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentEditContainer(tableInfo, "css"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentDetailsContainer(tableInfo, configContext, "ts"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentDetailsContainer(tableInfo, "ts"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentDetailsContainer(tableInfo, configContext, "html"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentDetailsContainer(tableInfo, "html"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentDetailsContainer(tableInfo, configContext, "css"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentDetailsContainer(tableInfo, "css"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentCreate(tableInfo, configContext, "css"),
                Template = DefineTemplateNameAngular20.Angular20SubComponentCreate(tableInfo, "css"),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFieldCreateCss(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFieldCss(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFieldEditCss(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFieldCss(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFieldFilterCss(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFieldFilterCss(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentPrintTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentPrintTs(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentPrintHtml(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentPrintHtml(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentPrintCss(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentPrintCss(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentPrintModule(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentPrintModule(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentPrintRoutingModule(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentPrintRoutingModule(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentDetailsCss(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentDetailsCss(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentDetailsHtml(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentDetailsHtml(tableInfo),
                Operation = EOperation.Front_angular_Details,
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentDetailsFieldsTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentDetailsFieldsTs(tableInfo),
                Operation = EOperation.Front_angular_FieldDetails,
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentDetailsFieldsCss(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentDetailsFieldsCss(tableInfo),
                Operation = EOperation.Front_angular_FieldDetails,
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFieldFilterTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFieldFilterTs(tableInfo),
                Operation = EOperation.Front_angular_FieldFilter,
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFilterContainerTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFilterContainerTs(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFilterContainerHtml(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFilterContainerHTML(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFilterContainerCss(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFilterContainerCss(tableInfo),
                Flow = EFlowTemplate.Static,
                Layer = ELayer.Front,
                OverrideFile = overrideFile
            });

        }

        public override void DefineTemplateByTableInfoFields(Context configContext, TableInfo tableInfo, UniqueListInfo infos)
        {
            base.DefineTemplateByTableInfoFields(configContext, tableInfo, infos);
            infos = base.CastOrdenabledToUniqueListInfo(infos);

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Infos = infos,
                PathOutput = PathOutputAngular20.PathOutputAngular20ViewModel(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20ViewModel(tableInfo),
                TemplateFields = new List<TemplateField> {
                    new TemplateField {
                        TemplateName =  DefineTemplateNameAngular20.Angular20ViewModelItem(tableInfo),
                        TagTemplate = "<#fieldItem#>"
                    }
                },
                Operation = EOperation.Front_angular_ViewModel,
                Flow = EFlowTemplate.Field,
                Layer = ELayer.Front,
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Infos = infos,
                Template = DefineTemplateNameAngular20.Angular20ComponentServiceFieldsTs(tableInfo),
                TemplateFields = new List<TemplateField> {
                    new TemplateField {
                        TemplateName =  DefineTemplateNameAngular20.Angular20ComponentServiceFieldsRequiredTs(tableInfo),
                        TagTemplate = "<#riquered#>"
                    },
                     new TemplateField {
                        TemplateName =  DefineTemplateNameAngular20.Angular20ComponentServiceFieldsInfoTs(tableInfo),
                        TagTemplate = "<#infos#>"
                    }
                },
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentServiceFieldsTs(tableInfo, configContext),
                Operation = EOperation.Front_angular_Service_Fields,
                Flow = EFlowTemplate.Field,
                WithRestrictions = false,
                Layer = ELayer.Front,
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Infos = infos,
                Template = DefineTemplateNameAngular20.Angular20ComponentServiceFieldsCustomTs(tableInfo),
                TemplateFields = new List<TemplateField> {
                    new TemplateField {
                        TemplateName =  DefineTemplateNameAngular20.Angular20ComponentServiceFieldsRequiredTs(tableInfo),
                        TagTemplate = "<#riquered#>"
                    },
                     new TemplateField {
                        TemplateName =  DefineTemplateNameAngular20.Angular20ComponentServiceFieldsInfoTs(tableInfo),
                        TagTemplate = "<#infos#>"
                    }
                },
                PathOutput = PathOutputAngular20.PathOutputAngular20ComponentServiceFieldsCustomTs(tableInfo, configContext),
                Operation = EOperation.Front_angular_Service_Fields,
                Flow = EFlowTemplate.Field,
                WithRestrictions = false,
                Layer = ELayer.Front,
                OverrideFile = configContext.OverrideFiles
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Infos = infos,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentDetailsFieldsHtml(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentDetailsFieldsHtml(tableInfo),
                TemplateFields = new List<TemplateField> {
                    new TemplateField {
                        TemplateName =  DefineTemplateNameAngular20.Angular20SubComponentDetailsSectionHtml(tableInfo),
                        TagTemplate = "<#fieldItems#>"
                    }
                },
                Operation = EOperation.Front_angular_FieldDetails,
                Flow = EFlowTemplate.Field,
                Layer = ELayer.Front,
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Infos = infos,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFieldCreateHtml(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFieldHtml(tableInfo),
                TemplateFields = new List<TemplateField> {
                    new TemplateField {
                        TemplateName =  DefineTemplateNameAngular20.Angular20SubComponentFieldSectionHtml(tableInfo),
                        TagTemplate = "<#fieldItems#>"
                    }
                },
                Operation = EOperation.Front_angular_FieldsCreate,
                Flow = EFlowTemplate.FieldType,
                Layer = ELayer.Front,
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Infos = infos,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFieldEditdHtml(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFieldHtml(tableInfo),
                TemplateFields = new List<TemplateField> {
                    new TemplateField {
                        TemplateName =  DefineTemplateNameAngular20.Angular20SubComponentFieldSectionHtml(tableInfo),
                        TagTemplate = "<#fieldItems#>"
                    }
                },
                Operation = EOperation.Front_angular_FieldsEdit,
                Flow = EFlowTemplate.FieldType,
                Layer = ELayer.Front,
            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Infos = infos.Where(_ => HelperFieldConfig.IsTextEditor(tableInfo, _.PropertyName)),
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFieldCreateTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFieldCreateTs(tableInfo),
                TemplateFields = new List<TemplateField> {
                    new TemplateField {
                        TemplateName =  DefineTemplateNameAngular20.Angular20SubComponentFieldEditoHtmlKeyUp(tableInfo),
                        TagTemplate = "<#fieldItems#>"
                    }
                },
                Operation = EOperation.Front_angular_FieldsCreate,
                Flow = EFlowTemplate.Field,
                Layer = ELayer.Front,

            });

            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Infos = infos.Where(_ => HelperFieldConfig.IsTextEditor(tableInfo, _.PropertyName)),
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFieldEditTs(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFieldEditTs(tableInfo),
                TemplateFields = new List<TemplateField> {
                    new TemplateField {
                        TemplateName = DefineTemplateNameAngular20.Angular20SubComponentFieldEditoHtmlKeyUp(tableInfo),
                        TagTemplate = "<#fieldItems#>"
                    }
                },
                Operation = EOperation.Front_angular_FieldsEdit,
                Flow = EFlowTemplate.Field,
                Layer = ELayer.Front,

            });


            ExecuteTemplate(new ConfigExecutetemplate
            {
                TableInfo = tableInfo,
                ConfigContext = configContext,
                Infos = infos,
                PathOutput = PathOutputAngular20.PathOutputAngular20SubComponentFieldFilterHtml(tableInfo, configContext),
                Template = DefineTemplateNameAngular20.Angular20SubComponentFieldFilterHtml(tableInfo),
                TemplateFields = new List<TemplateField> {
                    new TemplateField {
                        TemplateName = DefineTemplateNameAngular20.Angular20SubComponentFieldFilterSectionHtml(tableInfo),
                        TagTemplate = "<#fieldItems#>"
                    }
                },
                Operation = EOperation.Front_angular_FieldFilter,
                Flow = EFlowTemplate.FieldType,
                Layer = ELayer.Front,
            });




        }


        #region TransformField

        public override string TransformField(ConfigExecutetemplate configExecutetemplate, TableInfo tableInfo, Info info, string propertyName, string textTemplate)
        {
            if (configExecutetemplate.Operation == EOperation.Front_angular_Service_Fields)
            {
                var _isPasswordConfirmation = HelperFieldConfig.IsPasswordConfirmation(tableInfo, propertyName);
                if (_isPasswordConfirmation)
                {
                    textTemplate = textTemplate + System.Environment.NewLine + "			confSenha: new FormControl(),";
                }
            }

            return base.TransformField(configExecutetemplate, tableInfo, info, propertyName, textTemplate);
        }


        public override string TransformFieldString(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            var field = string.Empty;

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
            {
                var str = HelperControlHtmlAngular20.MakeInputFilterHtml();
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm
                    .Replace("<#propertyName#>", propertyName)
                    .Replace("<#className#>", configExecutetemplate.TableInfo.ClassName);

                return field;
            }

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldsCreate || configExecutetemplate.Operation == EOperation.Front_angular_FieldsEdit || configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
            {
                var isStringLengthBig = base.IsStringLengthBig(info, configExecutetemplate.ConfigContext);
                var str = HelperControlHtmlAngular20.MakeInputHtml(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, isStringLengthBig, propertyName);
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm.Replace("<#propertyName#>", propertyName)
                    .Replace("<#className#>", configExecutetemplate.TableInfo.ClassName);

                return field;
            }


            return field;
        }

        public override string TransformFieldDateTime(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate, bool onlyDate = false)
        {
            var field = string.Empty;

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
            {
                var str = HelperControlHtmlAngular20.MakeDatePikerFilter();
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm.Replace("<#propertyName#>", propertyName);
                return field;
            }

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldsCreate || configExecutetemplate.Operation == EOperation.Front_angular_FieldsEdit || configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
            {
                var str = HelperControlHtmlAngular20.MakeDateTimePiker(IsRequired(info));
                if (onlyDate) str = HelperControlHtmlAngular20.MakeDatePiker(IsRequired(info));
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm.Replace("<#propertyName#>", propertyName)
                    .Replace("<#className#>", configExecutetemplate.TableInfo.ClassName);
                return field;
            }
            
            return field;
        }

        public override string TransformFieldBool(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            var field = string.Empty;

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
            {
                var str = HelperControlHtmlAngular20.MakeRadioFilter();
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm.Replace("<#propertyName#>", propertyName);
                return field;
            }

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldsCreate || configExecutetemplate.Operation == EOperation.Front_angular_FieldsEdit || configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
            {
                var str = HelperControlHtmlAngular20.MakeCheckbox(IsRequired(info));
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm.Replace("<#propertyName#>", propertyName)
                    .Replace("<#className#>", configExecutetemplate.TableInfo.ClassName);
                return field;
            }


            return field;
        }

        public override string TransformFieldPropertyInstance(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            var field = string.Empty;

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
            {
                var str = HelperControlHtmlAngular20.MakeDropDownFilter();

                var isMultiSelectFilter = HelperFieldConfig.IsMultiSelectFilter(configExecutetemplate.TableInfo, propertyName);
                if (isMultiSelectFilter)
                    str = HelperControlHtmlAngular20.MakeMultiSelectFilter();

                var isSelectSearch = HelperFieldConfig.IsSelectSearch(configExecutetemplate.TableInfo, propertyName);
                if (isSelectSearch)
                    str = HelperControlHtmlAngular20.MakeDropDownSeachFilter();

                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm
                    .Replace("<#propertyName#>", propertyName)
                    .Replace("<#ReletedClass#>", PropertyInstance(configExecutetemplate.TableInfo, info.PropertyName))
                    .Replace("<#fieldFilterName#>", PropertyInstanceFieldFilterDefault(configExecutetemplate.TableInfo, info.PropertyName));


                return field;
            }

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldsCreate || configExecutetemplate.Operation == EOperation.Front_angular_FieldsEdit || configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
            {
                var str = HelperControlHtmlAngular20.MakeDropDown(IsRequired(info));

                var isSelectSearch = HelperFieldConfig.IsSelectSearch(configExecutetemplate.TableInfo, propertyName);
                if (isSelectSearch)
                    str = HelperControlHtmlAngular20.MakeDropDownSeach(IsRequired(info));

                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm
                    .Replace("<#propertyName#>", propertyName)
                    .Replace("<#ReletedClass#>", PropertyInstance(configExecutetemplate.TableInfo, info.PropertyName))
                    .Replace("<#fieldFilterName#>", PropertyInstanceFieldFilterDefault(configExecutetemplate.TableInfo, info.PropertyName));

                return field;
            }


            return field;
        }

        public override string TransformFieldHtml(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            var field = string.Empty;

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldsCreate || configExecutetemplate.Operation == EOperation.Front_angular_FieldsEdit || configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
            {
                var htmlCtrl = HelperFieldConfig.FieldHtml(configExecutetemplate.TableInfo, propertyName);
                var str = HelperControlHtmlAngular20.MakeCtrl(htmlCtrl.HtmlField);

                if (configExecutetemplate.Operation == EOperation.Front_angular_FieldFilter)
                    str = HelperControlHtmlAngular20.MakeCtrl(htmlCtrl.HtmlFilter);

                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm.Replace("<#propertyName#>", propertyName)
                    .Replace("<#className#>", configExecutetemplate.TableInfo.ClassName);

                return field;
            }


            return field;
        }


        public override string TransformFieldUpload(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            var field = string.Empty;

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldsCreate || configExecutetemplate.Operation == EOperation.Front_angular_FieldsEdit)
            {
                var str = HelperControlHtmlAngular20.MakeUpload(info);
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm.Replace("<#propertyName#>", propertyName)
                    .Replace("<#className#>", configExecutetemplate.TableInfo.ClassName);
                return field;
            }

            return field;

        }

        public override string TransformFieldTextStyle(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            var field = string.Empty;

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldsCreate || configExecutetemplate.Operation == EOperation.Front_angular_FieldsEdit)
            {
                var str = HelperControlHtmlAngular20.MakeTextStyle(IsRequired(info));
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm
                    .Replace("<#propertyName#>", propertyName)
                    .Replace("<#className#>", configExecutetemplate.TableInfo.ClassName);

                return field;
            }

            return field;

        }

        public override string TransformFieldTextEditor(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            var field = string.Empty;

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldsCreate || configExecutetemplate.Operation == EOperation.Front_angular_FieldsEdit)
            {
                var str = HelperControlHtmlAngular20.MakeTextEditor(IsRequired(info));
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm
                    .Replace("<#propertyName#>", propertyName)
                    .Replace("<#ReletedClass#>", PropertyInstance(configExecutetemplate.TableInfo, info.PropertyName));

                return field;
            }

            return field;

        }

        public override string TransformFieldTextTag(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            var field = string.Empty;

            if (configExecutetemplate.Operation == EOperation.Front_angular_FieldsCreate || configExecutetemplate.Operation == EOperation.Front_angular_FieldsEdit)
            {
                var str = HelperControlHtmlAngular20.MakeTextTag(IsRequired(info));
                var itemForm = FormFieldReplace(configExecutetemplate.ConfigContext, configExecutetemplate.TableInfo, info, textTemplate, str);
                field = itemForm
                    .Replace("<#propertyName#>", propertyName);

                return field;
            }

            return field;
        }

        #endregion

        #region helpers

        protected override string CamelCaseTransform(string str)
        {
            var ex = this._camelCasingExceptions.Where(_ => str.ToUpper().StartsWith(_.ToUpper()));
            if (ex.IsAny())
            {
                var exceptionRule = ex.FirstOrDefault();
                var exceptionMath = ex.Where(_ => _.ToUpper() == str.ToUpper()).SingleOrDefault();
                if (exceptionMath.IsNotNull())
                    return str.Replace(exceptionMath.ToUpper(), exceptionMath);

                if (ex.Count() == 1)
                    return str.Replace(exceptionRule.ToUpper(), exceptionRule);

                throw new InvalidOperationException("Math CasingExceptions error");

            }

            return str.FisrtCharToLower();
        }

        protected override string ExpressionKeyNames(TableInfo tableInfo, bool camelCasing, EOperation operation = EOperation.Undefined)
        {
            var expression = new List<string>();

            if (operation == EOperation.Front_angular_Service)
            {

                if (tableInfo.Keys.IsAny())
                {
                    foreach (var item in tableInfo.Keys)
                    {
                        var key = camelCasing ? CamelCaseTransform(item) : item;
                        expression.Add(string.Format(" model.{0} != undefined", key));
                    }
                }
                return string.Join(" && ", expression.ToArray());

            }

            if (tableInfo.Keys.IsAny())
            {
                foreach (var item in tableInfo.Keys)
                {
                    var key = camelCasing ? CamelCaseTransform(item) : item;
                    expression.Add(string.Format(" model.{0} != result.data.{0}", key));
                }
            }
            return string.Join(" && ", expression.ToArray());

        }



        #endregion
    }
}

