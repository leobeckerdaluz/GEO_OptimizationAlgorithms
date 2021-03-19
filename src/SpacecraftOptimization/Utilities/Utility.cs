using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MathModelsDomain.Utilities
{
    /// <summary>
    /// Utility Tools
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Normalize Degrees
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        public static double Convert(double i)
        {
            i = i.RadiansToDegrees();
            if (Math.Abs(i) > 360.0)
            {
                i = 360.0 * (i / 360 - Math.Truncate(i / 360));
            }

            return i;
        }

        /// <summary>
        /// Normalize Degrees
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        public static void Convert(ref double a, ref double i)
        {
            i = i.RadiansToDegrees();
            if (Math.Abs(i) > 360.0)
            {
                i = 360.0 * (i / 360 - Math.Truncate(i / 360));
            }

            a /= 1000;
        }

        /// <summary>
        /// Converts Degree to Radian
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double DegreesToRadians(this double degree)
        {
            return degree * Math.PI / 180.0;
        }

        /// <summary>
        /// Convert Radian to Degree
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double RadiansToDegrees(this double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        /// <summary>
        /// Get an XML Setting File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static XDocument GetSettings(string fileName)
        {
            // return XDocument.Load(Environment.CurrentDirectory+"\\Settings\\" + fileName + ".xml");
            return XDocument.Load(Environment.CurrentDirectory+"//SpacecraftOptimization//Settings//" + fileName + ".xml");
        }

        /// <summary>
        /// Interpolate two points
        /// </summary>
        /// <param name="x"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns></returns>
        public static double Interpolate(double x,double x0,double y0, double x1,double y1)
        {
            return y0 + (y1 - y0) * ((x - x0) / (x1 - x0));
        }
    }
}
