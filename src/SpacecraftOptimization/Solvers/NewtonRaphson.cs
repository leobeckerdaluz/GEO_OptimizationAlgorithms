using MathModelsDomain.ModelsManagers;
using MathModelsDomain.Utilities;
using MathNet.Numerics.LinearAlgebra;
using SpaceConceptOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathModelsDomain.Solvers
{
    public class NewtonRaphson 
    {
        //public SunSyncOrbitRPT ss_orb { get; set; }

        public Func<SunSyncOrbitRPT, double> F1 { get; set; }
        public Func<SunSyncOrbitRPT, double> F2 { get; set; }
        public Func<SunSyncOrbitRPT, double> F1da { get; set; }
        public Func<SunSyncOrbitRPT, double> F1di { get; set; }
        public Func<SunSyncOrbitRPT, double> F2da { get; set; }
        public Func<SunSyncOrbitRPT, double> F2di { get; set; }

        public NewtonRaphson(SunSyncOrbitsWRptManager sso_mg)
        {
            F1 = sso_mg.F1;
            F2 = sso_mg.F2;
            F1da = sso_mg.partialF1_a;
            F1di = sso_mg.partialF1_i;
            F2da = sso_mg.partialF2_a;
            F2di = sso_mg.partialF2_i;
        }

        public double[,] Calculate(SunSyncOrbitRPT ss_orb, out int iterations)
        {
           

            double[,] x = new double[2, 1];
            double[,] F1_F2 = new double[2, 1];
            double[,] _j = new double[2, 2];

            iterations = 0;
            //i = i.DegreesToRadians();
            //a = a * 1000;

            //Console.WriteLine("ai: {0}, ii: {1}",a/1000.0,i.RadiansToDegrees());

            while (true)
            {
                iterations++;

                x[0, 0] = ss_orb.a;
                x[1, 0] = ss_orb.i;

                F1_F2[0, 0] = F1(ss_orb);
                F1_F2[1, 0] = F2(ss_orb);

                _j[0, 0] = F1da(ss_orb);
                _j[0, 1] = F1di(ss_orb);
                _j[1, 0] = F2da(ss_orb);
                _j[1, 1] = F2di(ss_orb);

                Matrix<double> xs = Matrix<double>.Build.DenseOfArray(
                  x);

                Matrix<double> fs = Matrix<double>.Build.DenseOfArray(
                  F1_F2);

                Matrix<double> j = Matrix<double>.Build.DenseOfArray(
                  _j);

                j = j.Inverse();

                //Console.WriteLine("f1: {0}, f2: {1}", fs[0, 0], fs[1, 0]);

                xs = xs.Subtract(j.Multiply(fs));

                

                //if (Math.Abs(Math.Abs(xs[0, 0]) - Math.Abs(a)) <= 1e-8 &&
                //    Math.Abs(Math.Abs(xs[1, 0]) - Math.Abs(i)) <= 1e-8)
                //    return xs.ToArray();
                if (Math.Abs(fs[0, 0]) <= 1e-18 &&
                    Math.Abs(fs[1, 0]) <= 1e-18)
                    return xs.ToArray();
                else
                {
                    ss_orb.a = xs[0, 0];
                    ss_orb.i = xs[1, 0];

                    if (double.IsNaN(ss_orb.a) || double.IsNaN(ss_orb.i))

                    {
                        Random r = new Random();

                        ss_orb.a = r.NextDouble() * (15000.0 - 4000.0) + 4000.0;
                        ss_orb.a *= 1000;
                        ss_orb.i = r.NextDouble() * (360.0);
                        ss_orb.i = ss_orb.i.DegreesToRadians();
                    }


                    //if(Math.Abs(i)> (360.0).DegreesToRadians())
                    //{
                    //    i = i - Math.Truncate(i);
                    //}

                }



                if (iterations%100000 == 0){
                    Console.WriteLine("while true!");
                }


            }
            
            
            //double[,] J = { { partialF1_a(), partialF1_i()},
            //{ partialF2_a(),partialF2_i() } };

            //double[] fx = { F1(), F2() };
        }
    }
}
