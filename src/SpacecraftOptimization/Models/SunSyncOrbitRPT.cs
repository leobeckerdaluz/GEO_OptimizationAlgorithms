using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.Models
{
    public class SunSyncOrbitRPT : Orbit, ICloneable
    {
        public double I { get; set; }
        public double D { get; set; }
        public double Q { get; set; }

        public double Rev
        {
            get
            {
                return I + Q / D; ;
            }
        }

        public SunSyncOrbitRPT(double i, double q, double d, double _e):
            base((i+q/d) * (2 * Math.PI / 86400),_e)
        {
            I = i;
            D = d;
            Q = q;
        }

        public SunSyncOrbitRPT() { }

        public new object Clone()
        {
            SunSyncOrbitRPT obr = new SunSyncOrbitRPT();
            obr.a = this.a;
            obr.e = this.e;
            obr.i = this.i;
            obr.ni = this.ni;
            obr.I = this.I;
            obr.Q = this.Q;
            obr.D = this.D;

            return obr;
        }
    }
}
