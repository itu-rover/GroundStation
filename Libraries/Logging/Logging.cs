using System;
using System.Drawing;

namespace RoverLogger
{
    public class RoverLogger
    {
        public string LogFilePath { get; set; } = "";
        public System.Windows.Forms.RichTextBox TextBox { get; set; }

        public void Log(string LogMessage, bool LogToFile = false, bool LogToConsole = true, bool LogToTextBox = false)
        {
            if (LogToConsole) System.Console.WriteLine(LogMessage);
            if (LogToTextBox && TextBox != null) TextBox.Text += LogMessage + "\n";
            if (LogToFile)
            {
                System.IO.StreamWriter Writer = new System.IO.StreamWriter(LogFilePath);
                Writer.WriteLine(LogMessage);
                Writer.Flush();
                Writer.Close();
            }
        }
    }
    
}