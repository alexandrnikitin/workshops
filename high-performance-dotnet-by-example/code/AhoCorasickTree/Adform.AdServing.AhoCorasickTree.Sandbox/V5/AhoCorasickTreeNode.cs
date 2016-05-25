using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Adform.AdServing.AhoCorasickTree.Sandbox.V5
{
    [DebuggerDisplay("Value = {Value}, TransitionCount = {_transitionsDictionary.Count}")]
    internal class AhoCorasickTreeNode
    {
        public char Value { get; private set; }
        public AhoCorasickTreeNode Failure { get; set; }

        private readonly List<string> _results;
        private readonly AhoCorasickTreeNode _parent;

        public List<string> Results { get { return _results; } }
        public AhoCorasickTreeNode ParentFailure { get { return _parent == null ? null : _parent.Failure; } }
        public IEnumerable<AhoCorasickTreeNode> Transitions { get { return _entries.Where(x => x.Key != 0).Select(x => x.Value); } }


        private Entry[] _entries;
        private int _count;


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
            if (_count == _entries.Length) Resize();
            var node = new AhoCorasickTreeNode(this, c);

            while (true)
            {
                var ind = c%_entries.Length;
                if (_entries[ind].Key == 0)
                {
                    _entries[ind].Key = c;
                    _entries[ind].Value = node;
                    _count++;
                    return node;
                }

                ind++;
                if (ind == _entries.Length) Resize();
            }
        }

        public AhoCorasickTreeNode GetTransition(char c)
        {
            if (_count == 0) return null;

            var ind = c%_entries.Length;
            while (true)
            {
                if (_entries[ind].Key == c) return _entries[ind].Value;
                if (_entries[ind].Key == 0) return null;
                ind++;
                if (ind == _entries.Length) return null;
            }
        }

        public bool ContainsTransition(char c)
        {
            return GetTransition(c) != null;
        }


        private void Resize()
        {
            var newSize = _entries.Length;

            reresize:
            newSize = newSize == 0 ? 1 : newSize*2;

            var newEntries = new Entry[newSize];

            foreach (var entry in _entries)
            {
                var key = entry.Key;
                var value = entry.Value;
                var ind = key%newSize;

                while (newEntries[ind].Key != 0)
                {
                    ind++;
                    if (ind == _entries.Length)
                    {
                        goto reresize;
                    }
                }

                newEntries[ind].Key = key;
                newEntries[ind].Value = value;
            }

            _entries = newEntries;
        }
    }

    internal struct Entry
    {
        public char Key;
        public AhoCorasickTreeNode Value;
    }
}