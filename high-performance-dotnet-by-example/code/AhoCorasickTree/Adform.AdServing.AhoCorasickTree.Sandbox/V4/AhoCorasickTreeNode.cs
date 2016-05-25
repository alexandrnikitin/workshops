using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Adform.AdServing.AhoCorasickTree.Sandbox.V4
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


        private int[] _buckets;
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

            _buckets = new int[0];
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
            if (_count == _buckets.Length) Resize();

            var node = new AhoCorasickTreeNode(this, c);
            var bucket = c % _buckets.Length;
            _entries[_count].Key = c;
            _entries[_count].Value = node;
            _entries[_count].Next = _buckets[bucket];
            _buckets[bucket] = _count;
            _count++;

            return node;
        }

        public AhoCorasickTreeNode GetTransition(char c)
        {
            if (_count == 0) return null;

            var bucket = c % _buckets.Length;

            for (int i = _buckets[bucket]; i >= 0; i = _entries[i].Next)
            {
                if (_entries[i].Key == c)
                {
                    return _entries[i].Value;
                }
            }

            return null;
        }

        public bool ContainsTransition(char c)
        {
            return GetTransition(c) != null;
        }


        private void Resize()
        {
            var size = _buckets.Length;
            var newSize = size == 0 ? 1 : size * 2;

            var newBuckets = new int[newSize];
            for (int i = 0; i < newSize; i++)
            {
                newBuckets[i] = -1;
            }

            var newEntries = new Entry[newSize];

            
            Array.Copy(_entries, 0, newEntries, 0, size);

            for (int i = 0; i < size; i++)
            {
                var bucket = newEntries[i].Key % newSize;
                newEntries[i].Next = newBuckets[bucket];
                newBuckets[bucket] = i;
            }

            _buckets = newBuckets;
            _entries = newEntries;

        }

    }

    internal struct Entry
    {
        public char Key;
        public AhoCorasickTreeNode Value;
        public int Next;
    }
}