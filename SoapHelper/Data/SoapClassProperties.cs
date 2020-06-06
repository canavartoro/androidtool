using DevExpress.Xpo;
using SoapHelper.Wsdl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SoapHelper.Data
{
    [Persistent("CLASS_PROPERTIES")]
    [OptimisticLocking(false), DeferredDeletion(false)]
    [DebuggerDisplay("Name = {Name}, Type = {Type}, PropertyClassType = {PropertyClassType}, IsArray = {IsArray}, IsEnum = {IsEnum}, IsComplexType = {IsComplexType}")]
    public class SoapClassProperties : XPObject
    {
        public SoapClassProperties() { }
        public SoapClassProperties(Session session) : base(session) { }

        SoapClasses fSoapClasses;
        [XmlIgnore()]
        [Browsable(false)]
        [Persistent("SOAP_CLASS")]
        [Association(@"SoapClasses-SoapClassProperties")]
        public SoapClasses SoapClasses
        {
            get { return fSoapClasses; }
            set { SetPropertyValue<SoapClasses>("SoapClasses", ref fSoapClasses, value); }
        }

        [Size(300)]
        [Persistent("NAME")]
        public string Name { get; set; }

        [Persistent("CLASS_TYPE")]
        public string Type { get; set; }

        private string fPropertyClassType;
        [Persistent("PROPERTY_CLASS_TYPE")]
        public string PropertyClassType
        {
            get { return fPropertyClassType; }
            set
            {
                string type = JavaTypeConverter.ToJavaType(value);
                if (!string.IsNullOrEmpty(type))
                {
                    SetPropertyValue<string>("PropertyClassType", ref fPropertyClassType, type);
                    IsComplexType = false;
                }
                else
                {
                    SetPropertyValue<string>("PropertyClassType", ref fPropertyClassType, value);
                    IsComplexType = true;
                }
                
            }
        }

        [Persistent("IS_ARRAY")]
        public bool IsArray { get; set; }

        [Persistent("IS_ENUM")]
        public bool IsEnum { get; set; }

        [Persistent("IS_COMPLEX_TYPE")]
        public bool IsComplexType { get; set; }


    }
}
