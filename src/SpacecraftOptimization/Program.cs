// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.GEOS;
// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.GEOS.Especies;
// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.GEOS.GEO;
// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.GEOS.GEO_T;
// //using AlgoritimosEvolutivos.Utils;
// using MathModelsDomain.ModelsManagers;
// using MathModelsDomain.Utilities;
// using SpaceConceptOptimizer.Models;
// using SpaceConceptOptimizer.ModelsManager;
// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

// namespace SpaceConceptOptimizer
// {
//     class Program
//     {
//         static Tuple<double, double, double> Operation(SunSyncOrbitRPT ss_orb,
//             Camera referencePayload)
//         {
//             int iterations = 0;
//             double a = 0;
//             double i = 0;

//             SunSyncOrbitsWRptManager ss_mg
//                 = new SunSyncOrbitsWRptManager();

//             ss_mg.NewtonRaphson(ss_orb, out iterations);

//             //ss_mg.NewtonRaphson(ss_orb, out iterations);

//             a = ss_orb.a;
//             i = ss_orb.i;

//             Utility.Convert(ref a, ref i);
//             ss_orb.i = i.DegreesToRadians();

//             double fov = FOVManager.FovMin(ss_orb);

//             Camera designedPayload =
//             CameraManager.DesignCamera(referencePayload, ss_orb, fov);

//             Satellite s = new Satellite();

//             s.T = 4;
//             s.Cd = 2.2;
//             s.Md = SatelliteManager.DryMassFromCamera(designedPayload);
//             s.Power = SatelliteManager.PowerFromCamera(designedPayload);

//             s.A = SatelliteManager.CrossSectionalArea(s.Power,
//                 s.Md);

//             s.At = Settings.Settings.AreaPercentOverSectionalArea * s.A;

//             Orbit deorbit = DeOrbitManager.DeOrbit(s, ss_orb);


//             Propulsion p =
//             new Propulsion(Settings.Settings.LaunchAltitudeError,
//             Settings.Settings.LaunchInclinationError, ss_orb, deorbit);
//             s.Propulsion = p;

//             p.DeltaV_a = PropulsionManager.HommanTransferDeltaV_a(p);
//             p.DeltaV_i = PropulsionManager.HommanTransferDeltaV_i(p);
//             p.DeltaV_deorbit = PropulsionManager.DeltaV_DeOrbit(p.FinalOrbit, p.DeOrbit);

//             s.Mp = PropulsionManager.StimateTotalMass(s) - s.Md;
//             p.DeltaV_pertubations = PropulsionManager.DragDeltaV(s, ss_orb, 0);


//             //Console.WriteLine("a: {0}, i: {1}, it: {2}, fov: {3}," +
//             //    "Hp(Odo): {4}, dVdo: {5}, " +
//             //    "dVa: {6}, dVi: {7}, dVdrag: {8}, dV: {9}, " +
//             //    "Md {10}, Acs: {11}, At: {12}, Mp: {13}, Mt: {14}, P:{15}, Pp:{16}, Mpay: {17}",
//             //    a, i, iterations, fov,
//             //    deorbit.Hp / 1000, p.DeltaV_deorbit,
//             //    p.DeltaV_a, p.DeltaV_i, p.DeltaV_pertubations, p.Total_deltaV,
//             //    s.Md, s.A, s.At, s.Mp, s.M, s.Power, designedPayload.Power, designedPayload.WeightOpt);

//             return new Tuple<double, double, double>(fov, designedPayload.FOV, s.M);
//         }

//         static void SaveResults(TimeSpan time,double nfob, double std, double bestnfob, Dictionary<string, double> paramResults,
//             Optimizer res, string name)
//         {
//             Dictionary<string, double> DeOrbitResults = new Dictionary<string, double>();

//             DeOrbitResults.Add("a", res.Deorbit.a / 1000);
//             DeOrbitResults.Add("e", res.Deorbit.e);
//             DeOrbitResults.Add("Ha", res.Deorbit.Ha / 1000);
//             DeOrbitResults.Add("Hp", res.Deorbit.Hp / 1000);
//             DeOrbitResults.Add("i", res.Deorbit.i.RadiansToDegrees());
//             DeOrbitResults.Add("Ra", res.Deorbit.Ra / 1000);
//             DeOrbitResults.Add("Rp", res.Deorbit.Rp / 1000);
//             DeOrbitResults.Add("Va", res.Deorbit.Va);
//             DeOrbitResults.Add("Vp", res.Deorbit.Vp);

//             Dictionary<string, double> SatelliteResults = new Dictionary<string, double>();

