// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.GEOS.GEO;
// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.GEOS.GEO_T;
// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.GEOS.GEO_V;
// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.Problems;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.GEOS.GEO_P;
// using SpaceConceptOptimizer.Models;
// using MathModelsDomain.ModelsManagers;
// using MathModelsDomain.Utilities;
// using SpaceConceptOptimizer.ModelsManager;
// using AlgoritimosEvolutivos.AlgoritimosEvolutivos.GEOS.Especies;

// namespace SpaceConceptOptimizer
// {
//     public partial class Optimizer : ProblemBase, ICloneable
//     {
//         public SunSyncOrbitRPT Ss_orb { get; set; }
//         public Camera ReferencePayload { get; set; }
//         public double FovMin { get; set; }
//         public Satellite Satellite { get; set; }
//         public Orbit Deorbit { get; set; }

//         public Optimizer(int SnapNFOBInc, bool realIndividuo) : base(null, SnapNFOBInc, realIndividuo) { }

//         public Optimizer(Func<double?, bool> criterioParada, int SnapNFOBInc, bool realIndividuo) :
//             base(criterioParada, SnapNFOBInc, realIndividuo)
//         {

//         }

//         //public override bool CriterioParada(double? fx, double nfob)
//         //{
//         //    if (criterioParada != null)
//         //        return criterioParada(fx);

//         //    if (fx == null)
//         //        return false;

//         //    return fx <= 196.94943319215918;

//         //    //return nfob >= 100;
//         //}



//         //public static void SetPlotYAxis(Chart chartPage)
//         //{
//         //    Axis yAxis = chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary);
//         //    yAxis.LogBase = 10.0;
//         //    yAxis.MinimumScale = 0.01;
//         //    yAxis.MaximumScale = 100;
//         //    yAxis.Crosses = XlAxisCrosses.xlAxisCrossesMinimum;
//         //}

//         public override GEO GetDefaultValuesGEO(int seed, double TAO, bool eng)
//         {
//             return new GEO(this, -5.12, 5.12, 11, 11 * 3, TAO, seed, true, eng, RealIndividuo);
//         }

//         public override GEOv GetDefaultValuesGEOVar(int seed, double TAO, bool eng)
//         {
//             return new GEOv(this, -5.12, 5.12, 11, 11 * 3, TAO, seed, true, eng, RealIndividuo);
//         }

//         public override GEOt GetDefaultValuesGEOt(int seed, bool eng)
//         {
//             return new GEOt(this, -5.12, 5.12, 11, 11 * 3, seed, true, eng, RealIndividuo);
//         }

//         public override GEOpPRAM GetDefaultValuesGEOpPRAM(int seed, int epochLenght, int tausLenght, int popSize, bool eng)
//         {
//             return new GEOpPRAM(this, epochLenght, tausLenght, -5.12, 5.12, 11, 11 * 3, seed, true, eng, popSize, RealIndividuo);
//         }

//         public override GEOt2 GetDefaultValuesGEOt2(int seed, bool eng)
//         {
//             return new GEOt2(this, -5.12, 5.12, 11, 11 * 3, seed, true, eng, RealIndividuo);
//         }


//         public override double ObjectiveFunction(params double[] parameters)
//         {
//             double fx = 0;

//             int I = (int)parameters[0];
//             int N = (int)parameters[1];
//             int D = (int)parameters[2];

//             ReferencePayload = new Camera(109, 26.5, 41.71, 0.048576886, 20,
//                 0.242884427939569, 12000, 6.5E-6);

//             Ss_orb = new SunSyncOrbitRPT(I, N, D, 0.00);

//             int iterations = 0;
//             double a = 0;
//             double i = 0;

//             SunSyncOrbitsWRptManager ss_mg
//                 = new SunSyncOrbitsWRptManager();

//             ss_mg.NewtonRaphson(Ss_orb, out iterations);

//             //ss_mg.NewtonRaphson(Ss_orb, out iterations);

//             a = Ss_orb.a;
//             i = Ss_orb.i;

//             Utility.Convert(ref a, ref i);
//             Ss_orb.i = i.DegreesToRadians();

//             FovMin = FOVManager.FovMin(Ss_orb);

//             Camera designedPayload =
//             CameraManager.DesignCamera(ReferencePayload, Ss_orb, FovMin);

//             Satellite = new Satellite();
//             Satellite.Payload = designedPayload;
//             Satellite.T = 4;
//             Satellite.Cd = 2.2;
//             Satellite.Md = SatelliteManager.DryMassFromCamera(designedPayload);
//             Satellite.Power = SatelliteManager.PowerFromCamera(designedPayload);

