// using AlgoritimosEvolutivos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.Models
{
    /// <summary>
    /// Represents the Propulsion Model for the lifecycle of the
    /// spacecraft
    /// </summary>
    public class Propulsion:ICloneable
    {
        /// <summary>
        /// Error of the Laucher for major semi-axis (% - User Entry)
        /// </summary>
        public double a_error { get; set; }

        /// <summary>
        /// Error of the Laucher for inclination *% - User Entry)
        /// </summary>
        public double i_error { get; set; }

        /// <summary>
        /// Orbit put by the laucher
        /// </summary>
        public Orbit InitialOrbit { get; set; }

        /// <summary>
        /// Calculated orbit for transfer
        /// </summary>
        public Orbit TransferOrbit { get; set; }

        /// <summary>
        /// Nominal Orbit
        /// </summary>
        public Orbit FinalOrbit { get; set; }

        /// <summary>
        /// De-Orbit
        /// </summary>
        public Orbit DeOrbit { get; set; }

        /// <summary>
        /// Change in velocity for transfer of 
        /// major semi-axis during Homman Transfer
        /// </summary>
        public double DeltaV_a { get; set; }

        /// <summary>
        /// Change in velocity for transfer of
        /// inclination during Homman Transfer
        /// </summary>
        public double DeltaV_i { get; set; }

        /// <summary>
        /// Change in velocity to maintain the satellite's
        /// orbit
        /// </summary>
        public double DeltaV_pertubations { get; set; }

        /// <summary>
        /// Cahnge in velocity needed to garantee the de-orbit
        /// </summary>
        public double DeltaV_deorbit { get; set; }

        /// <summary>
        /// Total Change in Velocity
        /// </summary>
        public double Total_deltaV
        {
            get
            {
                return Math.Abs(DeltaV_a) + Math.Abs(DeltaV_i) +
                 Math.Abs(DeltaV_pertubations) + Math.Abs(DeltaV_deorbit);
            }
        }

        public Propulsion() { }

        public Propulsion(double _a_error,
            double _i_error, Orbit _finalOrbit,
            Orbit _deorbit)
        {
            a_error = _a_error;
            i_error = _i_error;


            FinalOrbit = _finalOrbit;

            InitialOrbit = (Orbit)FinalOrbit.Clone();
            InitialOrbit.a -= a_error;
            InitialOrbit.i -= i_error;

            //Consider Circular
            InitialOrbit.e = 0.0;

            //Find Transfer Orbit
            TransferOrbit = new Orbit(InitialOrbit, FinalOrbit);

            DeOrbit = _deorbit;
        }

        public object Clone()
        {
            return new object();//Utility.InstantiateFunction(this);
        }
    }
}
