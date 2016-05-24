using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AhoCorasickTreeV3 = Adform.AdServing.AhoCorasickTree.Sandbox.V3.AhoCorasickTree;

namespace Adform.AdServing.AhoCorasickTree.SandboxApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new AhoCorasickTreeV3(new[] { "ab", "abc", "bcd" });


            for (var i = 0; i < 30000000; i++)
            {
                tree.Contains("bcd");
            }

        }
    }
}
