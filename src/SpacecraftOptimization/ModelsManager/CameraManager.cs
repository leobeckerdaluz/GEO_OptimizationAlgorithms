using SpaceConceptOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.ModelsManager
{
    public class CameraManager
    {
        public static Camera DesignCamera(Camera c0,
            Orbit nominalOrbit, double fov)
        {
            double cos_i = Math.Cos(nominalOrbit.i);
            double sin_i = Math.Sin(nominalOrbit.i);

            double Vdes = (Math.Sqrt((nominalOrbit.ni * cos_i - Settings.Settings.VE) * (nominalOrbit.ni * cos_i - Settings.Settings.VE) +
                               (nominalOrbit.ni * sin_i) * (nominalOrbit.ni * sin_i)
                             )) * Settings.Settings.R0;

            double Rv = Vdes / Settings.Settings.V0;

            double focalLenght = c0.PixelSize * nominalOrbit.Hp / Settings.Settings.MissionResolution;

            double rFl = focalLenght / c0.FocalLenght;

            double aparture = (c0.Aparture * rFl) * Rv;

            double r = aparture / c0.Aparture;
            double k = r < 0.5 ? 2 : 1;

            double mass = k * Math.Pow(r, 3) * c0.WeightOpt;
            double power = k * Math.Pow(r, 3) * c0.Power;


            return new Camera(power, mass,c0.WeightElec, aparture, Settings.Settings.MissionResolution,
                focalLenght,c0.NPixels,c0.PixelSize);
        }
    }
}
