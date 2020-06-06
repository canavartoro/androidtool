using SoapHelper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapHelper.Wsdl
{
    public class JavaClassCreator
    {
        public JavaClassCreator()
        {

        }

        public string Create(SoapClasses soapClass, WebServices webService)
        {
            StringBuilder strgetter = new StringBuilder();
            StringBuilder str = new StringBuilder();
            str.AppendFormat("package {0};", webService.PackageName).AppendLine().AppendLine();
            str.AppendLine("import java.util.Date;").AppendLine("import java.math.BigDecimal;").AppendLine();
            str.AppendLine(string.Concat("public class ", soapClass.Name, " {")).AppendLine();

            str.AppendFormat("    public {0}(){1}", soapClass.Name, "{}").AppendLine();

            for (int i = 0; i < soapClass.Properties.Count; i++)
            {
                SoapClassProperties property = soapClass.Properties[i];
                if (property.IsEnum)
                {
                    str.AppendFormat("    private String _{0} = {1}; //Enum {2}", property.Name, JavaTypeConverter.InitialToJavaType(property), property.PropertyClassType).AppendLine();

                    strgetter.AppendFormat("     public String get{0}() {1}", property.Name, "{").AppendLine();
                    strgetter.AppendFormat("        return _{0};", property.Name).AppendLine();
                    strgetter.AppendLine("    }").AppendLine();

                    strgetter.AppendFormat("    public void set{0}(String improvable) {1}", property.Name, "{").AppendLine();
                    strgetter.AppendFormat("        this._{0} = improvable;", property.Name).AppendLine();
                    strgetter.AppendLine("    }").AppendLine();
                }
                else if (property.IsArray)
                {
                    str.AppendFormat("    private {0} _{1} = {2}; //array\n", property.PropertyClassType, property.Name, JavaTypeConverter.InitialToJavaType(property)).AppendLine();

                    strgetter.AppendFormat("     public String get{0}() {1}", property.Name, "{").AppendLine();
                    strgetter.AppendFormat("        return _{0};", property.Name).AppendLine();
                    strgetter.AppendLine("    }").AppendLine();

                    strgetter.AppendFormat("    public void set{0}({1} improvable) {2}", property.Name, property.PropertyClassType, "{").AppendLine();
                    strgetter.AppendFormat("        this._{0} = improvable;", property.Name).AppendLine();
                    strgetter.AppendLine("    }").AppendLine();
                }
                else
                {
                    str.AppendFormat("    private {0} _{1} = {2};\n", property.PropertyClassType, property.Name, JavaTypeConverter.InitialToJavaType(property)).AppendLine();

                    strgetter.AppendFormat("     public {0} get{1}() {2}", property.PropertyClassType, property.Name, "{").AppendLine();
                    strgetter.AppendFormat("        return _{0};", property.Name).AppendLine();
                    strgetter.AppendLine("    }").AppendLine();

                    strgetter.AppendFormat("    public void set{0}({1} improvable) {2}", property.Name, property.PropertyClassType, "{").AppendLine();
                    strgetter.AppendFormat("        this._{0} = improvable;", property.Name).AppendLine();
                    strgetter.AppendLine("    }").AppendLine();
                }


            }

            str.Append(strgetter.ToString());


            str.AppendLine("}");
            return str.ToString();
        }
    }
}
