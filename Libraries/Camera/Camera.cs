using System;
using System.Drawing;

namespace RoverCamera
{
    public class CameraManager
    {
        public static List<RoverCam> CamerasConnected = new List<RoverCam>();
        
        public bool[] CheckStatusAll()
        {
            bool[] status = new bool[CamerasConnected.Count];
            for (int i = 0; i < status.Length; i++)
            {
                status[i] = CamerasConnected[i].IsConnected;
            }
            return status;
        }

        public static bool ReConnect(int ID)
        {
            bool stat = false;
            foreach (RoverCam item in CamerasConnected)
            {
                if (item.ID == ID) 
                {
                    item.Disconnect();
                    stat = item.Connect();
                }
            }
            return stat;
        }

        public static bool ReConnectAll()
        {
            foreach (RoverCam item in CamerasConnected)
            {
                item.Disconnect();
                if (!item.Connect())
                {
                    return false;
                }
            }
            return true;
        }

        public static bool DisconnectAll()
        {
            foreach (RoverCam item in CamerasConnected)
            {
                item.Disconnect();
            }
        }
    }

    class RoverCam
    {
        public int FrameRate { get; set; } = 20;
        public Size Resolution { get; set; } = new Size(640, 480);
        public int CameraID { get; set; }
        public bool IsConnected { get; set; } = false;
        public string CameraName { get; set; } = "";
        public string IP { get; set; } = "localhost";
        public int Port { get; set; } = 8080;
        private PictureBox PicBox { get; set; }

        public RoverCam(PictureBox PicBox, string CameraName = "", string IP = "localhost", int Port = 8000, int FrameRate = 20, Size Resolution = new Size(640, 480))
        {
            this.Port = Port;
            this.IP = IP;
            this.CameraID = CameraManager.CamerasConnected.Count;
            if (CameraName == "" || CameraName == null) CameraName = "Camera " + CameraID.ToString();
            this.FrameRate = FrameRate;
            this.Resolution = Resolution;
            this.CameraName = CameraName;
            CameraManager.CamerasConnected.Add(this);
            this.PicBox = PicBox;
        }

        public bool Connect()
        {
            while (!IsConnected)
            {
                IsConnected = ConnectToCamera();
            }
            return IsConnected;
        }

        public bool Disconnect()
        {
            while(IsConnected)
            {
                IsConnected = !DisconnectFromCamera();
            }
            return !IsConnected;
        }

        private bool ConnectToCamera()
        {
            // To be Written!
            return true;
        }

        private bool DisconnectFromCamera()
        {
            // To be written
            return true;
        }
    }
}