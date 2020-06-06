using SoapHelper.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapHelper.Wsdl
{
    public class SoapCreator : IDisposable
    {
        public WebServices WebService { get; set; }
        public string ProjectFolder { get; set; }

        private List<string> complexClasses = new List<string>();
        private static List<string> keysRegister = new List<string>();
        private static StringBuilder registerStrings = new StringBuilder();

        public SoapCreator()
        {

        }

        public SoapCreator(WebServices webserv)
        {
            this.WebService = webserv;
        }

        public void Create(WebServices webserv)
        {
            this.WebService = webserv;
            this.Create();
        }

        public void Create()
        {
            if (WebService == null) throw new ArgumentException("WebService");

            ProjectFolder = string.Concat(WebService.Path, "\\", WebService.PackageName.Replace(".", "\\"), "\\");

            if (!Directory.Exists(WebService.Path))
                Directory.CreateDirectory(WebService.Path);

            if (!Directory.Exists(ProjectFolder))
                Directory.CreateDirectory(ProjectFolder);

            Utility.WriteTrace(ProjectFolder);

            CreateUtils();
            CreateMarshals();
            CreateBaseClass();
            CreateSoapClass();
        }

        private void CreateClasses()
        {
            for (int loop = 0; loop < WebService.SoapClasses.Count; loop++)
            {
                SoapClasses soapClass = WebService.SoapClasses[loop];
                if (soapClass.IsEnum)
                {
                    CreateEnum(soapClass);
                }
                else if (soapClass.IsArray)
                {
                    CreateArrayClass(soapClass);
                }
                else
                {
                    CreateComplexClass(soapClass);
                }
            }
        }

        private void CreateComplexClass(SoapClasses complexClass)
        {
            if (complexClasses.Contains(complexClass.Name)) return;

            if (complexClass.IsEnum)
            {
                CreateEnum(complexClass);
                return;
            }
            else if (complexClass.IsArray)
            {
                CreateArrayClass(complexClass);
                return;
            }

            StringBuilder propText = new StringBuilder();
            StringBuilder propinfstr = new StringBuilder();
            StringBuilder setpropstr = new StringBuilder();
            StringBuilder getpropstr = new StringBuilder();
            string complexstr = FileHelper.GetManifestResourceStream("SoapComplexTypeClassTemplate");
            complexstr = complexstr.Replace("%%DATE%%", DateTime.Now.ToString()).Replace("%%PACKAGENAME%%", WebService.PackageName);
            complexstr = complexstr.Replace("%%CLASSNAME%%", complexClass.Name).Replace("%%PROPCOUNT%%", complexClass.Properties.Count.ToString());

            for (int i = 0; i < complexClass.Properties.Count; i++)
            {
                SoapClassProperties property = complexClass.Properties[i];

                propinfstr.AppendFormat("           case {0}:", i).AppendLine();
                propinfstr.AppendFormat("                info.name = \"{0}\";", property.Name).AppendLine();
                propinfstr.AppendFormat("                info.type = {0};", JavaTypeConverter.ClassTypeRetrievalString(property)).AppendLine();
                propinfstr.AppendLine("                break;");

                getpropstr.AppendFormat("           case {0}:", i).AppendLine();
                getpropstr.AppendFormat("                return {0};", property.Name).AppendLine();

                setpropstr.AppendFormat("           case {0}:", i).AppendLine();
                setpropstr.AppendLine("                if (value.toString().equalsIgnoreCase(\"anyType{}\")) {");
                setpropstr.AppendFormat("                    {0} = {1};", property.Name, JavaTypeConverter.InitialToJavaType(property)).AppendLine("                }");
                setpropstr.AppendLine("                else {");

                if (property.IsEnum)
                {
                    propText.AppendFormat("     public String {0} = {1}; //Enum {2}", property.Name, JavaTypeConverter.InitialToJavaType(property), property.PropertyClassType);
                    setpropstr.AppendFormat("                    {0} = value.toString();", property.Name).AppendLine();
                }
                else if (property.IsArray)
                {
                    propText.AppendFormat("     public {0} {1} = {2}; //array\n", property.PropertyClassType, property.Name, JavaTypeConverter.InitialToJavaType(property));
                    setpropstr.AppendFormat("                  {0} = {1};", property.Name, JavaTypeConverter.InitialToJavaType(property)).AppendLine();
                    setpropstr.AppendFormat("                  SoapObject prp = (SoapObject)value; ").AppendLine();
                    setpropstr.AppendFormat("                  {0}.loadSoapObject(prp); ", property.Name).AppendLine();
                }
                else
                {
                    propText.AppendFormat("     public {0} {1} = {2};\n", property.PropertyClassType, property.Name, JavaTypeConverter.InitialToJavaType(property));
                    if (property.IsComplexType)
                    {
                        setpropstr.AppendFormat("	                {0} = new {1}(); ", property.Name, property.PropertyClassType).AppendLine();
                        setpropstr.AppendFormat("                    {0}.loadSoapObject((SoapObject) value);", property.Name).AppendLine();
                    }
                    else
                    {
                        setpropstr.AppendFormat("	                {0} = {1}; ", property.Name, JavaTypeConverter.ConvertorForJavaType(property)).AppendLine();
                    }
                }

                setpropstr.AppendLine("                }").AppendLine("                break;");

                if (property.IsComplexType)
                {
                    SoapClasses _complexClass = WebService.SoapClasses.Where(x => x.Name == property.PropertyClassType).FirstOrDefault();
                    if (_complexClass != null)
                    {
                        CreateComplexClass(_complexClass);
                    }
                }

            }

            complexstr = complexstr.Replace("%%PROPERTIES%%", propText.ToString()).Replace("%%GETPROPERTY%%", getpropstr.ToString());
            complexstr = complexstr.Replace("%%SETPROP%%", setpropstr.ToString()).Replace("%%GETPROPINFO%%", propinfstr.ToString());


            using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, complexClass.Name, ".java")))
            {
                file.WriteLine(complexstr.ToString());
                file.Close();
            }
            Utility.WriteTrace(string.Format("{0}.java Oluşturuldu", complexClass.Name));
            complexClasses.Add(complexClass.Name);

            complexClass.Output = true;
            complexClass.Save();
        }

        private void CreateSoapClass()
        {
            string soapstring = FileHelper.GetManifestResourceStream("ClassTemplate");
            string importstring = FileHelper.GetManifestResourceStream("ServiceImportsTemplate");
            soapstring = soapstring.Replace("%%PACKAGENAME%%", this.WebService.PackageName).Replace("%%DATE%%", DateTime.Now.ToString());
            soapstring = soapstring.Replace("%%CLASSNAME%%", WebService.ServiceName + "Soap").Replace("%%NAMESPACE%%", WebService.NameSpace);
            soapstring = soapstring.Replace("%%IMPORTS%%", importstring).Replace("%%METHODS%%", CreateMethods()).Replace("%%ADDRESS%%", WebService.Url.Replace("localhost", "10.0.2.2"));

            using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, WebService.ServiceName, "Soap.java")))
            {
                file.Write(soapstring);
                file.Close();
            }
            Utility.WriteTrace(string.Format("{0}Soap.java Oluşturuldu", WebService.ServiceName));
        }

        private void CreateMarshals()
        {
            try
            {
                string strMarshal = FileHelper.GetManifestResourceStream("MarshalDecimal");
                using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, "MarshalDecimal.java")))
                {
                    file.WriteLine(strMarshal.Replace("%%PACKAGENAME%%", this.WebService.PackageName).Replace("%%DATE%%", DateTime.Now.ToString()));
                    file.Close();
                }
                Utility.WriteTrace(string.Format("MarshalDecimal.java Oluşturuldu"));

                strMarshal = FileHelper.GetManifestResourceStream("MarshalDouble");
                using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, "MarshalDouble.java")))
                {
                    file.WriteLine(strMarshal.Replace("%%PACKAGENAME%%", this.WebService.PackageName).Replace("%%DATE%%", DateTime.Now.ToString()));
                    file.Close();
                }
                Utility.WriteTrace(string.Format("MarshalDouble.java Oluşturuldu"));

                strMarshal = FileHelper.GetManifestResourceStream("MarshalFloat");
                using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, "MarshalFloat.java")))
                {
                    file.WriteLine(strMarshal.Replace("%%PACKAGENAME%%", this.WebService.PackageName).Replace("%%DATE%%", DateTime.Now.ToString()));
                    file.Close();
                }
                Utility.WriteTrace(string.Format("MarshalFloat.java Oluşturuldu"));

                strMarshal = FileHelper.GetManifestResourceStream("MarshalDate");
                using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, "MarshalDate.java")))
                {
                    file.WriteLine(strMarshal.Replace("%%PACKAGENAME%%", this.WebService.PackageName).Replace("%%DATE%%", DateTime.Now.ToString()));
                    file.Close();
                }
                Utility.WriteTrace(string.Format("MarshalDate.java Oluşturuldu"));

            }
            catch (Exception exc)
            {
                Utility.Hata(exc);
            }
        }

        private void CreateUtils()
        {
            try
            {
                string dateutil = FileHelper.GetManifestResourceStream("DateUtil");
                if (!string.IsNullOrEmpty(dateutil))
                {
                    using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, "DateUtil.java")))
                    {
                        file.WriteLine(dateutil.Replace("%%PACKAGENAME%%", WebService.PackageName).Replace("%%DATE%%", DateTime.Now.ToString()));
                        file.Close();
                    }
                    Utility.WriteTrace(string.Format("DateUtil.java Oluşturuldu"));
                }
            }
            catch (Exception exc)
            {
                Utility.Hata(exc);
            }
        }

        private string CreateMethods()
        {
            string methodstrorj = FileHelper.GetManifestResourceStream("MethodTemplate");
            StringBuilder strmethods = new StringBuilder();
            var functions = WebService.Functions.Where(x => x.Output == true).ToList();
            for (int loop = 0; loop < functions.Count; loop++)
            {
                keysRegister = new List<string>();
                registerStrings = new StringBuilder();

                var webfunc = functions[loop];
                string methodstr = methodstrorj.Replace("%%METHODNAME%%", webfunc.Name).Replace("%%OUTPUT%%", webfunc.OutputType).Replace("%%INPUT%%", webfunc.InputType);

                SoapClasses paramClass = WebService.SoapClasses.Where(x => x.Name == webfunc.InputType).FirstOrDefault();

                #region InputClass
                if (paramClass != null)
                {
                    StringBuilder propText = new StringBuilder();
                    StringBuilder propinfstr = new StringBuilder();
                    StringBuilder setpropstr = new StringBuilder();
                    StringBuilder getpropstr = new StringBuilder();
                    StringBuilder soappropstr = new StringBuilder();
                    string paramstr = FileHelper.GetManifestResourceStream("ParameterClassTemplate");
                    paramstr = paramstr.Replace("%%DATE%%", DateTime.Now.ToString()).Replace("%%PACKAGENAME%%", WebService.PackageName);
                    paramstr = paramstr.Replace("%%CLASSNAME%%", paramClass.Name).Replace("%%SOAPMETHODNAME%%", webfunc.Name);
                    paramstr = paramstr.Replace("%%NAMESPACE%%", WebService.NameSpace).Replace("%%SOAPMETHODNAME%%", webfunc.Name);
                    paramstr = paramstr.Replace("%%PROPCOUNT%%", paramClass.Properties.Count.ToString());

                    webfunc.RegisterClasses = paramClass.Name;
                    keysRegister.Add(paramClass.Name);
                    registerStrings.AppendLine(paramClass.RegisterText);
                    GetRegisterClasses(paramClass, webfunc);
                    CreateComplexClass(paramClass);

                    for (int i = 0; i < paramClass.Properties.Count; i++)
                    {
                        SoapClassProperties property = paramClass.Properties[i];

                        soappropstr.AppendFormat("\t\tPropertyInfo p{0} = new PropertyInfo();", i).AppendLine();
                        soappropstr.AppendFormat("\t\tp{1}.setName(\"{0}\");", property.Name, i).AppendLine();
                        soappropstr.AppendFormat("\t\tp{1}.setValue({0});", property.Name, i).AppendLine();
                        soappropstr.AppendFormat("\t\tp{1}.setType({0}.class);", property.PropertyClassType, i);
                        soappropstr.AppendFormat("\t\tp{0}.setNamespace(NAMESPACE);", i).AppendLine();
                        soappropstr.AppendFormat("\t\trequest.addProperty(p{0});", i).AppendLine().AppendLine();

                        propinfstr.AppendFormat("           case {0}:", i).AppendLine();
                        propinfstr.AppendFormat("                info.name = \"{0}\";", property.Name).AppendLine();
                        propinfstr.AppendFormat("                info.type = {0};", JavaTypeConverter.ClassTypeRetrievalString(property)).AppendLine();
                        propinfstr.AppendLine("                break;");

                        getpropstr.AppendFormat("           case {0}:", i).AppendLine();
                        getpropstr.AppendFormat("                return {0};", property.Name).AppendLine();

                        setpropstr.AppendFormat("           case {0}:", i).AppendLine();
                        setpropstr.AppendLine("                if (value.toString().equalsIgnoreCase(\"anyType{}\")) {");
                        setpropstr.AppendFormat("                    {0} = {1};", property.Name, JavaTypeConverter.InitialToJavaType(property)).AppendLine("                }");
                        setpropstr.AppendLine("                else {");

                        if (property.IsEnum)
                        {
                            propText.AppendFormat("     public String {0} = {1}; //Enum {2}", property.Name, JavaTypeConverter.InitialToJavaType(property), property.PropertyClassType);
                            setpropstr.AppendFormat("                    {0} = value.toString();", property.Name).AppendLine();

                        }
                        else if (property.IsArray)
                        {
                            setpropstr.AppendFormat("                  {0} = {1};", property.Name, JavaTypeConverter.InitialToJavaType(property)).AppendLine();
                            setpropstr.AppendFormat("                  SoapObject prp = (SoapObject)value; ").AppendLine();
                            setpropstr.AppendFormat("                  {0}.loadSoapObject(prp); ", property.Name).AppendLine();
                        }
                        else
                        {
                            propText.AppendFormat("     public {0} {1} = {2};\n", property.PropertyClassType, property.Name, JavaTypeConverter.InitialToJavaType(property));
                            setpropstr.AppendFormat("	                {0} = {1}; ", property.Name, JavaTypeConverter.ConvertorForJavaType(property)).AppendLine();
                        }

                        if (property.IsComplexType)
                        {
                            SoapClasses complexClass = WebService.SoapClasses.Where(x => x.Name == property.PropertyClassType).FirstOrDefault();
                            if (complexClass != null)
                            {
                                if (complexClass != null && !complexClass.IsEnum && !keysRegister.Contains(complexClass.Name))
                                {
                                    complexClass.Output = true;
                                    complexClass.Save();
                                    webfunc.RegisterClasses = string.Format("{0};{1}", webfunc.RegisterClasses, complexClass.Name);
                                    webfunc.Save();
                                    keysRegister.Add(complexClass.Name);
                                    registerStrings.AppendLine(complexClass.RegisterText);
                                    GetRegisterClasses(complexClass, webfunc);
                                }

                                CreateComplexClass(complexClass);
                            }
                        }

                        setpropstr.AppendLine("                }").AppendLine("                break;");
                    }

                    paramstr = paramstr.Replace("%%PROPERTIES%%", propText.ToString()).Replace("%%GETPROPERTY%%", getpropstr.ToString());
                    paramstr = paramstr.Replace("%%SETPROP%%", setpropstr.ToString()).Replace("%%GETPROPINFO%%", propinfstr.ToString());
                    paramstr = paramstr.Replace("%%SOAPPROPERTIES%%", soappropstr.ToString());


                    using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, paramClass.Name, ".java")))
                    {
                        file.Write(paramstr);
                        file.Close();
                    }

                    Utility.WriteTrace(string.Format("{0}.java Oluşturuldu", paramClass.Name));
                    complexClasses.Add(paramClass.Name);

                    paramClass.Output = true;
                    paramClass.Save();
                }
                #endregion

                #region ReturnClass
                SoapClasses returnClass = WebService.SoapClasses.Where(x => x.Name == webfunc.OutputType).FirstOrDefault();

                if (returnClass != null)
                {
                    StringBuilder propinfstr = new StringBuilder();
                    StringBuilder setpropstr = new StringBuilder();
                    StringBuilder getpropstr = new StringBuilder();
                    StringBuilder propText = new StringBuilder();
                    string returnstr = FileHelper.GetManifestResourceStream("ResponseTemplate");
                    returnstr = returnstr.Replace("%%DATE%%", DateTime.Now.ToString()).Replace("%%PACKAGENAME%%", WebService.PackageName);
                    returnstr = returnstr.Replace("%%CLASSNAME%%", returnClass.Name);
                    returnstr = returnstr.Replace("%%PROPCOUNT%%", returnClass.Properties.Count.ToString());


                    if (returnClass.Properties.Count > 0)
                    {
                        SoapClassProperties property = returnClass.Properties[0];


                        returnstr = returnstr.Replace("%%RESULTPROPNAME%%", property.Name);
                        returnstr = returnstr.Replace("%%RESULTPROPTYPE%%", property.PropertyClassType);
                        returnstr = returnstr.Replace("%%GETPROPINFO%%", JavaTypeConverter.ClassTypeRetrievalString(property));
                        returnstr = returnstr.Replace("%%RESULTPROPTYPE%%", property.PropertyClassType);
                        returnstr = returnstr.Replace("%%SETPROP%%", JavaTypeConverter.ConvertorForJavaType(property));

                        if (property.IsComplexType)
                        {
                            SoapClasses complexClass = WebService.SoapClasses.Where(x => x.Name == property.PropertyClassType).FirstOrDefault();
                            if (complexClass != null)
                            {
                                CreateComplexClass(complexClass);
                            }

                            StringBuilder loadobjstr = new StringBuilder();
                            loadobjstr.AppendFormat("		{0} = new {1}();", property.Name, property.PropertyClassType).AppendLine();
                            loadobjstr.AppendFormat("		{0}.loadSoapObject(property);", property.Name).AppendLine();
                            returnstr = returnstr.Replace("%%LOADSOAPOBJECT%%", loadobjstr.ToString());

                        }
                        else
                        {
                            returnstr = returnstr.Replace("%%LOADSOAPOBJECT%%", "");
                        }
                    }
                    else
                    {
                        returnstr = returnstr.Replace("%%RESULTPROPNAME%%", "");
                        returnstr = returnstr.Replace("%%RESULTPROPTYPE%%", "");
                        returnstr = returnstr.Replace("%%GETPROPINFO%%", "");
                        returnstr = returnstr.Replace("%%RESULTPROPNAME%%", "").Replace("%%GETPROPINFO%%", "");
                        returnstr = returnstr.Replace("%%RESULTPROPTYPE%%", "");
                    }

                    methodstr = methodstr.Replace("%%REGISTERCLASS%%", registerStrings.ToString());
                    strmethods.Append(methodstr);

                    using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, returnClass.Name, ".java")))
                    {
                        file.Write(returnstr);
                        file.Close();
                    }
                    Utility.WriteTrace(string.Format("{0}.java Oluşturuldu", returnClass.Name));
                    complexClasses.Add(returnClass.Name);

                    returnClass.Output = true;
                    returnClass.Save();
                } 
                #endregion
            }

            return strmethods.ToString();
        }

        private void CreateEnum(SoapClasses enumClass)
        {
            try
            {
                using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, Utility.ClearString(enumClass.Name), ".java")))
                {
                    file.WriteLine(FileHelper.COMPANY_DESC);
                    file.WriteLine("");
                    file.WriteLine(string.Concat("package ", WebService.PackageName, ";\n"));

                    StringBuilder enumstring = new StringBuilder();
                    enumstring.Append(string.Concat("public enum ", Utility.ClearString(enumClass.Name), " {")).AppendLine();
                    for (int k = 0; k < enumClass.Properties.Count; k++)
                    {
                        if (k > 0) enumstring.Append(",");
                        enumstring.AppendFormat("{0}(\"{1}\",{2})", Utility.ClearString(enumClass.Properties[k].Name), enumClass.Properties[k].Name, k);
                        if (k == enumClass.Properties.Count - 1 || k > 100)
                        {
                            enumstring.AppendLine(";");
                            break;
                        }
                    }

                    enumstring.AppendLine();
                    enumstring.AppendLine("    private String stringValue;").AppendLine("    private int intValue;").AppendLine();

                    enumstring.Append(string.Concat("    private ", Utility.ClearString(enumClass.Name), "(String strvalue, int value) {\n"));
                    enumstring.AppendLine("        stringValue = strvalue;").AppendLine("        intValue = value;").AppendLine("    }");
                    enumstring.AppendLine("    public int getNumericType() {\n        return intValue;\n                        }\n");
                    enumstring.AppendLine("    public String getStringType() {\n        return stringValue;\n                        \n}");
                    enumstring.AppendLine("    @Override");
                    enumstring.AppendLine("    public String toString() {\n        return stringValue;\n                        \n}");
                    enumstring.AppendLine("    public final boolean equals(int other) {\n        return this.intValue == other;\n                        }\n");
                    enumstring.AppendLine("    public final boolean equals(String other) {\n        return this.stringValue == other;\n                        }\n");
                    enumstring.AppendLine().Append("    }").AppendLine();
                    file.WriteLine(enumstring.ToString());
                    file.Close();
                }
                Utility.WriteTrace(string.Format("{0}.java Oluşturuldu", Utility.ClearString(enumClass.Name)));
                complexClasses.Add(enumClass.Name);
            }
            catch (Exception exc)
            {
                Utility.Hata(exc);
            }
        }

        private void CreateArrayClass(SoapClasses arrClass)
        {
            try
            {
                string arraystr = FileHelper.GetManifestResourceStream("ArrayObjectTemplate");
                arraystr = arraystr.Replace("%%DATE%%", DateTime.Now.ToString()).Replace("%%PACKAGENAME%%", WebService.PackageName);
                arraystr = arraystr.Replace("%%NAMESPACE%%", WebService.NameSpace).Replace("%%CLASSNAME%%", arrClass.Name);
                arraystr = arraystr.Replace("%%ELEMENTTYPE%%", JavaTypeConverter.ToElementJavaType(arrClass.ElementType)).Replace("%%ELEMENTTYPEDESC%%", JavaTypeConverter.ArrayElementTypeDesc(arrClass.ElementType));

                StringBuilder loadstr = new StringBuilder();
                if (arrClass.ElementType == "char")
                {
                    loadstr.AppendLine("                if (property.getProperty(loop) instanceof SoapPrimitive) {");
                    loadstr.AppendLine("                    SoapPrimitive pi = (SoapPrimitive) property.getProperty(loop);");
                    loadstr.AppendLine("                    int intchar = Integer.valueOf(pi.toString());");
                    loadstr.AppendLine("                    this.add((char) intchar);");
                    loadstr.AppendLine("                }");
                }
                else if (arrClass.ElementType == "Integer")
                {
                    loadstr.AppendLine("                if (property.getProperty(loop) instanceof SoapPrimitive) {");
                    loadstr.AppendLine("                    SoapPrimitive pi = (SoapPrimitive) property.getProperty(loop);");
                    loadstr.AppendLine("                    Integer item = Integer.valueOf(pi.toString());");
                    loadstr.AppendLine("                    this.add(item);");
                    loadstr.AppendLine("                }");
                }
                else if (arrClass.ElementType == "String")
                {
                    loadstr.AppendLine("                if (property.getProperty(loop) instanceof SoapPrimitive) {");
                    loadstr.AppendLine("                    SoapPrimitive pi = (SoapPrimitive) property.getProperty(loop);");
                    loadstr.AppendLine("                    String item = pi.toString();");
                    loadstr.AppendLine("                    this.add(item);");
                    loadstr.AppendLine("                }");
                }
                else if (arrClass.ElementType == "BigDecimal")
                {
                    loadstr.AppendLine("                if (property.getProperty(loop) instanceof SoapPrimitive) {");
                    loadstr.AppendLine("                    SoapPrimitive pi = (SoapPrimitive) property.getProperty(loop);");
                    loadstr.AppendLine("                    BigDecimal item = BigDecimal.valueOf(Double.parseDouble(pi.toString()));");
                    loadstr.AppendLine("                    this.add(item);");
                    loadstr.AppendLine("                }");
                }
                else if (arrClass.ElementType == "Object")
                {
                    loadstr.AppendLine("                if (property.getProperty(loop) != null) {");
                    loadstr.AppendLine("                    this.add((Object) property.getProperty(loop));");
                    loadstr.AppendLine("                }");
                }
                else
                {
                    loadstr.AppendLine("                if (property.getProperty(loop) instanceof SoapObject) {");
                    loadstr.AppendLine("                    SoapObject so = (SoapObject) property.getProperty(loop);");
                    loadstr.AppendFormat("                  {0} item = new {0}(); \n", arrClass.ElementType);
                    loadstr.AppendLine("                    item.loadSoapObject(so);");
                    loadstr.AppendLine("                    this.add(item);");
                    loadstr.AppendLine("                }");

                    SoapClasses elementClass = WebService.SoapClasses.Where(x => x.Name == arrClass.ElementType).FirstOrDefault();
                    if (elementClass != null)
                    {
                        CreateComplexClass(elementClass);
                    }

                }

                arraystr = arraystr.Replace("%%LOADSOAPOBJECTS%%", loadstr.ToString());

                using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, arrClass.Name, ".java")))
                {
                    file.Write(arraystr);
                }
                Utility.WriteTrace(string.Format("{0}.java Oluşturuldu", arrClass.Name));
                complexClasses.Add(arrClass.Name);
            }
            catch (Exception exc)
            {
                Utility.Hata(exc);
            }
        }

        private void CreateBaseClass()
        {
            using (FileHelper file = new FileHelper(string.Concat(ProjectFolder, "BaseObject.java")))
            {
                string baseobj = FileHelper.GetManifestResourceStream("BaseObject");
                baseobj = baseobj.Replace("%%DATE%%", DateTime.Now.ToString()).Replace("%%PACKAGENAME%%", WebService.PackageName);
                baseobj = baseobj.Replace("%%NAMESPACE%%", WebService.NameSpace);
                file.Write(baseobj);
            }
            Utility.WriteTrace(string.Format("BaseObject.java Oluşturuldu"));
        }

        #region RegisterMappings

        private void GetRegisterClasses(SoapClasses soapClass, WebFunctions webfunc)
        {
            for (int loop = 0; loop < soapClass.Properties.Count; loop++)
            {
                SoapClassProperties property = soapClass.Properties[loop];
                if (property.IsComplexType && !keysRegister.Contains(property.Name))
                {
                    var complexClass = WebService.SoapClasses.Where(x => x.Name == property.PropertyClassType).FirstOrDefault();
                    if (complexClass != null && !complexClass.IsEnum && !keysRegister.Contains(complexClass.Name))
                    {
                        complexClass.Output = true;
                        complexClass.Save();
                        webfunc.RegisterClasses = string.Format("{0};{1}", webfunc.RegisterClasses, complexClass.Name);
                        webfunc.Save();
                        keysRegister.Add(complexClass.Name);
                        registerStrings.AppendLine(complexClass.RegisterText);
                        GetRegisterClasses(complexClass, webfunc);
                    }

                }
            }


        }
        #endregion

        #region IDisposable
        ~SoapCreator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                keysRegister = null;
                registerStrings = null;
                if (WebService != null)
                {
                }
                WebService = null;
            }

            disposed = true;
        }
        #endregion
    }
}
