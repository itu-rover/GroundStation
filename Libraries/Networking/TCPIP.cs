using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace RoverTCPIPLibrary
{
    public class RoverConnection
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public bool ConnectForOneTime { get; set; } = false;
        public int RefreshRate { get; set; } = 10;
        public bool Connected { get; set; }

        public RoverConnection(string IPAddress, int Port)
        {
            this.Port = Port;
            this.IPAddress = IPAddress;
        }

        private TcpClient _Client = new TcpClient();

        public bool SendMessage(string msg)
        {
            // Returns True if message is sent 
            return Send(msg);
        }

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
                        this.Connected = ConnectToServer();
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



        private bool Send(string msg)
        {
            StreamWriter Stream = new StreamWriter(_Client.GetStream());
            Stream.WriteLine(msg);
            Stream.Flush();
            Stream.Close();
            return _Client.Connected;
        }

        private bool ConnectToServer()
        {
            _Client.Connect(IPAddress, Port);
            return _Client.Connected;
        }

        private bool DisconnectFromServer()
        {
            _Client.Close();
            return !_Client.Connected;
        }
    }
}
