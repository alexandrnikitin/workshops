using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Adform.AdServing.AhoCorasickTree.Sandbox.V10
{
    public class AhoCorasickTree
    {
        private byte[] _data;
        internal AhoCorasickTreeNode Root { get; set; }

        public AhoCorasickTree(IEnumerable<string> keywords)
        {
            Root = new AhoCorasickTreeNode();

            if (keywords != null)
            {
                foreach (var p in keywords)
                {
                    AddPatternToTree(p);
                }

                SetFailureNodes();

                // needed for offsets ;)
                SetOffsets();
                CreateArray();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe char getKey(byte* currentNodePtr, int ind)
        {
            return *(char*)(currentNodePtr + sizeof(int) + sizeof(int) + ind * (sizeof(char) + sizeof(int)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe byte* getNext(byte* b, byte* currentNodePtr, int ind)
        {
            return b + *(int*)(currentNodePtr + (sizeof(int) + sizeof(int) + ind * (sizeof(char) + sizeof(int)) + sizeof(char)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe byte* GetFailure(byte* b, byte* currentNodePtr)
        {
            return b + *(int*)(currentNodePtr + sizeof(int));
        }


        public unsafe bool Contains(string text)
        {
            fixed (byte* b = _data)
            fixed (char* p = text)
            {
                char c1, c2, c3, c4;
                int len = text.Length * 2;
                long* lptr = (long*)p;
                long l;

                byte* currentNodePtr = b;
                int size;
                int ind;
                char key;

                int count = len / 8;
                len -= (len / 8) * 8;
                while (count > 0)
                {
                    count--;
                    l = *lptr;
                    c1 = (char)(l & 0xffff);
                    c2 = (char)(l >> 16);
                    c3 = (char)(l >> 32);
                    c4 = (char)(l >> 48);
                    lptr++;

                    C1:
                    size = *(int*)currentNodePtr;
                    ind = c1 & (size - 1);
                    key = getKey(currentNodePtr, ind);
                    if (key == c1)
                    {
                        var nextPtr = getNext(b, currentNodePtr, ind);
                        if (nextPtr == b) return true;
                        currentNodePtr = nextPtr;
                    }
                    else
                    {
                        currentNodePtr = GetFailure(b, currentNodePtr);
                        if (currentNodePtr != b) goto C1;
                    }

                    C2:
                    size = *(int*)currentNodePtr;
                    ind = c2 & (size - 1);
                    key = getKey(currentNodePtr, ind);
                    if (key == c2)
                    {
                        var nextPtr = getNext(b, currentNodePtr, ind);
                        if (nextPtr == b) return true;
                        currentNodePtr = nextPtr;
                    }
                    else
                    {
                        currentNodePtr = GetFailure(b, currentNodePtr);
                        if (currentNodePtr != b) goto C2;
                    }

                    C3:
                    size = *(int*)currentNodePtr;
                    ind = c3 & (size - 1);
                    key = getKey(currentNodePtr, ind);
                    if (key == c3)
                    {
                        var nextPtr = getNext(b, currentNodePtr, ind);
                        if (nextPtr == b) return true;
                        currentNodePtr = nextPtr;
                    }
                    else
                    {
                        currentNodePtr = GetFailure(b, currentNodePtr);
                        if (currentNodePtr != b) goto C3;
                    }

                    C4:
                    size = *(int*)currentNodePtr;
                    ind = c4 & (size - 1);
                    key = getKey(currentNodePtr, ind);
                    if (key == c4)
                    {
                        var nextPtr = getNext(b, currentNodePtr, ind);
                        if (nextPtr == b) return true;
                        currentNodePtr = nextPtr;
                    }
                    else
                    {
                        currentNodePtr = GetFailure(b, currentNodePtr);
                        if (currentNodePtr != b) goto C4;
                    }
                }

                var cptr = (char*)lptr;
                while (len > 0)
                {
                    c1 = *cptr;
                    cptr++;
                    len -= 2;

                    C1:
                    size = *(int*)currentNodePtr;
                    ind = c1 & (size - 1);
                    key = getKey(currentNodePtr, ind);
                    if (key == c1)
                    {
                        var nextPtr = getNext(b, currentNodePtr, ind);
                        if (nextPtr == b) return true;
                        currentNodePtr = nextPtr;
                    }
                    else
                    {
                        currentNodePtr = GetFailure(b, currentNodePtr);
                        if (currentNodePtr != b) goto C1;
                    }
                }
            }

            return false;
        }


        public bool ContainsThatStart(string text)
        {
            return Contains(text, true);
        }

        private bool Contains(string text, bool onlyStarts)
        {
            var pointer = Root;

            for (var i = 0; i < text.Length; i++)
            {
                AhoCorasickTreeNode transition = null;
                while (transition == null)
                {
                    transition = pointer.GetTransition(text[i]);

                    if (pointer == Root)
                        break;

                    if (transition == null)
                        pointer = pointer.Failure;
                }

                if (transition != null)
                    pointer = transition;
                else if (onlyStarts)
                    return false;

                if (pointer.Results.Count > 0)
                    return true;
            }
            return false;
        }

        public IEnumerable<string> FindAll(string text)
        {
            var pointer = Root;

            foreach (var c in text)
            {
                var transition = GetTransition(c, ref pointer);

                if (transition != null)
                    pointer = transition;

                foreach (var result in pointer.Results)
                    yield return result;
            }
        }

        private AhoCorasickTreeNode GetTransition(char c, ref AhoCorasickTreeNode pointer)
        {
            AhoCorasickTreeNode transition = null;
            while (transition == null)
            {
                transition = pointer.GetTransition(c);

                if (pointer == Root)
                    break;

                if (transition == null)
                    pointer = pointer.Failure;
            }
            return transition;
        }

        private void SetFailureNodes()
        {
            var nodes = FailToRootNode();
            FailUsingBFS(nodes);
            Root.Failure = Root;
        }

        private void AddPatternToTree(string pattern)
        {
            var node = Root;
            foreach (var c in pattern)
            {
                node = node.GetTransition(c)
                       ?? node.AddTransition(c);
            }
            node.AddResult(pattern);

            node.IsWord = true;
        }

        private List<AhoCorasickTreeNode> FailToRootNode()
        {
            var nodes = new List<AhoCorasickTreeNode>();
            foreach (var node in Root.Transitions)
            {
                node.Failure = Root;
                nodes.AddRange(node.Transitions);
            }
            return nodes;
        }

        private void FailUsingBFS(List<AhoCorasickTreeNode> nodes)
        {
            while (nodes.Count != 0)
            {
                var newNodes = new List<AhoCorasickTreeNode>();
                foreach (var node in nodes)
                {
                    var failure = node.ParentFailure;
                    var value = node.Value;

                    while (failure != null && !failure.ContainsTransition(value))
                    {
                        failure = failure.Failure;
                    }

                    if (failure == null)
                    {
                        node.Failure = Root;
                    }
                    else
                    {
                        node.Failure = failure.GetTransition(value);
                        node.AddResults(node.Failure.Results);

                        if (!node.IsWord)
                        {
                            node.IsWord = failure.IsWord;
                        }
                    }

                    newNodes.AddRange(node.Transitions);
                }
                nodes = newNodes;
            }
        }

        private void CreateArray()
        {
            var data = new List<byte>();
            var queue = new Queue<AhoCorasickTreeNode>();
            queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                if (currentNode._entries.Length == 0) continue;

                data.AddRange(BitConverter.GetBytes(currentNode._entries.Length));
                data.AddRange(BitConverter.GetBytes(currentNode.Failure.Offset));

                if (currentNode._entries.Length > 0)
                {
                    foreach (var entry in currentNode._entries.Where(x => x.Key != 0))
                    {
                        queue.Enqueue(entry.Value);

                        data.AddRange(BitConverter.GetBytes(entry.Key));

                        data.AddRange(entry.Value.IsWord
                            ? BitConverter.GetBytes(0)
                            : BitConverter.GetBytes(entry.Value.Offset));
                    }
                }
            }

            _data = data.ToArray();
        }

        private void SetOffsets()
        {
            var roots = 0;
            var elements = 0;

            var queue = new Queue<AhoCorasickTreeNode>();
            queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                if (currentNode._entries.Length == 0) continue;

                currentNode.Offset = calcOffset(roots, elements);
                roots++;
                foreach (var entry in currentNode._entries.Where(x => x.Key != 0))
                {
                    queue.Enqueue(entry.Value);
                    elements++;
                }
            }
        }

        private int calcOffset(int roots, int childs)
        {
            return roots * (sizeof(int) + sizeof(int)) + childs * (sizeof(char) + sizeof(int));
        }
    }
}