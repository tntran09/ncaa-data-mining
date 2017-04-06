using DataMining.Utilities;
using System;
using System.IO;
using System.Linq;

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

                };

                if (selectedTransforms.Length == 0)
                    selectedTransforms = (Transformations[])Enum.GetValues(typeof(Transformations));

                foreach (Transformations fn in selectedTransforms)
                {
                    TransformData(
                        @"..\..\ARFF Files\" + TrainingFileName,
                        @"..\..\ARFF Files\dm_" + fn.ToString() + ".arff",
                        linesToSkip: 20,
                        columnIndex: 6,
                        fn: fn);
                    TransformData(
                        @"..\..\ARFF Files\" + TestFileName,
                        @"..\..\ARFF Files\dm2test_" + fn.ToString() + ".arff",
                        linesToSkip: 18,
                        columnIndex: 4,
                        fn: fn);
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
                char[] delimeters = { ',', '\t' };
                string[] arr = original[i].Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                int linearFinish = int.Parse(arr[columnIndex]);
                arr[columnIndex] = Math.Round(Functions.Map[fn](linearFinish), 3).ToString();
                //if (fn.ToString() == "Enum")
                //    arr[columnIndex] = "'" + arr[columnIndex] + "'";

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
