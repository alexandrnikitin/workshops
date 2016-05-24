using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;


namespace Adform.AdServing.AhoCorasickTree.Benchmarks
{
    [Config(typeof(Config))]
    public class MyBenchmark
    {
        private AhoCorasickTree _tree;

        public MyBenchmark()
        {
            _tree = new AhoCorasickTree(new[] {"ab", "abc", "bcd"});
        }

        [Benchmark]
        public bool Test()
        {
            return _tree.Contains("ab");
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