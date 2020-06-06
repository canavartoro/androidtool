using DevExpress.Xpo;
using SoapHelper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapHelper.Wsdl
{
    public class JavaTypeConverter
    {
        public static string ToJavaType(string soapType)
        {
            //one check for acceptable types
            //string[] acceptables = { "boolean", "int", "float", "double", "String" };

            //foreach (string val in acceptables)
            //{
            //    //if accpetable type then return
            //    if (soapType.Equals(val)) return soapType;
            //}

            //check for string
            if (soapType.Equals("String")) return "String";

            if (soapType.Equals("dateTime")) return "Date";

            if (soapType.Equals("boolean")) return "Boolean";

            if (soapType.Equals("double")) return "Double";

            if (soapType.Equals("float")) return "Float";

            if (soapType.Equals("string")) return "String";

            if (soapType.Equals("int")) return "Integer";

            //check for decimal and convert to float
            if (soapType.Equals("decimal")) return "BigDecimal";//float

            if (soapType.Equals("base64Binary")) return "byte[]";

            if (soapType.Equals("char")) return "char";

            if (soapType.Equals("")) return "anyType";

            return string.Empty;
        }

        public static string ToElementJavaType(string soapType)
        {
            //check for string
            if (soapType.Equals("String")) return "String";
            if (soapType.Equals("string")) return "String";

            if (soapType.Equals("dateTime")) return "Date";
            if (soapType.Equals("Date")) return "Date";

            if (soapType.Equals("boolean")) return "Boolean";
            if (soapType.Equals("Boolean")) return "Boolean";

            if (soapType.Equals("double")) return "Double";
            if (soapType.Equals("Double")) return "Double";

            if (soapType.Equals("float")) return "Float";
            if (soapType.Equals("Float")) return "Float";

            if (soapType.Equals("int")) return "Integer";
            if (soapType.Equals("Integer")) return "Integer";

            //check for decimal and convert to float
            if (soapType.Equals("decimal")) return "BigDecimal";//float
            if (soapType.Equals("Decimal")) return "BigDecimal";//float

            if (soapType.Equals("char")) return "char";//float

            if (soapType == "") return "Object";

            return soapType;
        }

        public static bool IsComplexType(string soapType)
        {
            //check for string
            if (soapType.Equals("String")) return false;
            if (soapType.Equals("string")) return false;

            if (soapType.Equals("dateTime")) return false;
            if (soapType.Equals("Date")) return false;

            if (soapType.Equals("boolean")) return false;
            if (soapType.Equals("Boolean")) return false;

            if (soapType.Equals("double")) return false;
            if (soapType.Equals("Double")) return false;

            if (soapType.Equals("float")) return false;
            if (soapType.Equals("Float")) return false;

            if (soapType.Equals("int")) return false;
            if (soapType.Equals("Integer")) return false;

            //check for decimal and convert to float
            if (soapType.Equals("decimal")) return false;//float
            if (soapType.Equals("Decimal")) return false;//float

            if (soapType.Equals("char")) return false;//float

            if (soapType == "") return false;

            return true;
        }

        public static string InitialToJavaType(SoapClassProperties prop)
        {
            if (prop.IsEnum)
            {
                SoapClasses cls = (from q in new XPQuery<SoapClasses>(XpoDefault.Session)
                                   where q.Name == prop.PropertyClassType
                                   select q).FirstOrDefault();
                if (cls != null)
                {
                    if (cls.Properties != null && cls.Properties.Count > 0)
                    {
                        return string.Format("\"{0}\"", cls.Properties[0].Name);
                    }
                    else
                        return "";
                }
            }

            //check for string
            if (prop.PropertyClassType == "String") return "\"\"";
            if (prop.PropertyClassType == "string") return "\"\"";
            if (prop.PropertyClassType == "char") return "' '";

            if (prop.PropertyClassType == "dateTime") return "new Date(1900, 1, 1)";
            if (prop.PropertyClassType == "Date") return "new Date(1900, 1, 1)";

            if (prop.PropertyClassType == "boolean") return "false";
            if (prop.PropertyClassType == "Boolean") return "false";

            if (prop.PropertyClassType == "double") return "new Double(0)";//Double
            if (prop.PropertyClassType == "Double") return "new Double(0)";

            if (prop.PropertyClassType == "float") return "new Float(0)";
            if (prop.PropertyClassType == "Float") return "new Float(0)";

            if (prop.PropertyClassType == "int") return "0";
            if (prop.PropertyClassType == "Integer") return "0";

            if (prop.PropertyClassType == "byte[]") return "null";

            //check for decimal and convert to float
            //if (prop.getPropertyClassType().Equals("decimal")) return "new Float(0)";//BigDecimal, new BigDecimal(0)
            if (prop.PropertyClassType == "BigDecimal") return "new BigDecimal(0)";//BigDecimal, new BigDecimal(0)

            if (prop.IsComplexType && prop.IsEnum == false) return string.Format("new {0}()", prop.PropertyClassType);

            if (prop.PropertyClassType == "Object") return "null";

            return "\"\"";
        }

        public static string ConvertorForJavaType(SoapClassProperties prop)
        {
            if (prop.PropertyClassType.ToLower() == "boolean")
            {
                return "Boolean.getBoolean(value.toString())";
            }
            else if (prop.PropertyClassType == "int" || prop.PropertyClassType == "Integer")
            {
                return "Integer.parseInt(value.toString())";
            }
            else if (prop.PropertyClassType.ToLower() == "string")
            {
                return "value.toString()";
            }
            else if (prop.PropertyClassType == "dateTime" || prop.PropertyClassType == "Date")
            {
                return "DateUtil.getDate(value.toString())";
            }
            else if (prop.PropertyClassType.ToLower() == "double")
            {
                return "Double.parseDouble(value.toString())";
            }
            else if (prop.PropertyClassType.ToLower() == "float")
            {
                return "Float.parseFloat(value.toString())";
            }
            else if (prop.PropertyClassType == "BigDecimal")
            {
                return "new BigDecimal(value.toString())";
            }
            else
            {
                if (prop.IsEnum)
                {
                    return "value.toString()";
                }
                else
                {
                    return string.Format("({0})value", prop.PropertyClassType);
                }
            }
            //return "value";
        }

        public static string ClassTypeRetrievalString(SoapClassProperties property)
        {
            if (property.IsEnum)
            {
                return "PropertyInfo.STRING_CLASS";
            }
            else
            {
                return ClassTypeRetrievalString(property.PropertyClassType);
            }
        }

        public static string ClassTypeRetrievalString(string propertyName)
        {
            if (propertyName.ToLower() == "boolean")
            {
                return "PropertyInfo.BOOLEAN_CLASS";
            }
            else if (propertyName == "int" || propertyName == "Integer")
            {
                return "PropertyInfo.INTEGER_CLASS";
            }
            else if (propertyName.ToLower() == "string")
            {
                return "PropertyInfo.STRING_CLASS";
            }
            else if (propertyName == "dateTime" || propertyName == "Date")
            {
                return "Date.class.getClass()";
            }
            else if (propertyName.ToLower() == "double")
            {
                return "Double.class.getClass()";
            }
            else if (propertyName.ToLower() == "decimal" || propertyName == "BigDecimal")
            {
                return "BigDecimal.class.getClass()";
            }
            else if (propertyName.ToLower() == "float")
            {
                return "Float.class.getClass()";
            }
            else if (propertyName.ToLower() == "byte[]")
            {
                return "new byte[0].getClass()";
            }
            else if (propertyName.ToLower() == "char")
            {
                return "PropertyInfo.INTEGER_CLASS";
            }
            else
            {
                return string.Format("{0}.class", propertyName);
            }
        }

        public static string loadSoapObjectString(string elementType)
        {
            StringBuilder loadstring = new StringBuilder();
            loadstring.AppendLine("    public void loadSoapObject(SoapObject property) {");
            loadstring.AppendLine("        if (property == null) return;");
            loadstring.AppendLine("        int itemCount = property.getPropertyCount();");
            loadstring.AppendLine("        if (itemCount > 0) {");
            loadstring.AppendLine("            for (int loop = 0; loop < itemCount; loop++) {");
            loadstring.AppendLine("                if (property.getProperty(loop) instanceof SoapPrimitive) {");
            loadstring.AppendLine("                    SoapPrimitive pi = (SoapPrimitive) property.getProperty(loop);");

            if (elementType.ToLower() == "boolean")
            {
                loadstring.AppendLine("                    String item = pi.toString();");
                loadstring.AppendLine("                    this.add(item);");
            }
            else if (elementType == "int" || elementType == "Integer")
            {
                loadstring.AppendLine("                    Integer item = Integer.valueOf(pi.toString());");
                loadstring.AppendLine("                    this.add(item);");
            }
            else if (elementType.ToLower() == "string")
            {
                loadstring.AppendLine("                    String item = pi.toString();");
                loadstring.AppendLine("                    this.add(item);");
            }
            else if (elementType == "dateTime" || elementType == "Date")
            {
                loadstring.AppendLine("                    Date item = DateUtil.getDate(pi.toString());");
                loadstring.AppendLine("                    this.add(item);");
            }
            else if (elementType.ToLower() == "double")
            {
                loadstring.AppendLine("                    Double item = Double.parseDouble(pi.toString());");
                loadstring.AppendLine("                    this.add(item);");
            }
            else if (elementType.ToLower() == "decimal" || elementType == "BigDecimal")
            {
                loadstring.AppendLine("                    BigDecimal item = BigDecimal.valueOf(Double.parseDouble(pi.toString()));");
                loadstring.AppendLine("                    this.add(item);");
            }
            else if (elementType.ToLower() == "float")
            {
                loadstring.AppendLine("                    Float item = Float.parseFloat(pi.toString());");
                loadstring.AppendLine("                    this.add(item);");
            }
            else if (elementType.ToLower() == "byte[]")
            {
                loadstring.AppendLine("                    byte[] item = Base64.decode(pi.toString(),0);");
                loadstring.AppendLine("                    this.add(item);");
            }
            else if (elementType.ToLower() == "char")
            {
                loadstring.AppendLine("                    int intchar = Integer.valueOf(pi.toString());");
                loadstring.AppendLine("                    this.add((char) intchar);");
            }
            else
            {
                loadstring.AppendLine("                if (property.getProperty(loop) != null) {");
                loadstring.AppendFormat("                    this.add(({0}) property.getProperty(loop));", elementType).AppendLine();
                loadstring.AppendLine("                }");
            }
            loadstring.AppendLine("                }").AppendLine("            }").AppendLine("        }").AppendLine("    }");
            return loadstring.ToString();
        }

        public static string ArrayElementTypeDesc(string elementType)
        {
            if (elementType.Equals("") || elementType.Equals("Object")) return "anyType";
            return elementType;
        }


    }
}
