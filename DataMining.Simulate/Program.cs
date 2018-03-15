using DataMining.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

namespace DataMining.Simulate
{
    class Program
    {
        // variables that determine type of simulation to run
        const bool SimulatingTestSet = true;
        //const bool UsingEnumData = false;
        const Transformation SelectedFn = Transformation.Quadratic;
        const int MaxSeed = 25;
        const string hiddenLayers = "a";

        // input and output file names
        const string wekaResults = "weka_predictions.txt";
        const string teamsTraining = "teams_training.txt";
        const string teamsTest = "teams_test.txt";
        const string simulationResultsFile = "simulation.txt";

        // training input not in order of year.
        static readonly int[] yearOrder = { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2009, 2010 };

        static void Main(string[] args)
        {
            // weka output does not include team names
            string[] teamNames = File.ReadAllLines(@"..\..\" + (SimulatingTestSet ? teamsTest : teamsTraining));
            int numberOfTournamentsToSim = teamNames.Length / 64;
            
            var simulator = SimulatingTestSet
                ? (Simulator) new TestSimulator()
                : new TrainingSimulator();

            for (int seed = 0; seed < MaxSeed; seed++)
            {
                List<string> output = new List<string>();
                string outputFileName = string.Join("_",
                    SimulatingTestSet ? "Test" : "Training",
                    SelectedFn.ToString(),
                    seed,
                    hiddenLayers,
                    ".txt");
                
                string[] wekaPredictions = Analyze(SelectedFn, seed);


                for (int i = 0; i < numberOfTournamentsToSim; i++)
                {
                    var tournamentPool = simulator.BuildTournamentPool(wekaPredictions, teamNames, i);
                    simulator.SimulateTournament(tournamentPool, SelectedFn, SimulatingTestSet ? 2018 : yearOrder[i]);

                    //Team[] tournamentPool = new Team[64];

                    //if (SimulatingTestSet && teamNames.Length != 64)
                    //{
                    //    Team t1 = BuildTeam(teamNames[0], wekaPredictions[0]);
                    //    Team t2 = null;

                    //    for (int j = 0, poolIndex = 0; j < teamNames.Length - 1; j++, poolIndex++)
                    //    {
                    //        t2 = BuildTeam(teamNames[j + 1], wekaPredictions[j + 1]);

                    //        // Only simulate a game if they are the same seed
                    //        if (t1.Seed == t2.Seed)
                    //        {
                    //            Team win = SimulateGame(t1, t2);
                    //            tournamentPool[poolIndex] = win;
                    //            j++;
                    //            t1 = BuildTeam(teamNames[j + 1], wekaPredictions[j + 1]);
                    //        }
                    //        else
                    //        {
                    //            tournamentPool[poolIndex] = t1;
                    //            t1 = t2;
                    //        }
                    //    }

                    //    //Team t64 = BuildTeam(teamNames[63], wekaPredictions[63]);
                    //    tournamentPool[63] = t2;
                    //    // FirstFour teams will not be at the bottom of the bracket
                    //}
                    //else
                    //{
                    //    for (int j = 0; j < 64; j++)
                    //    {
                    //        int idx = i * 64 + j;
                    //        Team t = BuildTeam(teamNames[idx], wekaPredictions[idx]);
                    //        tournamentPool[j] = t;
                    //    }
                    //}

                    //int totalGamePoints = 0;
                    //for (int round = 1; round <= 6; round++)
                    //{
                    //    tournamentPool = SimulateRound(tournamentPool);

                    //    int roundPoints = 0;
                    //    int pointsPerWin = (int)Math.Pow(2, (round - 1));
                    //    int nextRoundNum = round + 1;

                    //    foreach (Team winner in tournamentPool)
                    //    {
                    //        string projectionResult = "";

                    //        if (winner.ActualFinish >= Math.Round(Functions.Map[SelectedFn].Invoke(nextRoundNum), 3))
                    //        {
                    //            roundPoints += pointsPerWin;
                    //            projectionResult = "Proj " + winner.Name + " to round " + (nextRoundNum) + " CORRECT";
                    //            //o = winner.Seed + " " + winner.Name;
                    //        }
                    //        else
                    //        {
                    //            projectionResult = "Proj " + winner.Name + " to round " + (nextRoundNum) + " WRONG";
                    //            //o = winner.Seed + " " + winner.Name;
                    //        }

                    //        if (SimulatingTestSet)
                    //        {
                    //            Console.WriteLine(projectionResult);
                    //            output.Add(projectionResult);
                    //        }
                    //    }
                    //    roundPoints *= 10;
                    //    totalGamePoints += roundPoints;

                    //    string roundResult = "";
                    //    if (SimulatingTestSet)
                    //    {
                    //        roundResult = roundPoints + " scored in round " + round;
                    //        output.Add(roundResult);
                    //        output.Add("");
                    //    }
                    //    else
                    //    {
                    //        //o = roundPoints.ToString();
                    //        //output.Add(o);
                    //    }
                    //    Console.WriteLine(roundResult);
                    //}

                    //int year = yearOrder[i];
                    //string fullSimulationResult = totalGamePoints + " total in " + (SimulatingTestSet ? 2017 : year);
                    //trainingScores[i] = totalGamePoints;
                    //Console.WriteLine(fullSimulationResult);
                    //Console.WriteLine();
                    //output.Add(fullSimulationResult);
                    //output.Add("");

                    //File.WriteAllLines(@"..\..\SimulationResults\" + outputFileName, output);
                }
                
            }

            var analysis = simulator.GetAnalysis();
            analysis.AnalyzeData();

            var outputString = JsonConvert.SerializeObject(analysis, Formatting.Indented);
            var testOrTraining = SimulatingTestSet ? "Test" : "Training";
            File.WriteAllText($@"..\..\SimulationResults\{testOrTraining}-{SelectedFn.ToString()}-{MaxSeed}.txt", outputString);
            Console.ReadLine();
        }

        /// <summary>
        /// Runs WEKA analysis and returns the projection datas
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        static string[] Analyze(Transformation function, int randomSeed)
        {
            string wekaClassPath = @"C:\Users\toan\Downloads\weka-stable-3-8\weka\weka.jar";
            string arffPath = @"..\..\..\DataMining.Transform\ARFF Files\";

            string trainingInputFileName = arffPath + BuildDataSetFileName(function, true);
            string filterOutputFileName = @"..\..\ncaa-filtered.arff";
            string filterCmd = $"weka.filters.unsupervised.attribute.Remove -R 1-2 -i \"{trainingInputFileName}\" -o \"{filterOutputFileName}\"";
            string executeFilter = $"java -classpath \"{wekaClassPath}\" {filterCmd}";

            ProcessStartInfo filterProcessInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                FileName = "cmd.exe",
                Arguments = "/C " + executeFilter,
                UseShellExecute = false
            };
            Process.Start(filterProcessInfo).WaitForExit();

            string classifierOutputFileName = $@"..\..\{wekaResults}";
            string testFileName = SimulatingTestSet
                ? arffPath + BuildDataSetFileName(function, false)
                : filterOutputFileName;
            string classifierCmd = $"weka.classifiers.functions.MultilayerPerceptron " +
                $"-L 0.3 -M 0.2 -N 500 -V 0 -S {randomSeed} -E 20 -H {hiddenLayers} -t \"{filterOutputFileName}\" -T \"{testFileName}\" -c 5 -p 0 > \"{classifierOutputFileName}\"";
            string executeClassifier = $"java -classpath \"{wekaClassPath}\" {classifierCmd}";

            ProcessStartInfo classifierProcessInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                FileName = "cmd.exe",
                Arguments = "/C " + executeClassifier,
                UseShellExecute = false
            };
            Process.Start(classifierProcessInfo).WaitForExit();

            // TODO: output it to weka_predictions.txt, skip first 5 lines
            /*other perceptron options:
                -classifications weka.classifiers.evaluation.output.prediction.PlainText
            */
            string[] wekaPredictions = File.ReadAllLines(classifierOutputFileName);
            return wekaPredictions.Skip(5).ToArray();
        }

        static string BuildDataSetFileName(Transformation f, bool isTrainingFile)
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
}
