using System;
using System.Collections.Generic;

namespace DataMining.Utilities
{
    public class TestAnalysis
    {
        public Transformation Function { get; set; }
        // In each round there is a pool of teams and the percentage of simulations that predicted that result
        public Dictionary<int, Tuple<Team, double>[]> RoundResults { get; set; }
    }
}
