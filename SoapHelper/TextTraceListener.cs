using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoapHelper
{
    public class TextTraceListener : TraceListener
    {
        private RichTextBox _richTextBox;

        public TextTraceListener(RichTextBox richTextBox)
        {
            this._richTextBox = richTextBox;
        }

        public void SetThreadText(string str)
        {
            try
            {
                if (object.ReferenceEquals((object)this._richTextBox, (object)null))
                    return;
                if (this._richTextBox.InvokeRequired)
                {
                    this._richTextBox.Invoke((Delegate)new TextTraceListener.SetLabelCallback(this.SetThreadText), (object)str);
                }
                else
                {
                    if (((IEnumerable<string>)this._richTextBox.Lines).Count<string>() > 600)
                    {
                        this._richTextBox.Text = str;
                    }
                    else
                    {
                        this._richTextBox.AppendText(str);
                        this._richTextBox.ScrollToCaret();
                    }
                    Application.DoEvents();
                }
            }
            catch
            {
            }
        }

        public override void Write(string message)
        {
            this.SetThreadText(message);
        }

        public override void WriteLine(string message)
        {
            this.SetThreadText(message + "\r");
        }

        private delegate void SetLabelCallback(string str);
    }
}