//             SatelliteResults.Add("Acs", res.Satellite.A);
//             SatelliteResults.Add("A", res.Satellite.At);
//             SatelliteResults.Add("Cd", res.Satellite.Cd);
//             SatelliteResults.Add("M", res.Satellite.M);
//             SatelliteResults.Add("Md", res.Satellite.Md);
//             SatelliteResults.Add("Mp", res.Satellite.Mp);
//             SatelliteResults.Add("Wpld", res.Satellite.Payload.WeightOpt + res.Satellite.Payload.WeightElec);
//             SatelliteResults.Add("Ppld", res.Satellite.Payload.Power);
//             SatelliteResults.Add("FOV", res.Satellite.Payload.FOV);
//             SatelliteResults.Add("P", res.Satellite.Power);
//             SatelliteResults.Add("Lifetime", res.Satellite.T);


//             Dictionary<string, double> PropulsionResults = new Dictionary<string, double>();

//             PropulsionResults.Add("Delta Va", res.Satellite.Propulsion.DeltaV_a);
//             PropulsionResults.Add("Delta Vdo", res.Satellite.Propulsion.DeltaV_deorbit);
//             PropulsionResults.Add("Delta Vi", res.Satellite.Propulsion.DeltaV_i);
//             PropulsionResults.Add("Delta Vdrag", res.Satellite.Propulsion.DeltaV_pertubations);
//             PropulsionResults.Add("Delta V", res.Satellite.Propulsion.Total_deltaV);

//             Dictionary<string, double> OrbitResults = new Dictionary<string, double>();

//             OrbitResults.Add("a", res.Ss_orb.a / 1000);
//             OrbitResults.Add("e", res.Ss_orb.e);
//             OrbitResults.Add("Ha", res.Ss_orb.Ha / 1000);
//             OrbitResults.Add("Hp", res.Ss_orb.Hp / 1000);
//             OrbitResults.Add("i", res.Ss_orb.i.RadiansToDegrees());
//             OrbitResults.Add("Ra", res.Ss_orb.Ra / 1000);
//             OrbitResults.Add("Rp", res.Ss_orb.Rp / 1000);
//             OrbitResults.Add("Va", res.Ss_orb.Va);
//             OrbitResults.Add("Vp", res.Ss_orb.Vp);
//             OrbitResults.Add("FoVMin", res.FovMin);

//             StringBuilder str = new StringBuilder();

//             AlgoritimosEvolutivos.Utils.Utility.WriteCsvLine(str, "Rev Params");
//             AlgoritimosEvolutivos.Utils.Utility.ToCSV(str, paramResults);
//             AlgoritimosEvolutivos.Utils.Utility.JumpLines(str, 3);

//             AlgoritimosEvolutivos.Utils.Utility.WriteCsvLine(str, "Final Orbit");
//             AlgoritimosEvolutivos.Utils.Utility.ToCSV(str, OrbitResults);
//             AlgoritimosEvolutivos.Utils.Utility.JumpLines(str, 3);

//             AlgoritimosEvolutivos.Utils.Utility.WriteCsvLine(str, "Propulsion");
//             AlgoritimosEvolutivos.Utils.Utility.ToCSV(str, PropulsionResults);
//             AlgoritimosEvolutivos.Utils.Utility.JumpLines(str, 3);

//             AlgoritimosEvolutivos.Utils.Utility.WriteCsvLine(str, "DeOrbit");
//             AlgoritimosEvolutivos.Utils.Utility.ToCSV(str, DeOrbitResults);
//             AlgoritimosEvolutivos.Utils.Utility.JumpLines(str, 3);

//             AlgoritimosEvolutivos.Utils.Utility.WriteCsvLine(str, "Satellite");
//             AlgoritimosEvolutivos.Utils.Utility.ToCSV(str, SatelliteResults);
//             AlgoritimosEvolutivos.Utils.Utility.JumpLines(str, 3);

//             AlgoritimosEvolutivos.Utils.Utility.WriteCsvLine(str, "Best NFE;" + bestnfob);
//             AlgoritimosEvolutivos.Utils.Utility.WriteCsvLine(str, "Mean NFE;" + nfob);
//             AlgoritimosEvolutivos.Utils.Utility.WriteCsvLine(str, "STD NFE;" + std);
//             AlgoritimosEvolutivos.Utils.Utility.WriteCsvLine(str, "Time (sec);" + time.TotalSeconds);

//             AlgoritimosEvolutivos.Utils.Utility.SaveStrBuilderFile(str, "MDO/" + name + ".csv");
//             res.Output();
//             //Console.WriteLine("NFOB: " + nfob/50.0);
//             Console.WriteLine();
//         }


