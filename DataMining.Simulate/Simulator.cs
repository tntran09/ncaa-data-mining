using DataMining.Utilities;
using System;

namespace DataMining.Simulate
{
    public abstract class Simulator
    {
        public abstract Team[] BuildTournamentPool(string[] wekaPredictions, string[] teamNames, int tournamentIndex);
        public abstract int SimulateTournament(Team[] tournamentPool, Transformation selectedFunction, int year);
        public abstract IAnalysis GetAnalysis();

        public Team[] SimulateTournamentPool(Team[] tournamentPool, Transformation selectedFunction, Func<Team, Team, Team> simulationMethod)
        {
            if (tournamentPool.Length != 64)
            {
                throw new Exception("Tournament pool must be exactly 64 teams");
            }
            
            Team[] winnersPool = new Team[63];
            Team[] livingTournamentPool = new Team[64];
            Array.Copy(tournamentPool, livingTournamentPool, 64);

            int winnersIndex = 0;
            for (int round = 1; round <= 6; round++)
            {
                livingTournamentPool = SimulateRound(livingTournamentPool);

                foreach (Team winner in livingTournamentPool)
                {
                    // Add the winner to the winners pool (size = 63)
                    winnersPool[winnersIndex] = winner;
                    winnersIndex++;
                }
            }

            return winnersPool;
        }
        
        public int CalculateScoreOfBracket(Team[] tournamentPool, Team[] predictedWinners, Transformation selectedFunction)
        {
            if (tournamentPool.Length != 64)
            {
                throw new Exception("Tournament pool must be exactly 64 teams");
            }
            
            var actualWinners = SimulateTournamentPool(tournamentPool, selectedFunction, GetActualWinner);
            //var predictedWinners = SimulateTournamentPool(tournamentPool, selectedFunction, SimulateGame);
            
            // [0, 31]: 10
            // [32, 47]: 20
            // [48, 55]: 40
            // [56, 59]: 80
            // [60, 61]: 160
            // [62]: 320
            int totalScore = 0;
            for(int i = 0; i < actualWinners.Length; i++)
            {
                if (actualWinners[i].Name == predictedWinners[i].Name)
                {
                    totalScore += 10 * (int)Math.Pow(2, 5 - Math.Log(63 - i, 2));
                }
            }

            return totalScore;
        }
        
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
        protected Team[] SimulateRound(Team[] pool, Func<Team, Team, Team> simulationMethod = null)
        {
            // TODO: Remove default parameter
            simulationMethod = simulationMethod ?? SimulateGame;

            Team[] winners = new Team[pool.Length / 2];
            for (int i = 0; i < winners.Length; i++)
            {
                Team home = pool[i * 2];
                Team away = pool[i * 2 + 1];
                winners[i] = simulationMethod.Invoke(home, away);
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

        protected Team GetActualWinner(Team t1, Team t2)
        {
            return t1.ActualFinish >= t2.ActualFinish ? t1 : t2;
        }
    }
}
