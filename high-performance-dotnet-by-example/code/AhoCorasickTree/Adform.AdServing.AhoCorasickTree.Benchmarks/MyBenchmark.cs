using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using AhoCorasickTreeV2 = Adform.AdServing.AhoCorasickTree.Sandbox.V2.AhoCorasickTree;

namespace Adform.AdServing.AhoCorasickTree.Benchmarks
{
    [Config(typeof(Config))]
    public class MyBenchmark
    {
        private readonly AhoCorasickTree _tree;
        private readonly AhoCorasickTreeV2 _tree2;

        public MyBenchmark()
        {
            _tree = new AhoCorasickTree(new[] {"ab", "abc", "bcd"});
            _tree2 = new AhoCorasickTreeV2(new[] {"ab", "abc", "bcd"});
        }

        [Benchmark(Baseline = true)]
        public bool Test()
        {
            return _tree.Contains("ab");
        }

        [Benchmark]
        public bool Test2()
        {
            return _tree2.Contains("ab");
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