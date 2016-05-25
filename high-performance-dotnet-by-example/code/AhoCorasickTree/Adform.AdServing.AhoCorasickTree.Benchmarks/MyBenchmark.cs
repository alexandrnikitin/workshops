using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Jobs;
using AhoCorasickTreeV2 = Adform.AdServing.AhoCorasickTree.Sandbox.V2.AhoCorasickTree;
using AhoCorasickTreeV2a = Adform.AdServing.AhoCorasickTree.Sandbox.V2a.AhoCorasickTree;
using AhoCorasickTreeV3 = Adform.AdServing.AhoCorasickTree.Sandbox.V3.AhoCorasickTree;

namespace Adform.AdServing.AhoCorasickTree.Benchmarks
{
    [Config(typeof(Config))]
    public class MyBenchmark
    {
        private readonly AhoCorasickTree _tree;
        private readonly AhoCorasickTreeV2 _tree2;
        private readonly AhoCorasickTreeV2a _tree2a;
        private readonly AhoCorasickTreeV3 _tree3;

        public MyBenchmark()
        {
            _tree = new AhoCorasickTree(new[] {"ab", "abc", "bcd"});
            _tree2 = new AhoCorasickTreeV2(new[] {"ab", "abc", "bcd"});
            _tree2a = new AhoCorasickTreeV2a(new[] {"ab", "abc", "bcd"});
            _tree3 = new AhoCorasickTreeV3(new[] {"ab", "abc", "bcd"});
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

        [Benchmark]
        public bool Test2a()
        {
            return _tree2a.Contains("ab");
        }

        [Benchmark]
        public bool Test3()
        {
            return _tree3.Contains("ab");
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