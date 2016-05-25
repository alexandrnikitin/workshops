

namespace Adform.AdServing.AhoCorasickTree.SandboxApp
{
    using AhoCorasickTree = Adform.AdServing.AhoCorasickTree.Sandbox.V5.AhoCorasickTree;

    class Program
    {
        static void Main(string[] args)
        {
            var tree = new AhoCorasickTree(new[] {"ab", "abc", "bcd"});


            for (var i = 0; i < 300000000; i++)
            {
                tree.Contains("bcd");
            }
        }
    }
}