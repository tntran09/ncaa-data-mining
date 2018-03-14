using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMining.Utilities
{
    /// <summary>
    /// Model class which represents how a function performed on training data
    /// </summary>
    public class TrainingAnalysis : IAnalysis
    {
        public TrainingAnalysis()
        {
            this.TrainingScores = new List<int>();
        }

        public void AnalyzeData()
        {
            TrainingScores = TrainingScores.OrderBy(x => x).ToList();

            int sum = 0;
            int n = TrainingScores.Count;
            MinimumScore = TrainingScores[0];
            MaximumScore = TrainingScores[0];

            foreach(var score in TrainingScores)
            {
                sum += score;
                MinimumScore = Math.Min(MinimumScore, score);
                MaximumScore = Math.Max(MaximumScore, score);
            }

            MedianScore = n % 2 == 0
                ? (TrainingScores[n / 2 - 1] + TrainingScores[n / 2]) / 2
                : TrainingScores[n / 2];
            AverageScore = sum / n;

            double sumOfSquares = 0;
            foreach(var score in TrainingScores)
            {
                sumOfSquares += (score - AverageScore) * (score - AverageScore);
            }

            StandardDeviation = Math.Sqrt(sumOfSquares / (n - 1));
        }

        public Transformation Function { get; set; }
        public List<int> TrainingScores { get; set; }
        public double AverageScore { get; set; }
        public double StandardDeviation { get; set; }
        public double MedianScore { get; set; }
        public int MinimumScore { get; set; }
        public int MaximumScore { get; set; }

        // Need weka to output or expected and actual results
        public double AverageWekaCorrelation { get; set; }

        // ?
        public int NumberOfSimsAboveGlobalAverage { get; set; }
        public int NumberOfSimsBelowGlobalAverage { get; set; }
    }
}
