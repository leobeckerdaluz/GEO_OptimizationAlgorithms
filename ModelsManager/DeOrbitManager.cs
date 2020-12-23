// using MathModelsDomain.Utilities;
using SpaceConceptOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.ModelsManager
{
    public class DeOrbitManager
    {
        public static double PerigeeH_DeOrbit(double ballistic_coef, double apogee_altitude)
        {
            return -2.96741 * Math.Log(ballistic_coef) *
                (38.8249 * Math.Exp(-0.001 * apogee_altitude) - 50.7627) + 1017.1592;
        }

        public static Orbit DeOrbit(Satellite s, Orbit o)
        {
            double bc = s.At / 4.0 / s.Md;
            double apogee_altitude = o.Ha;
            double he = PerigeeH_DeOrbit(bc, apogee_altitude/1000.0)*1000;

            if (he > o.Hp)
                return o;

            return Orbit.FindOrbit(he, apogee_altitude);
        }
    }
}
