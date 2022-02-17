
// using LibOptimization;
// using System;



// Console.WriteLine("Rodando!");

// //Instantiation objective Function
// var func = new LibOptimization.BenchmarkFunction.clsBenchRosenblock(2);

// //Instantiation optimization class and set objective function.
// var opt = new LibOptimization.Optimization.clsOptPSO(func);

// opt.Init();

// // //Do calc
// opt.DoIteration();


// //per 100 iteration
// while (opt.DoIteration(100) == false)
// {
//     LibOptimization.Util.clsUtil.DebugValue(opt, ai_isOutValue: false);
// }
// LibOptimization.Util.clsUtil.DebugValue(opt);




// // //Check Error
// // if (opt.IsRecentError() == true)
// // {
// //     return;
// // }
// // else
// // {
// //     //Get Result
// //     LibOptimization.Util.clsUtil.DebugValue(opt);
// // }



// Console.WriteLine("Done!");