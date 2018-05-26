﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common.Agenda.StudentDataService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StudentData", Namespace="http://schemas.datacontract.org/2004/07/TellMe.Web.Wcf")]
    [System.SerializableAttribute()]
    public partial class StudentData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime BirthDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int[] ClassesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Common.Agenda.StudentDataService.BaseUserData[] ContactsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<bool> HasFoodRestrictionsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime BirthDate {
            get {
                return this.BirthDateField;
            }
            set {
                if ((this.BirthDateField.Equals(value) != true)) {
                    this.BirthDateField = value;
                    this.RaisePropertyChanged("BirthDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] Classes {
            get {
                return this.ClassesField;
            }
            set {
                if ((object.ReferenceEquals(this.ClassesField, value) != true)) {
                    this.ClassesField = value;
                    this.RaisePropertyChanged("Classes");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Common.Agenda.StudentDataService.BaseUserData[] Contacts {
            get {
                return this.ContactsField;
            }
            set {
                if ((object.ReferenceEquals(this.ContactsField, value) != true)) {
                    this.ContactsField = value;
                    this.RaisePropertyChanged("Contacts");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> HasFoodRestrictions {
            get {
                return this.HasFoodRestrictionsField;
            }
            set {
                if ((this.HasFoodRestrictionsField.Equals(value) != true)) {
                    this.HasFoodRestrictionsField = value;
                    this.RaisePropertyChanged("HasFoodRestrictions");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseUserData", Namespace="http://schemas.datacontract.org/2004/07/TellMe.Web.Wcf")]
    [System.SerializableAttribute()]
    public partial class BaseUserData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime BirthDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LoginField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime BirthDate {
            get {
                return this.BirthDateField;
            }
            set {
                if ((this.BirthDateField.Equals(value) != true)) {
                    this.BirthDateField = value;
                    this.RaisePropertyChanged("BirthDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email {
            get {
                return this.EmailField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailField, value) != true)) {
                    this.EmailField = value;
                    this.RaisePropertyChanged("Email");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Login {
            get {
                return this.LoginField;
            }
            set {
                if ((object.ReferenceEquals(this.LoginField, value) != true)) {
                    this.LoginField = value;
                    this.RaisePropertyChanged("Login");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="StudentDataService.IStudentDataService")]
    public interface IStudentDataService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStudentDataService/Insert", ReplyAction="http://tempuri.org/IStudentDataService/InsertResponse")]
        Common.Agenda.StudentDataService.StudentData Insert(Common.Agenda.StudentDataService.StudentData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStudentDataService/Insert", ReplyAction="http://tempuri.org/IStudentDataService/InsertResponse")]
        System.Threading.Tasks.Task<Common.Agenda.StudentDataService.StudentData> InsertAsync(Common.Agenda.StudentDataService.StudentData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStudentDataService/Update", ReplyAction="http://tempuri.org/IStudentDataService/UpdateResponse")]
        Common.Agenda.StudentDataService.StudentData Update(Common.Agenda.StudentDataService.StudentData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStudentDataService/Update", ReplyAction="http://tempuri.org/IStudentDataService/UpdateResponse")]
        System.Threading.Tasks.Task<Common.Agenda.StudentDataService.StudentData> UpdateAsync(Common.Agenda.StudentDataService.StudentData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStudentDataService/Delete", ReplyAction="http://tempuri.org/IStudentDataService/DeleteResponse")]
        void Delete(Common.Agenda.StudentDataService.StudentData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStudentDataService/Delete", ReplyAction="http://tempuri.org/IStudentDataService/DeleteResponse")]
        System.Threading.Tasks.Task DeleteAsync(Common.Agenda.StudentDataService.StudentData data);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IStudentDataServiceChannel : Common.Agenda.StudentDataService.IStudentDataService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StudentDataServiceClient : System.ServiceModel.ClientBase<Common.Agenda.StudentDataService.IStudentDataService>, Common.Agenda.StudentDataService.IStudentDataService {
        
        public StudentDataServiceClient() {
        }
        
        public StudentDataServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StudentDataServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StudentDataServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StudentDataServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Common.Agenda.StudentDataService.StudentData Insert(Common.Agenda.StudentDataService.StudentData data) {
            return base.Channel.Insert(data);
        }
        
        public System.Threading.Tasks.Task<Common.Agenda.StudentDataService.StudentData> InsertAsync(Common.Agenda.StudentDataService.StudentData data) {
            return base.Channel.InsertAsync(data);
        }
        
        public Common.Agenda.StudentDataService.StudentData Update(Common.Agenda.StudentDataService.StudentData data) {
            return base.Channel.Update(data);
        }
        
        public System.Threading.Tasks.Task<Common.Agenda.StudentDataService.StudentData> UpdateAsync(Common.Agenda.StudentDataService.StudentData data) {
            return base.Channel.UpdateAsync(data);
        }
        
        public void Delete(Common.Agenda.StudentDataService.StudentData data) {
            base.Channel.Delete(data);
        }
        
        public System.Threading.Tasks.Task DeleteAsync(Common.Agenda.StudentDataService.StudentData data) {
            return base.Channel.DeleteAsync(data);
        }
    }
}