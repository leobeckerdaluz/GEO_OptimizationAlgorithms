using MathModelsDomain.ModelsManagers;
using MathModelsDomain.Utilities;
using SpaceConceptOptimizer.Models;
using SpaceConceptOptimizer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpaceConceptOptimizer.ModelsManager
{
    /// <summary>
    /// Manager for Propulsion Velocity and Propelant Mass
    /// </summary>
    public class PropulsionManager
    {
        //public static double V(double a, double r)
        //{
        //    return Math.Sqrt(Settings.Settings.u0 * ((2.0 / r) - (1.0 / a)));
        //}

        /// <summary>
        /// Calculates the Delta V for the changes in the major semi-axis during the
        /// orbit transfers
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double HommanTransferDeltaV_a(Propulsion p)
        {

            return (p.TransferOrbit.Vp - p.InitialOrbit.Vp) +
                (p.FinalOrbit.Vp - p.TransferOrbit.Va);
        }

        /// <summary>
        /// Calculates the Delta V for the changes of inclination between
        /// orbits
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double HommanTransferDeltaV_i(Propulsion p)
        {
            double deltaI = (p.InitialOrbit.i - p.FinalOrbit.i);

            return 2 * p.FinalOrbit.Va * Math.Sin
                (Math.Abs(deltaI) / 2.0);
        }

        private static double[] Interpolate_p(Orbit o)
        {
            double[] p = new double[Settings.Settings.SolarModes.Count];
            double hp = o.Hp / 1000.0;

            XDocument settings_p = Utility.GetSettings("MSIS");

            for (int i = 0; i < p.Length; i++)
            {
                SolarMode sm = Settings.Settings.SolarModes[i].Mode;

                IEnumerable<XElement> values =
                    from item in settings_p.Element("MSIS").Element(
                         sm.GetType().GetEnumName(sm)).
                        Descendants("value")
                    where
    (double)item.Element("Hp") == hp
                    select item;

                if (values.Count() == 0)
                {

                    XElement e0 = settings_p.Element("MSIS").Element(
                          sm.GetType().GetEnumName(sm)).
                          Descendants("value").LastOrDefault(d =>
                          (double)d.Element("Hp") < hp);

                    XElement e1 = settings_p.Element("MSIS").Element(
                    sm.GetType().GetEnumName(sm)).
                    Descendants("value").FirstOrDefault(d =>
                    (double)d.Element("Hp") > hp);

                    if (e0 == null)
                    {
                        p[i] = double.Parse(((XElement)e1.LastNode).Value);
                    }
                    else if (e1 == null)
                    {
                        p[i] = double.Parse(((XElement)e0.LastNode).Value);
                    }
                    else
                    {
                        p[i] = Utility.Interpolate(hp, double.Parse(e0.Element("Hp").Value),
                            double.Parse(((XElement)e0.LastNode).Value), double.Parse(e1.Element("Hp").Value),
                            double.Parse(((XElement)e1.LastNode).Value));
                    }


                }
                else
                {
                    p[i] = double.Parse(values.First().Value);
                }

                p[i] *= (100.0 * 100.0 * 100.0 / 1000.0);
            }

            return p;
        }

        /// <summary>
        /// Calculates de Delta V to maintaina satellite at LEO
        /// </summary>
        /// <param name="i_sp"></param>
        /// <param name="s"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        // public static double DragDeltaV(Satellite s, Orbit o, int iteration)
        // {
        //     if (iteration > 100)
        //         return s.Propulsion.DeltaV_pertubations;

        //     //Atmospheric Density, Function of Altitude
        //     double[] ps = Interpolate_p(o);

        //     //Atmospheric acceleration
        //     //double a_d = (-1.0 / 2.0) * p *
        //     //    s.Cd * s.A * Math.Pow(o.Vp, 2);

        //     s.Propulsion.DeltaV_pertubations = 0;
        //     int cnt = 0;
        //     foreach (double p in ps)
        //     {
        //         double lifeTime = s.T * 3.154e+7;
        //         lifeTime *= (Settings.Settings.SolarModes[cnt].Years / s.T);

        //         s.Propulsion.DeltaV_pertubations +=
        //             Math.PI * (s.Cd * s.A / s.M) * p * o.a * o.Vp * (lifeTime / o.Tp);
        //         cnt++;
        //     }

        //     //s.M0 = s.M - FuelConsuption(deltaVM0, Settings.Settings.ISP, s);

        //     //double precision = Math.Abs(s.M - s.M0 + s.Md + delta_M);

        //     double mp = s.M * (1 - Math.Exp(-s.Propulsion.Total_deltaV / (Settings.Settings.g0 * Settings.Settings.ISP)));
        //     double mf = s.Md + mp;
        //     double precision = Math.Abs(mf - s.M);

        //     s.Mp = mp;

        //     iteration++;
        //     if (precision < Settings.Settings.PrecisionPropulsion)
        //     {
        //         //double delta_V = Settings.Settings.g0 * Settings.Settings.ISP
        //         //    * Math.Log(s.M0 / (s.M0 + delta_M));

        //         return s.Propulsion.DeltaV_pertubations;
        //     }

        //     return DragDeltaV(s, o, iteration);

        // }



        public static double DragDeltaV(Satellite s, Orbit o, int iteration)
        {
            //Atmospheric Density, Function of Altitude
            double[] ps = Interpolate_p(o);

            for(int i=0; i<100; i++){
                s.Propulsion.DeltaV_pertubations = 0;
                int cnt = 0;

                foreach (double p in ps){
                    double lifeTime = s.T * 3.154e+7 * (Settings.Settings.SolarModes[cnt].Years / s.T);

                    s.Propulsion.DeltaV_pertubations += Math.PI * (s.Cd * s.A / s.M) * p * o.a * o.Vp * (lifeTime / o.Tp);
                    
                    cnt++;
                }
                
                double mp = s.M * (1 - Math.Exp(-s.Propulsion.Total_deltaV / (Settings.Settings.g0 * Settings.Settings.ISP)));
                double mf = s.Md + mp;
                double precision = Math.Abs(mf - s.M);

                s.Mp = mp;

                if (precision < Settings.Settings.PrecisionPropulsion){
                    break;
                }
            }

            return s.Propulsion.DeltaV_pertubations;
        }




        /// <summary>
        /// Calculates de Delta V required to put the satellite in De-orbit
        /// </summary>
        /// <param name="f"></param>
        /// <param name="deorbit"></param>
        /// <returns></returns>
        public static double DeltaV_DeOrbit(Orbit f, Orbit deorbit)
        {
            //Wertz
            //double deltaV = f.Vp * (1 -
            //    Math.Sqrt((2.0 * (Settings.Settings.R0 + deorbit.Hp)) /
            //    (2.0 * Settings.Settings.R0 + deorbit.Hp + f.Hp)));

            double deltaV = f.Va - deorbit.Va;

            return deltaV;
        }

        /// <summary>
        /// Calculate the propelent mass for an specific propelent 
        /// based on its Delta V
        /// </summary>
        /// <param name="deltaV"></param>
        /// <param name="i_sp"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double TotalMass(double deltaV, Satellite s)
        {
            double mf = s.M *
                Math.Exp(-deltaV / (Settings.Settings.ISP * Settings.Settings.g0));

            return mf;
        }

        /// <summary>
        /// Stimates the total mass of the satellite based on Wertz
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double StimateTotalMass(Satellite s)
        {
            return s.Md * 1.27;
        }



    }
}
