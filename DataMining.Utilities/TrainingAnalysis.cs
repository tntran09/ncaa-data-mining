namespace DataMining.Utilities
{
    /// <summary>
    /// Model class which represents how a function performed on training data
    /// </summary>
    public class TrainingAnalysis
    {
        public Transformation Function { get; set; }
        public int[] TrainingScores { get; set; }
        public double AverageScore { get; set; }
        public double StandardDeviation { get; set; }
        public double MedianScore { get; set; }
        public int MinimumScore { get; set; }
        public int MaximumScore { get; set; }
        public double AverageWekaCorrelation { get; set; }

        // ?
        public int NumberOfSimsAboveGlobalAverage { get; set; }
        public int NumberOfSimsBelowGlobalAverage { get; set; }
    }
}
