using DevExpress.Xpo;
using SoapHelper.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SoapHelper.Wsdl
{
    public class WSDLParser
    {
        WebServices webserv = null;

        public string Namespace { get; set; }
        public string ServiceName { get; set; }

        public List<WebFunctions> Functions { get; set; }
        public List<SoapClasses> ComplexTypes { get; set; }

        public WSDLParser(WebServices project)
        {
            this.Functions = new List<WebFunctions>();
            this.ComplexTypes = new List<SoapClasses>();
            webserv = project;
        }

        public void Parse()
        {
            UriBuilder uriBuilder = new UriBuilder(webserv.Url);
            uriBuilder.Query = "WSDL";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Method = "GET";
            webRequest.Accept = "text/xml";

            ServiceDescription serviceDescription;
            using (WebResponse response = webRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    serviceDescription = ServiceDescription.Read(stream);
                }
            }

            var oldfuncs = (from q in new XPQuery<WebFunctions>(XpoDefault.Session)
                            where q.Service.Oid == webserv.Oid
                            select q).ToList();



            if (serviceDescription != null && serviceDescription.Services.Count > 0)
            {
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
                importer.ProtocolName = "Soap11";
                importer.AddServiceDescription(serviceDescription, null, null);
                importer.Style = ServiceDescriptionImportStyle.Client;
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties;

                Service service = serviceDescription.Services[0];

                this.Namespace = serviceDescription.TargetNamespace;
                this.ServiceName = service.Name;

                webserv.ServiceName = service.Name;
                webserv.NameSpace = serviceDescription.TargetNamespace;
                webserv.UpdateDate = DateTime.Now;

                for (int i = webserv.SoapClasses.Count - 1; i >= 0; i--) webserv.SoapClasses[i].Delete();
                
                #region Operation
                List<string> operationNames = new List<string>();
                //Loop through the port types in the service description and list all of the 
                //web service's operations and each operations input/output

                PortType portType = serviceDescription.PortTypes[0];
                //foreach (PortType portType in serviceDescription.PortTypes)
                {
                    int FunctionIndex = 0;
                    this.Functions = new List<WebFunctions>();
                    foreach (Operation operation in portType.Operations)
                    {
                        WebFunctions newFunc = new WebFunctions();
                        newFunc.Service = webserv;
                        var oldfunc = oldfuncs.Where(x => x.Name == operation.Name).FirstOrDefault();
                        if (oldfunc != null) newFunc.Output = oldfunc.Output;
                        newFunc.Name = operation.Name;
                        newFunc.SoapAction = this.Namespace + operation.Name;
                        operationNames.Add(operation.Name);

                        foreach (var message in operation.Messages)
                        {
                            if (message is OperationInput)
                            {
                                foreach (Message messagePart in serviceDescription.Messages)
                                {

                                    if (messagePart.Name != ((OperationMessage)message).Message.Name) continue;

                                    foreach (MessagePart part in messagePart.Parts)
                                    {
                                        newFunc.InputType = part.Element.Name;
                                    }
                                }
                            }
                            if (message is OperationOutput)
                            {
                                foreach (Message messagePart in serviceDescription.Messages)
                                {

                                    if (messagePart.Name != ((OperationMessage)message).Message.Name) continue;

                                    foreach (MessagePart part in messagePart.Parts)
                                    {
                                        newFunc.OutputType = part.Element.Name;
                                    }
                                }
                            }


                        }
                        newFunc.Save();
                        this.Functions.Add(newFunc);
                        FunctionIndex++;
                    }
                } //End listing of types

                for (int i = oldfuncs.Count - 1; i >= 0; i--) oldfuncs[i].Delete();

                #endregion

                #region Types

                Types types = serviceDescription.Types;
                XmlSchema xmlSchema = types.Schemas[0];

                foreach (object item in xmlSchema.Items)
                {
                    XmlSchemaComplexType _complexType = item as System.Xml.Schema.XmlSchemaComplexType;
                    XmlSchemaElement schemaElement = item as XmlSchemaElement;
                    XmlSchemaComplexType complexType = item as XmlSchemaComplexType;

                    if (schemaElement != null && JavaTypeConverter.IsComplexType(schemaElement.Name))
                    {

                        SoapClasses newClass = this.GetClass(schemaElement.Name);
                        newClass.Name = schemaElement.Name;
                        newClass.Service = webserv;
                        newClass.Type = ClassType.Unknown;
                        newClass.SuperClassType = string.Empty;
                        newClass.Output = false;

                        if (_complexType != null)
                        {
                            XmlSchemaContentModel model = _complexType.ContentModel;
                            XmlSchemaComplexContent complex = model as XmlSchemaComplexContent;
                            if (complex != null)
                            {
                                XmlSchemaComplexContentExtension extension = complex.Content as XmlSchemaComplexContentExtension;
                                if (extension != null)
                                {
                                    newClass.SuperClassType = extension.BaseTypeName.Name;
                                }
                            }
                        }

                        XmlSchemaType schemaType = schemaElement.SchemaType;
                        XmlSchemaComplexType schemaComplexType = schemaType as XmlSchemaComplexType;


                        if (schemaComplexType != null)
                        {
                            XmlSchemaParticle particle = schemaComplexType.Particle;
                            XmlSchemaSequence sequence = particle as XmlSchemaSequence;
                            if (sequence != null)
                            {
                                foreach (XmlSchemaElement childElement in sequence.Items)
                                {
                                    SoapClassProperties newProp = new SoapClassProperties(XpoDefault.Session);
                                    newProp.Name = childElement.Name;

                                    newProp.PropertyClassType = childElement.SchemaTypeName.Name;
                                    newProp.IsArray = childElement.SchemaTypeName.Name.StartsWith("ArrayOf");
                                    newClass.Properties.Add(newProp);
                                }
                            }
                        }
                        
                        newClass.Save();
                        //this.ComplexTypes.Add(newClass);
                    }
                    else if (complexType != null)
                    {
                        OutputElements(complexType.Particle, complexType.Name);

                        if (complexType.Particle == null)
                        {
                            GetProperties(xmlSchema, complexType.Name);
                        }
                    }
                }

                #region enums
                foreach (object xItem in xmlSchema.SchemaTypes.Values)
                {
                    XmlSchemaSimpleType item2 = xItem as XmlSchemaSimpleType;
                    if (item2 != null)
                    {
                        GetEnum(xmlSchema, item2.Name);
                    }
                }
                #endregion

                #endregion

            }

            webserv.Save();
        }

        private void OutputElements(XmlSchemaParticle particle, string name)
        {

            SoapClasses newClass = this.GetClass(name);
            newClass.Name = name;
            newClass.Type = ClassType.Unknown;
            newClass.IsArray = name.StartsWith("ArrayOf");
            newClass.Output = false;

            XmlSchemaSequence sequence = particle as XmlSchemaSequence;
            XmlSchemaChoice choice = particle as XmlSchemaChoice;
            XmlSchemaAll all = particle as XmlSchemaAll;

            if (sequence != null)
            {
                for (int i = 0; i < sequence.Items.Count; i++)
                {
                    XmlSchemaElement childElement = sequence.Items[i] as XmlSchemaElement;
                    XmlSchemaSequence innerSequence = sequence.Items[i] as XmlSchemaSequence;
                    XmlSchemaChoice innerChoice = sequence.Items[i] as XmlSchemaChoice;
                    XmlSchemaAll innerAll = sequence.Items[i] as XmlSchemaAll;

                    if (childElement != null)
                    {
                        SoapClassProperties newProp = new SoapClassProperties(XpoDefault.Session);
                        newProp.Name = childElement.Name;

                        newProp.PropertyClassType = childElement.SchemaTypeName.Name;
                        newProp.IsArray = childElement.SchemaTypeName.Name.StartsWith("ArrayOf");
                        newClass.Properties.Add(newProp);
                        newClass.ElementType = JavaTypeConverter.ToElementJavaType(childElement.SchemaTypeName.Name);
                    }
                    else OutputElements(sequence.Items[i] as XmlSchemaParticle, name);
                }
            }
            else if (choice != null)
            {
                for (int i = 0; i < choice.Items.Count; i++)
                {
                    XmlSchemaElement childElement = choice.Items[i] as XmlSchemaElement;
                    XmlSchemaSequence innerSequence = choice.Items[i] as XmlSchemaSequence;
                    XmlSchemaChoice innerChoice = choice.Items[i] as XmlSchemaChoice;
                    XmlSchemaAll innerAll = choice.Items[i] as XmlSchemaAll;

                    if (childElement != null)
                    {
                        SoapClassProperties newProp = new SoapClassProperties(XpoDefault.Session);
                        newProp.Name = childElement.Name;
                        newProp.PropertyClassType = childElement.SchemaTypeName.Name;
                        newProp.IsArray = childElement.SchemaTypeName.Name.StartsWith("ArrayOf");
                        newClass.Properties.Add(newProp);
                    }
                    else OutputElements(choice.Items[i] as XmlSchemaParticle, name);
                }

            }
            else if (all != null)
            {
                for (int i = 0; i < all.Items.Count; i++)
                {
                    XmlSchemaElement childElement = all.Items[i] as XmlSchemaElement;
                    XmlSchemaSequence innerSequence = all.Items[i] as XmlSchemaSequence;
                    XmlSchemaChoice innerChoice = all.Items[i] as XmlSchemaChoice;
                    XmlSchemaAll innerAll = all.Items[i] as XmlSchemaAll;

                    if (childElement != null)
                    {
                        SoapClassProperties newProp = new SoapClassProperties(XpoDefault.Session);
                        newProp.Name = childElement.Name;

                        newProp.PropertyClassType = childElement.SchemaTypeName.Name;
                        newProp.IsArray = childElement.SchemaTypeName.Name.StartsWith("ArrayOf");
                        newClass.Properties.Add(newProp);

                    }
                    else OutputElements(all.Items[i] as XmlSchemaParticle, name);
                }
            }
            newClass.Save();
            //this.ComplexTypes.Add(newClass);
        }

        private void GetProperties(XmlSchema xmlSchema, string elementname)
        {
            foreach (object xItem in xmlSchema.SchemaTypes.Values)
            {
                XmlSchemaComplexType item = xItem as XmlSchemaComplexType;
                #region item
                if (item != null)
                {
                    if (item.Name.Equals(elementname))
                    {
                        XmlSchemaContentModel model = item.ContentModel;
                        XmlSchemaComplexContent complex = model as XmlSchemaComplexContent;
                        SoapClasses newClass = this.GetClass(elementname);

                        if (complex != null && newClass != null)
                        {
                            XmlSchemaComplexContentExtension extension = complex.Content as XmlSchemaComplexContentExtension;
                            XmlSchemaParticle particle = extension.Particle;
                            XmlSchemaSequence sequence = particle as XmlSchemaSequence;

                            if (extension != null)
                            {
                                newClass.SuperClassType = extension.BaseTypeName.Name;
                            }

                            if (sequence != null)
                            {
                                foreach (XmlSchemaElement childElement in sequence.Items)
                                {
                                    if (newClass != null)
                                    {
                                        SoapClassProperties newProp = new SoapClassProperties(XpoDefault.Session);
                                        newProp.Name = childElement.Name;

                                        newProp.PropertyClassType = childElement.SchemaTypeName.Name;
                                        newProp.IsArray = childElement.SchemaTypeName.Name.StartsWith("ArrayOf");//.Equals("unbounded"));
                                        newClass.Properties.Add(newProp);
                                    }
                                }
                            }

                            newClass.Save();
                        }
                        return;
                    }
                }
                #endregion
            }
        }

        private void GetEnum(XmlSchema xmlSchema, string enumName)
        {
            var simpleTypes = xmlSchema.SchemaTypes.Values.OfType<XmlSchemaSimpleType>()
                                               .Where(t => t.Content is XmlSchemaSimpleTypeRestriction && t.Name == enumName);
            SoapClasses newClass = this.GetClass(enumName);
            if (newClass != null)
            {
                newClass.Service = webserv;
                newClass.IsArray = false;
                newClass.IsEnum = true;
                newClass.Output = false;
                newClass.SuperClassType = string.Empty;
                foreach (var simpleType in simpleTypes)
                {
                    var restriction = (XmlSchemaSimpleTypeRestriction)simpleType.Content;
                    var enumFacets = restriction.Facets.OfType<XmlSchemaEnumerationFacet>();

                    if (enumFacets.Any())
                    {
                        foreach (var facet in enumFacets)
                        {
                            SoapClassProperties newProp = new SoapClassProperties(XpoDefault.Session);
                            newProp.Name = facet.Value;
                            newProp.PropertyClassType = "String";
                            newProp.IsArray = false;
                            newClass.Properties.Add(newProp);
                        }

                        foreach (SoapClasses cls in this.ComplexTypes)
                        {
                            foreach (SoapClassProperties prp in cls.Properties)
                            {
                                if (prp.PropertyClassType == simpleType.Name)
                                {
                                    prp.IsEnum = true;
                                    prp.Save();
                                }
                            }
                        }
                    }
                }
                newClass.Save();
            }

        }

        private SoapClasses GetClass(string name)
        {
            SoapClasses newClass = this.ComplexTypes.Where(x => x.Name == name).FirstOrDefault<SoapClasses>();
            if (newClass == null)
            {
                newClass = new SoapClasses(XpoDefault.Session);
                newClass.Name = name;
                newClass.Service = webserv;
                newClass.Output = false;
                this.ComplexTypes.Add(newClass);
            }
            return newClass;
        }
    }
}
