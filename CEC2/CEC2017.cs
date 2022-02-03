// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

// namespace Alggs
// {
//     [CustomFunction(Enums.Funcs.CEC2017)]
//     public class CEC2017 : ProblemBase
//     {
//         private int D = 30;
//         private double[] Os;
//         private double[] Mr;




//         public override double ObjectiveFunction(params double[] parameters)
//         {
//             double[] fx = new double[] { 0 };

//             unsafe
//             {
//                 fixed (double* x = parameters)
//                 {
//                     fixed (double* f = fx)
//                     {
//                         cec17_test_func(x, f, parameters.Length, 1, Utils.Utility.funcCEC2017);
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
//     }
// }
