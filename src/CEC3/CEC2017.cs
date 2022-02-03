using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace CppBind
{
    public class CEC2017
    {
        // private int D = 30;
        // private double[] Os;
        // private double[] Mr;


        [DllImport(@"/home/lbluz/Documents/GEO_OptimizationAlgorithms/src/CEC3/libhello-cpp.so")]
        public static extern void PrintHelloWorld();

        // static void Main(string[] args)
        // {
        //     PrintHelloWorld();

        //     double fx = ObjectiveFunction();
        //     Console.Write(fx);

            
        // }
        

        // public double ObjectiveFunction(params double[] parameters)
        public static double ObjectiveFunction()
        {
            double[] fx = new double[] { 0 };

            // unsafe
            // {
            //     fixed (double* x = parameters)
            //     {
            //         fixed (double* f = fx)
            //         {
            //             cec17_test_func(x, f, parameters.Length, 1, Utils.Utility.funcCEC2017);
            //         }
            //     }
            // }   
            // return fx[0];


            Console.WriteLine("TESTANDO SOM AQUI DENTRO!");


            return 2;
            
            
            //double[] rsParameters = ShiftAndRotate(parameters, Os, Mr, 1);
            //double fx = Math.Pow(rsParameters[0], 2);
            //double sum = 0;
            //int i = 0;
            //for (i = 1; i < rsParameters.Length - 1; i++)
            //{
            //    sum += Math.Pow(rsParameters[i], 2);
            //}

            //fx += Math.Pow(10, 6) * sum;
            //fx += 100;
            
        }
    }
}