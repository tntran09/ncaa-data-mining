using DataMining.Utilities;

namespace DataMining.Simulate
{
    public interface ISimulator
    {
        Team[] BuildTournamentPool();
        int SimulateTournament();
    }
}
