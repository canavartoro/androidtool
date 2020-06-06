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
    [Persistent("WEB_SERV")]
    [OptimisticLocking(false), DeferredDeletion(false)]
    [DebuggerDisplay("Name = {Name}, Url = {Url}, NameSpace = {NameSpace}")]
    public class WebServices : XPObject
    {
        public WebServices() { }
        public WebServices(Session session) : base(session) { }

        Categories fCategory;
        [XmlIgnore()]
        [Browsable(false)]
        [Persistent("CATEGORY")]
        [Association(@"Categories-WebServices")]
        public Categories Category
        {
            get { return fCategory; }
            set { SetPropertyValue<Categories>("Category", ref fCategory, value); }
        }

        [XmlIgnore()]
        [PersistentAlias("Iif(Category is null, '', Category.Name)")]
        public string CategoryName
        {
            get
            {
                return Convert.ToString(EvaluateAlias("CategoryName"));
            }
        }

        [Size(300)]
        [Persistent("NAME")]
        public string Name { get; set; }


        [Size(300)]
        [Persistent("SERVICE_NAME")]
        public string ServiceName { get; set; }

        [Persistent("URL")]
        [Size(SizeAttribute.Unlimited)]
        public string Url { get; set; }

        [Persistent("PATH")]
        [Size(SizeAttribute.Unlimited)]
        public string Path { get; set; }

        [Persistent("NAME_SPACE")]
        [Size(SizeAttribute.Unlimited)]
        public string NameSpace { get; set; }

        [Persistent("PACKAGE_NAME")]
        [Size(SizeAttribute.Unlimited)]
        public string PackageName { get; set; }

        [Persistent("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        [XmlIgnore(), Association(@"WebServices-WebFunctions")]
        public XPCollection<WebFunctions> Functions
        {
            get { return GetCollection<WebFunctions>(@"Functions"); }
        }

        [XmlIgnore(), Association(@"WebServices-SoapClasses")]
        public XPCollection<SoapClasses> SoapClasses
        {
            get { return GetCollection<SoapClasses>(@"SoapClasses"); }
        }

    }

 

}
