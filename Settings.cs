using SpaceConceptOptimizer.Models;
// using SpaceConceptOptimizer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.Settings
{
    /// <summary>
    /// Program Settings
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Geocentric Gravity Constant
        /// </summary>
        public const double u0 = 3.986004418e14;

        /// <summary>
        /// Seconds in day
        /// </summary>
        public const double day_in_seconds = 86400.0;

        /// <summary>
        /// Earth Radius
        /// </summary>
        public const double R0 = 6378137.0;

        /// <summary>
        /// Pertubation Second Term
        /// </summary>
        public const double J2 = 1.08262617385222e-3;

        /// <summary>
        /// RAAN Pertubation for Sun Sync Orbits
        /// </summary>
        public const double RANN_tx_Sunsync = 1.9910638534437197e-7;

        /// <summary>
        /// Gravity
        /// </summary>
        public const double g0 = 9.80665;

        /// <summary>
        /// Mode of Solar Pressure (User Entry)
        /// </summary>
        public static List<SolarModeModel> SolarModes;

        /// <summary>
        /// ISP of the propellent
        /// </summary>
        public static double ISP;

        /// <summary>
        /// Percentage of the sectional area that the total area represents
        /// </summary>
        public static double AreaPercentOverSectionalArea { get; set; }

        /// <summary>
        /// Ground Velocity for the reference mission at Equator in the descendant
        /// </summary>
        public static double V0 { get; set; }

        /// <summary>
        /// Earth Angular Velocity
        /// </summary>
        public const double VE = 7.2921159E-5;

        /// <summary>
        /// Mission Resolution
        /// </summary>
        public static double MissionResolution { get; set; }

        /// <summary>
        /// Precision tolerance for the esimation of propelent mass
        /// </summary>
        public static double PrecisionPropulsion { get; set; }

        /// <summary>
        /// Altitude error of the launcher
        /// </summary>
        public static double LaunchAltitudeError { get; set; }

        /// <summary>
        /// Inclination error of the launcher
        /// </summary>
        public static double LaunchInclinationError { get; set; }
    }
}
