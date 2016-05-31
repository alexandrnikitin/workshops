using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Adform.AdServing.AhoCorasickTree.Benchmarks.Utils
{
    public static class ResourcesUtils
    {
        public static IEnumerable<string> GetKeywords()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Adform.AdServing.AhoCorasickTree.Benchmarks.Resources.keywords.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }
    }
}