using DevExpress.Xpo;
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
    [Persistent("WEB_FUNC")]
    [OptimisticLocking(false), DeferredDeletion(false)]
    [DebuggerDisplay("Name = {Name}, InputType = {InputType}, OutputType = {OutputType}")]
    public class WebFunctions : XPObject
    {
        public WebFunctions() { }
        public WebFunctions(Session session) : base(session) { }

        WebServices fService;
        [XmlIgnore()]
        [Browsable(false)]
        [Persistent("SERVICE")]
        [Association(@"WebServices-WebFunctions"), NoForeignKey]
        public WebServices Service
        {
            get { return fService; }
            set { SetPropertyValue<WebServices>("Service", ref fService, value); }
        }

        [XmlIgnore()]
        [PersistentAlias("Iif(Service is null, '', Service.Name)")]
        public string ServiceName
        {
            get
            {
                return Convert.ToString(EvaluateAlias("ServiceName"));
            }
        }

        [Size(300)]
        [Persistent("NAME")]
        public string Name { get; set; }

        [Persistent("SOAP_ACTION")]
        [Size(SizeAttribute.Unlimited)]
        public string SoapAction { get; set; }

        [Persistent("INPUT_TYPE")]
        [Size(SizeAttribute.Unlimited)]
        public string InputType { get; set; }

        [Persistent("OUTPUT_TYPE")]
        [Size(SizeAttribute.Unlimited)]
        public string OutputType { get; set; }

        [Persistent("IS_OUTPUT")]
        public bool Output { get; set; }

        [Persistent("REGISTER_CLASSES")]
        [Size(SizeAttribute.Unlimited)]
        public string RegisterClasses { get; set; }

    }

 
}
