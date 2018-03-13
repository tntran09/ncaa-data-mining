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
        public override Team[] BuildTournamentPool(string[] wekaPredictions, string[] teamNames, int tournamentIndex)
        {
            throw new NotImplementedException();
        }

        public override int SimulateTournament(Team[] tournamentPool, Transformation selectedFunction, int year)
        {
            throw new NotImplementedException();
        }
    }
}
