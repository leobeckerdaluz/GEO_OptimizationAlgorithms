// using AlgoritimosEvolutivos.Utils;
using MathModelsDomain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConceptOptimizer.Models
{
    public class Camera: ICloneable
    {
        public double WeightOpt { get; set; }

        public double WeightElec { get; set; }

        public double Power { get; set; }

        public double Resolution { get; set; }

        public double Aparture { get; set; }

        public double FocalLenght { get; set; }

        public int NPixels { get; set; }

        public double PixelSize { get; set; }

        public double FOV { get; set; }

        public Camera() { }

        public Camera(double power, double wopt,
            double welec,double aparture, double resolution,
            double focalLenght, int nPixels,
            double pixelSize)
        {
            WeightOpt = wopt;
            WeightElec = welec;
            Power = power;
            Aparture = aparture;
            Resolution = resolution;
            FocalLenght = focalLenght;
            PixelSize = pixelSize;
            NPixels = nPixels;

            //double ac = Math.Atan(90);
            FOV = 2*Math.Atan(((NPixels / 2) * PixelSize) / FocalLenght) * 180.0 / Math.PI;
            

            //FOV = MathModelsDomain.Utilities.Utility.Convert(FOV);
        }

        public Camera(double power, double wopt,
            double welec, double aparture, double resolution,
            double focalLenght, int nPixels,
            double pixelSize, double fov)
        {
            WeightOpt = wopt;
            WeightElec = welec;
            Power = power;
            Aparture = aparture;
            Resolution = resolution;
            FocalLenght = focalLenght;
            PixelSize = pixelSize;
            NPixels = nPixels;
            FOV = fov;
        }

        public object Clone()
        {
            return new object();// AlgoritimosEvolutivos.Utils.Utility.InstantiateFunction(this);
        }
    }
}
