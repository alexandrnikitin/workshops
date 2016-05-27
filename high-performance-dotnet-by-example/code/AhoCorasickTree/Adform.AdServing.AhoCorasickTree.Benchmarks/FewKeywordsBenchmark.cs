using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using AhoCorasickTreeV2 = Adform.AdServing.AhoCorasickTree.Sandbox.V2.AhoCorasickTree;
using AhoCorasickTreeV3 = Adform.AdServing.AhoCorasickTree.Sandbox.V3.AhoCorasickTree;
using AhoCorasickTreeV3a = Adform.AdServing.AhoCorasickTree.Sandbox.V3a.AhoCorasickTree;
using AhoCorasickTreeV4 = Adform.AdServing.AhoCorasickTree.Sandbox.V4.AhoCorasickTree;
using AhoCorasickTreeV4a = Adform.AdServing.AhoCorasickTree.Sandbox.V4a.AhoCorasickTree;
using AhoCorasickTreeV5 = Adform.AdServing.AhoCorasickTree.Sandbox.V5_old.AhoCorasickTree;

namespace Adform.AdServing.AhoCorasickTree.Benchmarks
{
    [Config(typeof(Config))]
    public class FewKeywordsBenchmark
    {
        private readonly AhoCorasickTree _tree;
        private readonly AhoCorasickTreeV2 _tree2;
        private readonly AhoCorasickTreeV3 _tree2a;
        private readonly AhoCorasickTreeV3a _tree3;
        private readonly AhoCorasickTreeV4 _tree4;
        private readonly AhoCorasickTreeV4a _tree4a;
        private readonly AhoCorasickTreeV5 _tree5;

        public FewKeywordsBenchmark()
        {
            _tree = new AhoCorasickTree(new[] {"ab", "abc", "bcd"});
            _tree2 = new AhoCorasickTreeV2(new[] {"ab", "abc", "bcd"});
            _tree2a = new AhoCorasickTreeV3(new[] {"ab", "abc", "bcd"});
            _tree3 = new AhoCorasickTreeV3a(new[] {"ab", "abc", "bcd"});
            _tree4 = new AhoCorasickTreeV4(new[] {"ab", "abc", "bcd"});
            _tree4a = new AhoCorasickTreeV4a(new[] {"ab", "abc", "bcd"});
            _tree5 = new AhoCorasickTreeV5(new[] {"ab", "abc", "bcd"});
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

        [Benchmark]
        public bool Test4()
        {
            return _tree4.Contains("ab");
        }

        [Benchmark]
        public bool Test4a()
        {
            return _tree4a.Contains("ab");
        }

        [Benchmark]
        public bool Test5()
        {
            return _tree5.Contains("ab");
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