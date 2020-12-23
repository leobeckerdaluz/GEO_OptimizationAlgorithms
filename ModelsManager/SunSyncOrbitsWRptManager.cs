using MathModelsDomain.Solvers;
using MathModelsDomain.Utilities;
using SpaceConceptOptimizer.Models;
using SpaceConceptOptimizer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathModelsDomain.ModelsManagers
{
    public class SunSyncOrbitsWRptManager 
    {
        
        /// <summary>
        /// Calculates an inclination for some specific values of a 
        /// Sun Sync Orbit
        /// </summary>
        /// <param name="ss_orb"></param>
        /// <returns></returns>
        public double Calculate_i(SunSyncOrbitRPT ss_orb)
        {
            

            return (Math.Acos((Settings.RANN_tx_Sunsync / ((-3.0 / 2.0) *
                    Math.Pow(Settings.R0, 2) / (Math.Pow(ss_orb.a, 2) * Math.Pow(1 - Math.Pow(ss_orb.e, 2), 2)) *
                    ss_orb.n0 * Settings.J2))) * 180) / Math.PI;
        }

        /// <summary>
        /// Iterates Newton Raphson to find the semi-major axis and inclination
        /// of a Sun Sync Orbit with Reptiotion
        /// </summary>
        /// <param name="ss_orb"></param>
        /// <param name="iterations"></param>
        public void NewtonRaphson(SunSyncOrbitRPT ss_orb,out int iterations)
        {
            NewtonRaphson nr = new Solvers.NewtonRaphson(this);

            Random r = new Random();

            //a = r.NextDouble() * (15000.0 - 4000.0) + 4000.0;
            //i = r.NextDouble() * (360.0);

            ss_orb.a = Math.Pow((Settings.u0 / Math.Pow(ss_orb.ni, 2)), 1.0 / 3.0);
            double k1 = 3.0 * Math.Pow(Settings.R0, 2) * Settings.J2 * Math.Sqrt(Settings.u0)
                / (4.0 * Math.Pow(1 - Math.Pow(ss_orb.e, 2), 2));

            ss_orb.i = Math.Acos(Settings.RANN_tx_Sunsync * Math.Pow(ss_orb.a, 3.5) / (2 * k1));

            double[,] results = nr.Calculate(ss_orb, out iterations);

            ss_orb.a = results[0, 0];
            ss_orb.i = results[1, 0];

        }

        public double partialF1_a(SunSyncOrbitRPT ss_orb)
        {
            double result = 0;
            double r0_srqt = Math.Pow(Settings.R0, 2);
            double a_3 = Math.Pow(ss_orb.a, 3);
            double e_sqrt = Math.Pow(ss_orb.e, 2);
            double n0 = Math.Sqrt(Settings.u0 / a_3);

            double _n = 3.0 * r0_srqt * Settings.J2 * Math.Cos(ss_orb.i);
            double _d = 2.0 * Math.Pow(1.0 - e_sqrt, 2);

            result = (_n / _d) * (-7.0 / (2.0 * a_3)) * n0;

            return result;
        }

        public double partialF1_i(SunSyncOrbitRPT ss_orb)
        {
            double result = 0;
            double r0_srqt = Math.Pow(Settings.R0, 2);
            double a_3 = Math.Pow(ss_orb.a, 3);
            double a_sqrt = Math.Pow(ss_orb.a, 2);
            double e_sqrt = Math.Pow(ss_orb.e, 2);
            double n0 = Math.Sqrt(Settings.u0 / a_3);

            double _n = (-3.0 * r0_srqt * Settings.J2 * Math.Sin(ss_orb.i));
            double _d = (2.0 * a_sqrt) * Math.Pow(1.0 - e_sqrt, 2);

            result = (_n / _d) * n0;

            return result;
        }

        public double partialF2_a(SunSyncOrbitRPT ss_orb)
        {
            double result = 0;
            double r0_srqt = Math.Pow(Settings.R0, 2);
            double a_3 = Math.Pow(ss_orb.a, 3);
            double a_4 = Math.Pow(ss_orb.a, 4);
            double e_sqrt = Math.Pow(ss_orb.e, 2);
            double n0 = Math.Sqrt(Settings.u0 / a_3);

            double term1 = (3.0 * n0) / (2.0 * ss_orb.a);
            double term2 = (Math.Sqrt(1.0 - e_sqrt) * (3.0 * Math.Cos(ss_orb.i) * Math.Cos(ss_orb.i) - 1.0))
                + 5.0 * Math.Cos(ss_orb.i) * Math.Cos(ss_orb.i) - 1.0;

            double _n = (21.0 * r0_srqt * Settings.J2 * term2 * n0);
            double _d = (8.0 * Math.Pow(1.0 - e_sqrt, 2) * a_3);

            result = term1 + (_n / _d);

            return result;
        }

        public double partialF2_i(SunSyncOrbitRPT ss_orb)
        {
            double result = 0;
            double r0_srqt = Math.Pow(Settings.R0, 2);
            double a_3 = Math.Pow(ss_orb.a, 3);
            double a_sqrt = Math.Pow(ss_orb.a, 2);
            double e_sqrt = Math.Pow(ss_orb.e, 2);
            double n0 = Math.Sqrt(Settings.u0 / a_3);

            double term1 = 3.0 * Math.Sqrt(1.0 - e_sqrt) * (-2.0 * Math.Cos(ss_orb.i) * Math.Sin(ss_orb.i))
                - 10.0 * Math.Cos(ss_orb.i) * Math.Sin(ss_orb.i);

            double _n = (-3.0 * r0_srqt * Settings.J2 * term1);
            double _d = (4.0 * a_sqrt * Math.Pow(1.0 - e_sqrt, 2));

            result = (_n / _d) * n0;

            return result;
        }

        public double F1(SunSyncOrbitRPT ss_orb)
        {
            double result = 0;
            double r0_srqt = Math.Pow(Settings.R0, 2);
            double a_3 = Math.Pow(ss_orb.a, 3);
            double a_sqrt = Math.Pow(ss_orb.a, 2);
            double e_sqrt = Math.Pow(ss_orb.e, 2);

            result = ((360.0 * Math.PI) / (365.2421897 * 180 * 86400)) +
                ((3.0 * r0_srqt * Settings.J2 * Math.Cos(ss_orb.i)) /
                (2.0 * a_sqrt * Math.Pow(1.0 - e_sqrt, 2))) * Math.Sqrt(Settings.u0 / a_3);

            return result;
        }

        public double F2(SunSyncOrbitRPT ss_orb)
        {
            double result = 0;
            double r0_srqt = Math.Pow(Settings.R0, 2);
            double a_3 = Math.Pow(ss_orb.a, 3);
            double a_sqrt = Math.Pow(ss_orb.a, 2);
            double e_sqrt = Math.Pow(ss_orb.e, 2);

            result = ss_orb.ni - Math.Sqrt(Settings.u0 / a_3) -
                ((3.0 * r0_srqt * Settings.J2) / (4.0 * a_sqrt * Math.Pow(1.0 - e_sqrt, 2))) *
                Math.Sqrt(Settings.u0 / a_3) *
                (Math.Sqrt(1.0 - e_sqrt) *
                (3.0 * Math.Cos(ss_orb.i)
                * Math.Cos(ss_orb.i) - 1.0) + 5.0 *
                Math.Cos(ss_orb.i) *
                Math.Cos(ss_orb.i) - 1.0);

            return result;
        }
    }
}
