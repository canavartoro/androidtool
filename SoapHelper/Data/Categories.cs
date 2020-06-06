using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SoapHelper.Data
{
    [Persistent("CATEGORY")]
    [OptimisticLocking(false), DeferredDeletion(false)]
    [DebuggerDisplay("Name = {Name}")]
    public class Categories : XPObject
    {
        public Categories() { }
        public Categories(Session session) : base(session) { }

        [Size(300)]
        [Persistent("NAME")]
        public string Name { get; set; }

        [XmlIgnore(), Association(@"Categories-WebServices"), NoForeignKey]
        public XPCollection<WebServices> Projects
        {
            get { return GetCollection<WebServices>(@"Projects"); }
        }
    }
}