//         static void GEO<TGEO, TPop>(double tao, Optimizer opt, double[] lowerLimits,
//             double[] upperLimits, int[] sizeParams) where TPop : Populacao
//             where TGEO : BaseGEO<TPop>
//         {
//             DateTime dtSt = DateTime.Now;
//             List<double> nfobs = new List<double>();
//             double nfob = 0;
//             TGEO best = null;
//             for (int i = 0; i < 100; i++)
//             {
//                 TGEO geo = null;
//                 if (typeof(TGEO) == typeof(GEO))
//                     geo = new GEO(opt, lowerLimits, upperLimits, sizeParams, 2 + 6 + 6, tao, i
//                    , true, true, true) as TGEO;
//                 else if (typeof(TGEO) == typeof(GEOt))
//                     geo = new GEOt(opt, lowerLimits, upperLimits, sizeParams, 2 + 6 + 6, i,
//                         true, true, true) as TGEO;
//                 else
//                     geo = new GEOt2(opt, lowerLimits, upperLimits, sizeParams, 2 + 6 + 6, i,
//                         true, true, true) as TGEO;

//                 TPop result = geo.Init();

//                 if (result.ValidSpace)
//                 {
//                     if (best == null || best.NFOB > geo.NFOB)
//                     {
//                         if (best != null)
//                         {
//                             if (best.NFOB > result.NFOB)
//                                 best = (TGEO)((TGEO)geo).Clone();
//                         }
//                         else
//                             best = (TGEO)((TGEO)geo).Clone();
//                     }

//                     nfobs.Add(geo.NFOB);
//                     nfob += geo.NFOB;

//                     Console.WriteLine("Seed: {0}, NFE: {1}",
//                         i, result.NFOB);
//                 }

//             }

//             //nfob = best.NFOB;
//             double var = 0;
//             nfob /= 100;

//             foreach (double nfo in nfobs)
//             {
//                 var += Math.Pow(nfo - nfob, 2);
//             }

//             double std = Math.Sqrt(var / (nfobs.Count - 1));

//             Dictionary<string, double> paramResults = new Dictionary<string, double>();

//             TPop bestPop = best.PHistBest;

//             if (bestPop == null)
//                 bestPop = best.PBest;

//             paramResults.Add("I", (int)bestPop.MultiParamFenotipo[0]);
//             paramResults.Add("N", (int)bestPop.MultiParamFenotipo[1]);
//             paramResults.Add("D", (int)bestPop.MultiParamFenotipo[2]);

//             Console.WriteLine("I: {0}, N: {1}, D: {2}",
//                 (int)bestPop.MultiParamFenotipo[0], (int)bestPop.MultiParamFenotipo[1],
//                 (int)bestPop.MultiParamFenotipo[2]);

//             Optimizer res = ((Optimizer)bestPop.Function);

//             DateTime dtEnd = DateTime.Now;
//             Console.WriteLine("Time: " + (dtEnd - dtSt));
//             if (typeof(TGEO) == typeof(GEO))
//                 SaveResults((dtEnd - dtSt), nfob, std, best.NFOB, paramResults, res, "FixGEO_100_MDO" + tao);
//             else if (typeof(TGEO) == typeof(GEOt))
//                 SaveResults((dtEnd - dtSt),nfob, std, best.NFOB, paramResults, res, "FixGEOt_100_MDO");
//             else
//                 SaveResults((dtEnd - dtSt),nfob, std, best.NFOB, paramResults, res, "FixGEOt2_100_MDO");
//         }



//         static void Main(string[] args)
//         {

//             CultureInfo.CurrentCulture = new CultureInfo("en-US");
//             Settings.Settings.SolarModes = new List<SolarModeModel>();

//             Settings.Settings.SolarModes.Add(
//                 new SolarModeModel(Utilities.SolarMode.High, 1));

//             Settings.Settings.SolarModes.Add(
//                 new SolarModeModel(Utilities.SolarMode.MediumHigh, 3));

//             Settings.Settings.LaunchAltitudeError = 20000;
//             Settings.Settings.LaunchInclinationError = (0.15).DegreesToRadians();
//             Settings.Settings.ISP = 225;
//             Settings.Settings.PrecisionPropulsion = 10e-5;

//             //Settings.Settings.AreaPercentOverSectionalArea = 4.37;
//             Settings.Settings.AreaPercentOverSectionalArea = 1.8;

//             Settings.Settings.V0 = 6.778309031049825E+03;
//             Settings.Settings.MissionResolution = 20;



//             double D = 60;

//             Optimizer opt = new Optimizer(1, true);

//             double[] lowerLimits = new double[] { 13, 0, 1 };
//             double[] upperLimits = new double[] { 15, D - 1, D };
//             int[] sizeParams = new int[] { 2, 6, 6 };

//             Console.WriteLine("Chose the algoritm");
//             Console.WriteLine("1 - GEO");
//             Console.WriteLine("2 - GEOt");
//             Console.WriteLine("3 - GEOt2");

//             int alg = int.Parse(Console.ReadLine());

//             double minTau = 0;
//             double maxTau = 0;
//             double stepTau = 0;

