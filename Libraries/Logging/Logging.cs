using System;
using System.Drawing;

namespace RoverLogger
{
    public class RoverLogger
    {
        // Path of the log file to be saved. (Include the file format Ex: '.txt')
        public string LogFilePath { get; set; } = "";
        public System.Windows.Forms.RichTextBox TextBox { get; set; }
        public bool LogToConsole { get; set; } = true;
        public bool LogToTextBox { get; set; } = false;
        public bool LogToFile { get; set; } = false;
        

        public void Log(string LogMessage)
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