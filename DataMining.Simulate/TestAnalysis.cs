using System;
using System.Collections.Generic;
using System.Linq;
using DataMining.Utilities;
using Newtonsoft.Json;

namespace DataMining.Simulate
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

            // Chop up the whole winners pool and group by round for readability in analysis results
            AggregateSimulationsByRound = new string[6][];
            AggregateSimulationsByRound[0] = AggregateSimulation.Take(32).Select(x => x.Item1.Name).ToArray();
            AggregateSimulationsByRound[1] = AggregateSimulation.Skip(32).Take(16).Select(x => x.Item1.Name).ToArray();
            AggregateSimulationsByRound[2] = AggregateSimulation.Skip(48).Take(8).Select(x => x.Item1.Name).ToArray();
            AggregateSimulationsByRound[3] = AggregateSimulation.Skip(56).Take(4).Select(x => x.Item1.Name).ToArray();
            AggregateSimulationsByRound[4] = AggregateSimulation.Skip(60).Take(2).Select(x => x.Item1.Name).ToArray();
            AggregateSimulationsByRound[5] = AggregateSimulation.Skip(62).Take(1).Select(x => x.Item1.Name).ToArray();

            // Try to simulate tournament with specific set of winners rather than comparing finishes
            TestScore = (new TestSimulator()).SimulateTournament(AggregateSimulation.Select(x => x.Item1).ToArray(), Function, Year);
        }

        private Tuple<Team, double> GetMostPopularPick(params Team[] teams)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            foreach(var team in teams)
            {
                counts[team.Name] = counts.ContainsKey(team.Name) ? counts[team.Name] + 1 : 1;
            }

            int max = counts.Values.Max();
            var winner = counts.First(kv => kv.Value == max).Key;
            return Tuple.Create(teams.First(t => t.Name == winner), max / (double) teams.Length);
        }
        
        public Transformation Function { get; set; }
        public int Year { get; set; }
        
        // Actual simulations based on random seed
        [JsonIgnore]
        public List<Team[]> Simulations { get; set; } = new List<Team[]>();

        // The pool of teams predicted to win each game and the percentage of simulations that predicted that result
        public Tuple<Team, double>[] AggregateSimulation { get; set; } = new Tuple<Team, double>[63];
        
        // Same as AggregateSimulation, grouped by round
        public string[][] AggregateSimulationsByRound { get; set; }

        // Test set may or may not produce a valuable number here, depending if the tournament has happened and finish attributes populated
        public int TestScore { get; set; }
    }
}
