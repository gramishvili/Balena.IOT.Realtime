using System;

namespace Balena.Geolocation
{
    public class GeolocationHelpers
    {
        
        //reference: https://www.ridgesolutions.ie/index.php/2013/11/14/algorithm-to-calculate-speed-from-two-gps-latitude-and-longitude-points-and-time-difference/
        public static double DistanceAsMeter(double lat1, double lon1, double lat2, double lon2) {
 
            // Convert degrees to radians
            lat1 = lat1 * Math.PI / 180.0;
            lon1 = lon1 * Math.PI / 180.0;
 
            lat2 = lat2 * Math.PI / 180.0;
            lon2 = lon2 * Math.PI / 180.0;
 
            // radius of earth in metres
            double r = 6378100;
 
            // P
            double rho1 = r * Math.Cos(lat1);
            double z1 = r * Math.Sin(lat1);
            double x1 = rho1 * Math.Cos(lon1);
            double y1 = rho1 * Math.Sin(lon1);
 
            // Q
            double rho2 = r * Math.Cos(lat2);
            double z2 = r * Math.Sin(lat2);
            double x2 = rho2 * Math.Cos(lon2);
            double y2 = rho2 * Math.Sin(lon2);
 
            // Dot product
            double dot = (x1 * x2 + y1 * y2 + z1 * z2);
            double cos_theta = dot / (r * r);
 
            double theta = Math.Acos(cos_theta);
 
            // Distance in Metres
            return r * theta;
        }

        public static double SpeedAsKMH(double distance, DateTime time1, DateTime time2)
        {
            var timeS = (time1.Ticks - time2.Ticks) / 1000.0;
            return timeS / distance;
        }
    }
}