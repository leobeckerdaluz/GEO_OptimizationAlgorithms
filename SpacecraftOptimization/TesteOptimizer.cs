// #define DEBUG_CONSOLE

using SpaceConceptOptimizer.Models;
using MathModelsDomain.ModelsManagers;
using MathModelsDomain.Utilities;
using SpaceConceptOptimizer.ModelsManager;
using SpaceConceptOptimizer;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
// using Funcoes_Definidas;

namespace SpaceDesignTeste
{
    public class TesteOptimizer
    {
        // public SunSyncOrbitRPT Ss_orb { get; set; }
        // public Camera ReferencePayload { get; set; }
        // public double FovMin { get; set; }
        // public Satellite Satellite { get; set; }
        // public Orbit Deorbit { get; set; }

        // public static bool ValidateRestrictions(int N, int D, double FOV_CameraPayload, double FovMin){
        //     return N < D 
        //         && FOV_CameraPayload >=1.05*FovMin
        //         && (double)N%D!=0;
        // }


        public static double ObjectiveFunction(List<double> fenotipo_variaveis_projeto){
            int I = (int)fenotipo_variaveis_projeto[0];
            int N = (int)fenotipo_variaveis_projeto[1];
            int D = (int)fenotipo_variaveis_projeto[2];
            

            SpaceConceptOptimizer.Settings.Settings.SolarModes = new List<SolarModeModel>();

            SpaceConceptOptimizer.Settings.Settings.SolarModes.Add(new SolarModeModel(SpaceConceptOptimizer.Utilities.SolarMode.High, 1));

            SpaceConceptOptimizer.Settings.Settings.SolarModes.Add(new SolarModeModel(SpaceConceptOptimizer.Utilities.SolarMode.MediumHigh, 3));

            SpaceConceptOptimizer.Settings.Settings.LaunchAltitudeError = 20000;

            // SpaceConceptOptimizer.Settings.Settings.LaunchInclinationError = (0.15).DegreesToRadians();
            SpaceConceptOptimizer.Settings.Settings.LaunchInclinationError = (0.015).DegreesToRadians();
            
            SpaceConceptOptimizer.Settings.Settings.ISP = 225;
            SpaceConceptOptimizer.Settings.Settings.PrecisionPropulsion = 10e-5;

            SpaceConceptOptimizer.Settings.Settings.AreaPercentOverSectionalArea = 1.8;
            // SpaceConceptOptimizer.Settings.Settings.AreaPercentOverSectionalArea = 4.37;

            SpaceConceptOptimizer.Settings.Settings.V0 = 6.778309031049825E+03;
            SpaceConceptOptimizer.Settings.Settings.MissionResolution = 20;






            double fx = 0;

            Camera ReferencePayload = new Camera(109, 26.5, 41.71, 0.048576886, 20, 0.242884427939569, 12000, 6.5E-6);
#if DEBUG_CONSOLE
            Console.WriteLine("ReferencePayload.WeightOpt: "+ReferencePayload.WeightOpt);
            Console.WriteLine("ReferencePayload.WeightElec: "+ReferencePayload.WeightElec);
            Console.WriteLine("ReferencePayload.Power: "+ReferencePayload.Power);
            Console.WriteLine("ReferencePayload.Resolution: "+ReferencePayload.Resolution);
            Console.WriteLine("ReferencePayload.Aparture: "+ReferencePayload.Aparture);
            Console.WriteLine("ReferencePayload.FocalLenght: "+ReferencePayload.FocalLenght);
            Console.WriteLine("ReferencePayload.NPixels: "+ReferencePayload.NPixels);
            Console.WriteLine("ReferencePayload.PixelSize: "+ReferencePayload.PixelSize);
            Console.WriteLine("ReferencePayload.FOV: "+ReferencePayload.FOV);
#endif


            SunSyncOrbitRPT Ss_orb = new SunSyncOrbitRPT(I, N, D, 0.00);
#if DEBUG_CONSOLE
            Console.WriteLine("Ss_orb.I: "+Ss_orb.I);
            Console.WriteLine("Ss_orb.N: "+Ss_orb.N);
            Console.WriteLine("Ss_orb.D: "+Ss_orb.D);
            Console.WriteLine("Ss_orb.Rev: "+Ss_orb.Rev);
#endif

            int iterations = 0;
            double a = 0;
            double i = 0;

            SunSyncOrbitsWRptManager ss_mg = new SunSyncOrbitsWRptManager();
#if DEBUG_CONSOLE
            Console.WriteLine("Ss_orb.Rev: "+Ss_orb.Rev);
#endif

            ss_mg.NewtonRaphson(Ss_orb, out iterations);

            a = Ss_orb.a;
            i = Ss_orb.i;
            
            Utility.Convert(ref a, ref i);
            Ss_orb.i = i.DegreesToRadians();
#if DEBUG_CONSOLE
            Console.WriteLine("a: "+a);
            Console.WriteLine("i: "+i);
            Console.WriteLine("Ss_orb.i: "+Ss_orb.a);
            Console.WriteLine("Ss_orb.i: "+Ss_orb.i);
#endif

            double FovMin = FOVManager.FovMin(Ss_orb);
#if DEBUG_CONSOLE
            Console.WriteLine("FovMin: "+FovMin);
#endif

            Camera designedPayload = CameraManager.DesignCamera(ReferencePayload, Ss_orb, FovMin);
#if DEBUG_CONSOLE
            Console.WriteLine("designedPayload: "+designedPayload);
            Console.WriteLine("designedPayload.WeightOpt: "+designedPayload.WeightOpt);
            Console.WriteLine("designedPayload.WeightElec: "+designedPayload.WeightElec);
            Console.WriteLine("designedPayload.Power: "+designedPayload.Power);
            Console.WriteLine("designedPayload.Resolution: "+designedPayload.Resolution);
            Console.WriteLine("designedPayload.Aparture: "+designedPayload.Aparture);
            Console.WriteLine("designedPayload.FocalLenght: "+designedPayload.FocalLenght);
            Console.WriteLine("designedPayload.NPixels: "+designedPayload.NPixels);
            Console.WriteLine("designedPayload.PixelSize: "+designedPayload.PixelSize);
            Console.WriteLine("designedPayload.FOV: "+designedPayload.FOV);
#endif

            Satellite Satellite = new Satellite();
            Satellite.Payload = designedPayload;
            Satellite.T = 4;
            Satellite.Cd = 2.2;
            Satellite.Md = SatelliteManager.DryMassFromCamera(designedPayload);
            Satellite.Power = SatelliteManager.PowerFromCamera(designedPayload);
            Satellite.A = SatelliteManager.CrossSectionalArea(Satellite.Power, Satellite.Md);
            Satellite.At = 1.8 * Satellite.A;
#if DEBUG_CONSOLE
            Console.WriteLine("Satellite.Md: "+Satellite.Md);
            Console.WriteLine("Satellite.Power: "+Satellite.Power);
            Console.WriteLine("Satellite.A: "+Satellite.A);
            Console.WriteLine("Satellite.At: "+Satellite.At);
#endif

            Orbit Deorbit = DeOrbitManager.DeOrbit(Satellite, Ss_orb);

            Propulsion p = new Propulsion(SpaceConceptOptimizer.Settings.Settings.LaunchAltitudeError, SpaceConceptOptimizer.Settings.Settings.LaunchInclinationError, Ss_orb, Deorbit);
            
            Satellite.Propulsion = p;

            p.DeltaV_a = PropulsionManager.HommanTransferDeltaV_a(p);
            p.DeltaV_i = PropulsionManager.HommanTransferDeltaV_i(p);
            p.DeltaV_deorbit = PropulsionManager.DeltaV_DeOrbit(p.FinalOrbit, p.DeOrbit);
#if DEBUG_CONSOLE
            Console.WriteLine("p.DeltaV_a: "+p.DeltaV_a);
            Console.WriteLine("p.DeltaV_i: "+p.DeltaV_i);
            Console.WriteLine("p.DeltaV_deorbit: "+p.DeltaV_deorbit);
#endif

            Satellite.Mp = PropulsionManager.StimateTotalMass(Satellite) - Satellite.Md;
#if DEBUG_CONSOLE
            Console.WriteLine("Satellite.Mp: "+Satellite.Mp);
            Console.WriteLine("Satellite.M: "+Satellite.M);
#endif

            // p.DeltaV_pertubations = PropulsionManager.DragDeltaV(Satellite, Ss_orb, 0);

            fx = Satellite.M;
            return fx;
        }
    }
}
