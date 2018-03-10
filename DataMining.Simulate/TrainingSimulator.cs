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
        public TrainingSimulator(string[] wekaPredictions) : base(wekaPredictions)
        {
        }

        public override Team[] BuildTournamentPool(string[] teamNames, int tournamentIndex)
        {
            // Training simulations are always fixed at 64 teams
            Team[] tournamentPool = new Team[64];
            for (int j = 0; j < 64; j++)
            {
                int idx = tournamentIndex * 64 + j;
                Team t = BuildTeam(teamNames[idx], WekaPredictions[idx]);
                tournamentPool[j] = t;
            }

            TournamentPool = tournamentPool;
            return tournamentPool;
        }

        public override int SimulateTournament()
        {
            throw new NotImplementedException();
        }
    }
}
