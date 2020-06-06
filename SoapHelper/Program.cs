using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoapHelper
{
    static class Utility
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AddListener();
            XPOStart();
            Application.Run(new FormMain());

        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

            string error = "Bir sistem hatası oluştu!\nBu hatayı sistem yöneticinize bildirin!\n" + e.Exception.ToString();

            WriteTrace(error);
            //System.Diagnostics.EventLog ev = new System.Diagnostics.EventLog("Application", System.Environment.MachineName, dom);

            //ev.WriteEntry(error, System.Diagnostics.EventLogEntryType.Error, 0);

            //ev.Close();

            Application.ExitThread();
        }

        public static void XPOStart()
        {
            try
            {
                string conString = SQLiteConnectionProvider.GetConnectionString(string.Format("{0}\\projects.ldb", System.Windows.Forms.Application.StartupPath));
                DevExpress.Xpo.Metadata.XPDictionary dictionary = new DevExpress.Xpo.Metadata.ReflectionDictionary();
                XpoDefault.DataLayer = XpoDefault.GetDataLayer(conString, AutoCreateOption.DatabaseAndSchema);
                DevExpress.Xpo.XpoDefault.Session = new DevExpress.Xpo.Session(XpoDefault.DataLayer);
                //DevExpress.Xpo.Helpers.SessionStateStack.SuppressCrossThreadFailuresDetection = true;
                //DevExpress.Xpo.Session.DefaultSession.UpdateSchema();

                //IDataStore store = XpoDefault.GetConnectionProvider(conn, AutoCreateOption.SchemaAlreadyExists);
                //var layer = new ThreadSafeDataLayer(dictionary, store);

            }
            catch (Exception ex)
            {
                Utility.Hata(ex);
            }
        }

        static string versiyon = null;

        public static string Versiyon
        {
            get
            {
                if (versiyon == null)
                {
                    Assembly entryPoint = Assembly.GetExecutingAssembly();
                    AssemblyName entryPointName = entryPoint.GetName();
                    Version entryPointVersion = entryPointName.Version;
                    versiyon = string.Format("{0}", entryPointVersion.ToString());
                }
                return versiyon;
            }
        }

        private static string traceName = "";
        public static string TraceName
        {
            get
            {
                return traceName;
            }
        }

        private static void AddListener()
        {
            try
            {
                if (!Directory.Exists(Application.StartupPath + "\\Trace"))
                    Directory.CreateDirectory(Application.StartupPath + "\\Trace");

                if (!Directory.Exists(Application.StartupPath + "\\Trace\\" + DateTime.Now.ToString("MM")))
                    Directory.CreateDirectory(Application.StartupPath + "\\Trace\\" + DateTime.Now.ToString("MM"));

                if (Directory.Exists(Application.StartupPath + "\\Trace\\" + DateTime.Now.AddMonths(1).ToString("MM")))
                    Directory.Delete(Application.StartupPath + "\\Trace\\" + DateTime.Now.AddMonths(1).ToString("MM"), true);

                traceName = Application.StartupPath + "\\Trace\\" + DateTime.Now.ToString("MM") + "\\" + DateTime.Now.ToString("dd_TRACE") + ".log";

                System.IO.StreamWriter writer = new System.IO.StreamWriter(traceName, true, System.Text.Encoding.GetEncoding("windows-1254"));

                System.Diagnostics.TextWriterTraceListener listener = new System.Diagnostics.TextWriterTraceListener(writer);

                System.Diagnostics.Trace.Listeners.Add(listener);

                System.Diagnostics.Trace.AutoFlush = true;

                System.Diagnostics.Trace.WriteLine("-> " + DateTime.Now.ToString() + "\tBaşladı");
            }
            catch
            {
                ;
            }
        }

        public static void WriteTrace(string str, [CallerMemberName] string callerName = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                //string modul = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().ToString();
                System.Diagnostics.Trace.WriteLine(string.Concat(callerName, "(", lineNumber, ")", "\t", DateTime.Now.ToString(), "\t", str));//System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() + "\t" + str + "\t" + modul);
            }
            catch
            {
                ;
            }
        }

        public static void Hata(string str)
        {
            WriteTrace(str);
            Cursor.Current = Cursors.Default;
            string modul = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().ToString();
            MessageBox.Show(string.Concat(str, "\nDetay:", modul, "\nBilgi:", DateTime.Now), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public static void Hata(Exception exc)
        {
            Cursor.Current = Cursors.Default;
            string modul = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().ToString();
            string str = string.Concat(exc.Message, "\n", exc.StackTrace, "\nDetay:", modul, "\nBilgi:", DateTime.Now);
            WriteTrace(str);
            MessageBox.Show(str, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public static void Bilgi(string str)
        {
            WriteTrace(str);
            Cursor.Current = Cursors.Default;
            string modul = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().ToString();
            MessageBox.Show(string.Concat(str, "\nDetay:", modul, "\nBilgi:", DateTime.Now), "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        public static bool Sor(string str)
        {
            WriteTrace(str);
            Cursor.Current = Cursors.Default;
            string modul = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().ToString();
            return MessageBox.Show(string.Concat(str, "\nDetay:", modul, "\nBilgi:", DateTime.Now), "BİLGİ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }


        public static string ClearString(string stringdata)
        {
            var resultstring = stringdata
                .Replace("İ", "i")
                .Replace("ı", "I")
                .Replace("Ğ", "G")
                .Replace("ğ", "g")
                .Replace("Ö", "O")
                .Replace("ö", "o")
                .Replace("Ü", "U")
                .Replace("ü", "u")
                .Replace("Ş", "S")
                .Replace("ş", "s")
                .Replace("Ç", "C")
                .Replace("ç", "c")
                .Replace(" ", "_");
            //using (MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes(stringdata)))
            //{
            //    StreamWriter sw = new StreamWriter(ms);
            //    sw.Flush();
            //    ms.Position = 0;
            //    StreamReader sr = new StreamReader(ms);
            //    resultstring = sr.ReadToEnd();
            //}

            return resultstring;
        }


    }
}
