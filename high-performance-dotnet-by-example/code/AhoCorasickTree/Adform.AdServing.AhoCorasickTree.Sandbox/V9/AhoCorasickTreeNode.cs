using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Adform.AdServing.AhoCorasickTree.Sandbox.V9
{
    [DebuggerDisplay("Value = {Value}, TransitionCount = {_transitionsDictionary.Count}")]
    internal class AhoCorasickTreeNode
    {
        public char Value { get; private set; }
        public AhoCorasickTreeNode Failure { get; set; }

        public bool IsWord;
        private readonly List<string> _results;
        private readonly AhoCorasickTreeNode _parent;

        public List<string> Results { get { return _results; } }
        public AhoCorasickTreeNode ParentFailure { get { return _parent == null ? null : _parent.Failure; } }
        public IEnumerable<AhoCorasickTreeNode> Transitions { get { return _entries.Where(x => x.Key != 0).Select(x => x.Value); } }


        private Entry[] _entries;
        private int _size;


        public AhoCorasickTreeNode() : this(null, ' ')
        {
        }

        private AhoCorasickTreeNode(AhoCorasickTreeNode parent, char value)
        {
            Value = value;
            _parent = parent;

            _results = new List<string>();

            _entries = new Entry[0];
        }

        public void AddResult(string result)
        {
            if (!_results.Contains(result))
            {
                _results.Add(result);
            }

            IsWord = true;
        }

        public void AddResults(IEnumerable<string> results)
        {
            foreach (var result in results)
            {
                AddResult(result);
            }

            IsWord = true;
        }

        public AhoCorasickTreeNode AddTransition(char c)
        {
            var node = new AhoCorasickTreeNode(this, c);

            if (_size == 0) Resize();

            while (true)
            {
                var ind = c & (_size - 1);

                if (_entries[ind].Key != 0 && _entries[ind].Key != c)
                {
                    Resize();
                    continue;
                }

                _entries[ind].Key = c;
                _entries[ind].Value = node;
                return node;
            }
        }

        public AhoCorasickTreeNode GetTransition(char c)
        {
            if (_size == 0) return null;

            var ind = c & (_size - 1);
            var keyThere = _entries[ind].Key;
            if (keyThere != 0 && (keyThere == c))
            {
                return _entries[ind].Value;
            }

            return null;
        }

        public bool ContainsTransition(char c)
        {
            return GetTransition(c) != null;
        }


        private void Resize()
        {
            var newSize = _entries.Length * 2;
            if (newSize == 0) newSize = 1;
            Resize(newSize);
        }

        private void Resize(int newSize)
        {
            var newEntries = new Entry[newSize];
            for (var i = 0; i < _entries.Length; i++)
            {
                var key = _entries[i].Key;
                var value = _entries[i].Value;
                var ind = key & (newSize - 1);

                if (newEntries[ind].Key != 0 && newEntries[ind].Key != key)
                {
                    Resize(newSize * 2);
                    return;
                }
                newEntries[ind].Key = key;
                newEntries[ind].Value = value;
            }
            _entries = newEntries;
            _size = newSize;

        }

    }

    internal struct Entry
    {
        public char Key;
        public AhoCorasickTreeNode Value;
    }
}