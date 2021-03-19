using MathModelsDomain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.Models
{
    /// <summary>
    /// This class represents one orbit
    /// </summary>
    public class Orbit : ICloneable
    {
        /// <summary>
        /// Semi-major axis
        /// </summary>
        public double a { get; set; }

        /// <summary>
        /// Ecccentricity
        /// </summary>
        public double e { get; set; }

        /// <summary>
        /// Inclination
        /// </summary>
        public double i { get; set; }

        /// <summary>
        /// Angular Velocity
        /// </summary>
        public double ni { get; set; }

        /// <summary>
        /// Angular Velocity without pertubations
        /// </summary>
        public double n0
        {
            get
            { return Math.Sqrt(Settings.Settings.u0 / Math.Pow(a, 3)); }
        }

        /// <summary>
        /// Perigee Radius
        /// </summary>
        public double Rp { get { return a * (1 - e); } }

        /// <summary>
        /// Apogee Radius
        /// </summary>
        public double Ra { get { return a * (1 + e); } }

        /// <summary>
        /// Perigee Altitude
        /// </summary>
        public double Hp { get { return Rp - Settings.Settings.R0; } }

        /// <summary>
        /// Apogee Altitude
        /// </summary>
        public double Ha { get { return Ra - Settings.Settings.R0; } }

        /// <summary>
        /// Velocity at Perigee
        /// </summary>
        public double Vp { get { return V(a, Rp); } }

        /// <summary>
        /// Velocity at Apogee
        /// </summary>
        public double Va { get { return V(a, Ra); } }

        /// <summary>
        /// Period based on the Perigee
        /// </summary>
        public double Tp
        {
            get
            {
                return 2 * Math.PI
                      * Math.Sqrt(Math.Pow(Rp, 3)
                      / Settings.Settings.u0);
            }
        }

        public Orbit(double _a, double _e, double _i)
        {
            a = _a * 1000;
            e = _e;
            i = _i;
        }

        public Orbit(double _ni, double _e)
        {
            ni = _ni;
            e = _e;
        }

        public Orbit(Orbit i, Orbit f)
        {
            FindTransferOrbit(i, f);
        }



        public Orbit()
        {
        }

        public static Orbit FindOrbit(double hp, double ha)
        {
            Orbit o = new Orbit();
            double ra = ha + Settings.Settings.R0;
            double rp = hp + Settings.Settings.R0;

            o.e = (ra - rp) / (ra + rp);

            o.a = rp / (1 - o.e);

            return o;

        }

        private void FindTransferOrbit(Orbit io, Orbit f)
        {

            e = (f.Rp - io.Rp) / (f.Rp + io.Rp);

            a = io.Rp / (1 - e);

            i = f.i;

        }

        public double V(double a, double r)
        {
            return Math.Sqrt(Settings.Settings.u0 * ((2.0 / r) - (1.0 / a)));
        }

        public object Clone()
        {
            Orbit obr = new Orbit();
            obr.a = this.a;
            obr.e = this.e;
            obr.i = this.i;
            obr.ni = this.ni;

            return obr;
        }
    }
}
