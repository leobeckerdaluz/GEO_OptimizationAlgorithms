using SpaceConceptOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.ModelsManager
{
    public class SatelliteManager
    {
        /// <summary>
        /// Calculates the Cross Sectional Area of a Satellite
        /// based on its Power and Mass
        /// </summary>
        /// <param name="power"></param>
        /// <param name="dry_mass"></param>
        /// <returns></returns>
        public static double CrossSectionalArea(double power,
            double dry_mass)
        {
            //double a =  0.026175*power - 0.00796176 * dry_mass;

            //if (a < 0)
            //    a = 0.015 * power;

            double a = 0.0216 * dry_mass;

            //double a = 0.015 * power;
            return a;
        }

        /// <summary>
        /// For satellites of +500W the payload power is
        /// 40-80% of its total power (Wertz)
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static double PowerFromCamera(Camera camera)
        {
            return camera.Power * 1/0.46;
        }


        /// <summary>
        /// For satellites the mass of the payload is
        /// 2 - 7 times less than the satellite's, so the average
        /// satellite mass is 3.3 times the mass of its payload
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static double DryMassFromCamera(Camera camera)
        {
            return (camera.WeightOpt+camera.WeightElec) * 1/0.31;
        }
    }
}