//             if (alg == 1)
//             {
//                 Console.WriteLine("Escolha um tau inicial");
//                 minTau = double.Parse(Console.ReadLine());

//                 Console.WriteLine("Escolha um incremento de tau");
//                 stepTau = double.Parse(Console.ReadLine());

//                 Console.WriteLine("Escolha um tau maximo");
//                 maxTau = double.Parse(Console.ReadLine());
//             }

//             double tau = minTau;
//             if (alg == 1)
//                 for (tau = minTau; tau <= maxTau; tau += stepTau)
//                 {
//                     Console.WriteLine("TAO: " + tau);
//                     GEO<GEO, Populacao>(tau, opt, lowerLimits, upperLimits, sizeParams);
//                     Console.Clear();
//                 }
//             else if (alg == 2)
//                 GEO<GEOt, PopulacaoT>(tau, opt, lowerLimits, upperLimits, sizeParams);
//             else
//                 GEO<GEOt2, PopulacaoT>(tau, opt, lowerLimits, upperLimits, sizeParams);

//             //ExtensiveSearchSpace(opt);
//             //SunSyncOrbitRPT ss_orb = new SunSyncOrbitRPT(14, 59, 60, 0.00);

//             //Tuple<double, double, double> m = Operation(ss_orb, refPayload);

//             Console.ReadLine();
//             Console.Clear();


//         }

//         public static void ExtensiveSearchSpace(Optimizer opt)
//         {
//             DateTime st = DateTime.Now;

//             StringBuilder MDOExtensiveSearchValidRes = new StringBuilder();
//             StringBuilder MDOExtensiveSearchInvalidRes = new StringBuilder();

//             List<Tuple<int, int, int, double>> extensiveResults =
//                     new List<Tuple<int, int, int, double>>();

//             for (int j = 0; j < 1; j++)
//             {
//                 //Camera refPayload = new Camera(109, 26.5, 41.71, 0.048576886, 20,
//                 //0.242884427939569, 12000, 6.5E-6);

//                 extensiveResults =
//                     new List<Tuple<int, int, int, double>>();

//                 MDOExtensiveSearchValidRes = new StringBuilder();
//                 MDOExtensiveSearchInvalidRes = new StringBuilder();


//                 for (int i = 13; i <= 15; i++)
//                 {
//                     for (int n = 1; n <= 59; n++)
//                     {
//                         for (int d = 1; d <= 60; d++)
//                         {

//                             //SunSyncOrbitRPT ss_orb = new SunSyncOrbitRPT(i, n, d, 0.00);

//                             //Tuple<double, double, double> m = Operation(ss_orb, refPayload);

//                             double m = opt.ObjectiveFunction(i, n, d);

//                             extensiveResults.Add(
//                                 new Tuple<int, int, int, double>(i, n, d, m));

//                             if(opt.ValidateRestrictions(i,n,d))
//                             {
//                                 MDOExtensiveSearchValidRes.AppendLine(i + ";" + n + ";" + d + ";" + m);
//                             }
//                             else
//                             {
//                                 MDOExtensiveSearchInvalidRes.AppendLine(i + ";" + n + ";" + d + ";" + m);
//                             }
//                             //if (n < d && (double)n % d != 0 && m.Item2 >= 1.05 * m.Item1)
//                             //{
//                             //    MDOExtensiveSearchValidRes.AppendLine(i + ";" + n + ";" + d + ";" + m.Item3);
//                             //}
//                             //else
//                             //{
//                             //    MDOExtensiveSearchInvalidRes.AppendLine(i + ";" + n + ";" + d + ";" + m.Item3);
//                             //}
//                         }
//                     }
//                 }

//                 extensiveResults = extensiveResults.OrderBy(r => r.Item4).ToList();

//                 Console.WriteLine("T:{0}, I:{1}, N:{2}, D:{3}, M:{4}",j, extensiveResults.First().Item1,
//                     extensiveResults.First().Item2, extensiveResults.First().Item3,
//                     extensiveResults.First().Item4);
//             }
//             DateTime end = DateTime.Now;
//             TimeSpan time = end - st;

//             //foreach (var res in extensiveResults)
//             //{
               
//             //}

//             MDOExtensiveSearchValidRes.AppendLine("Time (sec); " + time.TotalSeconds);
//             MDOExtensiveSearchInvalidRes.AppendLine("Time (sec); " + time.TotalSeconds);

//             AlgoritimosEvolutivos.Utils.Utility.SaveStrBuilderFile(MDOExtensiveSearchValidRes,
//                 "MDO/MDOExtensiveSearchValidResTime.csv");

//             AlgoritimosEvolutivos.Utils.Utility.SaveStrBuilderFile(MDOExtensiveSearchInvalidRes,
//                 "MDO/MDOExtensiveSearchInvalidResTime.csv");
//         }
//     }
// }
