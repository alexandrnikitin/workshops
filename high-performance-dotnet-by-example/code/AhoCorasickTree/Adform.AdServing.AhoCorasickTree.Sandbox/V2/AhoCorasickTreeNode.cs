using System.Collections.Generic;
using System.Diagnostics;

namespace Adform.AdServing.AhoCorasickTree.Sandbox.V2
{
    [DebuggerDisplay("Value = {Value}, TransitionCount = {_transitionsDictionary.Count}")]
    internal class AhoCorasickTreeNode
    {
        public char Value { get; private set; }
        public AhoCorasickTreeNode Failure { get; set; }

        private readonly List<string> _results;
        private readonly Dictionary<char, AhoCorasickTreeNode> _transitionsDictionary;
        private readonly AhoCorasickTreeNode _parent;

        public IEnumerable<string> Results { get { return _results; } }
        public AhoCorasickTreeNode ParentFailure { get { return _parent == null ? null : _parent.Failure; } }
        public IEnumerable<AhoCorasickTreeNode> Transitions { get { return _transitionsDictionary.Values; } }

        public AhoCorasickTreeNode() : this(null, ' ')
        {
        }

        private AhoCorasickTreeNode(AhoCorasickTreeNode parent, char value)
        {
            Value = value;
            _parent = parent;

            _results = new List<string>();
            _transitionsDictionary = new Dictionary<char, AhoCorasickTreeNode>();
        }

        public void AddResult(string result)
        {
            if (!_results.Contains(result))
            {
                _results.Add(result);
            }
        }

        public void AddResults(IEnumerable<string> results)
        {
            foreach (var result in results)
            {
                AddResult(result);
            }
        }

        public AhoCorasickTreeNode AddTransition(char c)
        {
            var node = new AhoCorasickTreeNode(this, c);
            _transitionsDictionary.Add(node.Value, node);

            return node;
        }

        public AhoCorasickTreeNode GetTransition(char c)
        {
            AhoCorasickTreeNode node;
            return _transitionsDictionary.TryGetValue(c, out node) ? node : null;
        }

        public bool ContainsTransition(char c)
        {
            return _transitionsDictionary.ContainsKey(c);
        }
    }
}