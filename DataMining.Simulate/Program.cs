using DataMining.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace DataMining.Simulate
{
    class Program
    {
        // variables that determine type of simulation to run
        const bool SimulatingTestSet = false;
        //const bool UsingEnumData = false;
        const Transformations SelectedFn = Transformations.Linear;

        // input and output file names
        const string wekaResults = "weka_predictions.txt";
        const string teamsTraining = "teams_training.txt";
        const string teamsTest = "teams_test.txt";
        const string simulationResultsFile = "simulation.txt";

        // training input not in order of year.
        static readonly int[] yearOrder = { 2016, 2015, 2014, 2013, 2012, 2011, 2009, 2010 };

        static void Main(string[] args)
        {
            List<string> output = new List<string>();
            string o = string.Empty;

            // weka output does not include team names
            string[] teamNames = File.ReadAllLines(@"..\..\" + (SimulatingTestSet ? teamsTest : teamsTraining));
            string[] wekaPredictions = Analyze(SelectedFn);

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
                    }
                }

                int totalGamePoints = 0;
                for (int round = 1; round <= 6; round++)
                {
                    tournamentPool = SimulateRound(tournamentPool);
                    
                    int roundPoints = 0;
                    int pointsPerWin = (int)Math.Pow(2, (round - 1));
                    int nextRoundNum = round + 1;

                    foreach (Team winner in tournamentPool)
                    {
                        if (winner.ActualFinish >= Functions.Map[SelectedFn].Invoke(nextRoundNum))
                        {
                            roundPoints += pointsPerWin;
                            //o = "Proj " + winner.Name + " to round " + (nextRoundNum) + " CORRECT";
                            //o = winner.Name;
                        }
                        else
                        {
                            //o = "Proj " + winner.Name + " to round " + (nextRoundNum) + " WRONG";
                            //o = winner.Name;
                        }

                        //Console.WriteLine(o);
                        //output.Add(o);
                    }
                    roundPoints *= 10;
                    totalGamePoints += roundPoints;

                    o = roundPoints + " scored in round " + round;
                    Console.WriteLine(o);
                    output.Add(o);
                    output.Add("");
                }
                o = totalGamePoints + " total in " + (SimulatingTestSet ? 2017 : year);
                Console.WriteLine(o);
                output.Add(o);
                output.Add("");

                File.WriteAllLines(@"..\..\" + simulationResultsFile, output);
            }
        }

        /// <summary>
        /// Runs WEKA analysis and returns the projection datas
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        static string[] Analyze(Transformations function)
        {
            string wekaClassPath = @"C:\Program Files\Weka-3-8\weka.jar";
            string arffPath = @"..\..\..\DataMining.Transform\ARFF Files\";

            string trainingInputFileName = arffPath + BuildDataSetFileName(function, true);
            string filterOutputFileName = @"..\..\ncaa-filtered.arff";
            string filterCmd = string.Format("weka.filters.unsupervised.attribute.Remove -R 1-2 -i \"{0}\" -o \"{1}\"", trainingInputFileName, filterOutputFileName);
            string executeFilter = string.Format("java -classpath \"{0}\" {1}", wekaClassPath, filterCmd);

            Process.Start("cmd.exe", "/C " + executeFilter).WaitForExit();

            string classifierOutputFileName = @"..\..\weka_predictions.txt";
            string testFileName = SimulatingTestSet
                ? arffPath + BuildDataSetFileName(function, false)
                : filterOutputFileName;
            string classifierCmd = string.Format("weka.classifiers.functions.MultilayerPerceptron -L 0.3 -M 0.2 -N 500 -V 0 -S 0 -E 20 -H a -t \"{0}\" -T \"{1}\" -c 5 -p 0 > \"{2}\"",
                filterOutputFileName,
                testFileName,
                classifierOutputFileName);
            string executeClassifier = string.Format("java -classpath \"{0}\" {1}", wekaClassPath, classifierCmd);

            Process.Start("cmd.exe", "/C " + executeClassifier).WaitForExit();

            // TODO: output it to weka_predictions.txt, skip first 5 lines
            /*other perceptron options:
                -classifications weka.classifiers.evaluation.output.prediction.PlainText
            */
            string[] wekaPredictions = File.ReadAllLines(@"..\..\" + wekaResults);
            return wekaPredictions.Skip(5).ToArray();
        }

        static string BuildDataSetFileName(Transformations f, bool isTrainingFile)
        {
            return string.Format("{0}_{1}.arff",
                isTrainingFile ? "dm" : "dm2test",
                f.ToString());
        }

        /// <summary>
        /// Builds a Team object with the projected finish from WEKA
        /// </summary>
        /// <param name="stats"></param>
        /// <param name="wekaOutput"></param>
        /// <returns></returns>
        static Team BuildTeam(string stats, string wekaOutput)
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

        /// <summary>
        /// Returns the team with the higher PredictedFinish. Ties go to Team1
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
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
