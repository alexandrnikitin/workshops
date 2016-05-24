using System.Collections.Generic;
using System.Linq;

namespace Adform.AdServing.AhoCorasickTree
{
    public class AhoCorasickTreeReversed
    {
        private readonly AhoCorasickTree _tree;

        public AhoCorasickTreeReversed(IEnumerable<string> keywords)
        {
            if (keywords != null)
            {
                _tree = new AhoCorasickTree(keywords.Select(k => new string(k.Reverse().ToArray())));
            }
            else
            {
                _tree = new AhoCorasickTree(Enumerable.Empty<string>());
            }
        }

        public string FindThatEnds(string text)
        {
            var reversed = new string(text.Reverse().ToArray());
            var result = _tree.FindAll(reversed).FirstOrDefault();
            if (result != null)
                result = new string(result.Reverse().ToArray());
            return result;
        }
    }
}