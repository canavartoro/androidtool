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
    [Persistent("SOAP_CLASS")]
    [OptimisticLocking(false), DeferredDeletion(false)]
    [DebuggerDisplay("Name = {Name}")]
    public class SoapClasses : XPObject
    {
        public SoapClasses() { }
        public SoapClasses(Session session) : base(session) { }

        WebServices fService;
        [XmlIgnore()]
        [Browsable(false)]
        [Persistent("SERVICE")]
        [Association(@"WebServices-SoapClasses")]
        public WebServices Service
        {
            get { return fService; }
            set { SetPropertyValue<WebServices>("Service", ref fService, value); }
        }

        [Size(300)]
        [Persistent("NAME")]
        public string Name { get; set; }

        [Persistent("CLASS_TYPE")]
        public ClassType Type { get; set; }

        [Persistent("IS_ARRAY")]
        public bool IsArray { get; set; }

        [Persistent("IS_ENUM")]
        public bool IsEnum { get; set; }

        [Size(300)]
        [Persistent("ELEMENT_TYPE")]
        public string ElementType { get; set; }

        [Size(300)]
        [Persistent("SUPER_CLASS_TYPE")]
        public string SuperClassType { get; set; }

        [Persistent("IS_OUTPUT")]
        public bool Output { get; set; }

        [XmlIgnore(), Association(@"SoapClasses-SoapClassProperties")]
        public XPCollection<SoapClassProperties> Properties
        {
            get { return GetCollection<SoapClassProperties>(@"Properties"); }
        }

        [XmlIgnore()]
        [NonPersistent]
        [Browsable(false)]
        public string RegisterText
        {
            get
            {
                return string.Format("\t\t\tenvelope.addMapping(NAMESPACE, \"{0}\", {0}.class);", Name);
                //return string.Format("\t\t\tenvelope.addMapping(NAMESPACE, \"{0}\", new {0}().getClass());", Name);
            }
        }

        //[XmlIgnore()]
        //[NonPersistent]
        //[Browsable(false)]
        //public string PropertyText
        //{
        //    get
        //    {
        //        StringBuilder str = new StringBuilder();
        //        for (int i = 0; i < Properties.Count; i++) str.AppendFormat("{0}({1}){2}", Properties[i].Name, Properties[i].PropertyClassType, i > 0 ? "," : "");
        //        return str.ToString();
        //    }
        //}

    }


}
