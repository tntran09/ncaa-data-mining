using DataMining.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transform
{
    class Program
    {
        const string TrainingFileName = "dm_lin.arff";
        const string TestFileName = "dm2test_lin.arff";

        public static void Main(string[] args)
        {
            try
            {
                Transformations[] selectedTransforms = {
                                                            Transformations.Exponential,
                                                            Transformations.CubeRoot,
                                                            Transformations.Cubic,
                                                            Transformations.Inverse,
                                                            Transformations.Quadratic,
                                                            Transformations.Sqrt,
                                                            Transformations.Cube0,
                                                            Transformations.Exp15,
                                                            Transformations.Exp18
                                                       };
                if (selectedTransforms.Length == 0)
                    selectedTransforms = (Transformations[])Enum.GetValues(typeof(Transformations));
                foreach (Transformations t in selectedTransforms)
                {
                    TransformData(
                        @"..\..\ARFF Files\" + TrainingFileName,
                        @"..\..\ARFF Files\dm_" + t.ToString() + ".arff",
                        linesToSkip: 20,
                        columnIndex: 6,
                        fn: t);
                    TransformData(
                        @"..\..\ARFF Files\" + TestFileName,
                        @"..\..\ARFF Files\dm2test_" + t.ToString() + ".arff",
                        linesToSkip: 18,
                        columnIndex: 4,
                        fn: t);
                }
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void TransformData(string inputFilePath, string outputFilePath, int linesToSkip, int columnIndex, Transformations fn)
        {
            string[] original = File.ReadAllLines(inputFilePath);
            string[] modified = new string[original.Length];

            for (int i = 0; i < linesToSkip; i++)
            {
                modified[i] = original[i];
            }

            for (int i = linesToSkip; i < original.Length; i++)
            {
                char[] delimeters = ",\t".ToCharArray();
                string[] arr = original[i].Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                int val = int.Parse(arr[columnIndex]);
                arr[columnIndex] = Math.Round(Functions.Map[fn](val), 4).ToString();
                if (fn.ToString() == "Enum")
                    arr[columnIndex] = "'" + arr[columnIndex] + "'";

                modified[i] = arr[0] + "," + new string('\t', (6 - (arr[0].Length + 1) / 4)) + string.Join(",\t", arr.Skip(1));
            }

            File.WriteAllLines(outputFilePath, modified);

            Console.WriteLine("Transform from "
                + inputFilePath.Substring(inputFilePath.LastIndexOf(@"\"))
                + " to "
                + outputFilePath
                + " complete.");
        }
    }
}
