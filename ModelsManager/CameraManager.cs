using SpaceConceptOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Settings2;

namespace SpaceConceptOptimizer.ModelsManager
{
    public class CameraManager
    {
        public static Camera DesignCamera(Camera c0,
            Orbit nominalOrbit, double fov)
        {
            double cos_i = Math.Cos(nominalOrbit.i);
            double sin_i = Math.Sin(nominalOrbit.i);
            // Console.WriteLine(nominalOrbit.i);
            // Console.WriteLine(cos_i);
            // Console.WriteLine(sin_i);

            double Vdes = (Math.Sqrt((nominalOrbit.ni * cos_i - Settings.Settings.VE) * (nominalOrbit.ni * cos_i - Settings.Settings.VE) + (nominalOrbit.ni * sin_i) * (nominalOrbit.ni * sin_i) )) * Settings.Settings.R0;
            // Console.WriteLine("Vdes: "+Vdes);

            double Rv = Vdes / Settings.Settings.V0;
            // Console.WriteLine("Rv: "+Rv);

            double focalLenght = c0.PixelSize * nominalOrbit.Hp / Settings.Settings.MissionResolution;
            // Console.WriteLine("focal: "+focalLenght);
            // Console.WriteLine("c0.PixelSize: "+c0.PixelSize);
            // Console.WriteLine("nominalOrbit.Hp: "+nominalOrbit.Hp);
            // Console.WriteLine("Settings.Settings.MissionResolution: "+Settings.Settings.MissionResolution);

            double rFl = focalLenght / c0.FocalLenght;
            // Console.WriteLine("rFl: "+rFl);

            double aparture = (c0.Aparture * rFl) * Rv;
            // Console.WriteLine("aparture: "+aparture);

            double r = aparture / c0.Aparture;
            double k = r < 0.5 ? 2 : 1;
            // Console.WriteLine("r: "+r);
            // Console.WriteLine("k: "+k);

            double mass = k * Math.Pow(r, 3) * c0.WeightOpt;
            double power = k * Math.Pow(r, 3) * c0.Power;
            // Console.WriteLine("mass: "+mass);
            // Console.WriteLine("power: "+power);


            return new Camera(power, mass,c0.WeightElec, aparture, Settings.Settings.MissionResolution,
                focalLenght,c0.NPixels,c0.PixelSize);
        }
    }
}
