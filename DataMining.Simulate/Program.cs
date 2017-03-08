using DataMining.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DataMining.Simulate
{
    class Program
    {
        // variables that determine type of simulation to run
        const bool SimulatingTestSet = true;
        //const bool UsingEnumData = false;
        const Transformations FN = Transformations.Exp18;

        // input and output file names
        const string wekaResults = "weka_predictions.txt";
        const string teamsTraining = "teams_training.txt";
        const string teamsTest = "teams_test.txt";
        const string simulationResultsFile = "simulation.txt";

        // training input not in order of year.
        static readonly int[] yearOrder = { 2014, 2013, 2012, 2011, 2009, 2010 };

        static void Main(string[] args)
        {
            List<string> output = new List<string>();
            string o = string.Empty;

            // weka output does not include team names
            string[] teamNames = File.ReadAllLines(@"..\..\" + (SimulatingTestSet ? teamsTest : teamsTraining));
            string[] wekaPredictions = File.ReadAllLines(@"..\..\" + wekaResults);

            int ITERATIONS = teamNames.Length / 64;

            for (int i = 0; i < ITERATIONS; i++)
            {
                Team[] tournamentPool = new Team[64];
                int year = yearOrder[i];

                if (SimulatingTestSet && teamNames.Length != 64)
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
                        int idx = i * 64 + j;
                        Team t = BuildTeam(teamNames[idx], wekaPredictions[idx]);
                        tournamentPool[j] = t;
                        //string[] team_data = teamNames[i * 64 + j].Split(",\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //string[] proj_data = wekaPredictions[i * 64 + j].Split(",\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //tournamentPool[j] = new Team()
                        //{
                        //    Name = team_data[0].Trim(),
                        //    Year = int.Parse(team_data[1].Trim()),
                        //    ActualFinish = double.Parse(proj_data[1]),
                        //    PredictedFinish = double.Parse(proj_data[2])
                        //};
                    }
                }

                int totalGamePoints = 0;
                for (int rnd = 1; rnd <= 6; rnd++)
                {
                    tournamentPool = SimulateRound(tournamentPool);
                    
                    int roundPoints = 0;
                    int pointsPerWin = (int)Math.Pow(2, (rnd - 1));
                    int nextRoundNum = rnd + 1;

                    foreach (Team w in tournamentPool)
                    {
                        if (w.ActualFinish >= Functions.Map[FN](nextRoundNum))
                        {
                            roundPoints += pointsPerWin;
                            o = "Proj " + w.Name + " to round " + (nextRoundNum) + " CORRECT";
                            o = w.Name;
                        }
                        else
                        {
                            o = "Proj " + w.Name + " to round " + (nextRoundNum) + " WRONG";
                            o = w.Name;
                        }

                        Console.WriteLine(o);
                        output.Add(o);
                    }
                    roundPoints *= 10;
                    totalGamePoints += roundPoints;

                    o = roundPoints + " scored in round " + rnd;
                    Console.WriteLine(o);
                    //output.Add(o);
                    output.Add("");
                }
                o = totalGamePoints + " total in " + (SimulatingTestSet ? 2015 : year);
                Console.WriteLine(o);
                //output.Add(o);
                output.Add("");

                File.WriteAllLines(@"..\..\" + simulationResultsFile, output);
            }
        }

        static Team BuildTeam(string stats, string wekaOutput)
        {
            string[] team_data = stats.Split(",\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] proj_data = wekaOutput.Split(",\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return new Team()
            {
                Name = team_data[0].Trim().Substring(1, team_data[0].Length-2),
                Year = int.Parse(team_data[1].Trim()),
                Seed = int.Parse(team_data[2].Trim()),
                ActualFinish = double.Parse(proj_data[1]),
                PredictedFinish = double.Parse(proj_data[2])
            };
        }

        static Team[] SimulateRound(Team[] pool)
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

        static Team SimulateGame(Team t1, Team t2)
        {
            return t1.PredictedFinish >= t2.PredictedFinish ? t1 : t2;
        }
    }

    class Team
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
    }
}
