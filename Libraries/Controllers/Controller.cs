using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace RoverController
{
    public enum ControllerType
    {
        DualShock4,
        DualShock3,
        Joystick
    }

    public class Controller
    {
        public bool IsConnected { get; set; } = false;
        public ControllerType Type { get; set; } = ControllerType.Joystick;
        public string ID { get; set; }

        public Controller(ControllerType Type)
        {

            this.Type = Type;
        }

        public bool Connect()
        {
            while (!IsConnected)
            {
                IsConnected = ConnectTheController();
            }
            return IsConnected;
        }

        

        private bool ConnectTheController()
        {

        }

    }
    
}