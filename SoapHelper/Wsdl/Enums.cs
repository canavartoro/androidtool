using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapHelper.Wsdl
{
    public enum ClassType { Parameter, Response, ComplexType, Unknown };

    public enum MarshalType { MarshalDate, MarshalDecimal, MarshalDouble, MarshalFloat }
}
