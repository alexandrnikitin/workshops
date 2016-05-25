using AhoCorasickTreeV4 = Adform.AdServing.AhoCorasickTree.Sandbox.V4.AhoCorasickTree;

namespace Adform.AdServing.AhoCorasickTree.SandboxApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new AhoCorasickTreeV4(new[] {"ab", "abc", "bcd"});


            for (var i = 0; i < 300000000; i++)
            {
                tree.Contains("bcd");
            }
        }
    }
}