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

    public class ControllerManager
    {
        public static List<Controller> ControllersConnected = new List<Controller>();

        public static List<Controller> ReturnControllersByType(ControllerType Type)
        {
            List<Controller> list = new List<Controller>();

            foreach (Controller item in ControllersConnected)
            {
                if (item.Type == Type)
                {
                    list.add(item);
                }
            }           
            return list; 
        }

        public static bool[] ReturnControllerStatus()
        {
            bool[] status = new bool[ControllersConnected.Count];

            for (int i = 0; i < status.Length; i++)
            {
                status[i] = ControllersConnected[i].IsConnected;
            }
            return status;
        }

        public static void DisconnectAll()
        {
            foreach (Controller item in ControllersConnected)
            {
                item.Disconnect();
            }
            return true;
        }

        public static bool DisconnectByID(string ID)
        {
            foreach (Controller item in ControllersConnected)
            {
                if (item.ID == ID)
                {
                    item.Disconnect();
                }
            }
            return false;
        }
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
            ControllerManager.ControllersConnected.Add(this);
            return IsConnected;
        }

        public bool Disconnect()
        {
            return !IsConnected;
        }


        private bool ConnectTheController()
        {

        }

    }
    
}