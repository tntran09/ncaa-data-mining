namespace DataMining.Utilities
{
    /// <summary>
    /// Model class which represents how a function performed on training data
    /// </summary>
    public class TrainingAnalysis
    {
        public Transformation Function { get; set; }
        public double TotalAverage { get; set; }
        public double StandardDeviation { get; set; }
        public double Median { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public double AverageWekaCorrelation { get; set; }
        public int NumberOfSimsAboveGlobalAverage { get; set; }
        public int NumberOfSimsBelowGlobalAverage { get; set; }
    }
}
