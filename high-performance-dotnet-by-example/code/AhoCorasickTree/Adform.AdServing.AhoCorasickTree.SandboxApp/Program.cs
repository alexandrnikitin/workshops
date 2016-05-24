using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adform.AdServing.AhoCorasickTree.SandboxApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new AhoCorasickTree(new[] { "ab", "abc", "bcd" });


            for (var i = 0; i < 30000000; i++)
            {
                tree.Contains("bcd");
            }

        }
    }
}
