using MathModelsDomain.Utilities;
using SpaceConceptOptimizer.Models;
using SpaceConceptOptimizer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathModelsDomain.ModelsManagers
{
    /// <summary>
    /// Manager for FOV Model Calculations
    /// </summary>
    public class FOVManager 
    {
        
        public FOVManager()
        {
            
        }

        /// <summary>
        /// Calculates the Min Fov of an Orbit
        /// </summary>
        /// <param name="orbit"></param>
        /// <returns></returns>
        public static double FovMin(SunSyncOrbitRPT orbit)
        {
            double fovMin;

            double teta = (4.0 * Math.Pow(Math.PI, 2)) / (orbit.ni * 86400) / orbit.D;
            teta = Math.Asin(Math.Sin(teta) * Math.Sin(orbit.i));

            double r0_sqrd = Math.Pow(Settings.R0, 2);
            double cos_half_Teta = Math.Cos(teta / 2);
            double sin_half_Teta = Math.Sin(teta / 2);

            fovMin = 2.0 * Math.Asin((Settings.R0 /
                Math.Sqrt(r0_sqrd + Math.Pow(Settings.R0 + orbit.Hp, 2) - 2.0 *
                Settings.R0 * (Settings.R0 + orbit.Hp) * cos_half_Teta)) * sin_half_Teta);

            return fovMin.RadiansToDegrees();
        }
    }
}
