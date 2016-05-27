using System.Linq;
using Adform.AdServing.AhoCorasickTree.Benchmarks.Utils;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using AhoCorasickTreeV2 = Adform.AdServing.AhoCorasickTree.Sandbox.V2.AhoCorasickTree;
using AhoCorasickTreeV3 = Adform.AdServing.AhoCorasickTree.Sandbox.V3.AhoCorasickTree;
using AhoCorasickTreeV3a = Adform.AdServing.AhoCorasickTree.Sandbox.V3a.AhoCorasickTree;
using AhoCorasickTreeV4 = Adform.AdServing.AhoCorasickTree.Sandbox.V4.AhoCorasickTree;
using AhoCorasickTreeV5 = Adform.AdServing.AhoCorasickTree.Sandbox.V5.AhoCorasickTree;
using AhoCorasickTreeV6 = Adform.AdServing.AhoCorasickTree.Sandbox.V6.AhoCorasickTree;
using AhoCorasickTreeV7 = Adform.AdServing.AhoCorasickTree.Sandbox.V7.AhoCorasickTree;
using AhoCorasickTreeV8 = Adform.AdServing.AhoCorasickTree.Sandbox.V8.AhoCorasickTree;
using AhoCorasickTreeV9 = Adform.AdServing.AhoCorasickTree.Sandbox.V9.AhoCorasickTree;

namespace Adform.AdServing.AhoCorasickTree.Benchmarks
{
    [Config(typeof(Config))]
    public class ManyKeywordsBenchmark
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
        private readonly AhoCorasickTree _tree;
        private readonly AhoCorasickTreeV2 _tree2;
        private readonly AhoCorasickTreeV3 _tree3;
        private readonly AhoCorasickTreeV3a _tree3a;
        private readonly AhoCorasickTreeV4 _tree4;
        private readonly AhoCorasickTreeV5 _tree5;
        private readonly AhoCorasickTreeV6 _tree6;
        private readonly AhoCorasickTreeV7 _tree7;
        private readonly AhoCorasickTreeV8 _tree8;
        private readonly AhoCorasickTreeV9 _tree9;

        public ManyKeywordsBenchmark()
        {
            var keywords = ResourcesUtils.GetKeywords().ToArray();

            _tree = new AhoCorasickTree(keywords);
            _tree2 = new AhoCorasickTreeV2(keywords);
            _tree3 = new AhoCorasickTreeV3(keywords);
            _tree3a = new AhoCorasickTreeV3a(keywords);
            _tree4 = new AhoCorasickTreeV4(keywords);
            _tree5 = new AhoCorasickTreeV5(keywords);
            _tree6 = new AhoCorasickTreeV6(keywords);
            _tree7 = new AhoCorasickTreeV7(keywords);
            _tree8 = new AhoCorasickTreeV8(keywords);
            _tree9 = new AhoCorasickTreeV9(keywords);
        }

        [Benchmark(Baseline = true)]
        public bool Test()
        {
            return _tree.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test2()
        {
            return _tree2.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test3()
        {
            return _tree3.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test3a()
        {
            return _tree3a.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test4()
        {
            return _tree4.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test5()
        {
            return _tree5.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test6()
        {
            return _tree6.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test7()
        {
            return _tree7.Contains(UserAgent);
        }
        [Benchmark]
        public bool Test8()
        {
            return _tree8.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test9()
        {
            return _tree9.Contains(UserAgent);
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