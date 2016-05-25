using System.Linq;
using Adform.AdServing.AhoCorasickTree.Benchmarks.Utils;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using AhoCorasickTreeV2 = Adform.AdServing.AhoCorasickTree.Sandbox.V2.AhoCorasickTree;
using AhoCorasickTreeV3 = Adform.AdServing.AhoCorasickTree.Sandbox.V3.AhoCorasickTree;
using AhoCorasickTreeV3a = Adform.AdServing.AhoCorasickTree.Sandbox.V3a.AhoCorasickTree;
using AhoCorasickTreeV4 = Adform.AdServing.AhoCorasickTree.Sandbox.V4.AhoCorasickTree;
using AhoCorasickTreeV4a = Adform.AdServing.AhoCorasickTree.Sandbox.V4a.AhoCorasickTree;
using AhoCorasickTreeV5 = Adform.AdServing.AhoCorasickTree.Sandbox.V5.AhoCorasickTree;

namespace Adform.AdServing.AhoCorasickTree.Benchmarks
{
    [Config(typeof(Config))]
    public class ManyKeywordsBenchmark
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
        private readonly AhoCorasickTree _tree;
        private readonly AhoCorasickTreeV2 _tree2;
        private readonly AhoCorasickTreeV3 _tree2a;
        private readonly AhoCorasickTreeV3a _tree3;
        private readonly AhoCorasickTreeV4 _tree4;
        private readonly AhoCorasickTreeV4a _tree4a;
        private readonly AhoCorasickTreeV5 _tree5;

        public ManyKeywordsBenchmark()
        {
            var keywords = ResourcesUtils.GetKeywords().ToArray();

            _tree = new AhoCorasickTree(keywords);
            _tree2 = new AhoCorasickTreeV2(keywords);
            _tree2a = new AhoCorasickTreeV3(keywords);
            _tree3 = new AhoCorasickTreeV3a(keywords);
            _tree4 = new AhoCorasickTreeV4(keywords);
            _tree4a = new AhoCorasickTreeV4a(keywords);
            _tree5 = new AhoCorasickTreeV5(keywords);
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
        public bool Test2a()
        {
            return _tree2a.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test3()
        {
            return _tree3.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test4()
        {
            return _tree4.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test4a()
        {
            return _tree4a.Contains(UserAgent);
        }

        [Benchmark]
        public bool Test5()
        {
            return _tree5.Contains(UserAgent);
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