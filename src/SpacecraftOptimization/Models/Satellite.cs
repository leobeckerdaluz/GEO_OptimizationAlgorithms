// using AlgoritimosEvolutivos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.Models
{
    /// <summary>
    /// This class respresents a Satellite
    /// </summary>
    public class Satellite:ICloneable
    {
        /// <summary>
        /// Satellite's payload
        /// </summary>
        public Camera Payload { get; set; }

        /// <summary>
        /// Satellite's Propulsion
        /// </summary>
        public Propulsion Propulsion { get; set; }

        /// <summary>
        /// Satellite's Power
        /// </summary>
        public double Power { get; set; }

        /// <summary>
        /// Drag Coefficient (User Entry)
        /// </summary>
        public double Cd { get; set; }

        /// <summary>
        /// Cross-Sctional Area
        /// </summary>
        public double A { get; set; }

        /// <summary>
        /// Total Area
        /// </summary>
        public double At { get; set; }

        /// <summary>
        /// Total Mass
        /// </summary>
        public double M
        {
            get
            {
                return Md + Mp;
            }
        }

        /// <summary>
        /// Mass after orbit aquisition
        /// </summary>
        public double M0 { get; set; }

        /// <summary>
        /// Dry Mass
        /// </summary>
        public double Md { get; set; }

        /// <summary>
        /// Propellent Mass
        /// </summary>
        public double Mp { get; set; }

        /// <summary>
        /// Mission Life Time (User Entry)
        /// </summary>
        public double T { get; set; }

        public object Clone()
        {
            return new object();//Utility.InstantiateFunction(this);
        }


    }
}
