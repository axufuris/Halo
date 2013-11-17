using System;

namespace Halo.Utilities
{
    public class MathManager
    {
        /// <summary>
        /// Distances in miles between two Lat/Long Pairs
        /// </summary>
        /// <param name="sourceLatitude">The source latitude.</param>
        /// <param name="sourceLongitude">The source longitude.</param>
        /// <param name="destinationLatitude">The destination latitude.</param>
        /// <param name="destinationLongitude">The destination longitude.</param>
        /// <returns></returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static double Distance(double sourceLatitude, double sourceLongitude, double destinationLatitude, double destinationLongitude)
        {
            // gates 7/31/2013 changed the parameter names to source and destination
            double theta = sourceLongitude - destinationLongitude;
            double dist = Math.Sin(Deg2Rad(sourceLatitude)) * Math.Sin(Deg2Rad(destinationLatitude)) +
                    Math.Cos(Deg2Rad(sourceLatitude)) * Math.Cos(Deg2Rad(destinationLatitude)) * Math.Cos(Deg2Rad(theta));
            double ret = (Rad2Deg(Math.Acos(dist)) * 60 * 1.1515);

            return ret;
        }

        /// <summary>
        /// Rounds up to nearest [nearestHoldNumber].
        /// </summary>
        /// <param name="nearestHoldNumber">The nearest hold number.</param>
        /// <param name="originalNumber">The original number.</param>
        /// <returns>Rounded Int</returns>
        /// <createdate>4-12-2013</createdate>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public static int RoundUpToNearestWholeNumber(int nearestHoldNumber, int originalNumber)
        {
            //return (nearestHoldNumber * Convert.ToInt32(Convert.ToDouble(originalNumber) / nearestHoldNumber + 0.5));
            return ((int)Math.Ceiling(((decimal)originalNumber / nearestHoldNumber))) * nearestHoldNumber;  // Andy pretty sure this does the same
        }

        /// <summary>
        /// Deg2rads the specified deg.
        /// </summary>
        /// <param name="deg">The deg.</param>
        /// <returns></returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        private static double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        /// <summary>
        /// Rad2degs the specified RAD.
        /// </summary>
        /// <param name="rad">The RAD.</param>
        /// <returns></returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        private static double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }  /// End of Class
}
