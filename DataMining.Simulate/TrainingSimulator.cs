using System;
using DataMining.Utilities;

namespace DataMining.Simulate
{
    public class TrainingSimulator : Simulator
    {
        public TrainingAnalysis Analysis { get; set; } = new TrainingAnalysis();

        public override IAnalysis GetAnalysis()
        {
            //Analysis.AnalyzeData();
            return Analysis;
        }

        public override Team[] BuildTournamentPool(string[] wekaPredictions, string[] teamNames, int tournamentIndex)
        {
            // Training simulations are always fixed at 64 teams
            Team[] tournamentPool = new Team[64];
            for (int j = 0; j < 64; j++)
            {
                int idx = tournamentIndex * 64 + j;
                Team t = BuildTeam(teamNames[idx], wekaPredictions[idx]);
                tournamentPool[j] = t;
            }
            
            return tournamentPool;
        }

        public int SimulateTournament2(Team[] tournamentPool, Transformation selectedFunction, int year)
        {
            var winners = base.SimulateTournamentPool(tournamentPool, selectedFunction, base.GetActualWinner);
            int score = base.CalculateScoreOfBracket(tournamentPool, winners, selectedFunction);

            Console.WriteLine($"{score} total in {year}");
            Console.WriteLine();

            this.Analysis.Function = selectedFunction;
            this.Analysis.TrainingScores.Add(score);

            return score;
        }

        public override int SimulateTournament(Team[] tournamentPool, Transformation selectedFunction, int year)
        {
            Team[] livingTournamentPool = new Team[64];
            Array.Copy(tournamentPool, livingTournamentPool, 64);

            int totalGamePoints = 0;
            for (int round = 1; round <= 6; round++)
            {
                livingTournamentPool = SimulateRound(livingTournamentPool);

                int roundPoints = 0;
                int pointsPerWin = (int)Math.Pow(2, (round - 1));
                int nextRoundNum = round + 1;

                foreach (Team winner in livingTournamentPool)
                {
                    // Compare the actual Finish field to the current round's result
                    if (winner.ActualFinish >= Math.Round(Functions.Map[selectedFunction].Invoke(nextRoundNum), 3))
                    {
                        roundPoints += pointsPerWin;
                    }
                    
                }
                roundPoints *= 10;
                totalGamePoints += roundPoints;
            }
            
            Console.WriteLine(totalGamePoints + " total in " + year);
            Console.WriteLine();

            this.Analysis.Function = selectedFunction;
            this.Analysis.TrainingScores.Add(totalGamePoints);
            return totalGamePoints;
        }
    }
}
