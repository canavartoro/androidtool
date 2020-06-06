using SoapHelper.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoapHelper.Wsdl
{
    public class FileHelper : IDisposable
    {
       public const string COMPANY_DESC = @"/*
by Canavar.Toro 03.06.2020
*/";
        string CACH_PATH = Application.StartupPath + "\\proj\\";
        StreamWriter writer;

        public FileHelper()
        {
        }

        public FileHelper(string file)
        {
            writer = new StreamWriter(file, false, Encoding.GetEncoding("Windows-1254"));
        }

        public static String GetManifestResourceStream(string file)
        {
            StringBuilder contents = new StringBuilder();

            try
            {
                string ResourceUrl = "SoapHelper.wsdl2ksoap.{0}.txt";
                Assembly _assembly = Assembly.GetExecutingAssembly();
                StreamReader _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream(string.Format(ResourceUrl, file)));
                try
                {
                    contents.Append(_textStreamReader.ReadToEnd().TrimStart().TrimEnd().Trim());
                    _textStreamReader.Close();
                }
                finally
                {
                }
            }
            catch (IOException ex)
            {
                Trace.WriteLine(ex.StackTrace);
            }

            return contents.ToString();
        }

        public static bool CreateFolderStructure(string parentPath, string packageName)
        {
            string output = parentPath;
            string output_folder = output + "\\" + packageName.Replace(".", "\\");

            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }

            if (!Directory.Exists(output_folder)) Directory.CreateDirectory(output_folder);

            return true;
        }

        public void Write(string data)
        {
            if (writer != null)
            {
                writer.Write(data);
            }
        }

        public void WriteLine(string data)
        {
            if (writer != null)
            {
                writer.WriteLine(data);
            }
        }

        public void Close()
        {
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
        }



        #region IDisposable
        ~FileHelper()
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
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
                writer = null;
            }

            disposed = true;
        }
        #endregion
    }
}
