using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMining.Utilities;

namespace DataMining.Simulate
{
    public class TestSimulator : Simulator
    {
        public TestAnalysis Analysis { get; set; } = new TestAnalysis();

        public override Team[] BuildTournamentPool(string[] wekaPredictions, string[] teamNames, int tournamentIndex)
        {
            Team[] tournamentPool = new Team[64];

            if (teamNames.Length != 64)
            {
                Team t1 = BuildTeam(teamNames[0], wekaPredictions[0]);
                Team t2 = null;

                for (int j = 0, poolIndex = 0; j < teamNames.Length - 1; j++, poolIndex++)
                {
                    t2 = BuildTeam(teamNames[j + 1], wekaPredictions[j + 1]);

                    // Only simulate a game if they are the same seed
                    if (t1.Seed == t2.Seed)
                    {
                        Team win = SimulateGame(t1, t2);
                        tournamentPool[poolIndex] = win;
                        j++;
                        t1 = BuildTeam(teamNames[j + 1], wekaPredictions[j + 1]);
                    }
                    else
                    {
                        tournamentPool[poolIndex] = t1;
                        t1 = t2;
                    }
                }

                //Team t64 = BuildTeam(teamNames[63], wekaPredictions[63]);
                tournamentPool[63] = t2;
                // FirstFour teams will not be at the bottom of the bracket
            }
            else
            {
                for (int j = 0; j < 64; j++)
                {
                    int idx = tournamentIndex * 64 + j;
                    Team t = BuildTeam(teamNames[idx], wekaPredictions[idx]);
                    tournamentPool[j] = t;
                }
            }

            return tournamentPool;
        }

        public override int SimulateTournament(Team[] tournamentPool, Transformation selectedFunction, int year)
        {
            Team[] winnersPool = new Team[63];
            Team[] livingTournamentPool = new Team[64];
            Array.Copy(tournamentPool, livingTournamentPool, 64);

            int totalGamePoints = 0;
            int winnersIndex = 0;
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
                        Console.WriteLine("Proj " + winner.Name + " to round " + (nextRoundNum) + " CORRECT");
                        //o = winner.Seed + " " + winner.Name;
                    }
                    else
                    {
                        Console.WriteLine("Proj " + winner.Name + " to round " + (nextRoundNum) + " WRONG");
                        //o = winner.Seed + " " + winner.Name;
                    }

                    // Add the winner to the winners pool (size = 63)
                    winnersPool[winnersIndex] = winner;
                    winnersIndex++;
                }
                roundPoints *= 10;
                totalGamePoints += roundPoints;
            }

            Console.WriteLine(totalGamePoints + " total in " + year);
            Console.WriteLine();

            this.Analysis.Function = selectedFunction;
            this.Analysis.Simulations.Add(winnersPool);
            return totalGamePoints;
        }

        public override IAnalysis GetAnalysis()
        {
            return Analysis;
        }
    }
}
