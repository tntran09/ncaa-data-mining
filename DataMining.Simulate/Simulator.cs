using DataMining.Utilities;
using System;

namespace DataMining.Simulate
{
    public abstract class Simulator
    {
        public string[] TeamNames { get; set; }
        public string[] WekaPredictions { get; set; }

        public Simulator(string[] teamNames, string[] wekaPredictions)
        {
            TeamNames = teamNames;
            WekaPredictions = wekaPredictions;
        }
        
        public abstract Team[] BuildTournamentPool();
        public abstract int SimulateTournament();
        
        /// <summary>
        /// Builds a Team object with the projected finish from WEKA
        /// </summary>
        /// <param name="stats"></param>
        /// <param name="wekaOutput"></param>
        /// <returns></returns>
        protected Team BuildTeam(string stats, string wekaOutput)
        {
            string[] team_data = stats.Split(",\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] proj_data = wekaOutput.Split(",\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return new Team()
            {
                Name = team_data[0].Trim().Replace("'", ""),
                Year = int.Parse(team_data[1].Trim()),
                Seed = int.Parse(team_data[2].Trim()),
                ActualFinish = double.Parse(proj_data[1]),
                PredictedFinish = double.Parse(proj_data[2])
            };
        }
    }
}