//             Satellite.A = SatelliteManager.CrossSectionalArea(Satellite.Power,
//                 Satellite.Md);

//             Satellite.At = Settings.Settings.AreaPercentOverSectionalArea * Satellite.A;

//             Deorbit = DeOrbitManager.DeOrbit(Satellite, Ss_orb);


//             Propulsion p =
//             new Propulsion(Settings.Settings.LaunchAltitudeError,
//             Settings.Settings.LaunchInclinationError, Ss_orb, Deorbit);
//             Satellite.Propulsion = p;

//             p.DeltaV_a = PropulsionManager.HommanTransferDeltaV_a(p);
//             p.DeltaV_i = PropulsionManager.HommanTransferDeltaV_i(p);
//             p.DeltaV_deorbit = PropulsionManager.DeltaV_DeOrbit(p.FinalOrbit, p.DeOrbit);

//             Satellite.Mp = PropulsionManager.StimateTotalMass(Satellite) - Satellite.Md;
//             p.DeltaV_pertubations = PropulsionManager.DragDeltaV(Satellite, Ss_orb, 0);


//             fx = Satellite.M;
//             //Console.WriteLine("Fx: " + fx);
//             //AddNFOB(getBestFx);
//             return fx;
//         }

//         public void Output()
//         {
//             Console.WriteLine("a: {0}, i: {1}, fov: {2}," +
//                 "Hp(Odo): {3}, dVdo: {4}, " +
//                 "dVa: {5}, dVi: {6}, dVdrag: {7}, dV: {8}, " +
//                 "Md {9}, Acs: {10}, At: {11}, Mp: {12}, Mt: {13}, P:{14}",
//                 Ss_orb.a/1000, Ss_orb.i.RadiansToDegrees(), FovMin,
//                 Deorbit.Hp / 1000, Satellite.Propulsion.DeltaV_deorbit,
//                 Satellite.Propulsion.DeltaV_a, Satellite.Propulsion.DeltaV_i, Satellite.Propulsion.DeltaV_pertubations, Satellite.Propulsion.Total_deltaV,
//                 Satellite.Md, Satellite.A, Satellite.At, Satellite.Mp, Satellite.M, Satellite.Power);
//         }

//         public override bool ValidateRestrictions(params double[] parameters)
//         {
//             int N = (int)parameters[1];
//             int D = (int)parameters[2];

//             return N < D 
//                 && Satellite.Payload.FOV>=1.05*FovMin
//                 && (double)N%D!=0;
//         }

//         public override GEOpV1 GetDefaultValuesGEOp(int seed, double TAO, int popSize, bool eng)
//         {
//             throw new NotImplementedException();
//         }

//         public override GEOvpV1 GetDefaultValuesGEOvp(int seed, double TAO, int popSize, bool eng)
//         {
//             throw new NotImplementedException();
//         }

//         public override GEOt2Var GetDefaultValuesGEOt2Var(int seed, bool eng)
//         {
//             throw new NotImplementedException();
//         }

//         public override GEOtVar GetDefaultValuesGEOtVar(int seed, bool eng)
//         {
//             throw new NotImplementedException();
//         }

//         public override GEOts GetDefaultValuesGEOts(int seed, bool eng)
//         {
//             throw new NotImplementedException();
//         }

//         public object Clone()
//         {
//             Optimizer opt = new Optimizer(SnapNFOBInc, RealIndividuo);

//             opt.Deorbit = (Orbit)this.Deorbit.Clone();
//             opt.ReferencePayload = (Camera)this.ReferencePayload.Clone();
//             opt.Satellite = (Satellite)this.Satellite.Clone();
//             opt.Ss_orb = (SunSyncOrbitRPT)this.Ss_orb.Clone();

//             return opt;
            
//         }

//         public override GEOpV2 GetDefaultValuesGEOp2(int seed, double TAO, int popSize, bool eng)
//         {
//             throw new NotImplementedException();
//         }

//         public override GEOVpV2 GetDefaultValuesGEOvp2(int seed, double TAO, int popSize, bool eng)
//         {
//             throw new NotImplementedException();
//         }

//         public override bool CriterioParada(Populacao p, double nfe)
//         {
//             if(p!=null)
//             {
//                 int I = (int)p.MultiParamFenotipo[0];
//                 int N = (int)p.MultiParamFenotipo[1];
//                 int D = (int)p.MultiParamFenotipo[2];

//                 return I == 14 && N == 59 && D == 60 || nfe>=100000;
//             }

//             return false;
//         }
//     }
// }
