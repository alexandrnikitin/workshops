using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Adform.AdServing.AhoCorasickTree.Benchmarks
{
    using AhoCorasickTreeBaseline = Adform.AdServing.AhoCorasickTree.Sandbox.V8.AhoCorasickTree;
    using AhoCorasickTreeImproved = Adform.AdServing.AhoCorasickTree.Sandbox.V10.AhoCorasickTree;

    [Config(typeof(Config))]
    public class FewKeywordsBenchmark
    {
        private readonly AhoCorasickTreeBaseline _treeBaseline;
        private readonly AhoCorasickTreeImproved _treeImproved;

        public FewKeywordsBenchmark()
        {
            _treeBaseline = new AhoCorasickTreeBaseline(new[] {"ab", "abc", "bcd"});
            _treeImproved = new AhoCorasickTreeImproved(new[] { "ab", "abc", "bcd" });
        }

        [Benchmark(Baseline = true)]
        public bool Test()
        {
            return _treeBaseline.Contains("ab");
        }

        [Benchmark]
        public bool TestImproved()
        {
            return _treeImproved.Contains("ab");
        }

        private class Config : ManualConfig
        {
            public Config()
            {
                Add(new Job {LaunchCount = 1, WarmupCount = 5, TargetCount = 5});
            }
        }
    }
}