using DataMining.Utilities;
using System;

namespace DataMining.Simulate
{
    public abstract class Simulator
    {
        public abstract Team[] BuildTournamentPool(string[] wekaPredictions, string[] teamNames, int tournamentIndex);
        public abstract int SimulateTournament(Team[] tournamentPool, Transformation selectedFunction, int year);
        public abstract IAnalysis GetAnalysis();
        
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

        /// <summary>
        /// Simulates a round of games between every 2 teams
        /// </summary>
        /// <param name="pool"></param>
        /// <returns></returns>
        protected Team[] SimulateRound(Team[] pool)
        {
            Team[] winners = new Team[pool.Length / 2];
            for (int i = 0; i < winners.Length; i++)
            {
                Team home = pool[i * 2];
                Team away = pool[i * 2 + 1];
                winners[i] = SimulateGame(home, away);
            }

            return winners;
        }

        /// <summary>
        /// Returns the team with the higher PredictedFinish. Ties go to Team1
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        protected Team SimulateGame(Team t1, Team t2)
        {
            return t1.PredictedFinish >= t2.PredictedFinish ? t1 : t2;
        }
    }
}
