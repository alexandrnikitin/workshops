using AhoCorasickTreeV3a = Adform.AdServing.AhoCorasickTree.Sandbox.V3a.AhoCorasickTree;

namespace Adform.AdServing.AhoCorasickTree.SandboxApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new AhoCorasickTreeV3a(new[] {"ab", "abc", "bcd"});


            for (var i = 0; i < 30000000; i++)
            {
                tree.Contains("bcd");
            }
        }
    }
}