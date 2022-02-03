// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.InteropServices;
// using System.Text;
// using System.Threading.Tasks;

// namespace Alggs1
// {
//     // [CustomFunction(Enums.Funcs.CEC2017)]
//     public class Alggs2
//     {
//         [DllImport("DebugCEC2017/CEC2017.dll", CallingConvention = CallingConvention.Cdecl)]
//         private unsafe extern static void cec17_test_func(double* x, double* f, int nx, int mx, int func_num);


//         public static double ObjectiveFunction(params double[] parameters)
//         {
//             int fun_num = 2;


//             double[] fx = new double[] { 0 };

//             unsafe
//             {
//                 fixed (double* x = parameters)
//                 {
//                     fixed (double* f = fx)
//                     {
//                         cec17_test_func(x, f, parameters.Length, 1, fun_num);
//                     }
//                 }
//             }  

//             //double[] rsParameters = ShiftAndRotate(parameters, Os, Mr, 1);
//             //double fx = Math.Pow(rsParameters[0], 2);
//             //double sum = 0;
//             //int i = 0;
//             //for (i = 1; i < rsParameters.Length - 1; i++)
//             //{
//             //    sum += Math.Pow(rsParameters[i], 2);
//             //}

//             //fx += Math.Pow(10, 6) * sum;
//             //fx += 100;
            
//             return fx[0];
//         }
    
    
    
    
//         public static void Main(string[] args)
//         {    
//             Console.WriteLine("Rodando!");

//             ObjectiveFunction();

//             Console.WriteLine("Done!");
//         }
//     }
// }
