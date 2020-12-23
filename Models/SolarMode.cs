using SpaceConceptOptimizer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.Models
{
    // public enum SolarMode
    // {
    //     Low,
    //     MediumLow,
    //     MediumHigh,
    //     High
    // }

    
    public class SolarModeModel
    {
        public SolarMode Mode { get; set; }
        

        public int Years { get; set; }

        public SolarModeModel(SolarMode mode, int years)
        {
            Mode = mode;
            Years = years;
        }
    }
}
