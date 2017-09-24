using System;
using System.Drawing;

namespace RoverMap
{
    public class GPSLocation
    {
        public float Lat { get; set; } = 0f;
        public float Lon { get; set; } = 0f;
        public DateTime Time { get; set; } = new DateTime();

        public GPSLocation(float Lon, float Lat)
        {
            this.Lat = Lat;
            this.Lon = Lon;
        }

        public GPSLocation()
        {
            
        }

        public override string ToString()
        {
            return string.Format("DateTime: {0}, Latitude: {1}, Longitude: {2}", Time.ToString(), Lat, Lon);
        }
    }

    public class LocationManager
    {
        public static List<GPSLocation> History = new List<GPSLocation>();

        public static void WriteHistoryToFile(string FilePath, bool ClearHistory = false)
        {
            System.IO.StreamWriter Writer = new System.IO.StreamWriter(FilePath);
            // Create Group Of Now's History
            Writer.WriteLine(string.Format("\nLogging Started At: {0}", DateTime.Now.ToString()));

            // Write Each History Location
            foreach (GPSLocation item in History)
            {
                Writer.WriteLine(item.ToString());
                Writer.Flush();
            }
            // Close Writer
            Writer.Close();
            // If User Wants, Clear History
            if (ClearHistory) History.Clear();
        }
    }

    class RoverMap
    {
        private GPSLocation Location { get; set; } = new GPSLocation();

        public RoverMap()
        {

        }

        public GPSLocation GetLocation()
        {
            return Location;
        }

        public bool UpdateLocation(float Lon, float Lat)
        {
            // Add old one to the history
            LocationManager.History.Add(this.Location);

            // Update the new one
            this.Location.Time = DateTime.Now;
            this.Location.Lat = Lat;
            this.Location.Lon = Lon;

            return true;
        }

        public bool RefreshLocation()
        {
            // Refresh the control in here
        }

    }
}