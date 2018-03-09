using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMining.Utilities;

namespace DataMining.Simulate
{
    public class TrainingSimulator : Simulator
    {
        public TrainingSimulator(string[] teamNames, string[] wekaPredictions) : base(teamNames, wekaPredictions)
        {
        }

        public override Team[] BuildTournamentPool()
        {
            //Team[] tournamentPool = new Team[];
            for (int j = 0; j < 64; j++)
            {
                //int idx = i * 64 + j;
                //Team t = BuildTeam(TeamNames[idx], WekaPredictions[idx]);
                //tournamentPool[j] = t;
            }
            return null;
        }

        public override int SimulateTournament()
        {
            throw new NotImplementedException();
        }
    }
}
