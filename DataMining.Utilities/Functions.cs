using System;
using System.Collections.Generic;

namespace DataMining.Utilities
{
    public static class Functions
    {
        public static Dictionary<Transformation, Func<int, double>> Map = new Dictionary<Transformation, Func<int, double>>()
        {
            { Transformation.CubeRoot, finish => NthRoot(finish - 4, 3) },
            { Transformation.Cubic, finish => Math.Pow(finish - 4, 3) },
            { Transformation.Exponential, finish => Math.Pow(2, finish) },
            { Transformation.Inverse, finish => finish == 0 ? 0 : (1 - (1.0 / finish)) },
            { Transformation.Linear, finish => finish },
            { Transformation.Quadratic, finish => Math.Pow(finish, 2) },
            { Transformation.LargeCubeRoot, finish => NthRoot(10 * (finish - 4), 3) },
            { Transformation.Sqrt, finish => Math.Sqrt(finish) },
            { Transformation.Sqrt10, finish => Math.Sqrt(finish * 10) },
            { Transformation.Cube0, finish => Math.Pow(finish, 3) },
            { Transformation.Exp15, finish => Math.Pow(1.5, finish) },
            { Transformation.Exp18, finish => Math.Pow(1.8, finish) },
            { Transformation.Exp16, finish => Math.Pow(1.6, finish) }
        };

        public static double NthRoot(double x, double y)
        {
            if (x < 0 && y % 2 == 0)
                throw new ArgumentException("Cannot take even root of negative number");

            return x < 0 ? -1 * Math.Pow(-x, 1 / y) : Math.Pow(x, 1 / y);
        }
    }
}
