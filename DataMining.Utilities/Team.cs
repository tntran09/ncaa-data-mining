namespace DataMining.Utilities
{
    public class Team
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public int Seed { get; set; }
        public double ActualFinish { get; set; }
        public double PredictedFinish { get; set; }

        public override string ToString()
        {
            return Year + " " + Name + " finished " + ActualFinish;
        }

        public string GetFinishedOutput()
        {
            // TODO: replace implicit ToString calls with this
            return Year + " " + Name + " finished " + ActualFinish;
        }
    }
}
