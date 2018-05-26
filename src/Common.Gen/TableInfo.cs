using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{

    public class HtmlCtrl
    {

        public string HtmlField { get; set; }
        public string HtmlFilter { get; set; }

    }

    public static class FieldsHtml
    {

        public static HtmlCtrl Radio(Dictionary<string, string> dataItem, string name)
        {
            var htmlField = RadioMake(dataItem, name.FisrtCharToLower(), string.Format("vm.model.{0}", name.FisrtCharToLower()), true);
            var htmlFilter = RadioMake(dataItem, name.FisrtCharToLower(), string.Format("vm.modelFilter.{0}", name.FisrtCharToLower()), false);

            return new HtmlCtrl
            {
                HtmlField = htmlField,
                HtmlFilter = htmlFilter
            };

        }

        public static HtmlCtrl Select(Dictionary<string, string> dataItem, string name)
        {
            var htmlField = SelectMake(dataItem, name.FisrtCharToLower(), string.Format("vm.model.{0}", name.FisrtCharToLower()), true);
            var htmlFilter = SelectMake(dataItem, name.FisrtCharToLower(), string.Format("vm.modelFilter.{0}", name.FisrtCharToLower()), false);

            return new HtmlCtrl
            {
                HtmlField = htmlField,
                HtmlFilter = htmlFilter
            };

        }

        private static string RadioMake(Dictionary<string, string> dataItem, string name, string model, bool isFormControl)
        {
            var html = string.Empty;
            foreach (var item in dataItem)
            {
                if (isFormControl)
                    html += "<div class='radio'><label><input type='radio' name='" + name + "' value='" + item.Key.Replace("'", "") + "' [(ngModel)]='" + model + "' formControlName='" + name + "' [checked]=\"" + model + " == '" + item.Key.Replace("'", "") + "'\"/> " + item.Value + "</label></div>";
                else
                    html += "<div class='radio'><label><input type='radio' name='" + name + "' value='" + item.Key.Replace("'", "") + "' [(ngModel)]='" + model + "' [checked]=\"" + model + " == '" + item.Key.Replace("'", "") + "'\"/> " + item.Value + "</label></div>";
            }

            return html;
        }

        private static string SelectMake(Dictionary<string, string> dataItem, string name, string model, bool isFormControl)
        {
            var html = string.Empty;

            var formControl = string.Empty;
            if (isFormControl)
                formControl = "formControlName = '" + name + "'";

            html = "<select class='form-control' name='" + name + "' [(ngModel)]='" + model + "'" + formControl + ">";
            foreach (var item in dataItem)
            {
                html += "<option value='" + item.Key.Replace("'", "") + "'>" + item.Value.Replace("'", "") + "</option>";
            }
            html += "</select>";
            return html;
        }
    }
    public enum TypeCtrl
    {
        Radio,
        Select

    }

    public class RouteConfig
    {
        public string Route { get; set; }
    }

    public class MethodConfig
    {
        public string SignatureControllerTemplate { get; set; }
        public string SignatureAppTemplate { get; set; }
        public string Verb { get; set; }
        public string CallTemplate { get; set; }
        public string ParameterReturn { get; set; }
        public string Route { get; set; }
        public string Dto { get; set; }

    }

    public class FieldConfig
    {

        public FieldConfig init(TypeCtrl type)
        {
            this.TypeCtrl = type;
            switch (this.TypeCtrl)
            {
                case TypeCtrl.Radio:
                    this.HTML = FieldsHtml.Radio(this.DataItem, this.Name);
                    break;
                case TypeCtrl.Select:
                    this.HTML = FieldsHtml.Select(this.DataItem, this.Name);
                    break;
                default:
                    break;
            }

            return this;
        }

        public FieldConfig()
        {
            this.Create = true;
            this.Edit = true;
            this.List = true;
            this.Details = true;
            this.Filter = true;
            this.DataItem = new Dictionary<string, string>();
            this.Upload = false;
            this.Attributes = new List<string>();
        }


        public List<string> Attributes { get; set; }
        public string Name { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool List { get; set; }
        public bool Details { get; set; }
        public bool Filter { get; set; }
        public int Order { get; set; }
        public TypeCtrl TypeCtrl { get; set; }
        public HtmlCtrl HTML { get; set; }
        public Dictionary<string, string> DataItem { get; set; }
        public bool Upload { get; set; }
        public bool SelectSearch { get; set; }
        public bool Tags { get; set; }
        public bool TextEditor { get; set; }
        public bool TextStyle { get; set; }
        public bool Password { get; set; }
        public bool PasswordConfirmation { get; set; }
        public bool Email { get; set; }
        public bool MultiSelectFilter { get; set; }
        public int ColSize { get; set; }
        public bool IgnoreBigLength { get; set; }
        public string TypeCustom { get; set; }

    }

    /// <summary>
    ///  Define o modelo de exibição dos campos nas telas
    /// </summary>
    public enum FieldConfigShow
    {
        /// <summary>
        ///  ShowAll - Mostra todos e o usuário configura os que devem ser escondidos
        /// </summary>
        ShowAll,
        /// <summary>
        ///  HideAll - Esconde todos e o usuário configura os que devem ser mostrados
        /// </summary>
        HideAll
    }

    public class TableInfo
    {
        public TableInfo()
        {
            this.CodeCustomImplemented = false;           
            this.MakeFront = false;
            this.Authorize = true;
            this.FieldsConfigShow = FieldConfigShow.ShowAll;
            this.UsePathStrategyOnDefine = false;
        }

        public bool UsePathStrategyOnDefine { get; set; }


        public string DataItemFieldName { get; set; }

        public FieldConfigShow FieldsConfigShow { get; set; }

        public bool Authorize { get; set; }

        public List<FieldConfig> FieldsConfig { get; set; }

        public List<MethodConfig> MethodConfig { get; set; }
        
        private string _tableName;
        
        public string BoundedContext { get; set; }

        public bool MakeFront { get; set; }


        public string TableName
        {
            get
            {
                return this._tableName.IsNull() ? this.ClassName : this._tableName;
            }
            set
            {
                this._tableName = value;
            }
        }

        public string ClassNameFormated { get; set; }

        public string ClassName { get; set; }

        public string ToolsName { get; set; }

        public bool IsCompositeKey { get { return Keys != null ? Keys.Count() > 1 : false; } }

        public IEnumerable<string> Keys { get; set; }

        public IEnumerable<string> KeysTypes { get; set; }
        
        public bool CodeCustomImplemented { get; set; }

		public IEnumerable<Info> ReletedClass { get; set; }
        

        #region Obsolet

        public string ClassNameRigth { get; set; }
        public string TableHelper { get; set; }
        public string LeftKey { get; set; }
        public string RightKey { get; set; }
        public bool TwoCols { get; set; }


        #endregion

    }
}
