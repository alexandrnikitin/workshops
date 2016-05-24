using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AhoCorasickTreeV2 = Adform.AdServing.AhoCorasickTree.Sandbox.V2.AhoCorasickTree;

namespace Adform.AdServing.AhoCorasickTree.SandboxApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new AhoCorasickTreeV2(new[] { "ab", "abc", "bcd" });


            for (var i = 0; i < 30000000; i++)
            {
                tree.Contains("bcd");
            }

        }
    }
}
