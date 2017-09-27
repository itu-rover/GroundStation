using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.ComponentModel;

namespace RoverTCPIP
{
    public class RoverConnection
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public bool ConnectForOneTime { get; set; } = false;

        // When refresh rate is 0, push messages will be sent within a while loop without delay
        // Be careful about the buffer overflows
        public int RefreshRate { get; set; } = 10;

        // Some server applications exit when the sender closes the stream, to prevent,
        // set writer cleanup to false
        public bool WriterCleanUpEnabled { get; set; } = false;

        // Push message is the message sending function that works in another thread, 
        // when it is not "empty", the client will push the messages with the refresh rate (frequency)
        // This property is private, to access it use StartPushMessage or StopPushMessage Functions.
        private string PushMessage { get; set; } = "empty";

        // Returns if client is pushing messages to the server
        public bool IsPushMessageEnabled 
        {
            get
            {
                if (client.Connected && BG.IsBusy && PushMessage != "empty") return true;
                else return false;
            }
        }

        public bool IsThreaded
        {
            get
            {
                if (BG.IsBusy && Connected) return true;
                else return false;
            }
        }

        public bool Connected
        {
            get
            {
                return _Client.Connected;
            }
        }

        public RoverConnection(string IPAddress, int Port)
        {
            this.Port = Port;
            this.IPAddress = IPAddress;
            BG.WorkerSportsCancellation = true;
            BG.WorkerReportsProgress = false;
            BG.DoWork += BackWorkerFunction;
        }

        private TcpClient _Client = new TcpClient();
        private BackgroundWorker BG = new BackgroundWorker();

        // Threaded
        public void StartPushMessage(string msg)
        {
            if(!IsThreaded)
            {
                this.DisconnectFromServer();
                this.BeginConnectionThreaded();
            }
            PushMessage = msg;
        }

        public void StopPushMessage()
        {
            PushMessage = "empty";
        }

        public void BackWorkerFunction(object sender, DoWorkEventArgs e)
        {
            this.ConnectToServer();
            while(Connected)
            {
                // Keep worker alive
                if (PushMessage != "empty") Send(PushMessage);
                if (RefreshRate != 0) System.Threading.Thread.Sleep(1000 / RefreshRate);
            }
        }

        public void BeginConnectionThreaded()
        {
            if (Connected) this.AbortConnection();
            BG.RunWorkerAsync();
        }

        public void AbortConnectionThreaded()
        {
            BG.CancelAsync();
            this.DisconnectFromServer();
            StopPushMessage();
        }
        // Threaded

        // Non-Threaded
        public bool BeginConnection()
        {
            // If Not Connected Begin Connecting Algorithm
            if (!Connected)
            {
                if (this.ConnectForOneTime)
                {
                    return ConnectToServer();
                }
                else
                {
                    // Break the loop if connection is succesfull
                    while (!this.Connected)
                    {
                        ConnectToServer();
                        System.Threading.Thread.Sleep(1000 / RefreshRate);
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool AbortConnection()
        {
            // Returns true or false, related to Succesfull or Unsuccesfull disconnection.
            return DisconnectFromServer();
        }

        public bool SendMessage(string msg)
        {
            // Returns True if message is sent 
            return Send(msg);
        }
        // Non-Threaded
        

        // privates
        private bool Send(string msg)
        {
            if (_Client.Connected)
            {
                StreamWriter Stream = new StreamWriter(_Client.GetStream());
                Stream.WriteLine(msg);
                Stream.Flush();
                if (WriterCleanUpEnabled) Stream.Close();
                return _Client.Connected;
            }
            else
            {
                return false;
            }
        }

        private bool ConnectToServer()
        {
            try
            {
                _Client.Connect(IPAddress, Port);
                return _Client.Connected;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool DisconnectFromServer()
        {
            _Client.Close();
            return !_Client.Connected;
        }
        // privates        
    }
}
