using System.Linq;
using Adform.AdServing.AhoCorasickTree.Benchmarks.Utils;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Adform.AdServing.AhoCorasickTree.Benchmarks
{
    using AhoCorasickTreeBaseline = Adform.AdServing.AhoCorasickTree.Sandbox.V8.AhoCorasickTree;
    using AhoCorasickTreeImproved = Adform.AdServing.AhoCorasickTree.Sandbox.V10.AhoCorasickTree;

    [Config(typeof(Config))]
    public class ManyKeywordsBenchmark
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

        private readonly AhoCorasickTreeBaseline _treeBaseline;
        private readonly AhoCorasickTreeImproved _treeImproved;

        public ManyKeywordsBenchmark()
        {
            var keywords = ResourcesUtils.GetKeywords().ToArray();

            _treeBaseline = new AhoCorasickTreeBaseline(keywords);
            _treeImproved = new AhoCorasickTreeImproved(keywords);
        }

        [Benchmark(Baseline = true)]
        public bool Test()
        {
            return _treeBaseline.Contains(UserAgent);
        }

        [Benchmark]
        public bool TestImproved()
        {
            return _treeImproved.Contains(UserAgent);
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