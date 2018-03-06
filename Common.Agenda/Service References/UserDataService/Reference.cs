﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common.Agenda.UserDataService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseUserData", Namespace="http://schemas.datacontract.org/2004/07/TellMe.Web.Wcf")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Common.Agenda.UserDataService.SchoolUserData))]
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SchoolUserData", Namespace="http://schemas.datacontract.org/2004/07/TellMe.Web.Wcf")]
    [System.SerializableAttribute()]
    public partial class SchoolUserData : Common.Agenda.UserDataService.BaseUserData {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int[] ClassesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Common.Agenda.UserDataService.UserRoleEnaumData UserRoleField;
        
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
        public Common.Agenda.UserDataService.UserRoleEnaumData UserRole {
            get {
                return this.UserRoleField;
            }
            set {
                if ((this.UserRoleField.Equals(value) != true)) {
                    this.UserRoleField = value;
                    this.RaisePropertyChanged("UserRole");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserRoleEnaumData", Namespace="http://schemas.datacontract.org/2004/07/TellMe.Web.Wcf")]
    public enum UserRoleEnaumData : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Admin = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Coordinator = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Teacher = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UserDataService.IUserDataService")]
    public interface IUserDataService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserDataService/Insert", ReplyAction="http://tempuri.org/IUserDataService/InsertResponse")]
        Common.Agenda.UserDataService.SchoolUserData Insert(Common.Agenda.UserDataService.SchoolUserData classData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserDataService/Insert", ReplyAction="http://tempuri.org/IUserDataService/InsertResponse")]
        System.Threading.Tasks.Task<Common.Agenda.UserDataService.SchoolUserData> InsertAsync(Common.Agenda.UserDataService.SchoolUserData classData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserDataService/Update", ReplyAction="http://tempuri.org/IUserDataService/UpdateResponse")]
        Common.Agenda.UserDataService.SchoolUserData Update(Common.Agenda.UserDataService.SchoolUserData classData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserDataService/Update", ReplyAction="http://tempuri.org/IUserDataService/UpdateResponse")]
        System.Threading.Tasks.Task<Common.Agenda.UserDataService.SchoolUserData> UpdateAsync(Common.Agenda.UserDataService.SchoolUserData classData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserDataService/Delete", ReplyAction="http://tempuri.org/IUserDataService/DeleteResponse")]
        void Delete(Common.Agenda.UserDataService.SchoolUserData classData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserDataService/Delete", ReplyAction="http://tempuri.org/IUserDataService/DeleteResponse")]
        System.Threading.Tasks.Task DeleteAsync(Common.Agenda.UserDataService.SchoolUserData classData);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUserDataServiceChannel : Common.Agenda.UserDataService.IUserDataService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserDataServiceClient : System.ServiceModel.ClientBase<Common.Agenda.UserDataService.IUserDataService>, Common.Agenda.UserDataService.IUserDataService {
        
        public UserDataServiceClient() {
        }
        
        public UserDataServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UserDataServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserDataServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserDataServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Common.Agenda.UserDataService.SchoolUserData Insert(Common.Agenda.UserDataService.SchoolUserData classData) {
            return base.Channel.Insert(classData);
        }
        
        public System.Threading.Tasks.Task<Common.Agenda.UserDataService.SchoolUserData> InsertAsync(Common.Agenda.UserDataService.SchoolUserData classData) {
            return base.Channel.InsertAsync(classData);
        }
        
        public Common.Agenda.UserDataService.SchoolUserData Update(Common.Agenda.UserDataService.SchoolUserData classData) {
            return base.Channel.Update(classData);
        }
        
        public System.Threading.Tasks.Task<Common.Agenda.UserDataService.SchoolUserData> UpdateAsync(Common.Agenda.UserDataService.SchoolUserData classData) {
            return base.Channel.UpdateAsync(classData);
        }
        
        public void Delete(Common.Agenda.UserDataService.SchoolUserData classData) {
            base.Channel.Delete(classData);
        }
        
        public System.Threading.Tasks.Task DeleteAsync(Common.Agenda.UserDataService.SchoolUserData classData) {
            return base.Channel.DeleteAsync(classData);
        }
    }
}