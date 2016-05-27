

using Adform.AdServing.AhoCorasickTree.Benchmarks.Utils;

namespace Adform.AdServing.AhoCorasickTree.SandboxApp
{
    using AhoCorasickTree = Adform.AdServing.AhoCorasickTree.Sandbox.V6.AhoCorasickTree;

    class Program
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

        static void Main(string[] args)
        {
            var tree = new AhoCorasickTree(ResourcesUtils.GetKeywords());

            for (var i = 0; i < 30000000; i++)
            {
                tree.Contains(UserAgent);
            }
        }
    }
}