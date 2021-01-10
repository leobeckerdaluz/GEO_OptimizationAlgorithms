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
        public double N { get; set; }
        public double D { get; set; }

        public double Rev
        {
            get
            {
                return I + N / D; ;
            }
        }

        public SunSyncOrbitRPT(double i, double n, double d, double _e):
            base((i+n/d) * (2 * Math.PI / 86400),_e)
        {
            I = i;
            N = n;
            D = d;
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
            obr.N = this.N;
            obr.D = this.D;

            return obr;
        }
    }
}
