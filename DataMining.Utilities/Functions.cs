using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Utilities
{
    public enum Transformations { Cubic, CubeRoot, Exponential, Inverse, Linear, Quadratic, LargeCubeRoot, Sqrt, Sqrt10, Cube0, Exp15, Exp18 }

    public static class Functions
    {
        public static Dictionary<Transformations, Func<int, double>> Map = new Dictionary<Transformations, Func<int, double>>()
        {
            { Transformations.CubeRoot, finish => NthRoot(finish - 4, 3) },
            { Transformations.Cubic, finish => Math.Pow(finish - 4, 3) },
            { Transformations.Exponential, finish => Math.Pow(2, finish) },
            { Transformations.Inverse, finish => finish == 0 ? 0 : (1 - (1.0 / finish)) },
            { Transformations.Linear, finish => finish },
            { Transformations.Quadratic, finish => Math.Pow(finish, 2) },
            { Transformations.LargeCubeRoot, finish => NthRoot(10 * (finish - 4), 3) },
            { Transformations.Sqrt, finish => Math.Sqrt(finish) },
            { Transformations.Sqrt10, finish => Math.Sqrt(finish * 10) },
            { Transformations.Cube0, finish => Math.Pow(finish, 3) },
            { Transformations.Exp15, finish => Math.Pow(1.5, finish) },
            { Transformations.Exp18, finish => Math.Pow(1.8, finish) }
        };

        public static double NthRoot(double x, double y)
        {
            if (x < 0 && y % 2 == 0)
                throw new ArgumentException("Cannot take even root of negative number");

            return x < 0 ? -1 * Math.Pow(-x, 1 / y) : Math.Pow(x, 1 / y);
        }
    }
}
