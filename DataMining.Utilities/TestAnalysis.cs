using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMining.Utilities
{
    public class TestAnalysis : IAnalysis
    {
        public void AnalyzeData()
        {
            for(int i = 0; i < 63; i++)
            {
                int numberOfSims = Simulations.Count;
                Team[] allPredictionsForGame = new Team[numberOfSims];

                for (int j = 0; j < numberOfSims; j++)
                {
                    allPredictionsForGame[j] = Simulations[j][i];
                }

                AggregateSimulation[i] = GetMostPopularPick(allPredictionsForGame);
            }
        }

        private Tuple<string, double> GetMostPopularPick(params Team[] teams)
        {
            Dictionary<Team, int> counts = new Dictionary<Team, int>();
            foreach(var team in teams)
            {
                counts[team] = counts.ContainsKey(team) ? counts[team] + 1 : 1;
            }

            int max = counts.Values.Max();
            var winner = counts.First(kv => kv.Value == max).Key; 
            return Tuple.Create(winner.Name, max / (double) teams.Length);
        }

        public Transformation Function { get; set; }
        // Actual simulations based on random seed
        public List<Team[]> Simulations { get; set; } = new List<Team[]>();

        // In each round there is a pool of teams and the percentage of simulations that predicted that result
        public Tuple<string, double>[] AggregateSimulation { get; set; } = new Tuple<string, double>[63];
        // Test set may or may not produce a valuable number here, depending if the tournament has happened and finish attributes populated
        public int TestScore { get; set; }
    }
}
